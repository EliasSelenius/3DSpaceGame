using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame.Particles {
    public class SpaceDustParticles : ParticleSystem {

        //private MeshRenderer mr;

        private static Mesh mesh;

        private static float outerRadius = 100;

        private OpenTK.Vector3 spawnPos => transform.position + transform.forward * 20f;

        static SpaceDustParticles() {
            mesh = Meshes.GetMesh("dust_particle");
            mesh.Init();
        }

        public SpaceDustParticles() : base(45, false) {

        }

        public override void Start() {
            //mr = gameObject.GetComp<MeshRenderer>();
        }

        public override void Update() {
            base.Update();

            for (int i = 0; i < DisabledParticles.Count(); i++) {
                Spawn();
            }
        }

        protected override bool ParticleEndCondition(Particle p) {
            return p.transform.DistToSq(spawnPos) > outerRadius * outerRadius;
        }

        protected override void RenderParticle(Particle p) {
            //mr.material.Apply(Program.StandardShader);
            //mr.mesh.Render();
            //Draw.Sprite();
            mesh.Render();
        }

        protected override void StartParticle(Particle p) {
            
            p.transform.position = spawnPos + Random.Vec3(10, outerRadius);

            p.velocity = Random.Vec3(.4f);
            p.rotationalVelocity = Random.Vec3(3.14f);
            //p.transform.scale = OpenTK.Vector3.One * .1f;
        }

        protected override void UpdateParticle(Particle p) {
            
        }
    }
}
