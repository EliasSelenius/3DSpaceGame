using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Jint;
using Jint.Native;
using Jint.Runtime;
using Jint.Runtime.Interop;
using System.Reflection;

namespace _3DSpaceGame {
    public static class JsConsole {

        private readonly static Thread thread;

        private readonly static Engine jsEngine;

        static JsConsole() {
            thread = new Thread(threadLoop) {
                IsBackground = true
            };

            jsEngine = new Engine(c => c.AllowClr(typeof(Program).Assembly, typeof(Nums.vec3).Assembly));
            //jsEngine.SetValue("Program", TypeReference.CreateTypeReference(jsEngine, typeof(Program)));

            jsEngine.Execute("const sg = importNamespace('_3DSpaceGame');" +
                             "const nums = importNamespace('Nums');" +
                             "const program = sg.Program;" +
                             "const vec3 = nums.vec3;" +
                             "const vec2 = nums.vec2;");

            jsEngine.SetValue("print", new Action<object>(print));

            jsEngine.Execute(System.IO.File.ReadAllText("data/scripts/console.js"));
        }

        public static void print(object obj) {
            var members = obj.GetType().GetMembers();

            foreach (var member in members) {
                Console.WriteLine(member);
            }
        }

        public static void Start() => thread.Start();

        private static void threadLoop() {
            while(true) {

                Console.Write(" > ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                try {
                    jsEngine.Execute(input);
                } catch (Exception e) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.ToString());
                    Console.ResetColor();
                    continue;
                }
            }
        }


    }
}
