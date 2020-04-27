using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using _3DSpaceGame.Physics;

namespace _3DSpaceGame {
    public class PlayerShipController : Component {

        private PhysicsBody p;
        private Physics.Collider collider;

        public override void Start() {
            p = gameObject.GetComp<PhysicsBody>();
            collider = gameObject.GetComp<Physics.Collider>();

            Input.FixedMouse(true);
        }

        private Quaternion camrot = Quaternion.Identity;
        private float defFov = 70;
        private float zoom = 1;

        public override void Update() {

            // Transform.transformPoint() = (new Vector4(cpos) * transform.matrix).Xyz + transform.position;

            // Camera controlls:

            zoom = MyMath.Clamp(zoom + Input.MouseWheelDelta / 3f, 1, 100);

            camrot = camrot.Rotate(new Vector3(Input.MouseDelta.y / 100f, -Input.MouseDelta.x / 100f, 0));

            Camera.MainCamera.transform.position = transform.position + camrot.CalcForward().ToNumsVec() * -10 * zoom;
            Camera.MainCamera.transform.LookAt(transform.position, camrot.CalcUp().ToNumsVec());

            if (!Input.IsKeyDown(OpenTK.Input.Key.AltLeft)) {
                transform.rotation = Quaternion.Slerp(transform.rotation, camrot, .05f);
            }


            /*
            if (collider.IsIntersecting) {
                Console.WriteLine("Intersecting!");
            }*/


            // Ship movment controlls
            var fovTarget = defFov;
            float speed = .5f;
            if (Input.IsKeyDown(OpenTK.Input.Key.LShift)) {
                fovTarget = 90;
                speed *= 2.3f;
            }
            Camera.MainCamera.FOV = MyMath.Lerp(Camera.MainCamera.FOV, fovTarget, .1f);
            var i = Input.Wasd * speed;
            var f = transform.forward * i.y;
            f += transform.left * i.x * .5f;
            p.AddForce(f);

        }

        public override void OnCollision(collision collision) {
            /*var m = gameObject.GetComp<MeshRenderer>().mesh;
            m.indices.RemoveRange(0, 10);
            m.Apply();*/
        }
    }
}
