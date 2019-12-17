using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    public static class Random {

        private static readonly System.Random random = new System.Random();

        public static double Next() => random.NextDouble();
        public static float Nextf() => (float)random.NextDouble();

        public static double Range(double max) => random.NextDouble() * max;
        public static float Rangef(float max) => (float)random.NextDouble() * max;

        public static double Range(double min, double max) => min + random.NextDouble() * (max - min);
        public static float Rangef(float min, float max) => min + (float)random.NextDouble() * (max - min);

        public static Vector3 Vec3(float mag) => UnitVec3() * Rangef(mag);
        public static Vector3 Vec3(float minmag, float maxmag) => UnitVec3() * Rangef(minmag, maxmag);

        public static Glow.Color32bit RgbColor() => new Glow.Color32bit(Nextf(), Nextf(), Nextf(), 1f);
        public static Glow.Color32bit RgbaColor() => new Glow.Color32bit(Nextf(), Nextf(), Nextf(), Nextf());
        

        public static Vector3 UnitVec3() {
            return new Vector3 {
                X = Rangef(-1f, 1f),
                Y = Rangef(-1f, 1f),
                Z = Rangef(-1f, 1f),
            }.Normalized();
        }

    }
}
