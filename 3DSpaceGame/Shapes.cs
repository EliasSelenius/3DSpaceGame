﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

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
            var verts = new List<Vector3> {
                new Vector3(-X,N,Z), new Vector3(X,N,Z), new Vector3(-X,N,-Z), new Vector3(X,N,-Z),
                new Vector3(N,Z,X), new Vector3(N,Z,-X), new Vector3(N,-Z,X), new Vector3(N,-Z,-X),
                new Vector3(Z,X,N), new Vector3(-Z,X, N), new Vector3(Z,-X,N), new Vector3(-Z,-X, N)
            };

            var ind = new uint[] {
                0,4,1,0,9,4,9,5,4,4,5,8,4,8,1,
                8,10,1,8,3,10,5,3,8,5,2,3,2,7,3,
                7,10,3,7,6,10,7,11,6,11,0,6,0,1,6,
                6,1,10,9,0,11,9,11,2,9,2,5,7,2,11
            };

            var m = new Mesh(verts.Select(x => new Vertex(x, Vector2.Zero, Vector3.UnitY)), ind);
            return m;
        }

    }
}