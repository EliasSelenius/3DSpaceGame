using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using Glow;

namespace _3DSpaceGame {
    class Camera : Component {

        public static Camera MainCamera;

        public float FOV = 70;

        public float NearPlane = .1f;
        public float FarPlane = 500;
        
        public void SetToMain() {
            MainCamera = this;
        }

        public override void Start() {
            SetToMain();
        }



        public void UpdateCamUniforms(ShaderProgram program) {
            var p = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), (float)Program.Window.Width / Program.Window.Height, NearPlane, FarPlane);

            program.SetMat4("cam_projection", p);


            var lookat = Matrix4.LookAt(transform.position, transform.position + transform.forward, transform.up);
            program.SetMat4("cam_view", lookat);

            program.SetVec3("cam_pos", transform.position.X, transform.position.Y, transform.position.Z);

        }

    }
}
