using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using ABStudio.FileFormats.DAT;
using ABStudio.FileFormats.PVR;
using ABStudio.Forms;
using SevenZip;
using SevenZip.Compression.LZMA;

namespace ABStudio.FileFormats.ZSTREAM
{
    public class ZSTREAMFile
    {
        public DATFile associatedDAT = null;
        public DATFile.SpriteData associatedSD = null;

        bool noInit = false;
        int atlas = -1;
        string encKey = "";
        bool wasLZMA = false;

        Meta meta = new Meta();

        class Meta
        {
            public string app = "";
            public string format = "";
            public string image = "";
            public int sizeW = -1, sizeH = -1, scale = -1;
            public string version = "";
        }

        List<PlacementInfo> placementInfos = new List<PlacementInfo>();

        class PlacementInfo
        {
            public int x = -1;
            public int y = -1;
            public int w = -1;
            public int h = -1;
            public int position = -1;
            public int length = -1;
        }

        static string[] keyList = new string[]
        {
            "55534361505170413454534E56784D49317639534B39554330795A75416E6232", // Classic
            "7A65506865737435666151755832533241707265403472654368417445765574", // Seasons
            "55534361505170413454534E56784D49317639534B39554330795A75416E6232", // Rio
            "526D67645A304A656E4C466757776B5976434C326C5361684662456846656334", // Space
            "416E3874336D6E38553673706951307A4848723361316C6F44725261336D7445", // Star Wars
            "4230706D3354416C7A6B4E3967687A6F65324E697A456C6C50644E3068516E69", // Star Wars II
            "454A52626357683831594734597A6A664C41504D7373416E6E7A785161446E31"  // Friends
        };

        static string[] keyNames = new string[]
        {
            "Classic",
            "Seasons",
            "Rio",
            "Space",
            "Star Wars",
            "Star Wars II",
            "Friends",
        };

        public ZSTREAMFile()
        {
            this.noInit = true;
        }

        public ZSTREAMFile(string filename) : this(File.ReadAllBytes(filename)) { }

        public ZSTREAMFile(byte[] zstreamData)
        {
            if (zstreamData[0] != '{')
            {
                bool isLZMA = zstreamData[1] == 'L' && zstreamData[2] == 'Z' && zstreamData[3] == 'M' && zstreamData[4] == 'A';

                if(!isLZMA)
                {
                    for (int i = 0; i < keyList.Length; i++)
                    {
                        try
                        {
                            byte[] decoded = Decrypt(zstreamData, keyList[i]);
                            if (decoded != null)
                            {
                                zstreamData = decoded;
                                encKey = keyList[i];
                                break;
                            }
                        }
                        catch (Exception) { }
                    }

                    if(encKey == "")
                        throw new Exception("Couldn't decrypt encrypted file. It might have been encrypted with an unknown key, or its content is invalid.");

                    isLZMA = zstreamData[1] == 'L' && zstreamData[2] == 'Z' && zstreamData[3] == 'M' && zstreamData[4] == 'A';
                }


                if(isLZMA)
                {
                    this.wasLZMA = true;

                    SevenZip.Compression.LZMA.Decoder lzmaDecoder = new SevenZip.Compression.LZMA.Decoder();

                    MemoryStream msIn = new MemoryStream(zstreamData.Skip(9).ToArray());
                    using (MemoryStream msOut = new MemoryStream())
                    {
                        // Read the decoder properties
                        byte[] properties = new byte[5];
                        msIn.Read(properties, 0, 5);


                        // Read in the decompress file size.
                        byte[] fileLengthBytes = new byte[8];
                        msIn.Read(fileLengthBytes, 0, 8);
                        long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                        lzmaDecoder.SetDecoderProperties(properties);
                        lzmaDecoder.Code(msIn, msOut, msIn.Length, fileLength, null);

                        msIn.Close();

                        zstreamData = msOut.ToArray();
                    }
                }
            }

            JObject jo = JObject.Parse(Encoding.UTF8.GetString(zstreamData));
            JToken root = jo.Root;


            JToken meta = root.SelectToken("meta");
            this.meta.app = meta.SelectToken("app").ToObject<string>();
            this.meta.format = meta.SelectToken("format").ToObject<string>();
            this.meta.image = meta.SelectToken("image").ToObject<string>();
            this.meta.version = meta.SelectToken("version").ToObject<string>();

            JToken metaSize = meta.SelectToken("size");
            this.meta.sizeW = metaSize.SelectToken("w").ToObject<int>();
            this.meta.sizeH = metaSize.SelectToken("h").ToObject<int>();
            this.meta.scale = metaSize.SelectToken("scale").ToObject<int>();

            DATFile dat = DATFile.NewSpriteData();
            DATFile.SpriteData spriteData = dat.GetAsSpriteData();
            spriteData.associatedZSTREAM = this;

            spriteData.filenames.Add(this.meta.image);

            JToken frames = root.SelectToken("frames");
            foreach (JToken frame in frames.Children())
            {
                DATFile.SpriteData.Sprite sprite = new DATFile.SpriteData.Sprite();

                sprite.name = frame.SelectToken("filename").ToObject<string>();

                JToken pivot = frame.SelectToken("pivot");
                sprite.orig = new Point(
                    pivot.SelectToken("x").ToObject<int>(),
                    pivot.SelectToken("y").ToObject<int>());

                JToken fframe = frame.SelectToken("frame");
                sprite.rect = new Rectangle(
                    fframe.SelectToken("x").ToObject<int>(),
                    fframe.SelectToken("y").ToObject<int>(),
                    fframe.SelectToken("w").ToObject<int>(),
                    fframe.SelectToken("h").ToObject<int>());

                JToken stream = frame.SelectToken("stream");
                if (stream != null) {
                    if(stream.SelectToken("atlas") != null)
                        this.atlas = stream.SelectToken("atlas").ToObject<int>();

                    PlacementInfo pInfo = new PlacementInfo();
                    pInfo.x = sprite.rect.X;
                    pInfo.y = sprite.rect.Y;
                    pInfo.w = stream.SelectToken("width").ToObject<int>();
                    pInfo.h = stream.SelectToken("height").ToObject<int>();
                    pInfo.position = stream.SelectToken("position").ToObject<int>();
                    pInfo.length = stream.SelectToken("length").ToObject<int>();

                    placementInfos.Add(pInfo);

                    int xDisp = (pInfo.w - sprite.rect.Width) / 2;
                    int yDisp = (pInfo.h - sprite.rect.Height) / 2;

                    sprite.rect.X += xDisp;
                    sprite.rect.Y += yDisp;
                }



                spriteData.sprites.Add(sprite);
            }

            associatedDAT = dat;
            associatedSD = spriteData;
        }

