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
        private MainSection file;
        private Type type => file.section.GetType();
        public string Type => type.Name.Substring(0, type.Name.Length - "Section".Length);
        public byte[] AsBytes => file.AsBytes();
        public byte[] AsJSONBytes => file.AsJSONBytes();


        private DATFile() { }
        public DATFile(string filename) : this(File.ReadAllBytes(filename)) { }
        public DATFile(byte[] data)
        {
            bool isHeaderless = (data[0] != 'K' || data[1] != 'A' || data[2] != '3' || data[3] != 'D');
            file = new MainSection(data, isHeaderless);
        }

        public SpriteData GetAsSpriteData()
        {
            if (type == typeof(SpriteSection))
                return (file.section as SpriteSection).file;
            else
                throw new Exception("Current file isn't of type " + type.ToString() + ".");
        }

        public static DATFile NewSpriteData()
        {
            DATFile dat = new DATFile();

            dat.file = new MainSection();
            dat.file.section = new SpriteSection();

            return dat;
        }
    }
}
