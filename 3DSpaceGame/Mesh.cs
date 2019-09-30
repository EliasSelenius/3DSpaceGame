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
    public struct Vertex {
        public Vector3 pos;
        public Vector2 uv;
        public Vector3 normal;

        public Vertex(Vector3 p, Vector2 u, Vector3 n) {
            pos = p; uv = u; normal = n;
        }
    }

    public class Mesh : IRenderable {

        private VertexArray vao;
        private Buffer<Vertex> vbo;
        private Buffer<uint> ebo;

        public readonly List<Vertex> vertices;
        public readonly List<uint> indices;

        public Mesh() {
            vertices = new List<Vertex>();
            indices = new List<uint>();
        }

        public Mesh(IEnumerable<Vertex> verts, IEnumerable<uint> indc) {
            vertices = verts.ToList();
            indices = indc.ToList();
        }
        

        public void Init() {


            vao = new VertexArray();

            vbo = new Buffer<Vertex>();
            vbo.Initialize(vertices.ToArray(), OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

            ebo = new Buffer<uint>();
            ebo.Initialize(indices.ToArray(), OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

            vao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, vbo);
            vao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ElementArrayBuffer, ebo);


            vao.AttribPointer(0, 3, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 8, 0);
            vao.AttribPointer(1, 2, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 8, sizeof(float) * 3);
            vao.AttribPointer(2, 3, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 8, sizeof(float) * 5);

        }


        public void Render() {
            vao.DrawElements(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, indices.Count, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt);
        }

        public void AddVertex(Vector3 p, Vector2 u, Vector3 n) => vertices.Add(new Vertex(p, u, n));

        public void AddTriangle(uint a, uint b, uint c) {
            indices.Add(a);
            indices.Add(b);
            indices.Add(c);
        }

    }


    
}
