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

            var offset = -transform.forward;
            offset *= MyMath.Cos(mouseOffset.X);
            offset += transform.right * MyMath.Sin(mouseOffset.X);
            var newcampos = offset * 5f + transform.up * 1.5f;

            transform.LookAt(Camera.MainCamera.transform.position);

            //Camera.MainCamera.gameObject.Position = MyMath.Lerp(Camera.MainCamera.gameObject.Position, gameObject.Position - gameObject.Forward * 5f + gameObject.Up * 1.5f, .9f);
            //Camera.MainCamera.transform.position = transform.position + newcampos;
            //Camera.MainCamera.transform.rotation = transform.rotation;
            //Camera.MainCamera.transform.LookAt(transform.position);

            var i = Input.Wasd;
            var f = transform.forward * i.Y;
            f += transform.left * i.X;
            //p.AddForce(f);

        }
    }
}
