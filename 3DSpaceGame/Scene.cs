using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {

    public class Scene {
        private readonly List<GameObject> gameObjects = new List<GameObject>();

        internal void _RemoveObject(GameObject o) {
            gameObjects.Remove(o);
        }

        internal void _AddObject(GameObject o) {
            gameObjects.Add(o);
        }

        public GameObject InitObject(params Component[] comps) {

            // create Object
            var g = new GameObject();
            g.AddComps(comps);
            g.Start();
            g.EnterScene(this);    

            return g;
        }

        public void Update() {
            for (int i = 0; i < gameObjects.Count; i++) {
                gameObjects[i].EarlyUpdate();
            }
            for (int i = 0; i < gameObjects.Count; i++) {
                gameObjects[i].Update();
            }
        }

        public void Render() {
            foreach (var item in gameObjects) {
                item.Render();
            }
        }

    }
}
