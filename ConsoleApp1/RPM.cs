using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BasicESP
{
    class RPM
    {
        private static IntPtr pHandle = IntPtr.Zero;

        public static bool IsValid(UInt64 Address)
        {
            return (Address >= 0x10000 && Address < 0x000F000000000000);
        }

        public static IntPtr OpenProcess(int pId)
        {
            pHandle = NativeMethods.OpenProcess(NativeMethods.PROCESS_VM_READ | NativeMethods.PROCESS_VM_WRITE | NativeMethods.PROCESS_VM_OPERATION, false, pId);

            return pHandle;
        }

        public static IntPtr GetHandle()
        {
            return pHandle;
        }

        public static void CloseProcess()
        {
            NativeMethods.CloseHandle(pHandle);
        }

        public static T Read<T>(UInt64 address)
        {
            byte[] Buffer = new byte[Marshal.SizeOf(typeof(T))];
            NativeMethods.ReadProcessMemory(pHandle, address, Buffer, (uint)Buffer.Length, out IntPtr ByteRead);

            GCHandle handle = GCHandle.Alloc(Buffer, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return stuff;
        }

        public static bool Write<T>(UInt64 address, T t)
        {
            Byte[] Buffer = new Byte[Marshal.SizeOf(typeof(T))];
            GCHandle handle = GCHandle.Alloc(t, GCHandleType.Pinned);
            Marshal.Copy(handle.AddrOfPinnedObject(), Buffer, 0, Buffer.Length);
            handle.Free();

            NativeMethods.VirtualProtectEx(pHandle, (IntPtr)address, (uint)Buffer.Length, NativeMethods.PAGE_READWRITE, out uint oldProtect);
            return NativeMethods.WriteProcessMemory(pHandle, address, Buffer, (uint)Buffer.Length, out IntPtr ptrBytesWritten);
        }

        public static string ReadString(UInt64 address, UInt64 _Size)
        {
            byte[] buffer = new byte[_Size];

            NativeMethods.ReadProcessMemory(pHandle, address, buffer, _Size, out IntPtr BytesRead);

            var nullIndex = Array.IndexOf(buffer, (byte)0);
            nullIndex = (nullIndex == -1) ? (int)_Size : nullIndex;
            return Encoding.ASCII.GetString(buffer, 0, nullIndex);
        }
    }
}
