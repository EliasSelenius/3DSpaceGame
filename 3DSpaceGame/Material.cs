using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    public struct Material {
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;
        public float shininess;

        public void Apply() {
            Program.StandardShader.SetVec3("material.ambient", ambient);
            Program.StandardShader.SetVec3("material.diffuse", diffuse);
            Program.StandardShader.SetVec3("material.specular", specular);
            Program.StandardShader.SetFloat("material.shininess", shininess);
        }


        #region Static materials
        public static Material Emerald => new Material {
            ambient = new Vector3(0.0215f, 0.1745f, 0.0215f),
            diffuse = new Vector3(0.07568f, 0.61424f, 0.07568f),
            specular = new Vector3(0.633f, 0.727811f, 0.633f),
            shininess = 0.6f
        };
        public static Material Jade => new Material {
            ambient = new Vector3(0.135f, 0.2225f, 0.1575f),
            diffuse = new Vector3(0.54f, 0.89f, 0.63f),
            specular = new Vector3(0.316228f, 0.316228f, 0.316228f),
            shininess = 0.1f
        };
        public static Material Obsidian => new Material {
            ambient = new Vector3(0.05375f, 0.05f, 0.06625f),
            diffuse = new Vector3(0.18275f, 0.17f, 0.22525f),
            specular = new Vector3(0.332741f, 0.328634f, 0.346435f),
            shininess = 0.3f
        };
        public static Material Pearl => new Material {
            ambient = new Vector3(0.25f, 0.20725f, 0.20725f),
            diffuse = new Vector3(1f, 0.829f, 0.829f),
            specular = new Vector3(0.296648f, 0.296648f, 0.296648f),
            shininess = 0.088f
        };
        public static Material Ruby => new Material {
            ambient = new Vector3(0.1745f, 0.01175f, 0.01175f),
            diffuse = new Vector3(0.61424f, 0.04136f, 0.04136f),
            specular = new Vector3(0.727811f, 0.626959f, 0.626959f),
            shininess = 0.6f
        };
        public static Material Turquoise => new Material {
            ambient = new Vector3(0.1f, 0.18725f, 0.1745f),
            diffuse = new Vector3(0.396f, 0.74151f, 0.69102f),
            specular = new Vector3(0.297254f, 0.30829f, 0.306678f),
            shininess = 0.1f
        };
        public static Material Brass => new Material {
            ambient = new Vector3(0.329412f, 0.223529f, 0.027451f),
            diffuse = new Vector3(0.780392f, 0.568627f, 0.113725f),
            specular = new Vector3(0.992157f, 0.941176f, 0.807843f),
            shininess = 0.2179487f
        };
        public static Material Bronze => new Material {
            ambient = new Vector3(0.2125f, 0.1275f, 0.054f),
            diffuse = new Vector3(0.714f, 0.4284f, 0.18144f),
            specular = new Vector3(0.393548f, 0.271906f, 0.166721f),
            shininess = 0.2f
        };
        public static Material Chrome => new Material {
            ambient = new Vector3(0.25f, 0.25f, 0.25f),
            diffuse = new Vector3(0.4f, 0.4f, 0.4f),
            specular = new Vector3(0.774597f, 0.774597f, 0.774597f),
            shininess = 0.6f
        };
        public static Material Copper => new Material {
            ambient = new Vector3(0.19125f, 0.0735f, 0.0225f),
            diffuse = new Vector3(0.7038f, 0.27048f, 0.0828f),
            specular = new Vector3(0.256777f, 0.137622f, 0.086014f),
            shininess = 0.1f
        };
        public static Material Gold => new Material {
            ambient = new Vector3(0.24725f, 0.1995f, 0.0745f),
            diffuse = new Vector3(0.75164f, 0.60648f, 0.22648f),
            specular = new Vector3(0.628281f, 0.555802f, 0.366065f),
            shininess = 0.4f
        };
        public static Material Silver => new Material {
            ambient = new Vector3(0.19225f, 0.19225f, 0.19225f),
            diffuse = new Vector3(0.50754f, 0.50754f, 0.50754f),
            specular = new Vector3(0.508273f, 0.508273f, 0.508273f),
            shininess = 0.4f
        };
        public static Material BlackPlastic => new Material {
            ambient = new Vector3(0f, 0f, 0f),
            diffuse = new Vector3(0.01f, 0.01f, 0.01f),
            specular = new Vector3(0.5f, 0.5f, 0.5f),
            shininess = 0.25f
        };
        public static Material CyanPlastic => new Material {
            ambient = new Vector3(0f, 0.1f, 0.06f),
            diffuse = new Vector3(0f, 0.5098039f, 0.5098039f),
            specular = new Vector3(0.5019608f, 0.5019608f, 0.5019608f),
            shininess = 0.25f
        };
        public static Material GreenPlastic => new Material {
            ambient = new Vector3(0f, 0f, 0f),
            diffuse = new Vector3(0.1f, 0.35f, 0.1f),
            specular = new Vector3(0.45f, 0.55f, 0.45f),
            shininess = 0.25f
        };
        public static Material RedPlastic => new Material {
            ambient = new Vector3(0f, 0f, 0f),
            diffuse = new Vector3(0.5f, 0f, 0f),
            specular = new Vector3(0.7f, 0.6f, 0.6f),
            shininess = 0.25f
        };
        public static Material WhitePlastic => new Material {
            ambient = new Vector3(0f, 0f, 0f),
            diffuse = new Vector3(0.55f, 0.55f, 0.55f),
            specular = new Vector3(0.7f, 0.7f, 0.7f),
            shininess = 0.25f
        };
        public static Material YellowPlastic => new Material {
            ambient = new Vector3(0f, 0f, 0f),
            diffuse = new Vector3(0.5f, 0.5f, 0f),
            specular = new Vector3(0.6f, 0.6f, 0.5f),
            shininess = 0.25f
        };
        public static Material BlackRubber => new Material {
            ambient = new Vector3(0.02f, 0.02f, 0.02f),
            diffuse = new Vector3(0.01f, 0.01f, 0.01f),
            specular = new Vector3(0.4f, 0.4f, 0.4f),
            shininess = 0.078125f
        };
        public static Material CyanRubber => new Material {
            ambient = new Vector3(0f, 0.05f, 0.05f),
            diffuse = new Vector3(0.4f, 0.5f, 0.5f),
            specular = new Vector3(0.04f, 0.7f, 0.7f),
            shininess = 0.078125f
        };
        public static Material GreenRubber => new Material {
            ambient = new Vector3(0f, 0.05f, 0f),
            diffuse = new Vector3(0.4f, 0.5f, 0.4f),
            specular = new Vector3(0.04f, 0.7f, 0.04f),
            shininess = 0.078125f
        };
        public static Material RedRubber => new Material {
            ambient = new Vector3(0.05f, 0f, 0f),
            diffuse = new Vector3(0.5f, 0.4f, 0.4f),
            specular = new Vector3(0.7f, 0.04f, 0.04f),
            shininess = 0.078125f
        };
        public static Material WhiteRubber => new Material {
            ambient = new Vector3(0.05f, 0.05f, 0.05f),
            diffuse = new Vector3(0.5f, 0.5f, 0.5f),
            specular = new Vector3(0.7f, 0.7f, 0.7f),
            shininess = 0.078125f
        };
        public static Material YellowRubber => new Material {
            ambient = new Vector3(0.05f, 0.05f, 0f),
            diffuse = new Vector3(0.5f, 0.5f, 0.4f),
            specular = new Vector3(0.7f, 0.7f, 0.04f),
            shininess = 0.078125f
        };
        #endregion

    }
}
