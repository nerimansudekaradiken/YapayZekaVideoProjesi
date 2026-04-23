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
            picStoryImage = new PictureBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)picStoryImage).BeginInit();
            SuspendLayout();
            // 
            // txtPrompt
            // 
            txtPrompt.BackColor = Color.LavenderBlush;
            txtPrompt.Location = new Point(12, 37);
            txtPrompt.Multiline = true;
            txtPrompt.Name = "txtPrompt";
            txtPrompt.PlaceholderText = "Hikaye konusunu (prompt) buraya giriniz...";
            txtPrompt.Size = new Size(526, 109);
            txtPrompt.TabIndex = 0;
            txtPrompt.TextChanged += txtPrompt_TextChanged;
            // 
            // btnGenerate
            // 
            btnGenerate.BackColor = Color.LavenderBlush;
            btnGenerate.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnGenerate.ForeColor = Color.Maroon;
            btnGenerate.Location = new Point(12, 152);
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
            rtbLog.Location = new Point(12, 203);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(526, 284);
            rtbLog.TabIndex = 2;
            rtbLog.Text = "";
            // 
            // picStoryImage
            // 
            picStoryImage.BackColor = Color.LavenderBlush;
            picStoryImage.BorderStyle = BorderStyle.FixedSingle;
            picStoryImage.Location = new Point(591, 37);
            picStoryImage.Name = "picStoryImage";
            picStoryImage.Size = new Size(562, 388);
            picStoryImage.SizeMode = PictureBoxSizeMode.Zoom;
            picStoryImage.TabIndex = 4;
            picStoryImage.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.LavenderBlush;
            label1.Location = new Point(591, 428);
            label1.Name = "label1";
            label1.Size = new Size(104, 20);
            label1.TabIndex = 5;
            label1.Text = "Hikaye Görseli";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FloralWhite;
            ClientSize = new Size(1280, 524);
            Controls.Add(label1);
            Controls.Add(picStoryImage);
            Controls.Add(rtbLog);
            Controls.Add(btnGenerate);
            Controls.Add(txtPrompt);
            Name = "Form1";
            Text = "Yapay Zeka Entegrasyonlu Video Üretme Projesi ";
            ((System.ComponentModel.ISupportInitialize)picStoryImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtPrompt;
        private Button btnGenerate;
        private RichTextBox rtbLog;
        private PictureBox picStoryImage;
        private Label label1;
    }
}
