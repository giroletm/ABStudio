using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABStudio
{
    public static class ByteArrayExtensions
    {
        public static string GetString(this byte[] bytes, int idx) { int i = idx; return bytes.GetString(ref i); }
        public static string GetString(this byte[] bytes, ref int idx)
        {
            ushort charCount = bytes.GetUInt16(ref idx);

            string output = "";
            for (int i = 0; i < charCount; i++)
            {
                output += (char)bytes[idx + i];
            }

            idx += charCount;

            return output;
        }

        public static float GetFloat(this byte[] bytes, int idx) { int i = idx; return bytes.GetFloat(ref i); }
        public static float GetFloat(this byte[] bytes, ref int idx)
        {
            float val = System.BitConverter.ToSingle(bytes.Skip(idx).Take(4).Reverse().ToArray(), 0);
            idx += 4;
            return val;
        }

        public static long GetInt64(this byte[] bytes, int idx) { int i = idx; return bytes.GetInt64(ref i); }
        public static long GetInt64(this byte[] bytes, ref int idx)
        {
            return (long)bytes.GetUInt64(ref idx);
        }

        public static ulong GetUInt64(this byte[] bytes, int idx) { int i = idx; return bytes.GetUInt64(ref i); }
        public static ulong GetUInt64(this byte[] bytes, ref int idx)
        {
            ulong val = ((ulong)bytes[idx + 7]) | ((ulong)bytes[idx + 6] << 8) | ((ulong)bytes[idx + 5] << 16) | ((ulong)bytes[idx + 4] << 24) | ((ulong)bytes[idx + 3] << 32) | ((ulong)bytes[idx + 2] << 40) | ((ulong)bytes[idx + 1] << 48) | ((ulong)bytes[idx] << 56);
            idx += 8;
            return val;
        }

        public static int GetInt32(this byte[] bytes, int idx) { int i = idx; return bytes.GetInt32(ref i); }
        public static int GetInt32(this byte[] bytes, ref int idx)
        {
            return (int)bytes.GetUInt32(ref idx);
        }

        public static uint GetUInt32(this byte[] bytes, int idx) { int i = idx; return bytes.GetUInt32(ref i); }
        public static uint GetUInt32(this byte[] bytes, ref int idx)
        {
            uint val = ((uint)bytes[idx + 3]) | ((uint)bytes[idx + 2] << 8) | ((uint)bytes[idx + 1] << 16) | ((uint)bytes[idx] << 24);
            idx += 4;
            return val;
        }

        public static short GetInt16(this byte[] bytes, int idx) { int i = idx; return bytes.GetInt16(ref i); }
        public static short GetInt16(this byte[] bytes, ref int idx)
        {
            return (short)bytes.GetUInt16(ref idx);
        }

        public static ushort GetUInt16(this byte[] bytes, int idx) { int i = idx; return bytes.GetUInt16(ref i); }
        public static ushort GetUInt16(this byte[] bytes, ref int idx)
        {
            ushort val = (ushort)(((uint)bytes[idx + 1]) | ((uint)bytes[idx] << 8));
            idx += 2;
            return val;
        }
    }

    public static class ByteListExtensions
    {
        public static void AddString(this List<byte> bytes, string item)
        {
            bytes.AddUInt16((ushort)item.Length);

            foreach (char c in item)
            {
                bytes.Add((byte)(c & 0xFF));
            }
        }

        public static void AddFloat(this List<byte> bytes, float item)
        {
            foreach (byte b in BitConverter.GetBytes(item))
            {
                bytes.Add(b);
            }
        }

        public static void InsertFloat(this List<byte> bytes, int index, float item)
        {
            int i = index;
            foreach (byte b in BitConverter.GetBytes(item))
            {
                bytes[i] = b;
                i++;
            }
        }

        public static void AddInt64(this List<byte> bytes, long item)
        {
            bytes.AddUInt64((uint)item);
        }

        public static void InsertInt64(this List<byte> bytes, int index, long item)
        {
            bytes.InsertUInt64(index, (uint)item);
        }

        public static void AddUInt64(this List<byte> bytes, ulong item)
        {
            bytes.Add((byte)((item >> 16) & 0xFF));
            bytes.Add((byte)((item >> 56) & 0xFF));
            bytes.Add((byte)((item >> 48) & 0xFF));
            bytes.Add((byte)((item >> 40) & 0xFF));
            bytes.Add((byte)((item >> 32) & 0xFF));
            bytes.Add((byte)((item >> 24) & 0xFF));
            bytes.Add((byte)((item >> 8) & 0xFF));
            bytes.Add((byte)(item & 0xFF));
        }

        public static void InsertUInt64(this List<byte> bytes, int index, ulong item)
        {
            bytes[index] = (byte)((item >> 56) & 0xFF);
            bytes[index + 1] = (byte)((item >> 48) & 0xFF);
            bytes[index + 2] = (byte)((item >> 40) & 0xFF);
            bytes[index + 3] = (byte)((item >> 32) & 0xFF);
            bytes[index + 4] = (byte)((item >> 24) & 0xFF);
            bytes[index + 5] = (byte)((item >> 16) & 0xFF);
            bytes[index + 6] = (byte)((item >> 8) & 0xFF);
            bytes[index + 7] = (byte)(item & 0xFF);
        }

        public static void AddInt32(this List<byte> bytes, int item)
        {
            bytes.AddUInt32((uint)item);
        }

        public static void InsertInt32(this List<byte> bytes, int index, int item)
        {
            bytes.InsertUInt32(index, (uint)item);
        }

        public static void AddUInt32(this List<byte> bytes, uint item)
        {
            bytes.Add((byte)((item >> 24) & 0xFF));
            bytes.Add((byte)((item >> 16) & 0xFF));
            bytes.Add((byte)((item >> 8) & 0xFF));
            bytes.Add((byte)(item & 0xFF));
        }

        public static void InsertUInt32(this List<byte> bytes, int index, uint item)
        {
            bytes[index] = (byte)((item >> 24) & 0xFF);
            bytes[index + 1] = (byte)((item >> 16) & 0xFF);
            bytes[index + 2] = (byte)((item >> 8) & 0xFF);
            bytes[index + 3] = (byte)(item & 0xFF);
        }

        public static void AddInt16(this List<byte> bytes, short item)
        {
            bytes.AddUInt16((ushort)item);
        }

        public static void InsertInt16(this List<byte> bytes, int index, short item)
        {
            bytes.InsertUInt16(index, (ushort)item);
        }

        public static void AddUInt16(this List<byte> bytes, uint item)
        {
            bytes.Add((byte)((item >> 8) & 0xFF));
            bytes.Add((byte)(item & 0xFF));
        }

        public static void InsertUInt16(this List<byte> bytes, int index, ushort item)
        {
            bytes[index] = (byte)((item >> 8) & 0xFF);
            bytes[index + 1] = (byte)(item & 0xFF);
        }
    }
}
