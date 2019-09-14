using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    class Camera : Component {

        public static Camera MainCamera;

        public float FOV = 70;

        public float NearPlane = 1;
        public float FarPlane = 100;

        public void SetToMain() {
            MainCamera = this;
        }

        public override void Start() {
            SetToMain();
        }



        public void UpdateCamUniforms() {
            var p = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), (float)Program.Window.Width / Program.Window.Height, NearPlane, FarPlane);

            Program.ActiveShader.SetMat4("cam_projection", p);


            var lookat = Matrix4.LookAt(gameObject.Position, gameObject.Position - gameObject.Forward, gameObject.Up);
            Program.ActiveShader.SetMat4("cam_view", lookat);

            Program.ActiveShader.SetVec3("cam_pos", gameObject.Position.X, gameObject.Position.Y, gameObject.Position.Z);

        }

    }
}
