using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ABStudio.FileFormats.PVR
{
    public class PVRFile
    {
        private IntPtr tex = IntPtr.Zero;
        public bool isLegacy = false;

        public PVRFile(string filename) : this(File.ReadAllBytes(filename)) { }

        public PVRFile(byte[] pvrData)
        {
            if (pvrData[0] == 0x34 && pvrData[1] == 0 && pvrData[2] == 0 && pvrData[3] == 0)
                isLegacy = true;

            IntPtr dataPtr = Marshal.AllocHGlobal(pvrData.Length);
            Marshal.Copy(pvrData, 0, dataPtr, pvrData.Length);

            tex = PVRTexLib.PVRTexLibCreateTextureFromData(dataPtr);
            if (ReferenceEquals(tex, null))
                throw new Exception("Invalid PVR file.");

            Marshal.FreeHGlobal(dataPtr);
        }

        public PVRFile(Bitmap bmp, string format="r4g4b4a4")
        {
            PVRHeaderCreateParams headerCreateParams = new PVRHeaderCreateParams();
            headerCreateParams.PixelFormat = FormatStringToULong("r8g8b8a8");
            headerCreateParams.Width = (uint)bmp.Width;
            headerCreateParams.Height = (uint)bmp.Height;
            headerCreateParams.Depth = 1u;
            headerCreateParams.NumMipMaps = 1u;
            headerCreateParams.NumArrayMembers = 1u;
            headerCreateParams.NumFaces = 1u;
            headerCreateParams.ColourSpace = PVRTexLibColourSpace.PVRTLCS_Linear;
            headerCreateParams.ChannelType = PVRTexLibVariableType.PVRTLVT_UnsignedByteNorm;
            headerCreateParams.PreMultiplied = false;
            IntPtr header = PVRTexLib.PVRTexLibCreateTextureHeader(headerCreateParams);

            ulong textureSize = PVRTexLib.PVRTexLibGetTextureDataSize(header, -1, true, true);
            if (textureSize == 0)
                return;

            uint bytesPerPixel = PVRTexLib.PVRTexLibGetTextureBitsPerPixel(header) / 8U;

            // Number of pixel for this (2D) surface
            uint numPixels = (uint)(bmp.Width * bmp.Height);

            byte[] data = new byte[numPixels * bytesPerPixel];

            // All pixels in this surface
            for (uint pixel = 0U; pixel < numPixels; ++pixel)
            {
                uint currPixel = pixel * bytesPerPixel;

                Color col = bmp.GetPixel((int)(pixel % bmp.Width), (int)(pixel / bmp.Width));
                data[currPixel] = col.R;
                data[currPixel + 1] = col.G;
                data[currPixel + 2] = col.B;
                data[currPixel + 3] = col.A;
            }

            //bytes = data;

            IntPtr texPtr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, texPtr, data.Length);

            tex = PVRTexLib.PVRTexLibCreateTexture(header, texPtr);

            Marshal.FreeHGlobal(texPtr);

            if (format != "r8g8b8a8")
                TranscodeTex(format);
        }

        ~PVRFile()
        {
            if (!ReferenceEquals(tex, null))
                PVRTexLib.PVRTexLibDestroyTexture(tex);
        }

        public ulong GetFormat()
        {
            if (ReferenceEquals(tex, null))
                return 0xFFFFFFFF;

            IntPtr header = PVRTexLib.PVRTexLibGetTextureHeader(tex);
            if (ReferenceEquals(header, null))
                return 0xFFFFFFFF;

            return PVRTexLib.PVRTexLibGetTexturePixelFormat(header);
        }

        public string GetFormatStr()
        {
            ulong val = GetFormat();
            if (val == 0xFFFFFFFF)
                return "";

            return FormatULongToString(val);
        }

        public static string FormatULongToString(ulong format)
        {
            char c1 = (char)(format & 0xFF);
            char c2 = (char)((format >> 8) & 0xFF);
            char c3 = (char)((format >> 16) & 0xFF);
            char c4 = (char)((format >> 24) & 0xFF);
            byte n1 = (byte)((format >> 32) & 0xFF);
            byte n2 = (byte)((format >> 40) & 0xFF);
            byte n3 = (byte)((format >> 48) & 0xFF);
            byte n4 = (byte)((format >> 56) & 0xFF);

            return "" + c1 + n1 + c2 + n2 + c3 + n3 + c4 + n4;
        }

        public static ulong FormatStringToULong(string format)
        {
            ulong val = 0;
            val |= (ulong)(format[0] & 0xFF);
            val |= ((ulong)(format[2] & 0xFF) << 8);
            val |= ((ulong)(format[4] & 0xFF) << 16);
            val |= ((ulong)(format[6] & 0xFF) << 24);
            val |= (((ulong)(format[1] - '0') & 0xFF) << 32);
            val |= (((ulong)(format[3] - '0') & 0xFF) << 40);
            val |= (((ulong)(format[5] - '0') & 0xFF) << 48);
            val |= (((ulong)(format[7] - '0') & 0xFF) << 56);

            return val;
        }

        public void TranscodeTex(string format) => TranscodeTex(FormatStringToULong(format));
        public void TranscodeTex(ulong format)
        {
            IntPtr newTex = Transcode(format);

            PVRTexLib.PVRTexLibDestroyTexture(tex);

            tex = newTex;
        }

        private IntPtr Transcode(string format) => Transcode(FormatStringToULong(format));
        private IntPtr Transcode(ulong format)
        {
            IntPtr newTex = PVRTexLib.PVRTexLibCopyTexture(tex);
            if (ReferenceEquals(newTex, null))
                throw new Exception("Couldn't duplicate PVR file.");

            IntPtr newHeader = PVRTexLib.PVRTexLibGetTextureHeaderW(newTex);
            if (ReferenceEquals(newHeader, null))
            {
                PVRTexLib.PVRTexLibDestroyTexture(newTex);
                throw new Exception("Couldn't get duplicated PVR file header.");
            }

            if (PVRTexLib.PVRTexLibGetTexturePixelFormat(newHeader) == format)
                return newTex;

            PVRTexLibTranscoderOptions options = new PVRTexLibTranscoderOptions();
            options.SizeofStruct = 48;
            options.PixelFormat = format;
            options.ChannelType0 = PVRTexLibVariableType.PVRTLVT_UnsignedByteNorm;
            options.ChannelType1 = PVRTexLibVariableType.PVRTLVT_UnsignedByteNorm;
            options.ChannelType2 = PVRTexLibVariableType.PVRTLVT_UnsignedByteNorm;
            options.ChannelType3 = PVRTexLibVariableType.PVRTLVT_UnsignedByteNorm;
            options.Colourspace = PVRTexLibColourSpace.PVRTLCS_Linear;
            options.Quality = PVRTexLibCompressorQuality.PVRTLCQ_PVRTCNormal;
            options.DoDither = false;
            options.MaxRange = 1.0f;
            options.MaxThreads = 0u;

            bool didTranscode = PVRTexLib.PVRTexLibTranscodeTexture(newTex, options);
            if (!didTranscode)
            {
                PVRTexLib.PVRTexLibDestroyTexture(newTex);
                throw new Exception("Couldn't transcode PVR file.");
            }

            return newTex;
        }

        public void Save(string filename)
        {
            File.WriteAllBytes(filename, Save());
        }

        public byte[] Save()
        {
            string fn = System.IO.Path.GetTempFileName();

            if (!PVRTexLib.PVRTexLibSaveTextureToFile(tex, fn))
                throw new Exception("Couldn't save PVR file.");

            byte[] bytes = File.ReadAllBytes(fn);

            File.Delete(fn);

            int metaSize = bytes[0x30] | (bytes[0x31] << 8) | (bytes[0x32] << 16) | (bytes[0x33] << 24);
            bytes = bytes.Take(0x34).Concat(bytes.Skip(0x34 + metaSize)).ToArray();
            bytes[0x30] = 0;
            bytes[0x31] = 0;
            bytes[0x32] = 0;
            bytes[0x33] = 0;

            if(isLegacy)
            {
                byte[] newHeader = new byte[0x34];

                newHeader[0] = 0x34;
                newHeader[1] = 0;
                newHeader[2] = 0;
                newHeader[3] = 0;

                uint height = (uint)(bytes[0x18] | (bytes[0x19] << 8) | (bytes[0x19] << 16) | (bytes[0x19] << 24));
                newHeader[4] = bytes[0x18];
                newHeader[5] = bytes[0x19];
                newHeader[6] = bytes[0x1A];
                newHeader[7] = bytes[0x1B];

                uint width = (uint)(bytes[0x1C] | (bytes[0x1D] << 8) | (bytes[0x1E] << 16) | (bytes[0x1F] << 24));
                newHeader[8] = bytes[0x1C];
                newHeader[9] = bytes[0x1D];
                newHeader[0xA] = bytes[0x1E];
                newHeader[0xB] = bytes[0x1F];

                uint mipmapCount = (uint)(bytes[0x2C] | (bytes[0x2D] << 8) | (bytes[0x2E] << 16) | (bytes[0x2F] << 24)) - 1;
                newHeader[0xC] = (byte)(mipmapCount & 0xFF);
                newHeader[0xD] = (byte)((mipmapCount >> 8) & 0xFF);
                newHeader[0xE] = (byte)((mipmapCount >> 16) & 0xFF);
                newHeader[0xF] = (byte)((mipmapCount >> 24) & 0xFF);

                ulong currFormat = GetFormat();
                string currFormatStr = FormatULongToString(currFormat);

                ulong rgba4444 = FormatStringToULong("r4g4b4a4");
                ulong rgba8888 = FormatStringToULong("r8g8b8a8");
                ulong rgb565 = FormatStringToULong("r5g6b5\00");

                if (currFormat == rgba4444)
                    newHeader[0x10] = 0x10;
                else if (currFormat == rgba8888)
                    newHeader[0x10] = 0x12;
                else if (currFormat == rgb565)
                    newHeader[0x10] = 0x13;
                else
                    throw new Exception("PVR Legacy: unsupported format \"" + currFormatStr + "\".");

                newHeader[0x11] = 0;
                if (mipmapCount > 0)
                    newHeader[0x11] |= 1;
                if (currFormatStr.Contains('a'))
                    newHeader[0x11] |= 0x80;

                newHeader[0x12] = 0;
                newHeader[0x13] = 0;

                uint bpp = PVRTexLib.PVRTexLibGetFormatBitsPerPixel(currFormat);
                uint surfSize = (width * height) * (bpp / 8U);

                newHeader[0x14] = (byte)(surfSize & 0xFF);
                newHeader[0x15] = (byte)((surfSize >> 8) & 0xFF);
                newHeader[0x16] = (byte)((surfSize >> 16) & 0xFF);
                newHeader[0x17] = (byte)((surfSize >> 24) & 0xFF);

                newHeader[0x18] = (byte)(bpp & 0xFF);
                newHeader[0x19] = (byte)((bpp >> 8) & 0xFF);
                newHeader[0x1A] = (byte)((bpp >> 16) & 0xFF);
                newHeader[0x1B] = (byte)((bpp >> 24) & 0xFF);

                uint rMask = 0;
                uint gMask = 0;
                uint bMask = 0;
                uint aMask = 0;

                if (currFormat == rgba4444)
                {
                    rMask = 0xF000;
                    gMask = 0x0F00;
                    bMask = 0x00F0;
                    aMask = 0x000F;
                }
                else if (currFormat == rgba8888)
                {
                    rMask = 0xFF000000;
                    gMask = 0x00FF0000;
                    bMask = 0x0000FF00;
                    aMask = 0x000000FF;
                }
                else if (currFormat == rgb565)
                {
                    rMask = 0xF800;
                    gMask = 0x07E0;
                    bMask = 0x001F;
                    aMask = 0x0000;
                }
                else
                    throw new Exception("PVR Legacy: unsupported format \"" + currFormatStr + "\".");

                newHeader[0x1C] = (byte)(rMask & 0xFF);
                newHeader[0x1D] = (byte)((rMask >> 8) & 0xFF);
                newHeader[0x1E] = (byte)((rMask >> 16) & 0xFF);
                newHeader[0x1F] = (byte)((rMask >> 24) & 0xFF);

                newHeader[0x20] = (byte)(gMask & 0xFF);
                newHeader[0x21] = (byte)((gMask >> 8) & 0xFF);
                newHeader[0x22] = (byte)((gMask >> 16) & 0xFF);
                newHeader[0x23] = (byte)((gMask >> 24) & 0xFF);

                newHeader[0x24] = (byte)(bMask & 0xFF);
                newHeader[0x25] = (byte)((bMask >> 8) & 0xFF);
                newHeader[0x26] = (byte)((bMask >> 16) & 0xFF);
                newHeader[0x27] = (byte)((bMask >> 24) & 0xFF);

                newHeader[0x28] = (byte)(aMask & 0xFF);
                newHeader[0x29] = (byte)((aMask >> 8) & 0xFF);
                newHeader[0x2A] = (byte)((aMask >> 16) & 0xFF);
                newHeader[0x2B] = (byte)((aMask >> 24) & 0xFF);

                newHeader[0x2C] = (byte)0x50;
                newHeader[0x2D] = (byte)0x56;
                newHeader[0x2E] = (byte)0x52;
                newHeader[0x2F] = (byte)0x21;

                newHeader[0x30] = bytes[0x24];
                newHeader[0x31] = bytes[0x25];
                newHeader[0x32] = bytes[0x26];
                newHeader[0x33] = bytes[0x27];

                for (int i = 0; i < 0x34; i++)
                    bytes[i] = newHeader[i];
            }

            return bytes;
        }

        public Bitmap AsBitmap(uint level=0, uint array=0, uint face=0, uint slice=0)
        {
            IntPtr newTex = Transcode("r8g8b8a8");
            IntPtr newHeader = PVRTexLib.PVRTexLibGetTextureHeaderW(newTex);

            uint bytesPerPixel = PVRTexLib.PVRTexLibGetTextureBitsPerPixel(newHeader) / 8U;
            if (bytesPerPixel != 4)
            {
                PVRTexLib.PVRTexLibDestroyTexture(newTex);
                throw new Exception("PVR transcoding failed.");
            }

            // Width and height for this Mip level
            uint levelWidth = PVRTexLib.PVRTexLibGetTextureWidth(newHeader, level);
            uint levelHeight = PVRTexLib.PVRTexLibGetTextureHeight(newHeader, level);

            Bitmap bmp = new Bitmap((int)levelWidth, (int)levelHeight);

            // Number of pixel for this (2D) surface
            uint numPixels = levelWidth * levelHeight;

            IntPtr data = PVRTexLib.PVRTexLibGetTextureDataPtr(newTex, level, array, face, slice);
            if (ReferenceEquals(data, null))
            {
                PVRTexLib.PVRTexLibDestroyTexture(newTex);
                throw new Exception("Couldn't get pixel data for transcoded PVR file.");
            }


            // All pixels in this surface
            for (uint pixel = 0U; pixel < numPixels; ++pixel)
            {
                byte[] px = new byte[bytesPerPixel];
                Marshal.Copy(data, px, 0, (int)bytesPerPixel);

                bmp.SetPixel((int)(pixel % levelWidth), (int)(pixel / levelWidth), Color.FromArgb(px[3], px[0], px[1], px[2]));

                data = IntPtr.Add(data, (int)bytesPerPixel);
            }

            PVRTexLib.PVRTexLibDestroyTexture(newTex);

            return bmp;
        }
    }
}
