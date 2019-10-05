using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame.UI {
    public abstract class Element {

        public Canvas Canvas { get; private set; }
        public Element Parent { get; private set; }

        public readonly RectTransform rectTransform = new RectTransform();

        public bool IsLeafNode => this is ParentElement o ? o.children.Count == 0 : true;
        public bool IsRootNode => Parent == null;

        public Element() {
            Init(Program.canvas);
        }
        public Element(Element parent) {
            Init(Program.canvas, parent);
        }

        private void Init(Canvas c, Element p = null) {
            Canvas = c;
            Parent = p;

        }

        public virtual void OnHover() { }
        public virtual void OnClick() { }
        public virtual void Draw() { }

        public void Render() {
            rectTransform.ApplyTo(Canvas.UIShader);
            Draw();
        }

        

    }
}
