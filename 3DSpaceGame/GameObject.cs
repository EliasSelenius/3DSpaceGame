using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    public class GameObject {

        // nameing rule violation
#pragma warning disable IDE1006
        public Scene scene { get; private set; }
        public GameObject parent { get; private set; }
#pragma warning restore IDE1006
        private readonly List<GameObject> children = new List<GameObject>();

        public readonly Transform transform = new Transform();

        public bool IsParent => children.Count > 0;
        public bool IsChild => parent == null;

        public Matrix4 ModelMatrix {
            get {
                var m = transform.matrix;
                if (parent != null) {
                    return m * parent.ModelMatrix;
                }
                return m;
            }
        }
        
        public void AddChild(GameObject obj) {
            if (obj.scene != scene) {
                obj.EnterScene(scene);
            }
            children.Add(obj);
            obj.parent = this;
        }

        public void RemoveChild(GameObject obj) {
            children.Remove(obj);
            obj.parent = null;
        }


        #region Components

        private readonly List<Component> components = new List<Component>();

        public void AddComp(Component c) {
            components.Add(c);
            c.Init(this);
        }

        public void AddComps(params Component[] comps) {
            foreach (var item in comps) {
                AddComp(item);
            }
        }

        public T GetComp<T>() where T : Component =>
            (from o in components
             where o is T
             select o as T).FirstOrDefault();

        #endregion


        public bool HasStarted { get; private set; } = false;
        public void Start() {
            for (int i = 0; i < components.Count; i++) {
                components[i].Start();
            }
            HasStarted = true;
        }

        public void EnterScene(Scene s) {
            for (int i = 0; i < children.Count; i++) {
                children[i].EnterScene(s);
            }

            if (scene != null) {
                LeaveScene();
            }

            scene = s;
            scene._AddObject(this);
            for (int i = 0; i < components.Count; i++) {
                components[i].OnEnter();
            }
        }

        public void LeaveScene() {
            for (int i = 0; i < children.Count; i++) {
                children[i].LeaveScene();
            }
            for (int i = 0; i < components.Count; i++) {
                components[i].OnLeave();
            }
            scene._RemoveObject(this);
            scene = null;
        }

        public void EarlyUpdate() {
            for (int i = 0; i < components.Count; i++) {
                components[i].EarlyUpdate();
            }
        }

        public void Update() {
            for (int i = 0; i < components.Count; i++) {
                components[i].Update();
            }
        }

        public void Render() {
            foreach (var item in components) {
                item.Render();
            }
        }

    }
}
