using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using Nums;

namespace _3DSpaceGame {

    public class Particle {
        public readonly Transform transform = new Transform();
        public float time = 0;
        public vec3 velocity;
        public vec3 rotationalVelocity;
        public bool enabled = false;

        
        public void Reset() {
            time = 0;
            enabled = false;
            transform.position = vec3.zero;
            transform.rotation = Quaternion.Identity;
            transform.scale = vec3.one;
            velocity = vec3.zero;
        }
        
    }

    public abstract class ParticleSystem : Component {

        private readonly List<Particle> particles = new List<Particle>();

        protected IEnumerable<Particle> EnabledParticles => from p in particles
                                                          where p.enabled == true
                                                          select p;

        protected IEnumerable<Particle> DisabledParticles => from o in particles
                                                           where o.enabled == false
                                                           select o;

        public float spawnRate = 0;

        protected bool LocalCoords;

        public Material material = Material.Default;

        protected int ParticleCount {
            get => particles.Count;
            set {
                if (particles.Count < value) {
                    var num = value - particles.Count;
                    for (int i = 0; i < num; i++) {
                        particles.Add(new Particle());
                    }
                } else {
                    var num = particles.Count - value;
                    for (int i = 0; i < num; i++) {
                        particles.RemoveAt(0);
                    }
                }
            }
        }
        
        public ParticleSystem(int pcount, bool local = true, float spr = 0) {
            ParticleCount = pcount;
            LocalCoords = local;
            spawnRate = spr;
        }

        public override void EarlyUpdate() {

            // Update & kill current particles
            for (int i = 0; i < particles.Count; i++) {
                var p = particles[i];
                if (p.enabled) {

                    p.time += Program.DeltaTime;
                    p.transform.Translate(p.velocity * Program.DeltaTime);
                    p.transform.Rotate(p.rotationalVelocity * Program.DeltaTime);

                    UpdateParticle(p);

                    if (ParticleEndCondition(p)) {
                        p.Reset();
                    }
                }
            }
        }

        private float particlesQued = 0;
        public void Queue(float num) {
            particlesQued += num;
            for (int i = 0; i < MyMath.Floor(particlesQued); i++) {
                Spawn();
                particlesQued--;
            }
        }

        public void Spawn() {
            if (DisabledParticles.Count() > 0) {
                var p = DisabledParticles.ElementAt(0);
                p.enabled = true;
                StartParticle(p);
            }
        }

        public override void Update() {
            Queue(spawnRate * Program.DeltaTime);
        }

        public override void Render() {
            material.Apply(Program.StandardShader);
            for (int i = 0; i < particles.Count; i++) {
                var p = particles[i];
                if (!p.enabled) {
                    continue;
                }
                var m = p.transform.matrix;
                if (LocalCoords) {
                    m *= gameObject.ModelMatrix;
                }
                Program.StandardShader.SetMat4("obj_transform", m);
                RenderParticle(p);
            }
        }

        protected abstract bool ParticleEndCondition(Particle p);
        protected abstract void StartParticle(Particle p);
        protected abstract void UpdateParticle(Particle p);
        protected abstract void RenderParticle(Particle p);

    }
}
