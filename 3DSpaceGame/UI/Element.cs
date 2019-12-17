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

        public readonly RectTransform rectTransform = new RectTransform();

        public Glow.Color32bit background_color = new Glow.Color32bit(1f);
        
        public OpenTK.Vector2 size, pos;

        public float width {
            get => size.X;
            set => size.X = value;
        }
        public float height { 
            get => size.Y;
            set => size.Y = value;
        }

        public bool IsLeafNode => children.Count == 0;
        public bool IsRootNode => Parent == null;
        public bool HasParent => Parent != null;

        public bool Visible = true;
        public bool Active = true;

        public Element Hide(bool h) {
            Visible = Active = !h;
            return this;
        }

        internal void Init(Canvas c) => Canvas = c;
        private void Init(Element e) {
            Parent = e; Init(e.Canvas);
        }
        
        protected virtual void ConnectedToParent() { }
        protected virtual void OnHover() { }
        protected virtual void OnClick() {
            background_color = Random.RgbColor();
        }
        //protected virtual void Draw() { }


        private void ApplyUniforms() {



            /*
            if (HasParent) {
                p /= Parent.size;
                p += Parent.pos;
            }*/


            Canvas.UIShader.SetVec4("rectTransform", pos.X, pos.Y, width, height);
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

            if (!Active) return;

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
            pos = new OpenTK.Vector2(x, y);
            return this;
        }
        public Element Setpos(float xy) => Setpos(xy, xy);

        public Element Setsize(float w, float h) {
            width = w; height = h;
            return this;
        }

        public Element Setsize(float wh) => Setsize(wh, wh);

        public T AddChild<T>() where T : Element, new() {
            T elm = new T();
            children.Add(elm);
            elm.Init(this);

            elm.ConnectedToParent();

            return elm;
        }

    }
}
