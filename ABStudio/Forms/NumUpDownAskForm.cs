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
    public partial class NumUpDownAskForm : Form
    {
        public string Title
        {
            set => this.Text = value;
        }
        public decimal NumValue
        {
            get => questionNumUpDown.Value;
            set => questionNumUpDown.Value = value;
        }

        public NumUpDownAskForm(decimal defaultValue, decimal minValue, decimal maxValue, decimal increment=1.0m)
        {
            InitializeComponent();

            questionNumUpDown.DecimalPlaces = BitConverter.GetBytes(decimal.GetBits(increment)[3])[2]; // https://stackoverflow.com/a/13493771

            if (increment == 1)
                questionNumUpDown.DecimalPlaces = 0;

            questionNumUpDown.Increment = increment;
            questionNumUpDown.Minimum = minValue;
            questionNumUpDown.Maximum = maxValue;
            questionNumUpDown.Value = defaultValue;
        }
    }
}
