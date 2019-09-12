using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Input;

namespace _3DSpaceGame {
    static class Input {

        public static Vector2 Wasd => new Vector2(KeyAxis(Key.D, Key.A), KeyAxis(Key.W, Key.S));

        public static Vector2 MouseDelta => MousePos - (IsFixedMouse ? screenCenter : PrevMousePos);
        public static Vector2 MousePos { get; private set; }
        public static Vector2 PrevMousePos { get; private set; }

        private static KeyboardState keyboard;
        private static MouseState mouse;

        public static bool IsFixedMouse => fixedmouse && Program.Window.Focused;
        private static bool fixedmouse;

        private static Vector2 screenCenter => new Vector2(Program.Window.X + (Program.Window.Width / 2f), Program.Window.Y + (Program.Window.Height / 2f));

        public static void InitEvents() {
            Program.Window.MouseMove += Window_MouseMove;
            FixedMouse(false);
        }

        public static void FixedMouse(bool v) {
            fixedmouse = v;
            Program.Window.CursorVisible = !fixedmouse;
        }

        private static void Window_MouseMove(object sender, MouseMoveEventArgs e) {
            MousePos = new Vector2(e.X, e.Y);
            
        }

        public static void Update() {
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            PrevMousePos = MousePos;
            if (IsFixedMouse) {
                var c = screenCenter;
                Mouse.SetPosition(c.X, c.Y);
            }
        }

        public static bool IsKeyDown(Key k) {
            return keyboard.IsKeyDown(k);
        }

        public static float KeyAxis(Key a, Key b) {
            var an = keyboard.IsKeyDown(a) ? 1 : 0;
            var bn = keyboard.IsKeyDown(b) ? -1 : 0;
            return an + bn;
        }

    }
}
