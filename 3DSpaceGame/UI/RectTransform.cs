using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame.UI {
    public class RectTransform {

        public IUnit width = Unit.One;
        public IUnit height = Unit.One;

        public IUnit x_pos = Unit.Default;
        public IUnit y_pos = Unit.Default;

        public void ApplyTo(Glow.ShaderProgram shader) {
            shader.SetVec4("rectTransform", x_pos.Value, y_pos.Value, width.Value, height.Value);
        }

    }
}