        public void UpdateBitmap(Bitmap bmp, string filename)
        {
            this.meta.image = filename;
            this.meta.sizeW = bmp.Width;
            this.meta.sizeH = bmp.Height;
        }

        public Bitmap GetBitmap(string path)
        {
            byte[] rawData = null;

            try
            {
                rawData = File.ReadAllBytes(path);
            }
            catch(Exception)
            {
                try
                {
                    rawData = File.ReadAllBytes(path + ".7z");
                }
                catch (Exception)
                {
                    throw new Exception("Couldn't read find file \"" + path + "\"");
                }
            }

            if(rawData[0] == '7' && rawData[1] == 'z')
            {
                using (MemoryStream outMS = new MemoryStream())
                {
                    using (MemoryStream inMS = new MemoryStream(rawData))
                    {
                        using (Aspose.Zip.SevenZip.SevenZipArchive archive = new Aspose.Zip.SevenZip.SevenZipArchive(inMS))
                        {
                            foreach(Aspose.Zip.SevenZip.SevenZipArchiveEntry v in archive.Entries)
                            {
                                v.Extract(outMS);
                                break;
                            }
                        }
                    }

                    rawData = outMS.ToArray();
                }
            }

            Bitmap bmp = new Bitmap(this.meta.sizeW, this.meta.sizeH);

            using(Graphics g = Graphics.FromImage(bmp))
            {
                int numimg = 0;
                foreach (PlacementInfo pInfo in placementInfos)
                {
                    byte[] streamData = rawData.Skip(pInfo.position).Take(pInfo.length + 0x28).ToArray();

                    byte[] pvrVer = new byte[streamData.Length + 0xC];
                    pvrVer[0] = (byte)'P';
                    pvrVer[1] = (byte)'V';
                    pvrVer[2] = (byte)'R';
                    pvrVer[3] = 3;

                    pvrVer[4] = 0;
                    pvrVer[5] = 0;
                    pvrVer[6] = 0;
                    pvrVer[7] = 0;

                    string fmt = "";
                    int n = 0x20;
                    while (streamData[n] != 0 || n == 0x28) {
                        fmt += (char)streamData[n];
                        n++;
                    }
                    int half = fmt.Length / 2;
                    fmt = fmt.ToLower();

                    byte ch1 = (byte)((0 < half) ? fmt[0] : 0);
                    byte ch2 = (byte)((1 < half) ? fmt[1] : 0);
                    byte ch3 = (byte)((2 < half) ? fmt[2] : 0);
                    byte ch4 = (byte)((3 < half) ? fmt[3] : 0);
                    byte va1 = (byte)Convert.ToInt32(new string(new char[] { ((half < fmt.Length) ? fmt[half] : '0') }));
                    byte va2 = (byte)Convert.ToInt32(new string(new char[] { (((half + 1) < fmt.Length) ? fmt[half + 1] : '0') }));
                    byte va3 = (byte)Convert.ToInt32(new string(new char[] { (((half + 2) < fmt.Length) ? fmt[half + 2] : '0') }));
                    byte va4 = (byte)Convert.ToInt32(new string(new char[] { (((half + 3) < fmt.Length) ? fmt[half + 3] : '0')}));

                    pvrVer[8] = ch1;
                    pvrVer[9] = ch2;
                    pvrVer[0xA] = ch3;
                    pvrVer[0xB] = ch4;

                    pvrVer[0xC] = va1;
                    pvrVer[0xD] = va2;
                    pvrVer[0xE] = va3;
                    pvrVer[0xF] = va4;

                    pvrVer[0x10] = 0;
                    pvrVer[0x11] = 0;
                    pvrVer[0x12] = 0;
                    pvrVer[0x13] = 0;

                    pvrVer[0x14] = 4;
                    pvrVer[0x15] = 0;
                    pvrVer[0x16] = 0;
                    pvrVer[0x17] = 0;

                    pvrVer[0x18] = streamData[9];
                    pvrVer[0x19] = streamData[8];
                    pvrVer[0x1A] = 0;
                    pvrVer[0x1B] = 0;

                    pvrVer[0x1C] = streamData[7];
                    pvrVer[0x1D] = streamData[6];
                    pvrVer[0x1E] = 0;
                    pvrVer[0x1F] = 0;

                    pvrVer[0x20] = 1;
                    pvrVer[0x21] = 0;
                    pvrVer[0x22] = 0;
                    pvrVer[0x23] = 0;

                    pvrVer[0x24] = 1;
                    pvrVer[0x25] = 0;
                    pvrVer[0x26] = 0;
                    pvrVer[0x27] = 0;

                    pvrVer[0x28] = 1;
                    pvrVer[0x29] = 0;
                    pvrVer[0x2A] = 0;
                    pvrVer[0x2B] = 0;

                    pvrVer[0x2C] = 1;
                    pvrVer[0x2D] = 0;
                    pvrVer[0x2E] = 0;
                    pvrVer[0x2F] = 0;

                    pvrVer[0x30] = 0;
                    pvrVer[0x31] = 0;
                    pvrVer[0x32] = 0;
                    pvrVer[0x33] = 0;

                    for(int i = 0x28; i < streamData.Length; i++)
                    {
                        pvrVer[i + 0xC] = streamData[i];
                    }

                    PVRFile pvr = new PVRFile(pvrVer);
                    Bitmap subBmp = pvr.AsBitmap();

                    g.DrawImage(subBmp, new Point(pInfo.x, pInfo.y));
                }
            }

            return bmp;
        }

