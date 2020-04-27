using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Nums;
using Glow;


namespace _3DSpaceGame {

    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex {
        public vec3 pos;
        public vec2 uv;
        public vec3 normal;

        public Vertex(vec3 p, vec2 u, vec3 n) {
            pos = p; uv = u; normal = n;
        }

        public Vertex Lerp(Vertex other, float time) {
            return new Vertex(pos.lerp(other.pos, time), uv.lerp(other.uv, time), normal.lerp(other.normal, time));
        }

    }

    public class Mesh : IRenderable {

        private VertexArray vao;
        private Buffer<Vertex> vbo;
        private Buffer<uint> ebo;

        public readonly List<Vertex> vertices;
        public readonly List<uint> indices;

        public bool IsInitialized => vao != null;

        public List<Tuple<uint, uint, uint>> TriangleIndices {
            get {
                var res = new List<Tuple<uint, uint, uint>>();
                for (int i = 0; i < indices.Count; i += 3) {
                    res.Add(new Tuple<uint, uint, uint>(indices[i], indices[i + 1], indices[i + 2]));
                }
                return res;
            }
        }

        public Triangle[] Triangles {
            get {
                var res = new Triangle[TriangleIndices.Count];
                for (int i = 0; i < TriangleIndices.Count; i++) {
                    res[i] = new Triangle(vertices[(int)TriangleIndices[i].Item1], vertices[(int)TriangleIndices[i].Item2], vertices[(int)TriangleIndices[i].Item3],
                        TriangleIndices[i].Item1, TriangleIndices[i].Item2, TriangleIndices[i].Item3);
                }
                return res;
            }
        }

        public struct Triangle {
            public readonly Vertex v1, v2, v3;
            public readonly uint i1, i2, i3;
            public Triangle(Vertex v1, Vertex v2, Vertex v3, uint i1, uint i2, uint i3) {
                this.v1 = v1; this.v2 = v2; this.v3 = v3;
                this.i1 = i1; this.i2 = i2; this.i3 = i3;
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
            vbo.Initialize(vertices.ToArray(), OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);
            ebo.Initialize(indices.ToArray(), OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);
        }

        public void Render() => Render(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles);
        public void Render(OpenTK.Graphics.OpenGL4.PrimitiveType ptype) {
            vao.DrawElements(ptype, indices.Count, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt);
        }

        public void RenderNormals() {
            // TODO: implement
        }

        public void Mutate(Func<Vertex, Vertex> func) {
            for (int i = 0; i < vertices.Count; i++) {
                vertices[i] = func(vertices[i]);
            }
        }

        public void AddVertex(vec3 p, vec2 u, vec3 n) => vertices.Add(new Vertex(p, u, n));

        public void AddTriangle(uint a, uint b, uint c) {
            indices.Add(a);
            indices.Add(b);
            indices.Add(c);
        }

        public void Subdivide(int subdivisions = 1) {
            for (int i = 0; i < subdivisions; i++)
                subdivide();
            //GenNormals();
        }

        private void subdivide() {
            var ts = Triangles;

            this.vertices.Clear();
            this.indices.Clear();

            void _vertex(Vertex v) {
                var i = vertices.IndexOf(v);
                if (i == -1) {
                    vertices.Add(v);
                    i = vertices.Count - 1;
                }
                indices.Add((uint)i);
            }

            for (int i = 0; i < ts.Length; i++) {
                var t = ts[i];

                /*
                  
                       t.v3
                        o
                       / \
                  vm2 o---o vm3
                     / \ / \
                    o---o---o
                 t.v1  vm1   t.v2

                 */

                Vertex vm1 = t.v1.Lerp(t.v2, .5f),
                       vm2 = t.v1.Lerp(t.v3, .5f),
                       vm3 = t.v2.Lerp(t.v3, .5f);

                // triangle 1 (lower left)
                _vertex(t.v1); _vertex(vm1); _vertex(vm2);

                // triangle 2 (middle)
                _vertex(vm1); _vertex(vm3); _vertex(vm2);

                // triangle 3 (lower right)
                _vertex(vm1); _vertex(t.v2); _vertex(vm3);

                // triangle 4 (top)
                _vertex(vm2); _vertex(vm3); _vertex(t.v3);

            }
        }

        public vec3 GenNormal(Tuple<uint, uint, uint> face) => GenNormal(face.Item1, face.Item2, face.Item3);
        public vec3 GenNormal(uint a, uint b, uint c) => GenNormal((int)a, (int)b, (int)c);
        public vec3 GenNormal(int a, int b, int c) => GenNormal(vertices[a], vertices[b], vertices[c]);

        public static vec3 GenNormal(Vertex a, Vertex b, Vertex c) {
            var dir1 = a.pos - c.pos;
            var dir2 = b.pos - c.pos;
            return OpenTK.Vector3.Cross(dir1.ToOpenTKVec(), dir2.ToOpenTKVec()).ToNumsVec();
        }

        public void FlipIndices() {
            for (int i = 0; i < indices.Count; i += 3) {
                var t = indices[i];
                indices[i] = indices[i + 2];
                indices[i + 2] = t;
            }
        }

        public void GenNormals() {
            for (int i = 0; i < vertices.Count; i++) {
                var verts = from o in TriangleIndices
                            where o.Item1 == i || o.Item2 == i || o.Item3 == i
                            select (vertices[(int)o.Item1].pos + vertices[(int)o.Item2].pos + vertices[(int)o.Item3].pos) - vertices[i].pos;

                var vert = vertices[i];
                vert.normal = (vertices[i].pos - MyMath.AvgVec(verts.ToArray())).normalized;
                vertices[i] = vert;
            }
        }

    }


    
}
