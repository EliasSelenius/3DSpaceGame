using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Glow;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace _3DSpaceGame {
    class Program {

        public static GameWindow Window;

        public static Scene scene;
        public static ShaderProgram ActiveShader;
        
        static void Main(string[] args) {

            Window = new GameWindow(1600, 900);

            Window.Resize += Window_Resize;
            Window.RenderFrame += Window_RenderFrame;
            Window.UpdateFrame += Window_UpdateFrame;
            Window.Load += Window_Load;

            // input events:
            Input.InitEvents();

            Window.WindowState = WindowState.Fullscreen;


            //var o = OBJ.Load(new[] {
            //    "v -0.5 -0.5 0",
            //    "v -0.5 0.5 0",
            //    "v 0.5 -0.5 0",
            //    "v 0.5 0.5 0",
            //    "vt 0 0",
            //    "vt 0 1",
            //    "vt 1 0",
            //    "vt 1 1",
            //    "f 1/3 2/1 3/4",
            //    "f 4/2 3/4 2/1"
            //});

            /*
                 -.5f, -.5f, 0f, 1, 0,
                -.5f,  .5f, 0f, 0, 0,
                .5f, -.5f, 0f, 1, 1,
                .5f,  .5f, 0f, 0, 1
             */

            Window.Run();

        }

        private static void Window_Load(object sender, EventArgs e) {
            GL.ClearColor(0, 0, 0, 1);
            GL.Enable(EnableCap.DepthTest);

            var f = new Shader(ShaderType.FragmentShader, System.IO.File.ReadAllText("data/shaders/frag.glsl"));
            var v = new Shader(ShaderType.VertexShader, System.IO.File.ReadAllText("data/shaders/vert.glsl"));
            ActiveShader = new ShaderProgram(f, v);
            f.Dispose();
            v.Dispose();

            scene = new Scene();

            var cam = scene.InitObject(new Camera(), new CamFlyController());
            cam.Position.Z += 3;
            lightObj = scene.InitObject(new Sprite());

            scene.InitObject(new MeshRenderer(OBJ.LoadFile("data/models/StarterShip.obj").GenMesh()));

            ActiveShader.SetVec3("plight.color", 1, 1, 1);

        }

        private static GameObject lightObj;
        private static float time = 0;
        private static void Window_UpdateFrame(object sender, FrameEventArgs e) {
            scene.Update();
            Input.Update();

            // test point light:
            time += (float)e.Time;
            lightObj.Position.X = (float)Math.Cos(time) * 10;
            lightObj.Position.Z = (float)Math.Sin(time) * 10;
            ActiveShader.SetVec3("plight.pos", lightObj.Position.X, 0, lightObj.Position.Z);

        }

        private static void Window_RenderFrame(object sender, FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            ActiveShader.Use();
            Camera.MainCamera.UpdateCamUniforms();
            scene.Render();

            GL.Flush();
            Window.SwapBuffers();
        }

        private static void Window_Resize(object sender, EventArgs e) {
            GL.Viewport(0, 0, Window.Width, Window.Height);
        }
    }
}
