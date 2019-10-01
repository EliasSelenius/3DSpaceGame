using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {

    public class Particle {
        public readonly Transform transform = new Transform();
        public float time = 0;
        public Vector3 velocity;
        public bool enabled = false;

        public void Update() {
            time += Program.DeltaTime;
            transform.Translate(velocity * Program.DeltaTime);
        }

        public void Reset() {
            time = 0;
            enabled = false;
            transform.position = Vector3.Zero;
            transform.rotation = Quaternion.Identity;
            transform.scale = Vector3.One;
            velocity = Vector3.Zero;
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

        protected bool LocalCoords;
        protected float LifeTime;
        protected float SpawnRate;

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
        
        public ParticleSystem(float rate, float lifeTime, bool local = true) {
            SpawnRate = rate; LifeTime = lifeTime; LocalCoords = local;
            ParticleCount = (int)(SpawnRate * LifeTime);
        }

        public override void EarlyUpdate() {

            // Update & kill current particles
            for (int i = 0; i < particles.Count; i++) {
                var p = particles[i];
                if (p.enabled) {
                    p.Update();

                    UpdateParticle(p);

                    if (ParticleEndCondition(p)) {
                        p.Reset();
                    }
                }
            }

            // spawn particles
            Spawn(SpawnRate * Program.DeltaTime);
        }

        private float particlesQued = 0;
        public void Spawn(float num) {
            particlesQued += num;
            for (int i = 0; i < MyMath.Floor(particlesQued); i++) {
                Spawn();
            }
        }

        private void Spawn() {
            if (DisabledParticles.Count() > 0) {
                var p = DisabledParticles.ElementAt(0);
                p.enabled = true;
                StartParticle(p);
                particlesQued--;
            }
        }

        public override void Render() {
            Material.WhitePlastic.Apply(Program.StandardShader);
            for (int i = 0; i < particles.Count; i++) {
                var p = particles[i];
                if (!p.enabled) {
                    return;
                }
                var m = p.transform.matrix;
                if (LocalCoords) {
                    m *= gameObject.ModelMatrix;
                }
                Program.StandardShader.SetMat4("obj_transform", m);
                RenderParticle(p);
            }
        }

        protected virtual bool ParticleEndCondition(Particle p) => p.time > LifeTime;
        protected abstract void StartParticle(Particle p);
        protected abstract void UpdateParticle(Particle p);
        protected abstract void RenderParticle(Particle p);

    }

    public class PointEmission : ParticleSystem {

        private readonly IRenderable renderable;

        public PointEmission(IRenderable r, float rate, float lifeTime, bool local = true) : base(rate, lifeTime, local) {
            renderable = r;
        }

        protected override void StartParticle(Particle p) {
            p.transform.position = Random.UnitVec3() * Random.Rangef(10) + (LocalCoords ? Vector3.Zero : transform.position);
        }

        protected override void RenderParticle(Particle p) {
            renderable.Render();
        }

        protected override void UpdateParticle(Particle p) {


        }
    }

    public class GravitationalParticles : ParticleSystem {
        public GravitationalParticles(float rate, float lifeTime) : base(rate, lifeTime, false) {
        }

        protected override void RenderParticle(Particle p) {
            //Draw.Sprite();
            Draw.Point();
        }

        protected override void StartParticle(Particle p) {
            p.transform.position = transform.position + Random.Vec3(30);
            p.velocity = Random.Vec3(2);
        }

        protected override void UpdateParticle(Particle p) {
            for (int i = 0; i < EnabledParticles.Count(); i++) {
                var o = EnabledParticles.ElementAt(i);
                if (p == o) {
                    return;
                }
                p.velocity += p.transform.DirTo(o.transform).Normalized() * (1f / p.transform.DistToSq(o.transform)) * .1f;
            }
        }
    }
}
