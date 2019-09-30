using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame {
    public class SpaceDustParticles : ParticleSystem {

        public SpaceDustParticles() : base(10, 3, false) {

        }

        protected override void RenderParticle(Particle p) {
            Draw.Point();
        }

        protected override void StartParticle(Particle p) {
            p.transform.position = transform.position + Random.Vec3(15);
            p.velocity = Random.Vec3(2);
        }

        protected override void UpdateParticle(Particle p) {
            
        }
    }
}
