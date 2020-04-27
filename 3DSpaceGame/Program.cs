using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Glow;
using Nums;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

using _3DSpaceGame.Physics;

namespace _3DSpaceGame {
    class Program {

        public static GameWindow Window;

        public static Scene scene;
        public static ShaderProgram StandardShader;

        public static UI.Canvas canvas;

        public static float DeltaTime;

        public static Graphics graphics;
        public static Skybox skybox;

        private static Thread collisionDetectionThread;

        static void Main(string[] args) {

            Window = new GameWindow(1600, 900);

            Window.Resize += Window_Resize;
            Window.RenderFrame += Window_RenderFrame;
            Window.UpdateFrame += Window_UpdateFrame;
            Window.Load += Window_Load;

            // input events:
            Input.InitEvents();

            Window.WindowState = WindowState.Fullscreen;


            Window.Run();

        }

        private static void InitTestScene() {

            scene = new Scene();

            var cam = scene.InitObject(new Camera());
            cam.transform.position.z += 3;

            var ship = scene.InitObject(new MeshRenderer(Assets.OBJs["StarterShip.obj"].GenMesh(), Material.Brass),
                             new PhysicsBody());
            ship.GetComp<PhysicsBody>().AddForce(0, 10, -10);
            ship.transform.Rotate(new vec3(3.14f / 4f, 3.14f / 4f, 3.14f / 4f));

            var frogmesh = Meshes.GetMesh("TheFrog");
            var frog = scene.InitObject(new MeshRenderer(frogmesh, Material.Silver),
                                    new PlayerShipController(),
                                    new PhysicsBody(),
                                    new Particles.EngineFlames(-Vector3.UnitZ * 2.1f, .3f),
                                    new Particles.SpaceDustParticles(),
                                    new SphereCollider(2d) { offset = (0, 0, 3) },
                                    new SphereCollider(2d) { offset = (0, 0, -1) });
            frog.transform.position.z = 10;


            var stationmesh = Assets.OBJs["ClockWork.obj"].GenMesh();
            //stationmesh.FlipIndices();
            var station = scene.InitObject(new MeshRenderer(stationmesh, Material.CyanRubber), new RotateComp(vec3.unitz, .1f));
            station.transform.position.z = -150;
            station.transform.scale *= 15;
            //station.GetComp<MeshRenderer>().mesh.FlipIndices();

            ship = scene.InitObject(new MeshRenderer(Assets.OBJs["spaceCraft.obj"].GenMesh(), Material.Bronze), new PhysicsBody());
            ship.transform.position = vec3.one * 20;
            ship = scene.InitObject(new MeshRenderer(Assets.OBJs["SpaceShip.obj"].GenMesh(), Material.Chrome), new PhysicsBody());
            ship.transform.position = vec3.unitx * 10;


            var icospherePrefab = new Prefab(new Transform(new vec3(-10, 10, 10), vec3.one * 4f))
                .AddComp<MeshRenderer>(Shapes.GenIcosphere(), Material.Jade, OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles)
                .AddComp<SphereCollider>(4d)
                .AddComp<PhysicsBody>();

            int i = 0;
            foreach (var item in typeof(Material).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)) {
                var index = icospherePrefab.NewInstance();
                index.GetComp<MeshRenderer>().material = (item.GetValue(null) as Material? ?? new Material());
                index.transform.position -= vec3.unitx * 8f * i;
                index.EnterScene(scene);
                i++;
            }

            var icosphere = icospherePrefab.NewInstance();
            icosphere.AddComp(new IcoSphereSubdivisonTestComp());
            icosphere.transform.position = (40, 40, 50);
            icosphere.EnterScene(scene);



            Assets.Prefabs["sun"].NewInstance().EnterScene(scene);



            // ======= init test ui======
            canvas = new UI.Canvas();
            //var el = canvas.InitElement<UI.Element>();
            //el.Setsize(.4f).Setpos(-.5f);

        }

        private static void Window_Load(object sender, EventArgs e) {
            GL.ClearColor(.8f, .8f, .8f, 1);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.CullFace);

            // Init a generic sphere mesh
            var sphere = Shapes.GenIcosphere();
            sphere.Subdivide(2);
            sphere.Mutate(v => {
                v.pos = v.pos.normalized;
                return v;
            });
            Meshes.AddMesh("sphere", sphere);

            Assets.Load();

            StandardShader = Assets.Shaders["default"];
            graphics = new Graphics();

            skybox = new Skybox(new[] {
                Assets.Images["Skybox_left.png"],
                Assets.Images["Skybox_right.png"],
                Assets.Images["Skybox_up.png"],
                Assets.Images["Skybox_down.png"],
                Assets.Images["Skybox_front.png"],
                Assets.Images["Skybox_back.png"]
            });



            InitTestScene();
            

            // test dir light
            StandardShader.SetVec3("dirLight.color", 1f, 1f, 1f);
            StandardShader.SetVec3("dirLight.dir", -vec3.one);

            Console.WriteLine(GLObject.ListInstances());

            var glerror = GL.GetError();
            if (glerror != ErrorCode.NoError) {
                Console.WriteLine(glerror + " after load");
            }

            // test point light
            //ActiveShader.SetVec3("pointLight.pos", vec3.unitx);
            //ActiveShader.SetVec3("pointLight.color", 1, 1, 1);


            // init collision detection thread
            collisionDetectionThread = new Thread(collisionDetectionThreadStart) {
                IsBackground = true
            };
            collisionDetectionThread.Start();


            JsConsole.Start();

        }


        private static void collisionDetectionThreadStart() {
            while (true) {
                //Console.WriteLine("col loop");
                scene.CheckCollisions();
            }
        }

        private static void Window_UpdateFrame(object sender, FrameEventArgs e) {
            DeltaTime = (float)e.Time;
            scene.Update();
            canvas.Update();
            Input.Update();
        }


        private static void Window_RenderFrame(object sender, FrameEventArgs e) {

            graphics.Bind();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // scene
            GL.Enable(EnableCap.DepthTest);
            StandardShader.Use();
            Camera.MainCamera.UpdateCamUniforms(StandardShader);
            scene.Render();
            
            // skybox
            skybox.Render();

            // User interface
            GL.Disable(EnableCap.DepthTest);
            canvas.Render();


            // draw screen quad
            Graphics.BindDefault();
            graphics.Render();

            GL.Flush();
            Window.SwapBuffers();
        }

        private static void Window_Resize(object sender, EventArgs e) {
            GL.Viewport(0, 0, Window.Width, Window.Height);
        }
    }
}
