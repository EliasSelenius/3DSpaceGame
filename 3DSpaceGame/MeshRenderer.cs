using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame {

    public class MeshRenderer : Component {

        public readonly Mesh mesh;
        public Material material;
        public OpenTK.Graphics.OpenGL4.PrimitiveType primitiveType;

        public MeshRenderer(Mesh m, Material mat) : this(m, mat, OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles) { }

        public MeshRenderer(Mesh m, Material mat, OpenTK.Graphics.OpenGL4.PrimitiveType ptype) {
            mesh = m;
            material = mat;
            primitiveType = ptype;
        }

        public override void Render() {
            Program.StandardShader.SetMat4("obj_transform", gameObject.ModelMatrix);
            material.Apply(Program.StandardShader);
            mesh.Render(primitiveType);
        }

        public override void Start() {
            mesh.Init();
        }
    }
}
