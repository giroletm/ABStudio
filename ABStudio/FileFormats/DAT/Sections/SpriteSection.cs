using ABStudio.FileFormats.ZSTREAM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ABStudio.FileFormats.DAT
{
    public partial class DATFile
    {
        public class SpriteData
        {
            public List<string> filenames = new List<string>();
            public List<Sprite> sprites = new List<Sprite>();

            public ZSTREAMFile associatedZSTREAM = null;

            public class Sprite
            {
                public string name = "";
                public Rectangle rect = new Rectangle();
                public Point orig = new Point();
            }
            public byte[] AsJSONBytes()
            {
                if (associatedZSTREAM != null)
                    return associatedZSTREAM.Save();

                throw new Exception("Can't save as JSON without ZSTREAM information.");
            }
        }

        private sealed class SpriteSection : Section
        {
            public override string Magic => "SPRT";
            public SpriteData file = new SpriteData();

            public SpriteSection() : base() { }
            public SpriteSection(byte[] data, bool isHeaderless=false) : base(data, isHeaderless) { }

            public override byte[] AsJSONBytes()
            {
                return file.AsJSONBytes();
            }

            protected override byte[] AsCoreBytes()
            {
                List<byte> rawdata = new List<byte>();


                if(!this.isHeaderless)
                    rawdata.AddUInt16((ushort)file.filenames.Count);

                foreach (string filename in (this.isHeaderless ? (new string[] { file.filenames[0] }) : file.filenames.ToArray()))
                    rawdata.AddString(filename);

                rawdata.AddUInt16((ushort)file.sprites.Count);
                foreach (SpriteData.Sprite sprite in file.sprites)
                {
                    rawdata.AddString(sprite.name);
                    rawdata.AddUInt16((ushort)sprite.rect.X);
                    rawdata.AddUInt16((ushort)sprite.rect.Y);
                    rawdata.AddUInt16((ushort)sprite.rect.Width);
                    rawdata.AddUInt16((ushort)sprite.rect.Height);
                    rawdata.AddUInt16((ushort)sprite.orig.X);
                    rawdata.AddUInt16((ushort)sprite.orig.Y);
                }


                return rawdata.ToArray();
            }

            protected override void FromCoreBytes(byte[] data)
            {
                file.filenames = new List<string>();
                file.sprites = new List<SpriteData.Sprite>();

                int idx = 0;

                ushort texCount = this.isHeaderless ? (ushort)1 : data.GetUInt16(ref idx);
                for (int texN = 0; texN < texCount; texN++)
                    file.filenames.Add(data.GetString(ref idx));

                ushort spriteCount = data.GetUInt16(ref idx);
                for (int spriteN = 0; spriteN < spriteCount; spriteN++)
                {
                    SpriteData.Sprite sprite = new SpriteData.Sprite();

                    sprite.name = data.GetString(ref idx);
                    sprite.rect.X = data.GetUInt16(ref idx);
                    sprite.rect.Y = data.GetUInt16(ref idx);
                    sprite.rect.Width = data.GetUInt16(ref idx);
                    sprite.rect.Height = data.GetUInt16(ref idx);
                    sprite.orig.X = data.GetUInt16(ref idx);
                    sprite.orig.Y = data.GetUInt16(ref idx);

                    file.sprites.Add(sprite);
                }

                file.associatedZSTREAM = new ZSTREAMFile();
                file.associatedZSTREAM.associatedSD = file;
            }
        }
    }
}
