namespace grass
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
            components = new System.ComponentModel.Container();
            pictureBox1 = new PictureBox();
            timer1 = new System.Windows.Forms.Timer(components);
            button1 = new Button();
            checkBox1 = new CheckBox();
            label1 = new Label();
            label3 = new Label();
            GrassNoresp = new Label();
            label4 = new Label();
            GrassLimit_CB = new CheckBox();
            GrassBox = new GroupBox();
            label2 = new Label();
            textBox1 = new TextBox();
            groupBox1 = new GroupBox();
            groupBox3 = new GroupBox();
            label5 = new Label();
            textBox2 = new TextBox();
            OrgLimit_CB = new CheckBox();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            button2 = new Button();
            checkBox2 = new CheckBox();
            groupBox2 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            GrassBox.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ControlLightLight;
            pictureBox1.Location = new Point(12, 9);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1090, 640);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1;
            timer1.Tick += timer1_Tick;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.Location = new Point(160, 76);
            button1.Name = "button1";
            button1.Size = new Size(50, 23);
            button1.TabIndex = 1;
            button1.Text = "Reset";
            button1.UseVisualStyleBackColor = true;
            button1.Visible = false;
            button1.Click += button1_Click;
            // 
            // checkBox1
            // 
            checkBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(6, 17);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(78, 19);
            checkBox1.TabIndex = 2;
            checkBox1.Text = "StartGrow";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(6, 65);
            label1.Name = "label1";
            label1.Size = new Size(19, 15);
            label1.TabIndex = 3;
            label1.Text = "20";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(45, 65);
            label3.Name = "label3";
            label3.Size = new Size(53, 15);
            label3.TabIndex = 4;
            label3.Text = "GrassList";
            // 
            // GrassNoresp
            // 
            GrassNoresp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GrassNoresp.AutoSize = true;
            GrassNoresp.Location = new Point(45, 84);
            GrassNoresp.Name = "GrassNoresp";
            GrassNoresp.Size = new Size(31, 15);
            GrassNoresp.TabIndex = 5;
            GrassNoresp.Text = "GNR";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(6, 84);
            label4.Name = "label4";
            label4.Size = new Size(13, 15);
            label4.TabIndex = 6;
            label4.Text = "0";
            // 
            // GrassLimit_CB
            // 
            GrassLimit_CB.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GrassLimit_CB.AutoSize = true;
            GrassLimit_CB.Location = new Point(141, 12);
            GrassLimit_CB.Name = "GrassLimit_CB";
            GrassLimit_CB.Size = new Size(69, 19);
            GrassLimit_CB.TabIndex = 7;
            GrassLimit_CB.Text = "NoLimit";
            GrassLimit_CB.UseVisualStyleBackColor = true;
            // 
            // GrassBox
            // 
            GrassBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GrassBox.Controls.Add(button1);
            GrassBox.Controls.Add(checkBox1);
            GrassBox.Controls.Add(label2);
            GrassBox.Controls.Add(textBox1);
            GrassBox.Controls.Add(GrassLimit_CB);
            GrassBox.Controls.Add(label4);
            GrassBox.Controls.Add(label3);
            GrassBox.Controls.Add(label1);
            GrassBox.Controls.Add(GrassNoresp);
            GrassBox.Location = new Point(6, 10);
            GrassBox.Name = "GrassBox";
            GrassBox.Size = new Size(216, 107);
            GrassBox.TabIndex = 8;
            GrassBox.TabStop = false;
            GrassBox.Text = "Grass";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(45, 42);
            label2.Name = "label2";
            label2.Size = new Size(62, 15);
            label2.TabIndex = 10;
            label2.Text = "GrassLimit";
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox1.AutoCompleteCustomSource.AddRange(new string[] { "10000" });
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Location = new Point(6, 42);
            textBox1.MaxLength = 5;
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "MaxGrass";
            textBox1.Size = new Size(33, 16);
            textBox1.TabIndex = 9;
            textBox1.TabStop = false;
            textBox1.Text = "10000";
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.WordWrap = false;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Controls.Add(GrassBox);
            groupBox1.Location = new Point(1108, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(228, 647);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(textBox2);
            groupBox3.Controls.Add(OrgLimit_CB);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(button2);
            groupBox3.Controls.Add(checkBox2);
            groupBox3.Location = new Point(6, 123);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(216, 108);
            groupBox3.TabIndex = 10;
            groupBox3.TabStop = false;
            groupBox3.Text = "Organisms";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(45, 36);
            label5.Name = "label5";
            label5.Size = new Size(86, 15);
            label5.TabIndex = 17;
            label5.Text = "OrganismLimit";
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox2.AutoCompleteCustomSource.AddRange(new string[] { "10000" });
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Location = new Point(6, 36);
            textBox2.MaxLength = 5;
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "MaxGrass";
            textBox2.Size = new Size(33, 16);
            textBox2.TabIndex = 16;
            textBox2.TabStop = false;
            textBox2.Text = "100";
            textBox2.TextAlign = HorizontalAlignment.Center;
            textBox2.WordWrap = false;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // OrgLimit_CB
            // 
            OrgLimit_CB.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            OrgLimit_CB.AutoSize = true;
            OrgLimit_CB.Location = new Point(141, 22);
            OrgLimit_CB.Name = "OrgLimit_CB";
            OrgLimit_CB.Size = new Size(69, 19);
            OrgLimit_CB.TabIndex = 15;
            OrgLimit_CB.Text = "NoLimit";
            OrgLimit_CB.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(6, 84);
            label6.Name = "label6";
            label6.Size = new Size(13, 15);
            label6.TabIndex = 14;
            label6.Text = "0";
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(45, 59);
            label7.Name = "label7";
            label7.Size = new Size(77, 15);
            label7.TabIndex = 12;
            label7.Text = "OrganismList";
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(6, 59);
            label8.Name = "label8";
            label8.Size = new Size(19, 15);
            label8.TabIndex = 11;
            label8.Text = "20";
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label9.AutoSize = true;
            label9.Location = new Point(45, 84);
            label9.Name = "label9";
            label9.Size = new Size(32, 15);
            label9.TabIndex = 13;
            label9.Text = "ONR";
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.Location = new Point(160, 76);
            button2.Name = "button2";
            button2.Size = new Size(50, 23);
            button2.TabIndex = 3;
            button2.Text = "Reset";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            // 
            // checkBox2
            // 
            checkBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(6, 14);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(96, 19);
            checkBox2.TabIndex = 4;
            checkBox2.Text = "StartSimulate";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Location = new Point(6, 541);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(216, 100);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(1342, 661);
            Controls.Add(groupBox1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            GrassBox.ResumeLayout(false);
            GrassBox.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        public System.Windows.Forms.Timer timer1;
        private Button button1;
        private CheckBox checkBox1;
        private Label label1;
        private Label label3;
        public PictureBox pictureBox1;
        private Label GrassNoresp;
        private Label label4;
        private CheckBox GrassLimit_CB;
        private GroupBox GrassBox;
        private TextBox textBox1;
        private Label label2;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label5;
        private TextBox textBox2;
        private CheckBox OrgLimit_CB;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Button button2;
        private CheckBox checkBox2;

        
    }
}