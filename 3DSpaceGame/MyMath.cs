using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    public static class MyMath {

        public const float pi = (float)Math.PI;
        public const float tau = 2 * pi;

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a + ((b - a) * t);

        public static float Sin(float x) => (float)Math.Sin(x);
        public static float Cos(float x) => (float)Math.Cos(x);
        public static float Sqrt(float x) => (float)Math.Sqrt(x);
        public static float Asin(float x) => (float)Math.Asin(x);
        public static float Atan(float x) => (float)Math.Atan(x);
        public static float Atan2(float x, float y) => (float)Math.Atan2(x, y);
        public static float Acos(float x) => (float)Math.Acos(x);

        public static float Floor(float x) => (float)Math.Floor(x);
        public static float Fract(float x) => x - Floor(x);

        public static float NormAngle(float x) {
            return Fract(x / tau) * tau;
        }

        public static float Clamp(float v, float min, float max) => v < min ? min : v > max ? max : v;

        public static Nums.Vectors.Vec3 ToNumsVec(this Vector3 v) => new Nums.Vectors.Vec3(v.X, v.Y, v.Z);
        public static Vector3 ToOpenTKVec(this Nums.Vectors.Vec3 v) => new Vector3(v.x, v.y, v.z);

        public static string ToReadableStr(this float f) => f.ToString("###.###");
        public static string ToReadableStr(this Vector3 vec) => $"({vec.X.ToReadableStr()}, {vec.Y.ToReadableStr()}, {vec.Z.ToReadableStr()})";

    }
}