        public void SaveBitmap(Bitmap bmp, string path)
        {
            if (noInit)
                if (!AskForInfo())
                    throw new Exception("Can't proceed without ZSTREAM information");

            int pos = 0;
            byte[] outBytes = new byte[placementInfos.Last().position + placementInfos.Last().length + 0x28];

            string fmt = this.meta.format.ToLower();
            int half = fmt.Length / 2;

            char ch1 = (char)((0 < half) ? fmt[0] : 0);
            char ch2 = (char)((1 < half) ? fmt[1] : 0);
            char ch3 = (char)((2 < half) ? fmt[2] : 0);
            char ch4 = (char)((3 < half) ? fmt[3] : 0);
            char va1 = (char)((half < fmt.Length) ? fmt[half] : 0);
            char va2 = (char)(((half + 1) < fmt.Length) ? fmt[half + 1] : 0);
            char va3 = (char)(((half + 2) < fmt.Length) ? fmt[half + 2] : 0);
            char va4 = (char)(((half + 3) < fmt.Length) ? fmt[half + 3] : 0);

            fmt = new string(new char[] { ch1, va1, ch2, va2, ch3, va3, ch4, va4 });

            foreach (PlacementInfo pInfo in placementInfos)
            {
                Bitmap subbmp = bmp.Clone(new Rectangle(pInfo.x, pInfo.y, pInfo.w, pInfo.h), bmp.PixelFormat);

                PVRFile pvr = new PVRFile(subbmp, fmt);
                byte[] asData = pvr.Save().Skip(0x44).ToArray();
                byte[] header = new byte[0x28];

                subbmp.Dispose();

                int fullLen = pInfo.length + 0x28;

                header[0] = (byte)((fullLen >> 24) & 0xFF);
                header[1] = (byte)((fullLen >> 16) & 0xFF);
                header[2] = (byte)((fullLen >> 8) & 0xFF);
                header[3] = (byte)(fullLen & 0xFF);

                header[4] = 0;
                header[5] = 0x28;

                header[6] = (byte)((pInfo.w >> 8) & 0xFF);
                header[7] = (byte)(pInfo.w & 0xFF);

                header[8] = (byte)((pInfo.h >> 8) & 0xFF);
                header[9] = (byte)(pInfo.h & 0xFF);

                header[0xA] = (byte)((this.atlas >> 8) & 0xFF);
                header[0xB] = (byte)(this.atlas & 0xFF);

                header[0xC] = 0;
                header[0xD] = 0;
                header[0xE] = 0;
                header[0xF] = 0;
                header[0x10] = 0;
                header[0x11] = 0;
                header[0x12] = 0;
                header[0x13] = 0;
                header[0x14] = 0;
                header[0x15] = 0;
                header[0x16] = 0;
                header[0x17] = 0;

                header[0x18] = 0;
                header[0x19] = 1;
                header[0x1A] = 0;
                header[0x1B] = 1;
                header[0x1C] = 0;
                header[0x1D] = 1;
                header[0x1E] = 0;
                header[0x1F] = 1;

                int fmtLen = this.meta.format.Length;
                header[0x20] = (byte)((0 < fmtLen) ? this.meta.format[0] : 0);
                header[0x21] = (byte)((1 < fmtLen) ? this.meta.format[1] : 0);
                header[0x22] = (byte)((2 < fmtLen) ? this.meta.format[2] : 0);
                header[0x23] = (byte)((3 < fmtLen) ? this.meta.format[3] : 0);
                header[0x24] = (byte)((4 < fmtLen) ? this.meta.format[4] : 0);
                header[0x25] = (byte)((5 < fmtLen) ? this.meta.format[5] : 0);
                header[0x26] = (byte)((6 < fmtLen) ? this.meta.format[6] : 0);
                header[0x27] = (byte)((7 < fmtLen) ? this.meta.format[7] : 0);

                foreach (byte b in header)
                    outBytes[pos++] = b;

                foreach (byte b in asData)
                    outBytes[pos++] = b;
            }

            using (MemoryStream outMS = new MemoryStream())
            {
                using (MemoryStream inMS = new MemoryStream(outBytes))
                {
                    using (Aspose.Zip.SevenZip.SevenZipArchive archive = new Aspose.Zip.SevenZip.SevenZipArchive())
                    {
                        archive.CreateEntry(Path.GetFileNameWithoutExtension(path) + ".zstream", inMS);

                        archive.Save(outMS);
                    }
                }

                outBytes = outMS.ToArray();
            }


            File.WriteAllBytes(path + ".7z", outBytes);
        }

