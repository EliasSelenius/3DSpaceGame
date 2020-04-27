using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nums;

namespace _3DSpaceGame {
    public static class Shapes {

        public static Mesh GenCube() {
            var m = new Mesh();
            return m;
        }


        public static Mesh GenIcosphere() {
            const float X = .525731112119133606f;
            const float Z = .850650808352039932f;
            const float N = 0f;
            var verts = new List<vec3> {
                new vec3(-X,N,Z), new vec3(X,N,Z), new vec3(-X,N,-Z), new vec3(X,N,-Z),
                new vec3(N,Z,X), new vec3(N,Z,-X), new vec3(N,-Z,X), new vec3(N,-Z,-X),
                new vec3(Z,X,N), new vec3(-Z,X, N), new vec3(Z,-X,N), new vec3(-Z,-X, N)
            };

            var ind = new uint[] {
                1,4,0 ,4,9,0 ,4,5,9 ,8,5,4 ,1,8,4 ,
                1,10,8 ,10,3,8 ,8,3,5 ,3,2,5 ,3,7,2 ,
                3,10,7 ,10,6,7 ,6,11,7 ,6,0,11 ,6,1,0 ,
                10,1,6 ,11,0,9 ,2,11,9 ,5,2,9 ,11,2,7 
            };

            /*
             * 
            var ind = new uint[] {
                0,4,1,0,9,4,9,5,4,4,5,8,4,8,1,
                8,10,1,8,3,10,5,3,8,5,2,3,2,7,3,
                7,10,3,7,6,10,7,11,6,11,0,6,0,1,6,
                6,1,10,9,0,11,9,11,2,9,2,5,7,2,11
            };

             */

            var m = new Mesh(verts.Select(x => new Vertex(x, vec2.zero, vec3.unity)), ind);
            m.GenNormals();
            return m;
        }

    }
}
