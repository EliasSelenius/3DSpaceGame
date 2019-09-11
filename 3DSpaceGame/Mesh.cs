using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using Glow;
using System.Runtime.InteropServices;

namespace _3DSpaceGame {

    [StructLayout(LayoutKind.Sequential)]
    struct Vertex {
        public Vector3 pos;
        public Vector2 uv;
        public Vector3 normal;

        public Vertex(Vector3 p, Vector2 u, Vector3 n) {
            pos = p; uv = u; normal = n;
        }
    }

    class Mesh {
        public readonly List<Vertex> vertices = new List<Vertex>();
        public readonly List<uint> indices = new List<uint>();

        public void AddVertex(Vector3 p, Vector2 u, Vector3 n) => vertices.Add(new Vertex(p, u, n));

        public void AddTriangle(uint a, uint b, uint c) {
            indices.Add(a);
            indices.Add(b);
            indices.Add(c);
        }

        public VertexArray ToVAO() {
            var vao = new VertexArray();

            var vbo = new Buffer<Vertex>();
            vbo.Initialize(vertices.ToArray(), OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

            var ebo = new Buffer<uint>();
            ebo.Initialize(indices.ToArray(), OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

            vao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, vbo);
            vao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ElementArrayBuffer, ebo);

            vao.AttribPointer(Program.ActiveShader.GetAttribLocation("v_pos"), 3, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 8, 0);
            vao.AttribPointer(Program.ActiveShader.GetAttribLocation("v_uv"), 2, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 8, sizeof(float) * 3);

            return vao;
        }

    }
}
