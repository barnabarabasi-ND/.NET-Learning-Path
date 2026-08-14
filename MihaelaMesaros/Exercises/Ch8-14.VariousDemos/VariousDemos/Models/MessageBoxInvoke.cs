using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VariousDemos.Models
{
    internal class MessageBoxInvoke
    {
        //import from native library
        [DllImport("user32.dll")]
        public static extern int MessageBox(IntPtr parentRef, string text, string title, uint type);
    }

}
