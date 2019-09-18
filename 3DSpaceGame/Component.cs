using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame {
    public abstract class Component {


        // nameing rule violation
#pragma warning disable IDE1006
        public GameObject gameObject { get; private set; }
        public Transform transform => gameObject.transform;
#pragma warning restore IDE1006


        public void Init(GameObject g) {
            gameObject = g;
            if (gameObject.HasStarted) {
                Start();
            }
        }

        // when gameObject leaves the scene
        public virtual void OnLeave() { }
        // when gameObject enters a scene
        public virtual void OnEnter() { }
        // after gameObject has Initialized, if the component is added to an already initialized gameObject it will run imidiatly
        public virtual void Start() { }
        // the game loop
        public virtual void EarlyUpdate() { }
        public virtual void Update() { }
        // the render loop
        public virtual void Render() { }

    }
}
