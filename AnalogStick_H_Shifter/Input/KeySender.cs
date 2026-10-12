using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    /// <summary>
    /// Sends keyboard events as hardware scan codes via SendInput, so games that read
    /// DirectInput/raw input receive them like real key presses.
    /// </summary>
    static class KeySender
    {
        public static void Down(Keys key) => Send(key, false);

        public static void Up(Keys key) => Send(key, true);

        /// <summary>Taps the key once and keeps it held (cleaner signal for games that wait for a fresh press).</summary>
        public static void TapAndHold(Keys key)
        {
            Down(key);
            Up(key);
            Down(key);
        }

        static void Send(Keys key, bool up)
        {
            if (key == Keys.None) return;

            uint scan = MapVirtualKey((uint)key, MAPVK_VK_TO_VSC_EX);
            if (scan == 0) return;

            uint flags = KEYEVENTF_SCANCODE;
            if ((scan & 0xFF00) == 0xE000 || (scan & 0xFF00) == 0xE100) flags |= KEYEVENTF_EXTENDEDKEY;
            if (up) flags |= KEYEVENTF_KEYUP;

            var input = new INPUT
            {
                type = INPUT_KEYBOARD,
                U = new InputUnion { ki = new KEYBDINPUT { wScan = (ushort)(scan & 0xFF), dwFlags = flags } },
            };
            SendInput(1, new[] { input }, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>Localized, human readable key name ("1", "Num 3", "Ü", ...).</summary>
        public static string GetDisplayName(Keys key)
        {
            if (key == Keys.None) return "—";
            if (key >= Keys.D0 && key <= Keys.D9) return ((char)('0' + (key - Keys.D0))).ToString();

            uint scan = MapVirtualKey((uint)key, MAPVK_VK_TO_VSC_EX);
            if (scan != 0)
            {
                int lParam = (int)((scan & 0xFF) << 16);
                if ((scan & 0xFF00) == 0xE000) lParam |= 1 << 24;

                var sb = new StringBuilder(64);
                if (GetKeyNameText(lParam, sb, sb.Capacity) > 0)
                {
                    string name = sb.ToString();
                    return name.Length == 1 ? name.ToUpperInvariant() : name;
                }
            }
            return key.ToString();
        }

        // ------------------------------------------------------------------ Win32

        const uint INPUT_KEYBOARD = 1;
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_SCANCODE = 0x0008;
        const uint MAPVK_VK_TO_VSC_EX = 4;

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int GetKeyNameText(int lParam, StringBuilder lpString, int cchSize);

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public uint type;
            public InputUnion U;
        }

        // The union must contain MOUSEINPUT (the largest member) so the struct has the size Windows expects.
        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }
    }
}
