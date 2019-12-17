using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame.UI {
    public class RectTransform {

        /// <summary>
        /// Normalized device coordinate, where the dimmensions of the parrent is 
        /// </summary>
        public float ndc_width, ndc_height, ndc_x, ndc_y;


            /*
        public IUnit width = Unit.One;
        public IUnit height = Unit.One;

        public IUnit x_pos = Unit.Default;
        public IUnit y_pos = Unit.Default;
        */

        public void ApplyTo(Glow.ShaderProgram shader, RectTransform parent) {


            shader.SetVec4("rectTransform", ndc_x, ndc_y, ndc_width, ndc_height);
        }

    }
}
