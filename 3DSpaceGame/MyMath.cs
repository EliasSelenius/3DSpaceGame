using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    public static class MyMath {
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a + ((b - a) * t);
    }
}
