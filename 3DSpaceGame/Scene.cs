using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {

    public class Scene {
        private readonly List<GameObject> gameObjects = new List<GameObject>();

        public void RemoveObject(GameObject o) {
            gameObjects.Remove(o);
        }

        public void AddObject(GameObject o) {
            gameObjects.Add(o);
        }

        public GameObject InitObject(params Component[] comps) {
            var g = new GameObject();
            g.MoveInToScene(this);
            g.AddComps(comps);
            return g;
        }

        public void Update() {
            for (int i = 0; i < gameObjects.Count; i++) {
                gameObjects[i].Update();
            }
        }

        public void Render() {
            foreach (var item in gameObjects) {
                item.Render();
            }
        }

    }

    public class GameObject {

        public Scene scene { get; private set; }

        public GameObject parent;

        #region Transform

        public Matrix4 TransformMatrix {
            get {
                var m = LocalMatrix;
                if (parent != null) {
                    return m * parent.TransformMatrix;
                }
                return m;
            }
        }

        public Matrix4 LocalMatrix => Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(Position);

        public Vector3 Forward => LocalMatrix.Column2.Xyz;
        public Vector3 Right => LocalMatrix.Column0.Xyz;
        public Vector3 Up => LocalMatrix.Column1.Xyz;

        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
        public Quaternion Rotation = Quaternion.Identity;

        #region transformation helper funcs

        public void Translate(Vector3 v) {
            Position += v;
        }

        public void Rotate(Quaternion q) {
            Rotation *= q;
        }

        public void Rotate(Vector3 axis, float angle) {
            Rotation *= Quaternion.FromAxisAngle(axis, angle);
        }

        public void Rotate(Vector3 euler) {
            Rotation *= Quaternion.FromEulerAngles(euler);
        }

        #endregion

        #endregion

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

        public void MoveInToScene(Scene s) {
            if (scene != null) {
                scene.RemoveObject(this);
            }
            s.AddObject(this);
            scene = s;
        }

        public void MoveInToScene(Scene s, Vector3 pos, Quaternion rot) {
            MoveInToScene(s);
            Position = pos;
            Rotation = rot;
        }

        public void Update() {
            for (int i = 0; i < components.Count; i++) {
                components[i].Update();
            }
        }

        public void Render() {
            Program.ActiveShader.SetMat4("obj_transform", TransformMatrix);
            foreach (var item in components) {
                item.Render();
            }
        }

    }

    public abstract class Component {
        public GameObject gameObject { get; private set; }

        public void Init(GameObject g) {
            gameObject = g;
            Start(); // TODO: do start somewhere else
        }

        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void Render() { }

    }

}
