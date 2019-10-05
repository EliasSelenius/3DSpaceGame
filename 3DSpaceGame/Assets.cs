using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Glow;

using OpenTK.Graphics.OpenGL4;

namespace _3DSpaceGame {
    public static class Assets {

        public static readonly Dictionary<string, OBJ> OBJs = new Dictionary<string, OBJ>();
        public static readonly Dictionary<string, ShaderProgram> Shaders = new Dictionary<string, ShaderProgram>();

        public static void Load() {
            LoadObjs();
            //LoadShaders();

            //LoadShader("data/shaders/debugdraw.glsl", "debugdraw.glsl");
            LoadShader("data/shaders/userInterface.glsl", "userInterface.glsl");
        }

        private static DirectoryInfo CurrentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        private static FileInfo[] GetFiles(string p) => CurrentDir.GetFiles(p, SearchOption.AllDirectories);

        private static readonly Dictionary<string, ShaderType> shaderTypes = new Dictionary<string, ShaderType> {
            {"SHADER_VERT", ShaderType.VertexShader },
            {"SHADER_FRAG", ShaderType.FragmentShader }
        };

        private static void LoadShaders() {
            foreach (var item in GetFiles("*.glsl")) {
                LoadShader(item.FullName, item.Name);
            }
        }

        private static void LoadShader(string filename, string name) {
            var src = File.ReadAllText(filename);
            var shaders = new List<Shader>();
            foreach (var shtype in shaderTypes) {
                var ssrc = src.Replace("ShaderType", shtype.Key);
                //Console.WriteLine(ssrc);
                shaders.Add(new Shader(shtype.Value, ssrc));
            }
            Shaders.Add(name, new ShaderProgram(shaders.ToArray()));

            foreach (var shader in shaders) {
                shader.Dispose();
            }
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
