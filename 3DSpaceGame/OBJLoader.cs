using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {

    class OBJVertex {
        public int PositionIndex;
        public int NormalIndex;
        public int UVIndex;

        public OBJVertex(int pos, int ni = 0, int uvi = 0) {
            PositionIndex = pos;
            NormalIndex = ni;
            UVIndex = uvi;
        }
    }

    class OBJFace {
        public readonly OBJVertex[] vertices;
        public OBJFace(IEnumerable<OBJVertex> verts) {
            vertices = verts.ToArray();
        }
    }

    class OBJ {
        public readonly List<Vector3> Vertices = new List<Vector3>();
        public readonly List<Vector3> Normals = new List<Vector3>();
        public readonly List<Vector2> UVs = new List<Vector2>();

        public readonly List<OBJFace> Faces = new List<OBJFace>();
    }

    static class OBJLoader {

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
                        Console.WriteLine("(OBJ parser) problem parsing vertex position, at line " + (i + 1));
                    }

                } else if (LineMatch(line, "vt", out p)) {
                    // vertex UV

                    if (ParseFloats(p, 2, out float[] o)) {
                        res.UVs.Add(new Vector2(o[0], o[1]));
                    } else {
                        Console.WriteLine("(OBJ parser) problem parsing vertex texture cordinates, at line " + (i + 1));
                    }

                } else if (LineMatch(line, "vn", out p)) {
                    // vertex normal

                    if (ParseFloats(p, 3, out float[] o)) {
                        res.Normals.Add(new Vector3(o[0], o[1], o[2]));
                    } else {
                        Console.WriteLine("(OBJ parser) problem parsing vertex normal vector, at line " + (i + 1));
                    }

                } else if (LineMatch(line, "f", out p)) {
                    // face
                } else {
                    // unrecognized
                    Console.WriteLine("(OBJ parser) did not recognize: " + line + " at line " + (i + 1));
                }

                //Console.WriteLine(p);

            }

            return res;
        }

        private static bool ParseObjVertex(string str, out OBJVertex objvertex) {

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


    }
}
