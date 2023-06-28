
namespace ABStudio.Forms
{
    partial class ComboAskForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.questionComboBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // questionComboBox
            // 
            this.questionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.questionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.questionComboBox.FormattingEnabled = true;
            this.questionComboBox.Location = new System.Drawing.Point(12, 12);
            this.questionComboBox.Name = "questionComboBox";
            this.questionComboBox.Size = new System.Drawing.Size(317, 21);
            this.questionComboBox.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(131, 39);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // ComboAskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 74);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.questionComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComboAskForm";
            this.ShowIcon = false;
            this.Text = "Question";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox questionComboBox;
        private System.Windows.Forms.Button okButton;
    }
}