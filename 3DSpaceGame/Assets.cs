using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace _3DSpaceGame {
    public static class Assets {

        public static readonly Dictionary<string, OBJ> OBJs = new Dictionary<string, OBJ>();

        public static void Load() {

            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            foreach (var item in dir.GetFiles("*.obj", SearchOption.AllDirectories)) {
                Log("loading OBJ file: " + item.Name);
                OBJs.Add(item.Name, OBJ.LoadFile(item.FullName));
            }

        }

        private static void Log(string msg) {
            Console.WriteLine($"[Assets] {msg}");
        }

    }
}
