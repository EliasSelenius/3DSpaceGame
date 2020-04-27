using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame {
    class IcoSphereSubdivisonTestComp : Component {

        public override void Start() {
            var m = gameObject.GetComp<MeshRenderer>().mesh;

            Console.WriteLine("(Icosphere) Before subdivision: " + m.Triangles.Length + " triangles, " + m.vertices.Count + " vertices");
            m.Subdivide(2);

            for (int i = 0; i < m.vertices.Count; i++) {
                var v = m.vertices[i];
                v.pos = v.pos.normalized;

                m.vertices[i] = v;
            }
            m.Apply();
            Console.WriteLine("(Icosphere) After subdivision: " + m.Triangles.Length + " triangles, " + m.vertices.Count + " vertices");
        }

        public override void OnCollision(Physics.collision collision) {
            var m = gameObject.GetComp<MeshRenderer>().mesh;
            Console.WriteLine("daw");
            //m.vertices.RemoveAt(0);
            //m.Apply();
        }

    }
}
