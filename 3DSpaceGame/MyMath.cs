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

        public static Quaternion Rotate(this Quaternion rot, Vector3 axis, float angle) => rot * Quaternion.FromAxisAngle(axis, angle);
        public static Quaternion Rotate(this Quaternion rot, Vector3 euler) => rot * Quaternion.FromEulerAngles(euler);
        public static Quaternion Cnjgt(this Quaternion rot) {
            var c = rot;
            c.Conjugate();
            return c;
        }

        public static Vector3 Rotate(this Vector3 v, Quaternion rot) {
            return (rot * new Quaternion(v, 0) * rot.Cnjgt()).Xyz;
        }

        public static Vector3 CalcForward(this Quaternion rot) => 
            new Vector3(2f * (rot.X * rot.Z + rot.W * rot.Y),
                       2f * (rot.Y * rot.Z - rot.W * rot.X),
                       1f - 2f * (rot.X * rot.X + rot.Y * rot.Y));

        public static Vector3 CalcRight(this Quaternion rot) =>
            new Vector3(1f - 2f * (rot.Y * rot.Y + rot.Z * rot.Z),
                       2f * (rot.X * rot.Y + rot.W * rot.Z),
                       2f * (rot.X * rot.Z - rot.W * rot.Y));
        
        public static Vector3 CalcUp(this Quaternion rot) =>
            new Vector3(2f * (rot.X * rot.Y - rot.W * rot.Z),
                       1f - 2f * (rot.X * rot.X + rot.Z * rot.Z),
                       2f * (rot.Y * rot.Z + rot.W * rot.X));

    }
}
