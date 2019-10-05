using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame {
    public class SpaceDustParticles : ParticleSystem {

        private MeshRenderer mr;

        public SpaceDustParticles() : base(10, false) {

        }

        public override void Start() {
            mr = gameObject.GetComp<MeshRenderer>();
        }

        protected override bool ParticleEndCondition(Particle p) {
            return p.transform.DistToSq(transform) > 400;
        }

        protected override void RenderParticle(Particle p) {
            mr.material.Apply(Program.StandardShader);
            mr.mesh.Render();
        }

        protected override void StartParticle(Particle p) {
            p.transform.position = transform.position + Random.Vec3(20);
            p.velocity = Random.Vec3(.4f);
            p.transform.scale = OpenTK.Vector3.One * .1f;
        }

        protected override void UpdateParticle(Particle p) {
            
        }
    }
}
