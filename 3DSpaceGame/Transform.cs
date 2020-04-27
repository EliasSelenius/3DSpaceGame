using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using JsonParser;
using Nums;

namespace _3DSpaceGame {
    public class Transform {

        public static Transform FromJson(JObject json) {
            var res = new Transform();

            if (json.ContainsKey("position")) {
                var pos = json["position"] as JArray;
                res.position = new vec3(pos[0] as JNumber, pos[1] as JNumber, pos[2] as JNumber);
            }

            if (json.ContainsKey("scale")) {
                var s = json["scale"] as JArray;
                res.scale = new vec3(s[0] as JNumber, s[1] as JNumber, s[2] as JNumber);
            }

            if (json.ContainsKey("rotation")) {
                var r = json["rotation"] as JArray;
                res.rotation = new Quaternion(r[0] as JNumber, r[1] as JNumber, r[2] as JNumber, r[3] as JNumber);
            } // todo: euler, axis angle support here

            return res;
        }


        // nameing rule violation
#pragma warning disable IDE1006
        public Matrix4 matrix => Matrix4.CreateScale(scale.ToOpenTKVec()) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position.ToOpenTKVec());

        public vec3 forward => matrix.Row2.Xyz.ToNumsVec();
        public vec3 back => -forward;
        public vec3 right => matrix.Row0.Xyz.ToNumsVec();
        public vec3 left => -right;
        public vec3 up => matrix.Row1.Xyz.ToNumsVec();
        public vec3 down => -up;
#pragma warning restore IDE1006

        public vec3 position = vec3.zero;
        public vec3 scale = vec3.one;
        public Quaternion rotation = Quaternion.Identity;


        public Transform() { }
        public Transform(vec3 pos) => position = pos;
        public Transform(vec3 pos, vec3 scl) {
            position = pos; scale = scl;
        }
        public Transform(Quaternion rot) => rotation = rot;
        public Transform(vec3 pos, Quaternion rot) {
            position = pos; rotation = rot;
        }
        public Transform(vec3 pos, vec3 scl, Quaternion rot) {
            position = pos; scale = scl; rotation = rot;
        }

        public void Translate(vec3 v) {
            position += v;
        }

        public void Translate(float x, float y, float z) {
            position.x += x;
            position.y += y;
            position.z += z;
        }

        public void Rotate(Quaternion q) {
            rotation *= q;
        }

        public void Rotate(vec3 axis, float angle) {
            rotation *= Quaternion.FromAxisAngle(axis.ToOpenTKVec(), angle);
        }

        public void Rotate(vec3 euler) {
            rotation *= Quaternion.FromEulerAngles(euler.ToOpenTKVec());
        }


        public void LookIn(vec3 dir) => LookIn(dir, vec3.unity);
        public void LookIn(vec3 dir, vec3 up) {
            var m = Matrix4.LookAt(position.ToOpenTKVec(), (position + dir).ToOpenTKVec(), up.ToOpenTKVec()).Inverted();
            m.Row0.Xyz = -m.Row0.Xyz;
            m.Row2.Xyz = -m.Row2.Xyz;
            rotation = m.ExtractRotation();
        }

        public void LookAt(vec3 point) => LookAt(point, vec3.unity);
        public void LookAt(vec3 point, vec3 up) {

            var m = Matrix4.LookAt(position.ToOpenTKVec(), point.ToOpenTKVec(), up.ToOpenTKVec()).Inverted();
            m.Row0.Xyz = -m.Row0.Xyz;
            m.Row2.Xyz = -m.Row2.Xyz;
            rotation = m.ExtractRotation();

            #region Failed LookAt attempts
            //var z = (point - position).Normalized();
            //var y = up;
            //var x = vec3.Cross(y, z).Normalized();
            //y = vec3.Cross(x, z).Normalized();
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
            //var rotaxis = vec3.Cross(lookdir, targetdir);
            //var angle = vec3.CalculateAngle(lookdir, targetdir);
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
            //var dot = vec3.Dot(currentDir, targetDir) ;
            //var angle = MyMath.Acos(dot);
            //var axis = vec3.Cross(currentDir, targetDir);
            //Rotate(axis, angle);

            //var dir = (point - position).Normalized();
            //dir.Z *= -1;
            //Console.WriteLine("dir: " + dir);
            ////var f = (position + forward) - position;
            //var f = forward;
            //var rotationaxis = vec3.Cross(dir, f);
            //Console.WriteLine("axis: " + rotationaxis);
            //var angle = vec3.CalculateAngle(dir, f);
            //Console.WriteLine("angle: " + angle);
            //Rotate(rotationaxis, angle / 10f);
            //rotation.Normalize();

            //var f = (point - position).Normalized();
            //var l = vec3.Cross(up, f);
            //var u = vec3.Cross(l, f);
            //var m = new Matrix3(l.X, u.X, f.X,
            //                    l.Y, u.Y, f.Y,
            //                    l.Z, u.Z, f.Z);
            //rotation = m.ExtractRotation();

            //vec3 f = point - position;
            //vec3 u = up;
            //vec3 r = vec3.Cross(f, u);
            //rotation.W = MyMath.Sqrt(1f + r.X + u.Y + f.Z) * .5f;
            //float recip = 1f / (4f * rotation.W);
            //rotation.X = (u.Z - f.Y) * recip;
            //rotation.Y = (f.X - r.Z) * recip;
            //rotation.Z = (r.Y - u.X) * recip;

            //if ((targetPosition - m_Position) == glm::vec3(0, 0, 0)) return;
            //var direction = vec3.Normalize(point - position);
            //var r = vec3.Zero;
            //r.X = MyMath.Asin(-direction.Y);
            //r.Y = -MyMath.Atan2(-direction.X, -direction.Z);
            //Console.WriteLine("angle: " + r);
            //rotation = Quaternion.FromEulerAngles(r);
            #endregion
        }

        public void RotateAround(vec3 point, vec3 axis, float angle) => RotateAround(point, Quaternion.FromAxisAngle(axis.ToOpenTKVec(), angle));
        public void RotateAround(vec3 point, vec3 euler) => RotateAround(point, Quaternion.FromEulerAngles(euler.ToOpenTKVec()));
        public void RotateAround(vec3 point, Quaternion rot) {
            var v = position - point;
            var r = RotateVector(rot, v);
            position = point + r;
        }

        public static vec3 RotateVector(Quaternion rot, vec3 v) {
            var qv = new Quaternion(v.ToOpenTKVec(), 0);
            var c = rot;
            c.Conjugate();
            return (rot * qv * c).Xyz.ToNumsVec();
        }

        public vec3 DirTo(vec3 point) => point - position;
        public vec3 DirTo(Transform t) => t.position - position;
        public float DistTo(vec3 point) => DirTo(point).length;
        public float DistTo(Transform t) => DirTo(t).length;
        public float DistToSq(vec3 point) => (point - position).sqlength;
        public float DistToSq(Transform t) => (t.position - position).sqlength;

        public void SetDistTo(vec3 point, float dist) {
            var dir = position - point;
            position = point + (dir.normalized * dist);
        }

    }
}
