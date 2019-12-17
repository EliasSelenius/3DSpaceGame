using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;


namespace _3DSpaceGame.Particles {
    public class EngineFlames : ParticleSystem {

        public float spawnRate = 200f;

        private Vector3 offset;
        private float coneHeadRadius = 0;

        private readonly static Mesh mesh;

        static EngineFlames() {
            mesh = Meshes.GetMesh("dust_particle");
        }

        public EngineFlames(Vector3 ofs, float radius = 0) : base(20, false) {
            offset = ofs;
            coneHeadRadius = radius;
            material = new Material {
                emission = new Vector3(.9f, .5f, 0),
                ambient = new Vector3(.8f, .3f, 0),
                diffuse = new Vector3(.8f, .3f, 0),
                specular = new Vector3(1f),
                shininess = 1f
            };
        }

        public override void Update() {
            base.Update();

            this.Queue(spawnRate * Program.DeltaTime);
        }

        protected override bool ParticleEndCondition(Particle p) {
            return p.time > .1f;
        }

        protected override void RenderParticle(Particle p) {
            mesh.Render();
        }

        protected override void StartParticle(Particle p) {
            var o = transform.right * offset.X + transform.up * offset.Y + transform.forward * offset.Z;
            float f() => Random.Rangef(-coneHeadRadius, coneHeadRadius);
            p.transform.position = transform.position + o + transform.up * f() + transform.left * f();
            p.transform.Rotate(Random.Vec3(0, 5f));

            p.velocity = transform.back;
            p.rotationalVelocity = Random.Vec3(3.14f);
        }


        protected override void UpdateParticle(Particle p) { }
    }
}
