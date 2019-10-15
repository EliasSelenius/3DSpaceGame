using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using Glow;
using JsonParser;

using OpenTK.Graphics.OpenGL4;

namespace _3DSpaceGame {
    public static class Assets {

        private static DirectoryInfo CurrentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        private static FileInfo[] GetFiles(string p) => CurrentDir.GetFiles(p, SearchOption.AllDirectories);

        public static readonly Dictionary<string, OBJ> OBJs = new Dictionary<string, OBJ>();
        public static readonly Dictionary<string, ShaderProgram> Shaders = new Dictionary<string, ShaderProgram>();
        public static readonly Dictionary<string, string> ShaderSourceFiles = new Dictionary<string, string>();
        public static readonly Dictionary<string, Prefab> Prefabs = new Dictionary<string, Prefab>();

        public static void Load() {
            LoadObjs();

            LoadShaderSources();
            ProcessShaders();
            CompileShadersFromConfig();

            LoadPrefabs();

            //LoadShader("data/shaders/debugdraw.glsl", "debugdraw.glsl");
            //LoadShader("data/shaders/userInterface.glsl", "userInterface.glsl");
        }

        private static void LoadPrefabs() {

        }

        private static void CompileShadersFromConfig() {
            var j = Json.FromFile("data/shaders/ShadersConfig.json") as JArray;
            foreach (var item in j) {
                var sc = item as JObject;
                var fs = new Shader(ShaderType.FragmentShader, ShaderSourceFiles[sc["fragsrc"] as JString]);
                var vs = new Shader(ShaderType.VertexShader, ShaderSourceFiles[sc["vertsrc"] as JString]);
                Shaders.Add(sc["name"] as JString, new ShaderProgram(fs, vs));
                fs.Dispose();
                vs.Dispose();
            }
            Log("Finnished compiling shaders");
        }

        private static void LoadShaderSources() {
            foreach (var item in GetFiles("data/shaders/*.glsl")) {
                ShaderSourceFiles.Add(item.Name, File.ReadAllText(item.FullName));
            }
            Log("Finnished loading shader source code");
        }

        private static void ProcessShaders() {
            var rgx = new Regex("#include +\"(?<filename>[a-zA-Z._]+)\"");
            for (int i = 0; i < ShaderSourceFiles.Count; i++) {
                var item = ShaderSourceFiles.ElementAt(i);
                var m = rgx.Matches(item.Value);
                for (int j = 0; j < m.Count; j++) {
                    var match = m[j];
                    ShaderSourceFiles[item.Key] = item.Value.Replace(match.Value, ShaderSourceFiles[match.Groups["filename"].Value]);
                }
            }
            Log("Finnished processing shader source code");
        }


        private static void LoadObjs() {
            foreach (var item in GetFiles("*.obj")) {
                Log("loading OBJ file: " + item.Name);
                OBJs.Add(item.Name, OBJ.LoadFile(item.FullName));
            }
        }

        private static void Log(string msg) {
            Console.WriteLine($"[Assets] {msg}");
        }

    }
}
