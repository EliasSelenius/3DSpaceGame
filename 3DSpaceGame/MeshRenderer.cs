using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame {

    public class MeshRenderer : Component {

        public readonly Mesh mesh;

        public MeshRenderer(Mesh m) {
            mesh = m;
        }

        public override void Render() {
            mesh.Render();
        }

        public override void Start() {
            mesh.Init();
        }
    }
}
