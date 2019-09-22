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
        public static ShaderProgram StandardShader;

        public static float DeltaTime;

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
            GL.Enable(EnableCap.CullFace);

            var f = new Shader(ShaderType.FragmentShader, System.IO.File.ReadAllText("data/shaders/frag.glsl"));
            var v = new Shader(ShaderType.VertexShader, System.IO.File.ReadAllText("data/shaders/vert.glsl"));
            StandardShader = new ShaderProgram(f, v);
            f.Dispose();
            v.Dispose();

            Assets.Load();

            //Draw.Initialize();

            scene = new Scene();

            var cam = scene.InitObject(new Camera());
            cam.transform.position.Z += 3;

            var ship = scene.InitObject(new MeshRenderer(Assets.OBJs["StarterShip.obj"].GenMesh(), Material.Brass),
                             new PhysicsBody());
            ship.GetComp<PhysicsBody>().AddForce(0, 10, -10);
            ship.transform.Rotate(new Vector3(3.14f / 4f, 3.14f / 4f, 3.14f / 4f));

            var frog = scene.InitObject(new MeshRenderer(Assets.OBJs["TheFrog.obj"].GenMesh(), Material.Silver),
                                    new PlayerShipController(),
                                    new PhysicsBody());
            frog.transform.position.Z = 10;
            //frog.transform.Rotate(Vector3.UnitY * MyMath.pi);
            //cam.parent = ship;

            var station = scene.InitObject(new MeshRenderer(Assets.OBJs["ClockWork.obj"].GenMesh(), new Material { ambient = new Vector3(.5f,0,0), diffuse = new Vector3(0, 1, 0), specular = new Vector3(1,1,1), shininess = .5f}));
            station.transform.position.Z = -150;
            station.transform.scale *= 15;

            ship = scene.InitObject(new MeshRenderer(Assets.OBJs["spaceCraft.obj"].GenMesh(), Material.Bronze));
            ship.transform.position = Vector3.One * 20;
            ship = scene.InitObject(new MeshRenderer(Assets.OBJs["SpaceShip.obj"].GenMesh(), Material.Chrome));
            ship.transform.position = Vector3.UnitX * 10;


            // test dir light
            StandardShader.SetVec3("dirLight.color", 1f, 1f, 1f);
            StandardShader.SetVec3("dirLight.dir", -Vector3.One);

            Console.WriteLine(GLObject.ListInstances());

            var glerror = GL.GetError();
            if (glerror != ErrorCode.NoError) {
                Console.WriteLine(glerror + " after load");
            }

            // test point light
            //ActiveShader.SetVec3("pointLight.pos", Vector3.UnitX);
            //ActiveShader.SetVec3("pointLight.color", 1, 1, 1);
        }

        private static void Window_UpdateFrame(object sender, FrameEventArgs e) {
            DeltaTime = (float)e.Time;
            scene.Update();
            Input.Update();
        }


        private static void Window_RenderFrame(object sender, FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Draw.Line(Vector3.Zero, Vector3.One);

            StandardShader.Use();
            Camera.MainCamera.UpdateCamUniforms(StandardShader);
            scene.Render();



            GL.Flush();
            Window.SwapBuffers();
        }

        private static void Window_Resize(object sender, EventArgs e) {
            GL.Viewport(0, 0, Window.Width, Window.Height);
        }
    }
}
