using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nums;

namespace _3DSpaceGame.UI {
    public class Element {

        public Canvas Canvas { get; private set; }
        public Element Parent { get; private set; }
        public readonly List<Element> children = new List<Element>();
        public bool IsLeafNode => children.Count == 0;
        public bool IsRootNode => Parent == null;
        public bool HasParent => Parent != null;


        public vec2 size, pos;
        public float width {
            get => size.x;
            set => size.x = value;
        }
        public float height { 
            get => size.y;
            set => size.y = value;
        }


        public Glow.Color32bit background_color = new Glow.Color32bit(1f);        
        public bool Visible = true;
        public bool Active = true;

        private Element() { }
        
        private void Initialize(Element parent) {
            Canvas = parent.Canvas;
            Parent = parent;
        }

        internal static Element CreateElement<T>(Canvas c) where T : Element, new() {
            var e = new T();
            e.Canvas = c;
            return e;
        }

        protected virtual void ConnectedToParent() { }
        protected virtual void OnHover() { }
        protected virtual void OnClick() { }



        public T AddChild<T>() where T : Element, new() {
            T elm = new T();
            children.Add(elm);
            elm.Initialize(this);

            elm.ConnectedToParent();

            return elm;
        }

        private void ApplyUniforms() {



            /*
            if (HasParent) {
                p /= Parent.size;
                p += Parent.pos;
            }*/


            Canvas.UIShader.SetVec4("rectTransform", pos.x, pos.y, width, height);
            Canvas.UIShader.SetVec4("background_color", background_color.Red, background_color.Green, background_color.Blue, background_color.Alpha);
        }

        public void Render() {

            if (!Visible) return;

            ApplyUniforms();
            Graphics.RenderRect();

            for (int i = 0; i < children.Count; i++) {
                children[i].Render();
            }
        }

        public void UpdateEvents() {

            if (!Active && !Visible) return;

            var mp = Input.MousePos_ndc;
            var hs = size * .5f;
            if (mp.InsideBounds(pos - hs, pos + hs)) {
                this.OnHover();
                if (Input.LeftMousePressed)
                    this.OnClick();
            }

            for (int i = 0; i < children.Count; i++) {
                children[i].UpdateEvents();
            }
        }

        public Element Setpos(float x, float y) {
            pos = (x, y);
            return this;
        }
        public Element Setpos(float xy) => Setpos(xy, xy);

        public Element Setsize(float w, float h) {
            width = w; height = h;
            return this;
        }

        public Element Setsize(float wh) => Setsize(wh, wh);

        

    }
}
