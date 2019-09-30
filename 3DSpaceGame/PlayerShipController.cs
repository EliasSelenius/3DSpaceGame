using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    public class PlayerShipController : Component {

        private PhysicsBody p;

        public override void Start() {
            p = gameObject.GetComp<PhysicsBody>();

            Input.FixedMouse(true);
        }

        private Quaternion camrot = Quaternion.Identity;

        public override void Update() {

            // Transform.transformPoint() = (new Vector4(cpos) * transform.matrix).Xyz + transform.position;

            // Camera controlls:

            camrot = camrot.Rotate(new Vector3(Input.MouseDelta.Y / 100f, -Input.MouseDelta.X / 100f, 0));

            Camera.MainCamera.transform.position = transform.position + camrot.CalcForward() * -10;
            Camera.MainCamera.transform.LookAt(transform.position, camrot.CalcUp());
            
            if (!Input.IsKeyDown(OpenTK.Input.Key.AltLeft)) {
                transform.rotation = Quaternion.Slerp(transform.rotation, camrot, .05f);
            }


            // Ship movment controlls
            float speed = .5f;
            if (Input.IsKeyDown(OpenTK.Input.Key.LShift)) {
                speed *= 2f;
            }
            var i = Input.Wasd * speed;
            var f = transform.forward * i.Y;
            f += transform.left * i.X * .5f;
            p.AddForce(f);

        }
    }
}
