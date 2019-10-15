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

        public bool IsInitialized => vao != null;

        public List<Tuple<uint, uint, uint>> Triangles {
            get {
                var res = new List<Tuple<uint, uint, uint>>();
                for (int i = 0; i < indices.Count; i += 3) {
                    res.Add(new Tuple<uint, uint, uint>(indices[i], indices[i + 1], indices[i + 2]));
                }
                return res;
            }
        }

        public Mesh() {
            vertices = new List<Vertex>();
            indices = new List<uint>();
        }

        public Mesh(IEnumerable<Vertex> verts, IEnumerable<uint> indc) {
            vertices = verts.ToList();
            indices = indc.ToList();
        }
        

        public void Init() {

            if (IsInitialized) {
                //throw new Exception("Mesh is already initialized");
                Console.WriteLine("skipping Mesh init() because it was already initialized");
                return;
            }

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

        public void Apply() {

        }

        public void Render() => Render(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles);
        public void Render(OpenTK.Graphics.OpenGL4.PrimitiveType ptype) {
            vao.DrawElements(ptype, indices.Count, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt);
        }

        public void AddVertex(Vector3 p, Vector2 u, Vector3 n) => vertices.Add(new Vertex(p, u, n));

        public void AddTriangle(uint a, uint b, uint c) {
            indices.Add(a);
            indices.Add(b);
            indices.Add(c);
        }


        public Vector3 GenNormal(Tuple<uint, uint, uint> face) => GenNormal(face.Item1, face.Item2, face.Item3);
        public Vector3 GenNormal(uint a, uint b, uint c) => GenNormal((int)a, (int)b, (int)c);
        public Vector3 GenNormal(int a, int b, int c) => GenNormal(vertices[a], vertices[b], vertices[c]);

        public Vector3 GenNormal(Vertex a, Vertex b, Vertex c) {
            var dir1 = a.pos - c.pos;
            var dir2 = b.pos - c.pos;
            return Vector3.Cross(dir1, dir2);
        }

    }


    
}
