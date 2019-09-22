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

        private Vector2 mouseOffset;

        public override void Update() { 
            mouseOffset += Input.MouseDelta / 100f;
            //p.AddTorque(new Vector3(mdelta.Y, -mdelta.X, 0));

            //var offset = -transform.forward;
            //offset *= MyMath.Cos(mouseOffset.X);
            //offset += transform.right * MyMath.Sin(mouseOffset.X);
            //var newcampos = offset * 5f + transform.up * 1.5f;

            //var mxn = MyMath.NormAngle(mouseOffset.X);
            //var myn = MyMath.NormAngle(mouseOffset.Y);

            //var cpos = new Vector3(
            //    MyMath.Cos(mxn) * MyMath.Cos(myn),
            //    MyMath.Sin(myn),
            //    MyMath.Sin(mxn) * MyMath.Cos(myn)) * 4;


            // Transform.transformPoint() = (new Vector4(cpos) * transform.matrix).Xyz + transform.position;


            //Camera.MainCamera.transform.position
            //Camera.MainCamera.transform.rotation = transform.rotation;
            Camera.MainCamera.transform.LookAt(transform.position, transform.up);

            if (!Input.IsKeyDown(OpenTK.Input.Key.AltLeft)) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Camera.MainCamera.transform.rotation, .05f);
            }
            
            var i = Input.Wasd;
            var f = transform.forward * i.Y;
            f += transform.left * i.X;
            p.AddForce(f);

        }
    }
}
