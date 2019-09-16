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
        public Vector3 rotmotion;
        public float drag = .95f;

        public PhysicsBody() {

        }

        public PhysicsBody(float m) {
            mass = m;
        }

        public override void Update() {
            gameObject.Position += motion * Program.DeltaTime;
            gameObject.Rotate(rotmotion * Program.DeltaTime);
            motion *= drag;
            rotmotion *= drag;
        }

        public void AddTorque(Vector3 torque) {
            rotmotion += torque / mass;
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
