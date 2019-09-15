﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame {

    public class MeshRenderer : Component {

        public readonly Mesh mesh;
        public Material material;

        public MeshRenderer(Mesh m, Material mat) {
            mesh = m;
            material = mat;
        }

        public override void Render() {
            material.Apply();
            mesh.Render();
        }

        public override void Start() {
            mesh.Init();
        }
    }
}