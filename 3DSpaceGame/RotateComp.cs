using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame {
    class RotateComp : Component {

        public Nums.vec3 axis;
        public float speed;

        public RotateComp(Nums.vec3 a, float s) {
            axis = a; speed = s;
        }

        public override void Update() {
            transform.Rotate(axis, speed * Program.DeltaTime);
        }

    }
}
