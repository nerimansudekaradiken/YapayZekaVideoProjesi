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
            picSahne1 = new PictureBox();
            picSahne2 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            picSahne3 = new PictureBox();
            label3 = new Label();
            picSahne4 = new PictureBox();
            label4 = new Label();
            btnVideoUret = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)picSahne1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSahne2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSahne3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSahne4).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtPrompt
            // 
            txtPrompt.BackColor = Color.LavenderBlush;
            txtPrompt.Location = new Point(12, 24);
            txtPrompt.Multiline = true;
            txtPrompt.Name = "txtPrompt";
            txtPrompt.PlaceholderText = "Hikaye konusunu (prompt) buraya giriniz...";
            txtPrompt.ScrollBars = ScrollBars.Vertical;
            txtPrompt.Size = new Size(534, 109);
            txtPrompt.TabIndex = 0;
            // 
            // btnGenerate
            // 
            btnGenerate.BackColor = Color.LavenderBlush;
            btnGenerate.Font = new Font("Times New Roman", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            btnGenerate.ForeColor = Color.Maroon;
            btnGenerate.Location = new Point(12, 149);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(150, 45);
            btnGenerate.TabIndex = 1;
            btnGenerate.Text = "✍️ Hikaye Üret";
            btnGenerate.UseVisualStyleBackColor = false;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // rtbLog
            // 
            rtbLog.BackColor = Color.LavenderBlush;
            rtbLog.Location = new Point(12, 209);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(534, 301);
            rtbLog.TabIndex = 2;
            rtbLog.Text = "";
            // 
            // picSahne1
            // 
            picSahne1.BackColor = Color.LavenderBlush;
            picSahne1.BorderStyle = BorderStyle.FixedSingle;
            picSahne1.Location = new Point(3, 3);
            picSahne1.Name = "picSahne1";
            picSahne1.Size = new Size(244, 215);
            picSahne1.SizeMode = PictureBoxSizeMode.Zoom;
            picSahne1.TabIndex = 4;
            picSahne1.TabStop = false;
            // 
            // picSahne2
            // 
            picSahne2.BackColor = Color.LavenderBlush;
            picSahne2.BorderStyle = BorderStyle.FixedSingle;
            picSahne2.Location = new Point(253, 3);
            picSahne2.Name = "picSahne2";
            picSahne2.Size = new Size(245, 215);
            picSahne2.SizeMode = PictureBoxSizeMode.Zoom;
            picSahne2.TabIndex = 5;
            picSahne2.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.LavenderBlush;
            label1.Location = new Point(640, 242);
            label1.Name = "label1";
            label1.Size = new Size(61, 20);
            label1.TabIndex = 6;
            label1.Text = "Sahne 1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.LavenderBlush;
            label2.Location = new Point(890, 242);
            label2.Name = "label2";
            label2.Size = new Size(61, 20);
            label2.TabIndex = 7;
            label2.Text = "Sahne 2";
            // 
            // picSahne3
            // 
            picSahne3.BackColor = Color.LavenderBlush;
            picSahne3.BorderStyle = BorderStyle.FixedSingle;
            picSahne3.Location = new Point(3, 248);
            picSahne3.Name = "picSahne3";
            picSahne3.Size = new Size(244, 215);
            picSahne3.SizeMode = PictureBoxSizeMode.Zoom;
            picSahne3.TabIndex = 8;
            picSahne3.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.LavenderBlush;
            label3.Location = new Point(640, 490);
            label3.Name = "label3";
            label3.Size = new Size(61, 20);
            label3.TabIndex = 9;
            label3.Text = "Sahne 3";
            // 
            // picSahne4
            // 
            picSahne4.BackColor = Color.LavenderBlush;
            picSahne4.BorderStyle = BorderStyle.FixedSingle;
            picSahne4.Location = new Point(253, 248);
            picSahne4.Name = "picSahne4";
            picSahne4.Size = new Size(245, 215);
            picSahne4.SizeMode = PictureBoxSizeMode.Zoom;
            picSahne4.TabIndex = 10;
            picSahne4.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.LavenderBlush;
            label4.Location = new Point(890, 490);
            label4.Name = "label4";
            label4.Size = new Size(61, 20);
            label4.TabIndex = 11;
            label4.Text = "Sahne 4";
            // 
            // btnVideoUret
            // 
            btnVideoUret.BackColor = Color.LavenderBlush;
            btnVideoUret.Font = new Font("Times New Roman", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            btnVideoUret.ForeColor = Color.Maroon;
            btnVideoUret.Location = new Point(168, 149);
            btnVideoUret.Name = "btnVideoUret";
            btnVideoUret.Size = new Size(150, 45);
            btnVideoUret.TabIndex = 12;
            btnVideoUret.Text = "🎬 Video Oluştur";
            btnVideoUret.UseVisualStyleBackColor = false;
            btnVideoUret.Click += btnVideoUret_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Linen;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(picSahne4, 1, 1);
            tableLayoutPanel1.Controls.Add(picSahne3, 0, 1);
            tableLayoutPanel1.Controls.Add(picSahne1, 0, 0);
            tableLayoutPanel1.Controls.Add(picSahne2, 1, 0);
            tableLayoutPanel1.Location = new Point(632, 24);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(501, 491);
            tableLayoutPanel1.TabIndex = 13;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FloralWhite;
            ClientSize = new Size(1257, 717);
            Controls.Add(btnVideoUret);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(rtbLog);
            Controls.Add(btnGenerate);
            Controls.Add(txtPrompt);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Text = "Yapay Zeka Entegrasyonlu Video Üretme Projesi ";
            ((System.ComponentModel.ISupportInitialize)picSahne1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSahne2).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSahne3).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSahne4).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtPrompt;
        private Button btnGenerate;
        private RichTextBox rtbLog;
        private PictureBox picSahne1;
        private PictureBox picSahne2;
        private Label label1;
        private Label label2;
        private PictureBox picSahne3;
        private Label label3;
        private PictureBox picSahne4;
        private Label label4;
        private Button btnVideoUret;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
