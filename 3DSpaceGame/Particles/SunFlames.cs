using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nums;

namespace _3DSpaceGame.Particles {
    public class SunFlames : ParticleSystem {


        private static readonly Mesh mesh;
        static SunFlames() {
            mesh = Meshes.GetMesh("dust_particle");
        }
 

        public SunFlames() : base(40, true, 40) {
            material = Material.Sun;
        }


        protected override bool ParticleEndCondition(Particle p) {
            return p.time > 1;
        }

        protected override void RenderParticle(Particle p) {
            mesh.Render();
        }

        protected override void StartParticle(Particle p) {
            var r = Random.UnitVec3();
            p.transform.position = r;
            p.velocity = r/10f;
            p.transform.Rotate(Random.Vec3(0, 5f));
            p.transform.scale = vec3.one * .3f;

        }

        protected override void UpdateParticle(Particle p) { }
    }
}
