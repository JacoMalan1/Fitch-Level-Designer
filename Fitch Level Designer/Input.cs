using OpenTK.Input;
using OpenTK;
using System.Collections.Generic;

namespace Fitch_Level_Designer
{
    class Input
    {

        public static List<Key> keysDown;
        public static List<Key> keysDownLast;

        public static List<MouseButton> mouseDown;
        public static List<MouseButton> mouseDownLast;

        public static Vector2 mousePos;

        public static void Init(ref GameWindow window)
        {

            keysDown = new List<Key>();
            keysDownLast = new List<Key>();

            mousePos = Vector2.Zero;

            mouseDown = new List<MouseButton>();
            mouseDownLast = new List<MouseButton>();

            window.KeyUp += Window_KeyUp;
            window.KeyDown += Window_KeyDown;
            window.MouseDown += Window_MouseDown;
            window.MouseUp += Window_MouseUp;

        }

        public static void Update(ref GameWindow window)
        {

            keysDownLast = new List<Key>(keysDown);
            mouseDownLast = new List<MouseButton>(mouseDown);

            mousePos = new Vector2(window.Mouse.X, window.Mouse.Y);

        }

        private static void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            while (mouseDown.Contains(e.Button))
                mouseDown.Remove(e.Button);
        }

        private static void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            mouseDown.Add(e.Button);

        }

        private static void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            keysDown.Add(e.Key);
        }

        private static void Window_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            while (keysDown.Contains(e.Key))
                keysDown.Remove(e.Key);
        }

        public static bool KeyDown(Key key)
        {
            return keysDown.Contains(key);
        }
        
        public static bool KeyRelease(Key key)
        {
            return !(keysDown.Contains(key));
        }

        public static bool KeyPress(Key key)
        {
            return (!keysDown.Contains(key) && keysDownLast.Contains(key));
        }

        public static bool MouseDown(MouseButton button)
        {
            return mouseDown.Contains(button);
        }

        public static bool MouseRelease(MouseButton button)
        {
            return !(mouseDown.Contains(button));
        }

        public static bool MousePress(MouseButton button)
        {
            return (!mouseDown.Contains(button) && mouseDownLast.Contains(button));
        }

    }
}
