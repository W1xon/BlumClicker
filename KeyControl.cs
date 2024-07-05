using System;
using System.Runtime.InteropServices;

namespace BlumClickWinForm
{
    internal class KeyControl
    {
        [DllImport("user32.dll")]
        private static extern int GetAsyncKeyState(Int32 i);

        private static int controlKey = 17; 
        public static bool IsActiveCTRL()
        {
            return GetAsyncKeyState(controlKey) != 0;
        }
    }
}
