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
            UIShader = Assets.Shaders["ui"];
        }

        public readonly List<Element> rootElements = new List<Element>();

        public int PixelWidth => Program.Window.Width;
        public int PixelHeight => Program.Window.Height;

        public Element CreateElement<T>() where T : Element, new() {
            var e = Element.CreateElement<T>(this);
            rootElements.Add(e);
            return e;
        }

        public void Render() {

            UIShader.Use();

            for (int i = 0; i < rootElements.Count; i++) {
                rootElements[i].Render();
            }            
        }

        public void Update() {
            for (int i = 0; i < rootElements.Count; i++) {
                rootElements[i].UpdateEvents();
            }
        }

    }
}
