using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    public class PlayerShipController : Component {
        public override void Update() {
            //var mdelta = Input.MouseDelta / 100f;
            //gameObject.Rotate(gameObject.Up, mdelta.X);
            //gameObject.Rotate(gameObject.Right, mdelta.Y);

            gameObject.Rotate(gameObject.Up, .1f);
        }
    }
}
