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
    public partial class MCQAskForm : Form
    {
        public int ChosenAnswer => chosenAnswer;

        private int chosenAnswer;

        public MCQAskForm(string[] answers, string question)
        {
            InitializeComponent();

            this.Text = question;
            int winSize = TextRenderer.MeasureText(question, this.Font).Width + 97;

            int currX = 12;
            for (int i = 0; i < answers.Length; i++)
            {
                Button btn = new Button();
                btn.Location = new Point(currX, 12);
                btn.Text = answers[i];
                btn.Tag = i;

                btn.Width = TextRenderer.MeasureText(answers[i], btn.Font).Width + 38;
                currX += btn.Width + 6;

                btn.Click += new EventHandler(this.answerButton_Click);
                this.Controls.Add(btn);
            }

            this.Width = Math.Max(winSize, currX + 22);
        }

        private void answerButton_Click(object sender, EventArgs e)
        {
            this.chosenAnswer = (int)((sender as Button).Tag);
            this.DialogResult = DialogResult.OK;
        }
    }
}
