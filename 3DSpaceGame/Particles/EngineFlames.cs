using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;


namespace _3DSpaceGame.Particles {
    public class EngineFlames : ParticleSystem {


        private Vector3 offset;
        private float coneHeadRadius = 0;

        private readonly static Mesh mesh;

        private Physics.PhysicsBody pBody;

        static EngineFlames() {
            mesh = Meshes.GetMesh("dust_particle");
        }

        public EngineFlames(double ofs_x, double ofs_y, double ofs_z, double radius = 0) : this (new Vector3((float)ofs_x, (float)ofs_y, (float)ofs_z), (float)radius) { }

        public EngineFlames(Vector3 ofs, float radius = 0) : base(20, false, 200f) {
            offset = ofs;
            coneHeadRadius = radius;
            material = new Material {
                emission = new Nums.vec3(.9f, .5f, 0),
                ambient = new Nums.vec3(.8f, .3f, 0),
                diffuse = new Nums.vec3(.8f, .3f, 0),
                specular = Nums.vec3.one,
                shininess = 1f
            };
        }

        public override void Start() {
            base.Start();

            pBody = gameObject.GetComp<Physics.PhysicsBody>();
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

            p.velocity = pBody.motion * .8f;
            p.rotationalVelocity = Random.Vec3(3.14f);
        }


        protected override void UpdateParticle(Particle p) { }
    }
}
