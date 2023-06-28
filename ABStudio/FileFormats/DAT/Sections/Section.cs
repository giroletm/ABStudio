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
        private abstract class Section
        {
            public abstract string Magic { get; }

            public Section() { }
            public Section(byte[] data)
            {
                FromBytes(data);
            }

            public abstract byte[] AsJSONBytes();

            public byte[] AsBytes()
            {
                List<byte> rawdata = new List<byte>();
                foreach (char c in Magic)
                    rawdata.Add((byte)(c & 0xFF));

                byte[] actualData = this.AsCoreBytes();
                rawdata.AddUInt32((uint)actualData.Length);

                return rawdata.Concat(actualData).ToArray();
            }

            public void FromBytes(byte[] data)
            {
                for (int c = 0; c < Magic.Length; c++)
                {
                    if (data[c] != Magic[c])
                        throw new Exception("A " + this.GetType().Name + " section was initiated with an unmatching byte array.");
                }

                byte[] rawdata = data.Skip(8).Take((int)data.GetUInt32(4)).ToArray();

                this.FromCoreBytes(rawdata);
            }

            protected abstract byte[] AsCoreBytes();
            protected abstract void FromCoreBytes(byte[] data);
        }
    }
}
