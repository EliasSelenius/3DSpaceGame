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
        static readonly VertexArray spriteVao;
        static readonly Buffer<float> spriteVbo;
        static readonly Buffer<uint> spriteEbo;

        static readonly Mesh cubemesh;

        static Draw() {

            // Init Sprite
            spriteVbo = new Buffer<float>();
            spriteVbo.Initialize(new float[] {
                -.5f, -.5f, 0f, 1, 0,
                -.5f,  .5f, 0f, 0, 0,
                 .5f, -.5f, 0f, 1, 1,
                 .5f,  .5f, 0f, 0, 1
            }, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

            spriteEbo = new Buffer<uint>();
            spriteEbo.Initialize(new uint[] {
                0, 1, 2,
                3, 2, 1
            }, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

            spriteVao = new VertexArray();
            spriteVao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, spriteVbo);
            spriteVao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ElementArrayBuffer, spriteEbo);

            spriteVao.AttribPointer(Program.StandardShader.GetAttribLocation("v_pos"), 3, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 5, 0);
            spriteVao.AttribPointer(Program.StandardShader.GetAttribLocation("v_uv"), 2, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 5, sizeof(float) * 3);

            // Init cube

        }
        
        public static void Sprite() {
            spriteVao.DrawElements(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, 6, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt);
        }

        public static void Cube() {

        }

        public static void Point() {
            spriteVao.DrawElements(PrimitiveType.Points, 1, DrawElementsType.UnsignedInt);
        }
    }
}
