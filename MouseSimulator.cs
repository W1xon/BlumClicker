using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BlumClickWinForm
{
    internal class MouseSimulator
    {
        public struct MousePoint
        {
            public int X;
            public int Y;
            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out MousePoint mousePoint);
        [DllImport("user32.dll")]
        private static extern void mouse_event(int flags, int dx, int dy, int dwData, int dwExtraInfo);

        private static MousePoint GetCursorPosition()
        {
            GetCursorPos(out MousePoint currentPosition);
            return currentPosition;
        }
        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x,y);
        }
        public static void LeftButtonClick()
        {
            MousePoint position = GetCursorPosition();

            mouse_event((int)MouseEventFlags.LeftDown, position.X, position.Y, 0, 0);
            mouse_event((int)MouseEventFlags.LeftUp, position.X, position.Y, 0, 0);
        }
        public static void MouseMoveToPoint(Point position)
        {
            SetCursorPosition(position.X, position.Y);
            LeftButtonClick();
        }
    }
}
