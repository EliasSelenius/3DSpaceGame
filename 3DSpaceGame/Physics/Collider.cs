using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Nums;

namespace _3DSpaceGame.Physics {

    public struct collision {
        public readonly Collider other;
        public readonly vec3 point;
        public readonly vec3 direction;

        public collision(Collider o, vec3 p, vec3 d) {
            other = o; point = p; direction = d;
        }
    }

    public interface ICollideable<T> where T : Collider {
        bool Intersects(T other, out collision? collision);
    }

    public abstract class Collider : Component, ICollideable<SphereCollider>, ICollideable<CuboidCollider> {


        public bool IsIntersecting { get; private set; } = false;
        public vec3 offset;


        internal void checkCollision(Collider other) {
            bool isinter = false;
            collision? colls = null;
            if (other is SphereCollider s)
                isinter = Intersects(s, out colls);
            else if (other is CuboidCollider c)
                isinter = Intersects(c, out colls);

            if (isinter)
                gameObject.OnColliderIntersect((collision)colls);

        }


        public abstract bool Intersects(SphereCollider other, out collision? collision);
        public abstract bool Intersects(CuboidCollider other, out collision? collision);

    }

    public class SphereCollider : Collider {

        public float radius;

        public SphereCollider(double r) {
            radius = (float)r;
        }

        public vec3 getWorldPos() => transform.position + (new OpenTK.Matrix3(gameObject.ModelMatrix) * offset.ToOpenTKVec()).ToNumsVec();

        public override bool Intersects(SphereCollider other, out collision? collision) {


            var dir = other.getWorldPos() - getWorldPos();
            var dist = dir.length;
            var dirn = dir.normalized;
            var point = transform.position + dirn * radius;
            
            collision = null;
            if (dist - other.radius < radius) {
                collision = new collision(other, point, dirn * (dist - (radius + other.radius)));
                return true;
            }

            return false;
        }


        public override bool Intersects(CuboidCollider other, out collision? collision) {
            throw new NotImplementedException();
        }
    }

    public class CuboidCollider : Collider {
        public override bool Intersects(SphereCollider other, out collision? collision) {
            throw new NotImplementedException();
        }

        public override bool Intersects(CuboidCollider other, out collision? collision) {
            throw new NotImplementedException();
        }
    }

}
