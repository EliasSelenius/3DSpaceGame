using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
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

        public Vector3 Forward => LocalMatrix.Row2.Xyz;
        public Vector3 Back => -Forward;
        public Vector3 Right => LocalMatrix.Row0.Xyz;
        public Vector3 Left => -Right;
        public Vector3 Up => LocalMatrix.Row1.Xyz;
        public Vector3 Down => -Up;

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


        public bool HasStarted { get; private set; } = false;
        public void Start() {
            for (int i = 0; i < components.Count; i++) {
                components[i].Start();
            }
            HasStarted = true;
        }

        public void EnterScene(Scene s) {
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
            Program.ActiveShader.SetMat4("obj_transform", TransformMatrix);
            foreach (var item in components) {
                item.Render();
            }
        }

    }
}
