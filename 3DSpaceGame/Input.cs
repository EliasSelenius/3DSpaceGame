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
        public static Vector2 MousePos_ndc {
            get {
                var p = new Vector2(MousePos.X / Program.Window.Width, MousePos.Y / Program.Window.Height) * 2f - Vector2.One;
                p.Y = -p.Y;
                return p;
            }
        }

        public static Vector2 PrevMousePos { get; private set; }

        public static float MouseWheelDelta { get; private set; }

        private static KeyboardState keyboard;
        private static MouseState mouse;

        public static bool IsFixedMouse => fixedmouse && Program.Window.Focused;
        private static bool fixedmouse;

        private static Vector2 screenCenter => new Vector2(Program.Window.X + (Program.Window.Width / 2f), Program.Window.Y + (Program.Window.Height / 2f));

        public static bool LeftMousePressed { get; private set; }
        public static bool RightMousePressed { get; private set; }

        public static void InitEvents() {
            Program.Window.MouseMove += Window_MouseMove;
            Program.Window.MouseWheel += Window_MouseWheel;
            Program.Window.MouseDown += Window_MouseDown;
            FixedMouse(false);
        }

        private static void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            switch (e.Button) {
                case MouseButton.Left:
                    LeftMousePressed = true;
                    break;
                case MouseButton.Middle:
                    break;
                case MouseButton.Right:
                    RightMousePressed = true;
                    break;
                case MouseButton.Button1:
                    break;
                case MouseButton.Button2:
                    break;
                case MouseButton.Button3:
                    break;
                case MouseButton.Button4:
                    break;
                case MouseButton.Button5:
                    break;
                case MouseButton.Button6:
                    break;
                case MouseButton.Button7:
                    break;
                case MouseButton.Button8:
                    break;
                case MouseButton.Button9:
                    break;
                case MouseButton.LastButton:
                    break;
                default:
                    break;
            }
        }

        private static void Window_MouseWheel(object sender, MouseWheelEventArgs e) {
            MouseWheelDelta = e.DeltaPrecise;
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
            
            // reset data:
            MouseWheelDelta = 0;
            LeftMousePressed = RightMousePressed = false;

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

        public static bool MouseLeftButtonDown => mouse.IsButtonDown(MouseButton.Left);
        public static bool MouseRightButtonDown => mouse.IsButtonDown(MouseButton.Right);

    }
}
