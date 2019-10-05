using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame.UI {

    public static class Unit {

        public static readonly IUnit Default = new FloatUnit(0);
        public static readonly IUnit One = new FloatUnit(1);

        public static IUnit Parse(string str) {
            return null;
        }
    }   

    public interface IUnit {
        float Value { get; }
    }

    public struct FloatUnit : IUnit {
        public float Value { get; set; }

        public FloatUnit(float x) => Value = x;

        public static implicit operator FloatUnit(float x) => new FloatUnit { Value = x };
        public static implicit operator float(FloatUnit x) => x.Value;
    }

    public struct ViewUnit : IUnit {
        private float v;
        public float Value {
            get => v * 2;
            set => v = value;
        }
    }

    public struct ViewWidth : IUnit {
        private float v;
        public float Value {
            get => v * Program.canvas.PixelWidth;
            set => v = value;
        }
    }

    public struct ViewHeight : IUnit {
        private float v;
        public float Value {
            get => v * Program.canvas.PixelHeight;
            set => v = value;
        }
    }

}
