using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABStudio.Forms
{
    public partial class ComboAskForm : Form
    {
        public int ComboIndex
        {
            get => questionComboBox.SelectedIndex;
            set => questionComboBox.SelectedIndex = value;
        }

        public ComboAskForm(string[] coll, string question)
        {
            InitializeComponent();

            this.Text = question;
            int winSize = TextRenderer.MeasureText(question, this.Font).Width + 97;
                
            questionComboBox.Items.Clear();
            for (int i = 0; i < coll.Length; i++)
            {
                questionComboBox.Items.Add(coll[i]);
                int maybeWinSize = TextRenderer.MeasureText(coll[i], questionComboBox.Font).Width + 66;
                if (maybeWinSize > winSize)
                    winSize = maybeWinSize;
            }

            if (questionComboBox.Items.Count > 0)
                questionComboBox.SelectedIndex = 0;

            this.Width = winSize;
            okButton.Location = new Point(winSize / 2 - okButton.Width / 2, okButton.Location.Y);
        }
    }
}
