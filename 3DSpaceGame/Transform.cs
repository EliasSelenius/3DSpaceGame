using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using JsonParser;

namespace _3DSpaceGame {
    public class Transform {

        public static Transform FromJson(JObject json) {
            var res = new Transform();

            if (json.ContainsKey("position")) {
                var pos = json["position"] as JArray;
                res.position = new Vector3(pos[0] as JNumber, pos[1] as JNumber, pos[2] as JNumber);
            }

            if (json.ContainsKey("scale")) {
                var s = json["scale"] as JArray;
                res.scale = new Vector3(s[0] as JNumber, s[1] as JNumber, s[2] as JNumber);
            }

            if (json.ContainsKey("rotation")) {
                var r = json["rotation"] as JArray;
                res.rotation = new Quaternion(r[0] as JNumber, r[1] as JNumber, r[2] as JNumber, r[3] as JNumber);
            } // todo: euler, axis angle support here

            return res;
        }


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


        public Transform() { }
        public Transform(Vector3 pos) => position = pos;
        public Transform(Vector3 pos, Vector3 scl) {
            position = pos; scale = scl;
        }
        public Transform(Quaternion rot) => rotation = rot;
        public Transform(Vector3 pos, Quaternion rot) {
            position = pos; rotation = rot;
        }
        public Transform(Vector3 pos, Vector3 scl, Quaternion rot) {
            position = pos; scale = scl; rotation = rot;
        }

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


        public void LookIn(Vector3 dir) => LookIn(dir, Vector3.UnitY);
        public void LookIn(Vector3 dir, Vector3 up) {
            var m = Matrix4.LookAt(position, position + dir, up).Inverted();
            m.Row0.Xyz = -m.Row0.Xyz;
            m.Row2.Xyz = -m.Row2.Xyz;
            rotation = m.ExtractRotation();
        }

        public void LookAt(Vector3 point) => LookAt(point, Vector3.UnitY);
        public void LookAt(Vector3 point, Vector3 up) {

            var m = Matrix4.LookAt(position, point, up).Inverted();
            m.Row0.Xyz = -m.Row0.Xyz;
            m.Row2.Xyz = -m.Row2.Xyz;
            rotation = m.ExtractRotation();

            #region Failed LookAt attempts
            //var z = (point - position).Normalized();
            //var y = up;
            //var x = Vector3.Cross(y, z).Normalized();
            //y = Vector3.Cross(x, z).Normalized();
            //var rotm = new Matrix3();
            //rotm.Row0.X = x.X;
            //rotm.Row1.X = x.Y;
            //rotm.Row2.X = x.Z;

            //rotm.Row0.Y = y.X;
            //rotm.Row1.Y = y.Y;
            //rotm.Row2.Y = y.Z;

            //rotm.Row0.Z = z.X;
            //rotm.Row1.Z = z.Y;
            //rotm.Row2.Z = z.Z;

            //rotation = Quaternion.FromMatrix(rotm);

            //var lookdir = forward.ToNumsVec();
            //var targetdir = -(point - position).ToNumsVec();
            //var rotaxis = (lookdir.Cross(targetdir));
            //var angle = lookdir.AngleTo(targetdir);
            //Rotate(rotaxis.ToOpenTKVec(), -.1f);


            //var lookdir = forward;
            //var targetdir = (point - position);
            //var rotaxis = Vector3.Cross(lookdir, targetdir);
            //var angle = Vector3.CalculateAngle(lookdir, targetdir);
            //Rotate(rotaxis, .1f);

            /*
             x = cos(a)
             z = sin(a)
             a = acos(x) 
             */

            //var dir = (point - position).Normalized();
            //float ya = MyMath.Atan(dir.Z / dir.X) + (dir.X < 0 ? MyMath.pi : 0);
            //float xa = MyMath.Atan(dir.Y / dir.Z) * (dir.Z < 0 ? 1 : -1);
            //rotation = Quaternion.FromEulerAngles(0, -ya + MyMath.pi / 2f, 0) * Quaternion.FromEulerAngles(xa, 0, 0);


            //var targetDir = (point - position);
            //var currentDir = forward;
            //var dot = Vector3.Dot(currentDir, targetDir) ;
            //var angle = MyMath.Acos(dot);
            //var axis = Vector3.Cross(currentDir, targetDir);
            //Rotate(axis, angle);

            //var dir = (point - position).Normalized();
            //dir.Z *= -1;
            //Console.WriteLine("dir: " + dir);
            ////var f = (position + forward) - position;
            //var f = forward;
            //var rotationaxis = Vector3.Cross(dir, f);
            //Console.WriteLine("axis: " + rotationaxis);
            //var angle = Vector3.CalculateAngle(dir, f);
            //Console.WriteLine("angle: " + angle);
            //Rotate(rotationaxis, angle / 10f);
            //rotation.Normalize();

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

            //if ((targetPosition - m_Position) == glm::vec3(0, 0, 0)) return;
            //var direction = Vector3.Normalize(point - position);
            //var r = Vector3.Zero;
            //r.X = MyMath.Asin(-direction.Y);
            //r.Y = -MyMath.Atan2(-direction.X, -direction.Z);
            //Console.WriteLine("angle: " + r);
            //rotation = Quaternion.FromEulerAngles(r);
            #endregion
        }

        public void RotateAround(Vector3 point, Vector3 axis, float angle) => RotateAround(point, Quaternion.FromAxisAngle(axis, angle));
        public void RotateAround(Vector3 point, Vector3 euler) => RotateAround(point, Quaternion.FromEulerAngles(euler));
        public void RotateAround(Vector3 point, Quaternion rot) {
            var v = position - point;
            var r = RotateVector(rot, v);
            position = point + r;
        }

        public static Vector3 RotateVector(Quaternion rot, Vector3 v) {
            var qv = new Quaternion(v, 0);
            var c = rot;
            c.Conjugate();
            return (rot * qv * c).Xyz;
        }

        public Vector3 DirTo(Vector3 point) => point - position;
        public Vector3 DirTo(Transform t) => t.position - position;
        public float DistTo(Vector3 point) => DirTo(point).Length;
        public float DistTo(Transform t) => DirTo(t).Length;
        public float DistToSq(Vector3 point) => (point - position).LengthSquared;
        public float DistToSq(Transform t) => (t.position - position).LengthSquared;

        public void SetDistTo(Vector3 point, float dist) {
            var dir = position - point;
            position = point + (dir.Normalized() * dist);
        }

    }
}