        private bool AskForInfo()
        {
            string[] apps = new string[] { "ArtPacker", "Adobe" };
            using (MCQAskForm mcqAskForm = new MCQAskForm(apps, "JSON creation app"))
            {
                if (mcqAskForm.ShowDialog() == DialogResult.OK)
                {
                    meta.app = apps[mcqAskForm.ChosenAnswer];
                }
                else return false;
            }

            string[] versions = new string[] { "2.4.19" };
            using (MCQAskForm mcqAskForm = new MCQAskForm(versions, "JSON creation app version"))
            {
                if (mcqAskForm.ShowDialog() == DialogResult.OK)
                {
                    meta.version = versions[mcqAskForm.ChosenAnswer];
                }
                else return false;
            }

            string[] formats = new string[] { "RGBA4444", "RGBA8888", "RGB565" };
            using (MCQAskForm mcqAskForm = new MCQAskForm(formats, "JSON image format"))
            {
                if (mcqAskForm.ShowDialog() == DialogResult.OK)
                {
                    meta.format = formats[mcqAskForm.ChosenAnswer];
                }
                else return false;
            }

            string[] encryptions = (new string[] { "None" }).Concat(keyNames).Concat(new string[] { "Custom" }).ToArray();
            using (ComboAskForm comboAskForm = new ComboAskForm(encryptions, "JSON encryption"))
            {
                if (comboAskForm.ShowDialog() == DialogResult.OK)
                {
                    if (comboAskForm.ComboIndex > 0 && comboAskForm.ComboIndex < encryptions.Length - 1)
                        this.encKey = keyList[comboAskForm.ComboIndex - 1];
                    else if (comboAskForm.ComboIndex == encryptions.Length - 1)
                    {
                        string answer = Interaction.InputBox("Custom key:", "Enter a custom key");
                        this.encKey = answer;
                    }
                    else
                        this.encKey = "";
                }
                else return false;
            }

            string[] lzmas = new string[] { "Yes", "No" };
            using (MCQAskForm mcqAskForm = new MCQAskForm(lzmas, "JSON LZMA Compression?"))
            {
                if (mcqAskForm.ShowDialog() == DialogResult.OK)
                {
                    this.wasLZMA = (mcqAskForm.ChosenAnswer == 0) ? true : false;
                }
                else return false;
            }

            using (NumUpDownAskForm numUpDownAskForm = new NumUpDownAskForm(0, 0, 255, 1))
            {
                numUpDownAskForm.Title = "Atlas ID";
                if (numUpDownAskForm.ShowDialog() == DialogResult.OK)
                {
                    this.atlas = (int)numUpDownAskForm.NumValue;
                }
                else return false;
            }

            this.noInit = false;
            return true;
        }

