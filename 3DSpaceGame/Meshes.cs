using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame {
    public static class Meshes {

        private static readonly Dictionary<string, Mesh> meshes = new Dictionary<string, Mesh>();

        public static Mesh GetMesh(string name) {
            if (meshes.ContainsKey(name)) {
                return meshes[name];
            }
            var m = Assets.OBJs[name + ".obj"].GenMesh();
            m.Init();
            meshes.Add(name, m);
            return m;
        }
    }
}
