namespace YapayZekaVideoProjesi
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtPrompt = new TextBox();
            btnGenerate = new Button();
            rtbLog = new RichTextBox();
            SuspendLayout();
            // 
            // txtPrompt
            // 
            txtPrompt.BackColor = Color.LavenderBlush;
            txtPrompt.Location = new Point(25, 37);
            txtPrompt.Multiline = true;
            txtPrompt.Name = "txtPrompt";
            txtPrompt.PlaceholderText = "Hikaye konusunu (prompt) buraya giriniz...";
            txtPrompt.Size = new Size(650, 109);
            txtPrompt.TabIndex = 0;
            txtPrompt.TextChanged += txtPrompt_TextChanged;
            // 
            // btnGenerate
            // 
            btnGenerate.BackColor = Color.LavenderBlush;
            btnGenerate.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnGenerate.ForeColor = Color.Maroon;
            btnGenerate.Location = new Point(25, 152);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(150, 45);
            btnGenerate.TabIndex = 1;
            btnGenerate.Text = "Hikaye Üret.\r\n";
            btnGenerate.UseVisualStyleBackColor = false;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // rtbLog
            // 
            rtbLog.BackColor = Color.LavenderBlush;
            rtbLog.Location = new Point(25, 226);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(730, 284);
            rtbLog.TabIndex = 2;
            rtbLog.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FloralWhite;
            ClientSize = new Size(782, 553);
            Controls.Add(rtbLog);
            Controls.Add(btnGenerate);
            Controls.Add(txtPrompt);
            Name = "Form1";
            Text = "Yapay Zeka Entegrasyonlu Video Üretme Projesi ";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtPrompt;
        private Button btnGenerate;
        private RichTextBox rtbLog;
    }
}
