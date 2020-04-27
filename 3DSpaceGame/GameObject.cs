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
        public bool IsChild => parent != null;
        public bool IsRoot => parent == null;

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
        private List<Physics.Collider> colliders;

        private void addCollider(Physics.Collider c) {
            if (colliders == null)
                colliders = new List<Physics.Collider>();
            colliders.Add(c);
        }

        public void AddComp(Component c) {
            components.Add(c);
            if (c is Physics.Collider co)
                addCollider(co);
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

        public Component GetComp(Type type) =>
            (from o in components
             where type.IsInstanceOfType(o)
             select o).FirstOrDefault();

        public Component GetComp(string typename) => GetComp(Type.GetType(typename));

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

        internal void processColliders(int objIndex) {

            if (this.colliders == null) return;

            for (int i = 0; i < colliders.Count; i++) {
                for (int o = objIndex + 1; o < scene.gameObjects.Count; o++) {
                    var other = scene.gameObjects[o];
                    if (other.colliders == null) continue;
                    for (int j = 0; j < other.colliders.Count; j++) {
                        colliders[i].checkCollision(other.colliders[j]);
                        other.colliders[j].checkCollision(colliders[i]);
                    }
                }
            }
        }

        internal void OnColliderIntersect(Physics.collision collision) {
            for (int i = 0; i < components.Count; i++) {
                components[i].OnCollision(collision);
            }
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
