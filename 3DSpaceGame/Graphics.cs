using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

using Glow;

namespace _3DSpaceGame {
    public class Graphics {

        public static ShaderProgram QuadShader;

        private static VertexArray screenQuad;
        private static Buffer<float> vbo;
        private static Buffer<uint> ebo;

        static Graphics() {

            QuadShader = Assets.Shaders["screenquad"];

            screenQuad = new VertexArray();

            vbo = new Buffer<float>();
            vbo.Initialize(new[] {
                -1f, -1f, 0, 0,
                 1f, -1f, 1, 0,
                -1f,  1f, 0, 1,
                 1f,  1f, 1, 1,
            }, BufferUsageHint.StaticDraw);

            ebo = new Buffer<uint>();
            ebo.Initialize(new uint[] {
                0, 1, 2,
                2, 1, 3
            }, BufferUsageHint.StaticDraw);

            screenQuad.SetBuffer(BufferTarget.ArrayBuffer, vbo);
            screenQuad.SetBuffer(BufferTarget.ElementArrayBuffer, ebo);

            screenQuad.AttribPointer(QuadShader.GetAttribLocation("vPos"), 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
            screenQuad.AttribPointer(QuadShader.GetAttribLocation("texCoords"), 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, sizeof(float) * 2);
        }


        private readonly int fbo, rbo, texture;

        public Graphics() {
            // gen framebuffer
            fbo = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);

            // init texture
            texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Program.Window.Width, Program.Window.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture, 0);

            // init renderbuffer (depth & stencil)
            rbo = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, Program.Window.Width, Program.Window.Height);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo);

            
            var e = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (e != FramebufferErrorCode.FramebufferComplete) {
                Console.WriteLine("FRAMEBUFFER ERROR: " + e);
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Render() {
            QuadShader.Use();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            screenQuad.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt);
        }

        public void Bind() {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
        }

        public static void BindDefault() {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        
        public void Delete() {
            GL.DeleteFramebuffer(fbo);
            GL.DeleteTexture(texture);
            GL.DeleteRenderbuffer(rbo);
        }

    }
}
