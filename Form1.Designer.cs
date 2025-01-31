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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            timer1 = new System.Windows.Forms.Timer(components);
            button1 = new Button();
            checkBox1 = new CheckBox();
            label1 = new Label();
            label3 = new Label();
            GrassNoresp = new Label();
            label4 = new Label();
            GrassLimit_CB = new CheckBox();
            GrassBox = new GroupBox();
            groupBox4 = new GroupBox();
            trackBar2 = new TrackBar();
            label14 = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            groupBox1 = new GroupBox();
            label16 = new Label();
            label15 = new Label();
            trackBar3 = new TrackBar();
            pictureBox2 = new PictureBox();
            groupBox3 = new GroupBox();
            groupBox8 = new GroupBox();
            trackBar5 = new TrackBar();
            trackBar4 = new TrackBar();
            label17 = new Label();
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
            label13 = new Label();
            label12 = new Label();
            trackBar1 = new TrackBar();
            label11 = new Label();
            label10 = new Label();
            panel1 = new Panel();
            button3 = new Button();
            groupBox6 = new GroupBox();
            listBox1 = new ListBox();
            pictureBox4 = new PictureBox();
            comboBox1 = new ComboBox();
            panel3 = new Panel();
            pictureBox3 = new PictureBox();
            label19 = new Label();
            label18 = new Label();
            button4 = new Button();
            groupBox7 = new GroupBox();
            label20 = new Label();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            GrassBox.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            groupBox3.SuspendLayout();
            groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar4).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            panel1.SuspendLayout();
            groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            groupBox7.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
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
            button1.Location = new Point(85, 38);
            button1.Name = "button1";
            button1.Size = new Size(57, 22);
            button1.TabIndex = 1;
            button1.Text = "Kill Half";
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
            checkBox1.Size = new Size(50, 19);
            checkBox1.TabIndex = 2;
            checkBox1.Text = "Start";
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
            label3.Size = new Size(80, 15);
            label3.TabIndex = 4;
            label3.Text = "Grass amount";
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
            GrassLimit_CB.Location = new Point(62, 17);
            GrassLimit_CB.Name = "GrassLimit_CB";
            GrassLimit_CB.Size = new Size(69, 19);
            GrassLimit_CB.TabIndex = 7;
            GrassLimit_CB.Text = "NoLimit";
            GrassLimit_CB.UseVisualStyleBackColor = true;
            // 
            // GrassBox
            // 
            GrassBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GrassBox.Controls.Add(groupBox4);
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
            // groupBox4
            // 
            groupBox4.Controls.Add(trackBar2);
            groupBox4.Controls.Add(label14);
            groupBox4.Location = new Point(150, 12);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(60, 89);
            groupBox4.TabIndex = 10;
            groupBox4.TabStop = false;
            groupBox4.Text = "SunLVL";
            // 
            // trackBar2
            // 
            trackBar2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar2.AutoSize = false;
            trackBar2.Location = new Point(29, 13);
            trackBar2.Maximum = 50;
            trackBar2.Minimum = 1;
            trackBar2.Name = "trackBar2";
            trackBar2.Orientation = Orientation.Vertical;
            trackBar2.Size = new Size(25, 74);
            trackBar2.TabIndex = 11;
            trackBar2.TickFrequency = 3;
            trackBar2.TickStyle = TickStyle.TopLeft;
            trackBar2.Value = 10;
            trackBar2.Scroll += trackBar2_Scroll;
            // 
            // label14
            // 
            label14.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label14.AutoSize = true;
            label14.Location = new Point(6, 19);
            label14.Name = "label14";
            label14.Size = new Size(19, 15);
            label14.TabIndex = 13;
            label14.Text = "50";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(45, 42);
            label2.Name = "label2";
            label2.Size = new Size(34, 15);
            label2.TabIndex = 10;
            label2.Text = "Limit";
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
            textBox1.Text = "15000";
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.WordWrap = false;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox1.Controls.Add(label16);
            groupBox1.Controls.Add(label15);
            groupBox1.Controls.Add(trackBar3);
            groupBox1.Controls.Add(pictureBox2);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Controls.Add(GrassBox);
            groupBox1.Location = new Point(1110, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(228, 510);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(82, 297);
            label16.Name = "label16";
            label16.Size = new Size(55, 15);
            label16.TabIndex = 14;
            label16.Text = "Minimap";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(82, 484);
            label15.Name = "label15";
            label15.Size = new Size(57, 15);
            label15.TabIndex = 13;
            label15.Text = "x1  Zoom";
            // 
            // trackBar3
            // 
            trackBar3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar3.AutoSize = false;
            trackBar3.Location = new Point(6, 484);
            trackBar3.Maximum = 3;
            trackBar3.Minimum = 1;
            trackBar3.Name = "trackBar3";
            trackBar3.Size = new Size(70, 22);
            trackBar3.TabIndex = 1;
            trackBar3.Value = 1;
            trackBar3.ValueChanged += TrackBar3_ValueChanged;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Black;
            pictureBox2.Location = new Point(6, 315);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(216, 163);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 12;
            pictureBox2.TabStop = false;
            pictureBox2.MouseClick += pictureBox2_MouseClick;
            pictureBox2.MouseMove += pictureBox2_MouseMove;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(groupBox8);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(textBox2);
            groupBox3.Controls.Add(OrgLimit_CB);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(button2);
            groupBox3.Controls.Add(checkBox2);
            groupBox3.Location = new Point(6, 117);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(216, 114);
            groupBox3.TabIndex = 10;
            groupBox3.TabStop = false;
            groupBox3.Text = "Organisms";
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(trackBar5);
            groupBox8.Controls.Add(trackBar4);
            groupBox8.Controls.Add(label17);
            groupBox8.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox8.Location = new Point(150, 10);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(60, 98);
            groupBox8.TabIndex = 18;
            groupBox8.TabStop = false;
            groupBox8.Text = "Radiation";
            // 
            // trackBar5
            // 
            trackBar5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar5.AutoSize = false;
            trackBar5.LargeChange = 10;
            trackBar5.Location = new Point(29, 15);
            trackBar5.Maximum = 900;
            trackBar5.Minimum = 10;
            trackBar5.Name = "trackBar5";
            trackBar5.Orientation = Orientation.Vertical;
            trackBar5.Size = new Size(25, 77);
            trackBar5.SmallChange = 10;
            trackBar5.TabIndex = 14;
            trackBar5.TickFrequency = 3;
            trackBar5.TickStyle = TickStyle.TopLeft;
            trackBar5.Value = 10;
            trackBar5.Scroll += trackBar5_Scroll;
            // 
            // trackBar4
            // 
            trackBar4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar4.AutoSize = false;
            trackBar4.Location = new Point(-111, 13);
            trackBar4.Maximum = 50;
            trackBar4.Minimum = 1;
            trackBar4.Name = "trackBar4";
            trackBar4.Orientation = Orientation.Vertical;
            trackBar4.Size = new Size(25, 74);
            trackBar4.TabIndex = 11;
            trackBar4.TickFrequency = 3;
            trackBar4.TickStyle = TickStyle.TopLeft;
            trackBar4.Value = 10;
            // 
            // label17
            // 
            label17.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label17.Location = new Point(6, 19);
            label17.Name = "label17";
            label17.Size = new Size(25, 15);
            label17.TabIndex = 13;
            label17.Text = "100";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(45, 36);
            label5.Name = "label5";
            label5.Size = new Size(34, 15);
            label5.TabIndex = 17;
            label5.Text = "Limit";
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox2.AutoCompleteCustomSource.AddRange(new string[] { "10000" });
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Location = new Point(6, 36);
            textBox2.MaxLength = 4;
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "MaxGrass";
            textBox2.Size = new Size(33, 16);
            textBox2.TabIndex = 16;
            textBox2.TabStop = false;
            textBox2.Text = "2000";
            textBox2.TextAlign = HorizontalAlignment.Center;
            textBox2.WordWrap = false;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // OrgLimit_CB
            // 
            OrgLimit_CB.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            OrgLimit_CB.AutoSize = true;
            OrgLimit_CB.Location = new Point(62, 14);
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
            label7.Size = new Size(65, 15);
            label7.TabIndex = 12;
            label7.Text = "Population";
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
            button2.Location = new Point(85, 31);
            button2.Name = "button2";
            button2.Size = new Size(57, 24);
            button2.TabIndex = 3;
            button2.Text = "Kill Half";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // checkBox2
            // 
            checkBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(6, 14);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(50, 19);
            checkBox2.TabIndex = 4;
            checkBox2.Text = "Start";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(trackBar1);
            groupBox2.Location = new Point(6, 233);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(216, 61);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Tick interval";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(6, 37);
            label13.Name = "label13";
            label13.Size = new Size(63, 15);
            label13.TabIndex = 12;
            label13.Text = "TickInteval";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(6, 19);
            label12.Name = "label12";
            label12.Size = new Size(31, 15);
            label12.TabIndex = 1;
            label12.Text = "1000";
            // 
            // trackBar1
            // 
            trackBar1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar1.AutoSize = false;
            trackBar1.Location = new Point(45, 12);
            trackBar1.Maximum = 1000;
            trackBar1.Minimum = 1;
            trackBar1.MinimumSize = new Size(0, 15);
            trackBar1.Name = "trackBar1";
            trackBar1.RightToLeft = RightToLeft.Yes;
            trackBar1.RightToLeftLayout = true;
            trackBar1.Size = new Size(165, 22);
            trackBar1.SmallChange = 5;
            trackBar1.TabIndex = 2;
            trackBar1.TickFrequency = 30;
            trackBar1.Value = 1000;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(101, 233);
            label11.Name = "label11";
            label11.Size = new Size(28, 15);
            label11.TabIndex = 0;
            label11.Text = "Age";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(101, 252);
            label10.Name = "label10";
            label10.Size = new Size(89, 15);
            label10.TabIndex = 11;
            label10.Text = "Food/MaxFood";
            // 
            // panel1
            // 
            panel1.Controls.Add(button3);
            panel1.Controls.Add(groupBox6);
            panel1.Location = new Point(855, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(249, 658);
            panel1.TabIndex = 1;
            // 
            // button3
            // 
            button3.Location = new Point(14, 624);
            button3.Name = "button3";
            button3.Size = new Size(45, 23);
            button3.TabIndex = 0;
            button3.Text = "Close";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(listBox1);
            groupBox6.Controls.Add(pictureBox4);
            groupBox6.Controls.Add(comboBox1);
            groupBox6.Controls.Add(panel3);
            groupBox6.Controls.Add(label19);
            groupBox6.Controls.Add(label18);
            groupBox6.Controls.Add(label11);
            groupBox6.Controls.Add(label10);
            groupBox6.Location = new Point(14, 0);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(228, 618);
            groupBox6.TabIndex = 1;
            groupBox6.TabStop = false;
            groupBox6.Text = "Organism Status";
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.Gray;
            listBox1.FormattingEnabled = true;
            listBox1.ImeMode = ImeMode.On;
            listBox1.ItemHeight = 15;
            listBox1.Items.AddRange(new object[] { "" });
            listBox1.Location = new Point(8, 299);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(210, 304);
            listBox1.TabIndex = 17;
            // 
            // pictureBox4
            // 
            pictureBox4.Location = new Point(8, 270);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(26, 24);
            pictureBox4.TabIndex = 16;
            pictureBox4.TabStop = false;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(40, 270);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(89, 23);
            comboBox1.TabIndex = 15;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // panel3
            // 
            panel3.Controls.Add(pictureBox3);
            panel3.Location = new Point(6, 15);
            panel3.Name = "panel3";
            panel3.Size = new Size(216, 210);
            panel3.TabIndex = 14;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.Black;
            pictureBox3.Location = new Point(0, 0);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(216, 210);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 0;
            pictureBox3.TabStop = false;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(6, 252);
            label19.Name = "label19";
            label19.Size = new Size(89, 15);
            label19.TabIndex = 13;
            label19.Text = "Food/MaxFood";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(6, 235);
            label18.Name = "label18";
            label18.Size = new Size(28, 15);
            label18.TabIndex = 12;
            label18.Text = "Age";
            // 
            // button4
            // 
            button4.Location = new Point(1110, 626);
            button4.Name = "button4";
            button4.Size = new Size(102, 23);
            button4.TabIndex = 14;
            button4.Text = "Organism Status";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(label20);
            groupBox7.Location = new Point(1110, 514);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(228, 106);
            groupBox7.TabIndex = 15;
            groupBox7.TabStop = false;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(6, 19);
            label20.Name = "label20";
            label20.Size = new Size(44, 15);
            label20.TabIndex = 0;
            label20.Text = "label20";
            // 
            // panel2
            // 
            panel2.Controls.Add(pictureBox1);
            panel2.Location = new Point(12, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(1078, 631);
            panel2.TabIndex = 1;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.None;
            pictureBox1.BackColor = Color.Black;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1078, 631);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseClick += pictureBox1_MouseClick;
            pictureBox1.MouseWheel += pictureBox1_MouseWheel;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(1344, 661);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(button4);
            Controls.Add(groupBox7);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "MicroLife simulator";
            GrassBox.ResumeLayout(false);
            GrassBox.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar5).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar4).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            panel1.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }


        #endregion
        public System.Windows.Forms.Timer timer1;
        private Button button1;
        private CheckBox checkBox1;
        private Label label1;
        private Label label3;
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
        private Label label10;
        private PictureBox pictureBox2;
        private Label label12;
        private Label label11;
        private TrackBar trackBar1;
        private TrackBar trackBar2;
        private Label label14;
        private GroupBox groupBox4;
        private Label label13;
        private TrackBar trackBar3;
        private Label label15;
        private Panel panel1;
        private Button button3;
        private Button button4;
        private GroupBox groupBox6;
        private PictureBox pictureBox3;
        private GroupBox groupBox7;
        private Label label16;
        private GroupBox groupBox8;
        private TrackBar trackBar4;
        private Label label17;
        private TrackBar trackBar5;
        private Label label19;
        private Label label18;
        private Panel panel2;
        public PictureBox pictureBox1;
        private Panel panel3;
        private PictureBox pictureBox4;
        private ComboBox comboBox1;
        private ListBox listBox1;
        private Label label20;
    }
}