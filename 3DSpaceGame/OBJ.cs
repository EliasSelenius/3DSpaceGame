using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    class OBJ {
        public readonly List<Vector3> Vertices = new List<Vector3>();
        public readonly List<Vector3> Normals = new List<Vector3>();
        public readonly List<Vector2> UVs = new List<Vector2>();

        public readonly List<Face> Faces = new List<Face>();

        public class Vertex {
            public readonly int PositionIndex;
            public readonly int UVIndex;
            public readonly int NormalIndex;

            public Vertex(int pos, int uv, int normal) {
                PositionIndex = pos;
                UVIndex = uv;
                NormalIndex = normal;
            }
        }

        public class Face {
            public readonly Vertex[] vertices;
            public Face(IEnumerable<Vertex> verts) {
                vertices = verts.ToArray();
            }
        }

        #region parsing

        public static OBJ LoadFile(string filepath) => Load(System.IO.File.ReadAllLines(filepath));

        public static OBJ Load(string[] source) {
            OBJ res = new OBJ();

            for (int i = 0; i < source.Length; i++) {
                var line = source[i];

                string p;
                if (LineMatch(line, "v", out p)) {
                    // vertex position

                    if (ParseFloats(p, 3, out float[] o)) {
                        res.Vertices.Add(new Vector3(o[0], o[1], o[2]));
                    } else {
                        //Console.WriteLine("(OBJ parser) problem parsing vertex position, at line " + (i + 1));
                        Log("problem parsing vertex position", i + 1);
                    }

                } else if (LineMatch(line, "vt", out p)) {
                    // vertex UV

                    if (ParseFloats(p, 2, out float[] o)) {
                        res.UVs.Add(new Vector2(o[0], o[1]));
                    } else {
                        //Console.WriteLine("(OBJ parser) problem parsing vertex texture cordinates, at line " + (i + 1));
                        Log("problem parsing vertex texture cordinates", i + 1);
                    }

                } else if (LineMatch(line, "vn", out p)) {
                    // vertex normal

                    if (ParseFloats(p, 3, out float[] o)) {
                        res.Normals.Add(new Vector3(o[0], o[1], o[2]));
                    } else {
                        //Console.WriteLine("(OBJ parser) problem parsing vertex normal vector, at line " + (i + 1));
                        Log("problem parsing vertex normal vector", i + 1);
                    }

                } else if (LineMatch(line, "f", out p)) {
                    // face

                    if (ParseFace(p, out OBJ.Face f)) {
                        res.Faces.Add(f);
                    } else {
                        Log("problem parsing face", i + 1);
                    }

                } else {
                    // unrecognized
                    //Console.WriteLine("(OBJ parser) did not recognize: " + line + " at line " + (i + 1));
                    Log("did not recognize: " + line, i + 1);
                }

                //Console.WriteLine(p);

            }

            return res;
        }


        private static void Log(string msg, int line) {
            Console.WriteLine($"(OBJ Parser at line {line}) {msg}");
        }

        private static bool ParseFace(string str, out OBJ.Face face) {
            var list = str.Split(' ');
            var verts = new List<OBJ.Vertex>();
            var notfail = true;
            for (int i = 0; i < list.Length && notfail; i++) {
                notfail = ParseObjVertex(list[i], out OBJ.Vertex v);
                verts.Add(v);
            }
            face = new OBJ.Face(verts);
            return notfail;
        }

        private static bool ParseObjVertex(string str, out OBJ.Vertex objvertex) {
            try {
                var nums = str.Split('/').Select(x => int.Parse(x));

                var pos = nums.ElementAt(0);
                var uv = nums.ElementAt(1);
                var normal = 0;
                if (nums.Count() == 3) {
                    normal = nums.ElementAt(2);
                }

                objvertex = new OBJ.Vertex(pos, uv, normal);
                return true;

            } catch (Exception) {
                objvertex = null;
                return false;
            }
        }

        private static bool ParseFloats(string str, int length, out float[] result) {
            var list = str.Split(' ');
            if (list.Length != length) {
                result = null;
                return false;
            }
            result = new float[length];
            for (int i = 0; i < result.Length; i++) {
                result[i] = float.Parse(list[i]);
            }
            return true;
        }

        private static bool LineMatch(string line, string start, out string parameters) {
            var s = start + " ";
            if (line.StartsWith(s)) {
                parameters = line.Substring(s.Length);
                return true;
            }
            parameters = null;
            return false;
        }

        #endregion

    }
}
