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

        public PVRFile(string filename) : this(File.ReadAllBytes(filename)) { }

        public PVRFile(byte[] pvrData)
        {
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

        public string GetFormat()
        {
            if (ReferenceEquals(tex, null))
                return "";

            ulong val = PVRTexLib.PVRTexLibGetTexturePixelFormat(tex);

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
            if (!PVRTexLib.PVRTexLibSaveTextureToFile(tex, filename))
                throw new Exception("Couldn't save PVR file.");
        }

        public byte[] Save()
        {
            string fn = System.IO.Path.GetTempFileName();

            Save(fn);

            byte[] bytes = File.ReadAllBytes(fn);

            File.Delete(fn);

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
