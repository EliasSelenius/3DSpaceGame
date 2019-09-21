using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Glow;

namespace _3DSpaceGame {
    class Sprite : Component {

        static readonly VertexArray vao;
        static readonly Buffer<float> vbo;
        static readonly Buffer<uint> ebo;

        static Sprite() {
            vbo = new Buffer<float>();
            vbo.Initialize(new float[] {
                -.5f, -.5f, 0f, 1, 0,
                -.5f,  .5f, 0f, 0, 0,
                 .5f, -.5f, 0f, 1, 1,
                 .5f,  .5f, 0f, 0, 1
            }, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

            ebo = new Buffer<uint>();
            ebo.Initialize(new uint[] {
                0, 1, 2,
                3, 2, 1
            }, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

            vao = new VertexArray();
            vao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, vbo);
            vao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ElementArrayBuffer, ebo);

            vao.AttribPointer(Program.StandardShader.GetAttribLocation("v_pos"), 3, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 5, 0);
            vao.AttribPointer(Program.StandardShader.GetAttribLocation("v_uv"), 2, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 5, sizeof(float) * 3);
        }

        public readonly Texture2D texture;

        public override void Render() {
            vao.DrawElements(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, 6, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt);
        }
    }
}
