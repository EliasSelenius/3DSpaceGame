using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame.GameWorld {
    public class Sector {

        private Scene scene;


        public Sector() {

        }

        public void SetToActiveScene() {
            Program.scene = scene;
        }

        private void GenScene() {
            scene = new Scene();
        }

    }
}
