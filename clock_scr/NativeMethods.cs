using System;
using System.Runtime.InteropServices;

namespace clock_scr
{
    internal static class NativeMethods
    {
        internal const int WS_CHILD = 0x40000000;
        internal const int WS_VISIBLE = 0x10000000;
        internal const int WS_CLIPSIBLINGS = 0x04000000;
        internal const int WS_CLIPCHILDREN = 0x02000000;

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
    }
}