using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nums;

namespace _3DSpaceGame.Physics {
    public class Collider {

        public readonly List<Shape> shapes = new List<Shape>();

        public bool Intersects(Collider other) {
            return false;
        }

        public class Shape {
            public vec3 offset;
        }

        public class Sphere : Shape {

        }
    }
}
