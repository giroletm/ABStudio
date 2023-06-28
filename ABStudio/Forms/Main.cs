using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABStudio.FileFormats.DAT;
using ABStudio.Misc;

namespace ABStudio.Forms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }


        private void Main_Load(object sender, EventArgs e)
        {
            Main_Resize(sender, e);

            foreach(string displayName in Common.DisplayNames)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(displayName);
                item.Name = "new" + displayName + "ToolStripMenuItem";
                item.Click += new System.EventHandler(newFileToolStripMenuItem_Click);

                newToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            int wPerButton = (this.Size.Width - 46) / 2;
            int hPerButton = this.Size.Height - 78;
            newButton.Size = new Size(wPerButton, hPerButton);
            openButton.Location = new Point(this.Size.Width - wPerButton - 28, openButton.Location.Y);
            openButton.Size = new Size(wPerButton, hPerButton);
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fullName = (sender as ToolStripMenuItem).Name;
            string displayName = fullName.Substring(3, fullName.Length - 20);

            DATFile file = Common.MakeNew(displayName);

            if (file != null)
                Common.OpenEditor("", file);
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            DATFile file = Common.AskForNew();

            if (file != null)
                Common.OpenEditor("", file);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) => Common.OpenFileInNewEditor();
    }
}
