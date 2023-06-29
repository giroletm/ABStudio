using ABStudio.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABStudio.FileFormats.DAT
{
    public partial class DATFile
    {
        private sealed class MainSection : Section
        {
            public override string Magic => "KA3D";
            public Section section = null;

            public MainSection() : base() { }
            public MainSection(byte[] data, bool isHeaderless=false) {
                this.isHeaderless = isHeaderless;

                if (!isHeaderless)
                {
                    FromBytes(data);
                    return;
                }

                DialogResult dr = MessageBox.Show("The file you opened couldn't be identified."
                    + Environment.NewLine
                    + Environment.NewLine + "This can happen in two cases:"
                    + Environment.NewLine + "- The file you inputed is invalid"
                    + Environment.NewLine + "- The file you inputed is a v1.0.0 DAT file, which doesn't have identification sections"
                    + Environment.NewLine
                    + Environment.NewLine + "If you want to open the file as a v1.0.0 DAT file, click OK."
                    + Environment.NewLine + "Otherwise, click Cancel."
                    , "How do you want your file opened?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.Cancel)
                    throw new Exception("You chose not to open the file.");

                string asked = Common.AskForType();

                if(asked == "Spritesheet")
                {
                    section = new SpriteSection(data, isHeaderless);
                }
                else
                    throw new Exception("You cancelled file opening.");
            }

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
