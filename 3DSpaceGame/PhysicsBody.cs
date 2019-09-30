using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    public class PhysicsBody : Component {

        public float mass = 1;
        public Vector3 motion;
        public Quaternion rotmotion = Quaternion.Identity;
        public float drag = .03f;
        public float rotdrag = .05f;

        public PhysicsBody() {

        }

        public PhysicsBody(float m) {
            mass = m;
        }

        public override void EarlyUpdate() {
            transform.position += motion * Program.DeltaTime;
            //transform.Rotate(rotmotion);
            motion *= 1 - drag;
            rotmotion *= 1 - rotdrag;
        }

        public void AddTorque(Quaternion torque) {
            var axisangle = torque.ToAxisAngle();
            rotmotion *= Quaternion.FromAxisAngle(axisangle.Xyz, axisangle.W / mass);
        }

        public void AddTorque(Vector3 eulertorque) {
            AddTorque(Quaternion.FromEulerAngles(eulertorque));
        }

        public void AddForce(Vector3 force) {
            motion += force / mass;
        }

        public void AddForce(float x, float y, float z) {
            motion.X += x / mass;
            motion.Y += y / mass;
            motion.Z += z / mass;
        }

    }
}
