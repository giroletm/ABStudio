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
        private sealed class MainSection : Section
        {
            public override string Magic => "KA3D";
            public Section section = null;

            public MainSection() : base() { }
            public MainSection(byte[] data) : base(data) { }

            public override byte[] AsJSONBytes()
            {
                return section.AsJSONBytes();
            }

            protected override byte[] AsCoreBytes()
            {
                return section.AsBytes();
            }

            protected override void FromCoreBytes(byte[] data)
            {
                Type[] types = Assembly.GetAssembly(typeof(Section)).GetTypes().Where(t => t.IsSubclassOf(typeof(Section))).ToArray();

                string wantedMagic = "" + (char)data[0] + (char)data[1] + (char)data[2] + (char)data[3];

                bool supported = false;
                foreach (Type type in types)
                {
                    Section o = Activator.CreateInstance(type) as Section;
                    string typeMagic = o.Magic;

                    if (wantedMagic == typeMagic)
                    {
                        section = Activator.CreateInstance(type) as Section;
                        section.FromBytes(data);

                        supported = true;
                        break;
                    }
                }

                if (!supported)
                {
                    throw new Exception("Unsupported DAT section type \"" + wantedMagic + "\".");
                }
            }
        }
    }
}
