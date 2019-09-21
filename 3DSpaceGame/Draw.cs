using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Glow;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace _3DSpaceGame {
    public static class Draw {

        private static VertexArray lineVao;
        private static Buffer<float> lineVbo;
        private static Buffer<uint> lineEbo;

        private static ShaderProgram shader;

        public static void Initialize() {

            shader = Assets.Shaders["debugdraw.glsl"];

            shader.Use();

            lineVao = new VertexArray();
            lineVbo = new Buffer<float>();
            lineVbo.Initialize(new float[] { 0, 0, 0, 0, 0, 1 }, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);
            lineVao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, lineVbo);
            lineEbo = new Buffer<uint>();
            lineEbo.Initialize(new uint[] { 0, 1 }, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);
            lineVao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ElementArrayBuffer, lineEbo);
            lineVao.AttribPointer(shader.GetAttribLocation("pos"), 3, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);

            //Console.WriteLine(GL.GetError());

            //test = new Mesh(new[] {
            //    new Vertex(Vector3.Zero, Vector2.Zero, Vector3.UnitZ),
            //    new Vertex(Vector3.UnitX, Vector2.UnitX, Vector3.UnitZ),
            //    new Vertex(Vector3.UnitY, Vector2.UnitY, Vector3.UnitZ)
            //}, new uint[] {
            //    0, 1, 2
            //});
            //test.Init();
        }

        //private static readonly Mesh test;

        public static void Line(Vector3 p, Vector3 dir) {
            shader.Use();
            //Camera.MainCamera.UpdateCamUniforms(shader);
            lineVao.DrawElements(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, 2, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt);
            //Program.StandardShader.SetMat4("obj_transform", Matrix4.Identity);
            //Material.Emerald.Apply();
            //test.Render();

        }

    }
}
