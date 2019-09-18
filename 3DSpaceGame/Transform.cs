using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    public class Transform {

        // nameing rule violation
#pragma warning disable IDE1006
        public Matrix4 matrix => Matrix4.CreateScale(scale) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position);

        public Vector3 forward => matrix.Row2.Xyz;
        public Vector3 back => -forward;
        public Vector3 right => matrix.Row0.Xyz;
        public Vector3 left => -right;
        public Vector3 up => matrix.Row1.Xyz;
        public Vector3 down => -up;
#pragma warning restore IDE1006

        public Vector3 position = Vector3.Zero;
        public Vector3 scale = Vector3.One;
        public Quaternion rotation = Quaternion.Identity;


        public void Translate(Vector3 v) {
            position += v;
        }

        public void Rotate(Quaternion q) {
            rotation *= q;
        }

        public void Rotate(Vector3 axis, float angle) {
            rotation *= Quaternion.FromAxisAngle(axis, angle);
        }

        public void Rotate(Vector3 euler) {
            rotation *= Quaternion.FromEulerAngles(euler);
        }


        public void LookAt(Vector3 point) => LookAt(point, Vector3.UnitY);
        public void LookAt(Vector3 point, Vector3 up) {
            var dir = (point - position).Normalized();
            Console.WriteLine("dir: " + dir);
            //var f = (position + forward) - position;
            var f = forward;
            var rotationaxis = Vector3.Cross(dir, f);
            Console.WriteLine("axis: " + rotationaxis);
            var angle = Vector3.CalculateAngle(dir, f);
            Console.WriteLine("angle: " + angle);
            Rotate(rotationaxis, angle / 10f);

            //var f = (point - position).Normalized();
            //var l = Vector3.Cross(up, f);
            //var u = Vector3.Cross(l, f);
            //var m = new Matrix3(l.X, u.X, f.X,
            //                    l.Y, u.Y, f.Y,
            //                    l.Z, u.Z, f.Z);
            //rotation = m.ExtractRotation();

            //Vector3 f = point - position;
            //Vector3 u = up;
            //Vector3 r = Vector3.Cross(f, u);
            //rotation.W = MyMath.Sqrt(1f + r.X + u.Y + f.Z) * .5f;
            //float recip = 1f / (4f * rotation.W);
            //rotation.X = (u.Z - f.Y) * recip;
            //rotation.Y = (f.X - r.Z) * recip;
            //rotation.Z = (r.Y - u.X) * recip;
        }

    }
}