        public byte[] Save()
        {
            if (noInit)
                if (!AskForInfo())
                    throw new Exception("Can't proceed without ZSTREAM information");

            JObject root = new JObject();

            int position = 0;
            this.placementInfos.Clear();

            JArray jframes = new JArray();
            foreach(DATFile.SpriteData.Sprite sprite in associatedSD.sprites)
            {
                JObject jframeentry = new JObject();

                JArray jextrude = new JArray();
                jextrude.Add(0);
                jextrude.Add(0);
                jextrude.Add(0);
                jextrude.Add(0);
                jframeentry.Add("extrude", jextrude);

                jframeentry.Add("filename", sprite.name);

                JObject jframe = new JObject();
                int xPad = (sprite.rect.X <= 0) ? 0 : 1;
                int yPad = (sprite.rect.Y <= 0) ? 0 : 1;
                jframe.Add("h", sprite.rect.Height);
                jframe.Add("w", sprite.rect.Width);
                jframe.Add("x", sprite.rect.X - xPad);
                jframe.Add("y", sprite.rect.Y - yPad);
                jframeentry.Add("frame", jframe);

                JObject jpivot = new JObject();
                jpivot.Add("x", sprite.orig.X);
                jpivot.Add("y", sprite.orig.Y);
                jframeentry.Add("pivot", jpivot);
                
                jframeentry.Add("rotated", false);

                JObject jstream = new JObject();
                int wPad = xPad + (((sprite.rect.X + sprite.rect.Width) >= this.meta.sizeW) ? 0 : 1);
                int hPad = yPad + (((sprite.rect.Y + sprite.rect.Height) >= this.meta.sizeH) ? 0 : 1);

                int fmtBitsSum = 0;
                for (int i = (this.meta.format.Length / 2); i < this.meta.format.Length; i++)
                    fmtBitsSum += Convert.ToInt32(this.meta.format[i].ToString());
                int length = fmtBitsSum * ((sprite.rect.Width + wPad) * (sprite.rect.Height + hPad)) / 8; 

                jstream.Add("atlas", this.atlas);
                jstream.Add("height", sprite.rect.Height + hPad);
                jstream.Add("length", length);
                jstream.Add("position", position);
                jstream.Add("width", sprite.rect.Width + wPad);
                jframeentry.Add("stream", jstream);

                PlacementInfo pInfo = new PlacementInfo();
                pInfo.x = sprite.rect.X - xPad;
                pInfo.y = sprite.rect.Y - yPad;
                pInfo.w = sprite.rect.Width + wPad;
                pInfo.h = sprite.rect.Height + hPad;
                pInfo.position = position;
                pInfo.length = length;
                placementInfos.Add(pInfo);

                position += 0x28 + length;

                jframes.Add(jframeentry);
            }
            root.Add("frames", jframes);

            JObject jmeta = new JObject();
            jmeta.Add("app", this.meta.app);
            jmeta.Add("format", this.meta.format);
            jmeta.Add("image", this.meta.image);

            JObject jsize = new JObject();
            jsize.Add("h", this.meta.sizeH);
            jsize.Add("scale", this.meta.scale);
            jsize.Add("w", this.meta.sizeW);
            jmeta.Add("size", jsize);

            jmeta.Add("version", this.meta.version);

            root.Add("meta", jmeta);


            byte[] outBytes = Encoding.UTF8.GetBytes(root.ToString(Newtonsoft.Json.Formatting.None));

            if (wasLZMA)
            {
                SevenZip.Compression.LZMA.Encoder lzmaEncoder = new SevenZip.Compression.LZMA.Encoder();

                CoderPropID[] propIDs =
                {
                    CoderPropID.Algorithm
                };

                object[] properties =
                {
                    (System.Int32)(1)
                };

                lzmaEncoder.SetCoderProperties(propIDs, properties);

                MemoryStream msIn = new MemoryStream(outBytes);
                using (MemoryStream msOut = new MemoryStream())
                {
                    lzmaEncoder.Code(msIn, msOut, msIn.Length, -1, null);

                    msIn.Close();

                    UInt64 tLen = (UInt64)outBytes.Length;

                    byte[] header = new byte[] { 0x89, 0x4C, 0x5A, 0x4D, 0x41, 0x0D, 0x0A, 0x1A, 0x0A,
                                0x5D, 0x00, 0x00, 0x01, 0x00,
                                (byte)(tLen & 0xFF),
                                (byte)((tLen >> 8) & 0xFF),
                                (byte)((tLen >> 16) & 0xFF),
                                (byte)((tLen >> 24) & 0xFF),
                                (byte)((tLen >> 32) & 0xFF),
                                (byte)((tLen >> 40) & 0xFF),
                                (byte)((tLen >> 48) & 0xFF),
                                (byte)((tLen >> 52) & 0xFF)
                            };

                    outBytes = header.Concat(msOut.ToArray()).ToArray();
                }
            }

            if (encKey != "")
            {
                try
                {
                    outBytes = Encrypt(outBytes, encKey);
                }
                catch (Exception) { 
                    throw new Exception("Couldn't encrypt file.");
                }
            }

            return outBytes;
        }


