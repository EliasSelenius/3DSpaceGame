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

        public override void Update() { 
            var mdelta = Input.MouseDelta / 100f;
            //gameObject.Rotate(new Vector3(mdelta.Y, -mdelta.X, 0));
            p.AddTorque(new Vector3(mdelta.Y, -mdelta.X, 0));

            //Camera.MainCamera.gameObject.Position = MyMath.Lerp(Camera.MainCamera.gameObject.Position, gameObject.Position - gameObject.Forward * 5f + gameObject.Up * 1.5f, .9f);
            Camera.MainCamera.gameObject.Position = gameObject.Position - gameObject.Forward * 5f + gameObject.Up * 1.5f;
            Camera.MainCamera.gameObject.Rotation = gameObject.Rotation;

            var i = Input.Wasd;
            var f = gameObject.Forward * i.Y;
            f += gameObject.Left * i.X;
            p.AddForce(f);

        }
    }
}
