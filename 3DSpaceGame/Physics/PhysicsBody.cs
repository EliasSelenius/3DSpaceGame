using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using Nums;

namespace _3DSpaceGame.Physics {
    public class PhysicsBody : Component {

        public float mass = 1;
        public float bounciness = 2.5f;
        public bool isSolid = true;

        public vec3 motion;
        public Quaternion rotmotion = Quaternion.Identity;

        public vec3 momentum => motion * mass;

        public float drag = .03f;
        public float rotdrag = .05f;


        public PhysicsBody() {
            
        }

        public PhysicsBody(float m) {
            mass = m;
        }

        public override void EarlyUpdate() {
            transform.position += motion * Program.DeltaTime;
            transform.Rotate(rotmotion);
            motion *= 1 - drag;
            rotmotion = Quaternion.Slerp(rotmotion, Quaternion.Identity, rotdrag);
        }
        
        public override void OnCollision(collision collision) {

            if (isSolid) {
                transform.position += collision.direction;
            }

            var f = collision.direction.normalized;
            //Console.WriteLine(f);
            motion = motion.reflect(f);
        }

        public void AddTorque(Quaternion torque) {
            var axisangle = torque.ToAxisAngle();
            rotmotion *= Quaternion.FromAxisAngle(axisangle.Xyz, axisangle.W / mass);
        }

        public void AddTorque(vec3 eulertorque) {
            AddTorque(Quaternion.FromEulerAngles(eulertorque.ToOpenTKVec()));
        }

        public void AddForce(vec3 force) {
            motion += force / mass;
        }

        public void AddForce(float x, float y, float z) {
            motion.x += x / mass;
            motion.y += y / mass;
            motion.z += z / mass;
        }

    }
}
