using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame {
    public class Prefab {

        private readonly Transform transform;
        private readonly Dictionary<Type, object[]> components = new Dictionary<Type, object[]>();

        public Prefab(Transform t) {
            transform = t;
        }

        public Prefab AddComp<T>(params object[] args) where T : Component {
            components.Add(typeof(T), args);
            return this;
        }

        public GameObject NewInstance() {
            var g = new GameObject();
            g.transform.position = transform.position;
            g.transform.rotation = transform.rotation;
            g.transform.scale = transform.scale;

            foreach (var item in components) {
                g.AddComp(item.Key.GetConstructor(item.Value.Select(x => x.GetType()).ToArray()).Invoke(item.Value) as Component);
            }

            g.Start();
            return g;
        }

    }
}
