using System;
using System.Runtime.InteropServices;

namespace SteamApiUpgrade {
    internal static class Native {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr LoadLibrary(string libname);
    }
}
