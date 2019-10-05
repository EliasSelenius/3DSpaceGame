using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Glow;

namespace _3DSpaceGame.UI {
    public class Canvas {

        public static readonly ShaderProgram UIShader;

        static Canvas() {
            UIShader = Assets.Shaders["userInterface.glsl"];
        }

        private readonly List<Element> rootElements = new List<Element>();

        public int PixelWidth => Program.Window.Width;
        public int PixelHeight => Program.Window.Height;

        public T InitElement<T>() where T : Element, new() {
            var elm = new T();
            rootElements.Add(elm);
            return elm;
        }

        public void Render() {

            UIShader.Use();

            for (int i = 0; i < rootElements.Count; i++) {
                rootElements[i].Render();
            }            
        }

    }
}