        public static byte[] Decrypt(byte[] cipherData, string keyString)
        {
            byte[] key = new byte[32];
            for (int i = 0; i < keyString.Length; i += 2)
                key[i / 2] = Convert.ToByte(keyString.Substring(i, 2), 16);

            byte[] iv = new byte[16];

            try
            {
                using (var rijndaelManaged =
                       new RijndaelManaged { Key = key, IV = iv, Mode = CipherMode.CBC })
                using (var memoryStream =
                       new MemoryStream(cipherData))
                using (var cryptoStream =
                       new CryptoStream(memoryStream,
                           rijndaelManaged.CreateDecryptor(key, iv),
                           CryptoStreamMode.Read))
                using (var memoryStream2 =
                       new MemoryStream())
                {
                    (new StreamReader(cryptoStream)).BaseStream.CopyTo(memoryStream2);
                    return memoryStream2.ToArray();
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        public static byte[] Encrypt(byte[] cipherData, string keyString)
        {
            byte[] key = new byte[32];
            for (int i = 0; i < keyString.Length; i += 2)
                key[i / 2] = Convert.ToByte(keyString.Substring(i, 2), 16);

            byte[] iv = new byte[16];

            try
            {
                using (var rijndaelManaged =
                       new RijndaelManaged { Key = key, IV = iv, Mode = CipherMode.CBC })
                using (var memoryStream =
                       new MemoryStream(cipherData))
                using (var cryptoStream =
                       new CryptoStream(memoryStream,
                           rijndaelManaged.CreateEncryptor(key, iv),
                           CryptoStreamMode.Read))
                using (var memoryStream2 =
                       new MemoryStream())
                {
                    (new StreamReader(cryptoStream)).BaseStream.CopyTo(memoryStream2);
                    return memoryStream2.ToArray();
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
    }
}
