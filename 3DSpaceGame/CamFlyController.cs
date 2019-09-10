using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    class CamFlyController : Component {

        public override void Start() {
            Input.FixedMouse(true);
        }

        public override void Update() {
            var translation = new Vector3();
            var wasd = Input.Wasd;
            translation -= gameObject.Forward * wasd.Y;
            translation += gameObject.Right * wasd.X;
            translation += gameObject.Up * Input.KeyAxis(OpenTK.Input.Key.Space, OpenTK.Input.Key.LShift);
            gameObject.Position += translation * .5f;

            gameObject.Rotate(gameObject.Up, Input.MouseDelta.X / 100);
            gameObject.Rotate(gameObject.Right, Input.MouseDelta.Y / 100);
            gameObject.Rotate(gameObject.Forward, Input.KeyAxis(OpenTK.Input.Key.Q, OpenTK.Input.Key.E) / 10);

        }
    }
}
