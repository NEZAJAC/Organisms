namespace MicroLife_Simulator
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
            GrassLimit_CB = new CheckBox();
            GrassBox = new GroupBox();
            checkBox2 = new CheckBox();
            button8 = new Button();
            groupBox5 = new GroupBox();
            trackBar7 = new TrackBar();
            trackBar6 = new TrackBar();
            label24 = new Label();
            groupBox4 = new GroupBox();
            trackBar2 = new TrackBar();
            label14 = new Label();
            groupBox1 = new GroupBox();
            label16 = new Label();
            label15 = new Label();
            pictureBox2 = new PictureBox();
            groupBox3 = new GroupBox();
            trackBar3 = new TrackBar();
            AutoKill = new CheckBox();
            label21 = new Label();
            groupBox8 = new GroupBox();
            trackBar5 = new TrackBar();
            trackBar4 = new TrackBar();
            label17 = new Label();
            label20 = new Label();
            label5 = new Label();
            textBox2 = new TextBox();
            OrgLimit_CB = new CheckBox();
            label7 = new Label();
            label8 = new Label();
            button2 = new Button();
            groupBox2 = new GroupBox();
            label23 = new Label();
            label22 = new Label();
            label13 = new Label();
            label12 = new Label();
            trackBar1 = new TrackBar();
            label11 = new Label();
            label10 = new Label();
            panel1 = new Panel();
            button3 = new Button();
            groupBox6 = new GroupBox();
            label25 = new Label();
            progressBar1 = new ProgressBar();
            label3 = new Label();
            label2 = new Label();
            label6 = new Label();
            label4 = new Label();
            listBox1 = new ListBox();
            panel3 = new Panel();
            comboBox1 = new ComboBox();
            pictureBox4 = new PictureBox();
            label9 = new Label();
            pictureBox3 = new PictureBox();
            label19 = new Label();
            label18 = new Label();
            button7 = new Button();
            button5 = new Button();
            button4 = new Button();
            groupBox7 = new GroupBox();
            button10 = new Button();
            button9 = new Button();
            panel2 = new Panel();
            panel4 = new Panel();
            button6 = new Button();
            textBox3 = new TextBox();
            pictureBox1 = new PictureBox();
            button11 = new Button();
            GrassBox.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar6).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar3).BeginInit();
            groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar4).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            panel1.SuspendLayout();
            groupBox6.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            groupBox7.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
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
            button1.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(6, 92);
            button1.Name = "button1";
            button1.Size = new Size(57, 22);
            button1.TabIndex = 1;
            button1.Text = "Clean ";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // checkBox1
            // 
            checkBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(6, 12);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(48, 19);
            checkBox1.TabIndex = 2;
            checkBox1.Text = "Play";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(6, 10);
            label1.Name = "label1";
            label1.Size = new Size(19, 15);
            label1.TabIndex = 3;
            label1.Text = "20";
            // 
            // GrassLimit_CB
            // 
            GrassLimit_CB.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GrassLimit_CB.AutoSize = true;
            GrassLimit_CB.Location = new Point(56, 9);
            GrassLimit_CB.Name = "GrassLimit_CB";
            GrassLimit_CB.Size = new Size(69, 19);
            GrassLimit_CB.TabIndex = 7;
            GrassLimit_CB.Text = "NoLimit";
            GrassLimit_CB.UseVisualStyleBackColor = true;
            GrassLimit_CB.CheckedChanged += GrassLimit_CB_CheckedChanged;
            // 
            // GrassBox
            // 
            GrassBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GrassBox.Controls.Add(checkBox2);
            GrassBox.Controls.Add(button8);
            GrassBox.Controls.Add(groupBox5);
            GrassBox.Controls.Add(groupBox4);
            GrassBox.Controls.Add(button1);
            GrassBox.Location = new Point(6, 10);
            GrassBox.Name = "GrassBox";
            GrassBox.Size = new Size(216, 120);
            GrassBox.TabIndex = 8;
            GrassBox.TabStop = false;
            GrassBox.Text = "Food Control";
            // 
            // checkBox2
            // 
            checkBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(69, 95);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(81, 19);
            checkBox2.TabIndex = 20;
            checkBox2.Text = "Day/Night";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.Visible = false;
            // 
            // button8
            // 
            button8.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button8.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            button8.Location = new Point(6, 64);
            button8.Name = "button8";
            button8.Size = new Size(57, 22);
            button8.TabIndex = 15;
            button8.Text = "Create";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(GrassLimit_CB);
            groupBox5.Controls.Add(trackBar7);
            groupBox5.Controls.Add(trackBar6);
            groupBox5.Controls.Add(label24);
            groupBox5.Controls.Add(label1);
            groupBox5.Location = new Point(6, 12);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(139, 50);
            groupBox5.TabIndex = 14;
            groupBox5.TabStop = false;
            // 
            // trackBar7
            // 
            trackBar7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar7.AutoSize = false;
            trackBar7.LargeChange = 5000;
            trackBar7.Location = new Point(39, 19);
            trackBar7.Maximum = 50000;
            trackBar7.Minimum = 1;
            trackBar7.Name = "trackBar7";
            trackBar7.Size = new Size(94, 25);
            trackBar7.SmallChange = 1000;
            trackBar7.TabIndex = 14;
            trackBar7.TickFrequency = 0;
            trackBar7.TickStyle = TickStyle.TopLeft;
            trackBar7.Value = 7000;
            trackBar7.Scroll += trackBar7_Scroll;
            // 
            // trackBar6
            // 
            trackBar6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar6.AutoSize = false;
            trackBar6.Location = new Point(-32, 13);
            trackBar6.Maximum = 30;
            trackBar6.Minimum = 1;
            trackBar6.Name = "trackBar6";
            trackBar6.Orientation = Orientation.Vertical;
            trackBar6.Size = new Size(25, 74);
            trackBar6.TabIndex = 11;
            trackBar6.TickFrequency = 3;
            trackBar6.TickStyle = TickStyle.TopLeft;
            trackBar6.Value = 10;
            // 
            // label24
            // 
            label24.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label24.AutoSize = true;
            label24.Location = new Point(6, 29);
            label24.Name = "label24";
            label24.Size = new Size(31, 15);
            label24.TabIndex = 13;
            label24.Text = "7000";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(trackBar2);
            groupBox4.Controls.Add(label14);
            groupBox4.Location = new Point(151, 12);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(59, 102);
            groupBox4.TabIndex = 10;
            groupBox4.TabStop = false;
            groupBox4.Text = "SunLVL";
            // 
            // trackBar2
            // 
            trackBar2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar2.AutoSize = false;
            trackBar2.Location = new Point(28, 13);
            trackBar2.Maximum = 30;
            trackBar2.Minimum = -1;
            trackBar2.Name = "trackBar2";
            trackBar2.Orientation = Orientation.Vertical;
            trackBar2.Size = new Size(25, 83);
            trackBar2.TabIndex = 11;
            trackBar2.TickFrequency = 3;
            trackBar2.TickStyle = TickStyle.TopLeft;
            trackBar2.Value = 3;
            trackBar2.Scroll += trackBar2_Scroll;
            // 
            // label14
            // 
            label14.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label14.AutoSize = true;
            label14.Location = new Point(6, 19);
            label14.Name = "label14";
            label14.Size = new Size(13, 15);
            label14.TabIndex = 13;
            label14.Text = "3";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox1.Controls.Add(label16);
            groupBox1.Controls.Add(label15);
            groupBox1.Controls.Add(pictureBox2);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Controls.Add(GrassBox);
            groupBox1.Location = new Point(1110, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(228, 527);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(82, 340);
            label16.Name = "label16";
            label16.Size = new Size(55, 15);
            label16.TabIndex = 14;
            label16.Text = "Minimap";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.BackColor = Color.Black;
            label15.ForeColor = SystemColors.ButtonFace;
            label15.Location = new Point(6, 506);
            label15.Name = "label15";
            label15.Size = new Size(57, 15);
            label15.TabIndex = 13;
            label15.Text = "x1  Zoom";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Black;
            pictureBox2.Location = new Point(6, 358);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(216, 163);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 12;
            pictureBox2.TabStop = false;
            pictureBox2.MouseMove += pictureBox2_MouseMoveAndDown;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(trackBar3);
            groupBox3.Controls.Add(AutoKill);
            groupBox3.Controls.Add(label21);
            groupBox3.Controls.Add(groupBox8);
            groupBox3.Controls.Add(label20);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(textBox2);
            groupBox3.Controls.Add(OrgLimit_CB);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(button2);
            groupBox3.Location = new Point(6, 136);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(216, 134);
            groupBox3.TabIndex = 10;
            groupBox3.TabStop = false;
            groupBox3.Text = "Organisms Control";
            // 
            // trackBar3
            // 
            trackBar3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar3.AutoSize = false;
            trackBar3.LargeChange = 10;
            trackBar3.Location = new Point(75, 83);
            trackBar3.Maximum = 100;
            trackBar3.Minimum = 10;
            trackBar3.Name = "trackBar3";
            trackBar3.Size = new Size(70, 25);
            trackBar3.SmallChange = 1000;
            trackBar3.TabIndex = 20;
            trackBar3.TickFrequency = 0;
            trackBar3.TickStyle = TickStyle.TopLeft;
            trackBar3.Value = 100;
            trackBar3.Scroll += trackBar3_Scroll;
            // 
            // AutoKill
            // 
            AutoKill.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AutoKill.AutoSize = true;
            AutoKill.Location = new Point(77, 109);
            AutoKill.Name = "AutoKill";
            AutoKill.Size = new Size(68, 19);
            AutoKill.TabIndex = 19;
            AutoKill.Text = "AutoKill";
            AutoKill.UseVisualStyleBackColor = true;
            AutoKill.CheckedChanged += AutoKill_CheckedChanged;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(30, 74);
            label21.Name = "label21";
            label21.Size = new Size(13, 15);
            label21.TabIndex = 1;
            label21.Text = "0";
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(trackBar5);
            groupBox8.Controls.Add(trackBar4);
            groupBox8.Controls.Add(label17);
            groupBox8.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox8.Location = new Point(145, 10);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(65, 118);
            groupBox8.TabIndex = 18;
            groupBox8.TabStop = false;
            groupBox8.Text = "Radiation";
            // 
            // trackBar5
            // 
            trackBar5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar5.AutoSize = false;
            trackBar5.LargeChange = 10;
            trackBar5.Location = new Point(34, 15);
            trackBar5.Maximum = 200;
            trackBar5.Minimum = 1;
            trackBar5.Name = "trackBar5";
            trackBar5.Orientation = Orientation.Vertical;
            trackBar5.Size = new Size(25, 97);
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
            trackBar4.Location = new Point(-106, 13);
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
            label17.Size = new Size(19, 15);
            label17.TabIndex = 13;
            label17.Text = "10";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(10, 50);
            label20.Name = "label20";
            label20.Size = new Size(63, 15);
            label20.TabIndex = 0;
            label20.Text = "Egg Count";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(91, 19);
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
            textBox2.Location = new Point(76, 34);
            textBox2.MaxLength = 4;
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "MaxGrass";
            textBox2.Size = new Size(62, 16);
            textBox2.TabIndex = 16;
            textBox2.TabStop = false;
            textBox2.Text = "2000";
            textBox2.TextAlign = HorizontalAlignment.Center;
            textBox2.WordWrap = false;
            textBox2.TextChanged += textBox2_TextChanged;
            textBox2.KeyPress += textBoxNumeric_KeyPress;
            // 
            // OrgLimit_CB
            // 
            OrgLimit_CB.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            OrgLimit_CB.AutoSize = true;
            OrgLimit_CB.Location = new Point(6, 109);
            OrgLimit_CB.Name = "OrgLimit_CB";
            OrgLimit_CB.Size = new Size(69, 19);
            OrgLimit_CB.TabIndex = 15;
            OrgLimit_CB.Text = "NoLimit";
            OrgLimit_CB.UseVisualStyleBackColor = true;
            OrgLimit_CB.CheckedChanged += OrgLimit_CB_CheckedChanged;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(6, 19);
            label7.Name = "label7";
            label7.Size = new Size(65, 15);
            label7.TabIndex = 12;
            label7.Text = "Population";
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(25, 35);
            label8.Name = "label8";
            label8.Size = new Size(19, 15);
            label8.TabIndex = 11;
            label8.Text = "20";
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.Location = new Point(77, 63);
            button2.Name = "button2";
            button2.Size = new Size(62, 21);
            button2.TabIndex = 3;
            button2.Text = "Kill Half";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label23);
            groupBox2.Controls.Add(label22);
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(checkBox1);
            groupBox2.Controls.Add(trackBar1);
            groupBox2.Location = new Point(6, 276);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(216, 61);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Time Control";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(91, 13);
            label23.Name = "label23";
            label23.Size = new Size(99, 15);
            label23.TabIndex = 14;
            label23.Text = "Simulation Speed";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(6, 34);
            label22.Name = "label22";
            label22.Size = new Size(48, 15);
            label22.TabIndex = 13;
            label22.Text = "Latency";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.BackColor = Color.Black;
            label13.Location = new Point(60, 34);
            label13.Name = "label13";
            label13.Size = new Size(25, 15);
            label13.TabIndex = 12;
            label13.Text = "100";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(179, 34);
            label12.Name = "label12";
            label12.Size = new Size(31, 15);
            label12.TabIndex = 1;
            label12.Text = "1000";
            // 
            // trackBar1
            // 
            trackBar1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar1.AutoSize = false;
            trackBar1.Location = new Point(91, 30);
            trackBar1.Maximum = 1000;
            trackBar1.Minimum = 1;
            trackBar1.MinimumSize = new Size(0, 15);
            trackBar1.Name = "trackBar1";
            trackBar1.RightToLeft = RightToLeft.Yes;
            trackBar1.RightToLeftLayout = true;
            trackBar1.Size = new Size(90, 22);
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
            panel1.Controls.Add(button7);
            panel1.Controls.Add(button5);
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
            groupBox6.Controls.Add(label25);
            groupBox6.Controls.Add(progressBar1);
            groupBox6.Controls.Add(label3);
            groupBox6.Controls.Add(label2);
            groupBox6.Controls.Add(label6);
            groupBox6.Controls.Add(label4);
            groupBox6.Controls.Add(listBox1);
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
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(6, 308);
            label25.Name = "label25";
            label25.Size = new Size(46, 15);
            label25.TabIndex = 25;
            label25.Text = "Fatigue";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(102, 308);
            progressBar1.MarqueeAnimationSpeed = 1;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(117, 15);
            progressBar1.Step = 1;
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.TabIndex = 24;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(101, 289);
            label3.Name = "label3";
            label3.Size = new Size(24, 15);
            label3.TabIndex = 23;
            label3.Text = "Yes";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 289);
            label2.Name = "label2";
            label2.Size = new Size(47, 15);
            label2.TabIndex = 22;
            label2.Text = "Hungry";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(101, 270);
            label6.Name = "label6";
            label6.Size = new Size(29, 15);
            label6.TabIndex = 19;
            label6.Text = "True";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 270);
            label4.Name = "label4";
            label4.Size = new Size(78, 15);
            label4.TabIndex = 18;
            label4.Text = "CanDublicate";
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.Gray;
            listBox1.FormattingEnabled = true;
            listBox1.ImeMode = ImeMode.On;
            listBox1.ItemHeight = 15;
            listBox1.Items.AddRange(new object[] { "" });
            listBox1.Location = new Point(0, 329);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(228, 289);
            listBox1.TabIndex = 17;
            // 
            // panel3
            // 
            panel3.Controls.Add(comboBox1);
            panel3.Controls.Add(pictureBox4);
            panel3.Controls.Add(label9);
            panel3.Controls.Add(pictureBox3);
            panel3.Location = new Point(6, 15);
            panel3.Name = "panel3";
            panel3.Size = new Size(216, 210);
            panel3.TabIndex = 14;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(124, 181);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(89, 23);
            comboBox1.TabIndex = 15;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // pictureBox4
            // 
            pictureBox4.Location = new Point(96, 181);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(22, 23);
            pictureBox4.TabIndex = 16;
            pictureBox4.TabStop = false;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Black;
            label9.ForeColor = SystemColors.ButtonFace;
            label9.Location = new Point(6, 189);
            label9.Name = "label9";
            label9.Size = new Size(43, 15);
            label9.TabIndex = 1;
            label9.Text = "coords";
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
            label18.Location = new Point(6, 233);
            label18.Name = "label18";
            label18.Size = new Size(28, 15);
            label18.TabIndex = 12;
            label18.Text = "Age";
            // 
            // button7
            // 
            button7.Location = new Point(159, 624);
            button7.Name = "button7";
            button7.Size = new Size(83, 23);
            button7.TabIndex = 21;
            button7.Text = "PasteGenome";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button5
            // 
            button5.Location = new Point(69, 624);
            button5.Name = "button5";
            button5.Size = new Size(84, 23);
            button5.TabIndex = 20;
            button5.Text = "CopyGenome";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
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
            groupBox7.Controls.Add(button10);
            groupBox7.Controls.Add(button9);
            groupBox7.Location = new Point(1110, 535);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(228, 85);
            groupBox7.TabIndex = 15;
            groupBox7.TabStop = false;
            // 
            // button10
            // 
            button10.Location = new Point(198, 34);
            button10.Name = "button10";
            button10.Size = new Size(24, 23);
            button10.TabIndex = 2;
            button10.Text = "Y";
            button10.UseVisualStyleBackColor = true;
            button10.Visible = false;
            button10.Click += button10_Click;
            // 
            // button9
            // 
            button9.Location = new Point(6, 13);
            button9.Name = "button9";
            button9.Size = new Size(85, 23);
            button9.TabIndex = 2;
            button9.Text = "Add part";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(pictureBox1);
            panel2.Location = new Point(12, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(1078, 631);
            panel2.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Controls.Add(button6);
            panel4.Controls.Add(textBox3);
            panel4.Location = new Point(609, -10);
            panel4.Name = "panel4";
            panel4.Size = new Size(237, 641);
            panel4.TabIndex = 1;
            panel4.Visible = false;
            // 
            // button6
            // 
            button6.Location = new Point(3, 615);
            button6.Name = "button6";
            button6.Size = new Size(45, 23);
            button6.TabIndex = 1;
            button6.Text = "Close";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // textBox3
            // 
            textBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox3.Location = new Point(3, 10);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(225, 599);
            textBox3.TabIndex = 0;
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
            pictureBox1.MouseMove += pictureBox1_MouseMoveAndDown;
            pictureBox1.MouseWheel += pictureBox1_MouseWheel;
            // 
            // button11
            // 
            button11.Location = new Point(1230, 626);
            button11.Name = "button11";
            button11.Size = new Size(102, 23);
            button11.TabIndex = 16;
            button11.Text = "Save Image";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(1344, 661);
            Controls.Add(button11);
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
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar7).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar6).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar3).EndInit();
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
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            groupBox7.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }


        #endregion
        public System.Windows.Forms.Timer timer1;
        private Button button1;
        private CheckBox checkBox1;
        private Label label1;
        private CheckBox GrassLimit_CB;
        private GroupBox GrassBox;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label5;
        private TextBox textBox2;
        private CheckBox OrgLimit_CB;
        private Label label7;
        private Label label8;
        private Button button2;
        private Label label10;
        private PictureBox pictureBox2;
        private Label label12;
        private Label label11;
        private TrackBar trackBar1;
        private TrackBar trackBar2;
        private Label label14;
        private GroupBox groupBox4;
        private Label label13;
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
        private Label label4;
        private Label label6;
        private Label label9;
        private Button button5;
        private Panel panel4;
        private Button button6;
        private TextBox textBox3;
        private Button button7;
        private Label label21;
        private Label label22;
        private Label label23;
        private GroupBox groupBox5;
        private TrackBar trackBar7;
        private TrackBar trackBar6;
        private Label label24;
        private Button button8;
        private Label label3;
        private Label label2;
        private Label label25;
        private ProgressBar progressBar1;
        private Button button10;
        private Button button9;
        private CheckBox AutoKill;
        private TrackBar trackBar3;
        private CheckBox checkBox2;
        private Button button11;
    }
}