namespace MicroLife_Simulator
{

    public partial class Form1 : Form
    {
        static public int zoomValue = 1;
        int x = 0;
        int y = 0;
        Point bmpPosition = new Point();
        Point p = new Point(0, 0);

        public static Point BorderChecker(Point point, Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            int x = point.X;
            int y = point.Y;
            int newX = x >= width ? x - width + 1 : x < 1 ? x + width - 1 : x;
            int newY = y >= height ? y - height + 1 : y < 1 ? y + height - 1 : y;
            Point newpoint = new Point(newX, newY);
            return newpoint;
        }

        public static Point BorderChecker(int xx, int yy, Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            int newX = xx >= width ? xx - width + 1 : xx < 1 ? xx + width - 1 : xx;
            int newY = yy >= height ? yy - height + 1 : yy < 1 ? yy + height - 1 : yy;
            Point newpoint = new Point(newX, newY);
            return newpoint;
        }

        public static Point BorderChecker(int xx, int yy, int widthIN, int heightIN)
        {
            int width = widthIN;
            int height = heightIN;
            int newX = xx >= width ? xx - width + 1 : xx < 1 ? xx + width - 1 : xx;
            int newY = yy >= height ? yy - height + 1 : yy < 1 ? yy + height - 1 : yy;
            Point newpoint = new Point(newX, newY);
            return newpoint;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if ((trackBar1.Value != 0))
            {
                timer1.Interval = trackBar1.Value;
            }
            label12.Text = timer1.Interval.ToString();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if ((trackBar2.Value != 0))
            {
                Controller.sunLVL = trackBar2.Value;
            }
            label14.Text = Controller.sunLVL.ToString();
        }
        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            if ((trackBar5.Value != 0))
            {
                Controller.radiationLVL = trackBar5.Value;
            }
            label17.Text = Controller.radiationLVL.ToString();
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.TextLength > 0)
            {
                MAXorganis = int.Parse(textBox2.Lines[0]);
            }
        }
        private void textBoxNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            KillHalf();
        }
        private void KillHalf()
        {
            Boolean boo = false;
            foreach (var item in Controller.cellsList)
            {
                if (boo) { Controller.cellsListTORemove.Add(item); }
                boo = !boo;
            }
        }
        private void AutoKillProcent()
        {
            //trackBar3.Maximum = 100;
            if (Controller.cellsList.Count >= MAXorganis)
            {
                int intToDie = MAXorganis - trackBar3.Value * MAXorganis / trackBar3.Maximum;
                while (--intToDie > 0)
                {
                    Controller.cellsListTORemove.Add(Controller.cellsList[rand.Next(0, Controller.cellsList.Count)]);

                }
            }
            //label20.Text = intToDie.ToString();
        }



        private void pictureBox1_MouseMoveAndDown(object sender, MouseEventArgs e)
        {
            //label26.Text = e.X.ToString();
            //label27.Text = e.Y.ToString();

            //if (!dragOn)
            //{

            //    x = -e.X * bmp.Width * (zoomValue - 1) / pictureBox1.Width + bmp.Width / 2 + e.Location.X;
            //    y = -e.Y * bmp.Height * (zoomValue - 1) / pictureBox1.Height + bmp.Height / 2 + e.Location.Y;

            //    x = x < -bmp.Width * (zoomValue - 1) ? -bmp.Width * (zoomValue - 1) : x > 0 ? 0 : x;
            //    y = y < -bmp.Height * (zoomValue - 1) ? -bmp.Height * (zoomValue - 1) : y > 0 ? 0 : y;

            //    //label20.Text = e.X.ToString() + " " + bmp.Width.ToString();
            //    //label21.Text = e.Y.ToString() + " " + bmp.Height.ToString();
            //    //pictureBox1.Location = new Point(x, y);
            //}

        }
        private void pictureBox2_MouseMoveAndDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int x = -e.X * pictureBox1.Width / pictureBox2.Width + bmp.Width / 2;
                int y = -e.Y * pictureBox1.Height / pictureBox2.Height + bmp.Height / 2;

                x = x < -bmp.Width * (zoomValue - 1) ? -bmp.Width * (zoomValue - 1) : x > 0 ? 0 : x;
                y = y < -bmp.Height * (zoomValue - 1) ? -bmp.Height * (zoomValue - 1) : y > 0 ? 0 : y;

                //label20.Text = e.X.ToString() +" "+ bmp.Width.ToString();
                //label21.Text = e.Y.ToString() + " " + bmp.Height.ToString();
                pictureBox1.Location = new Point(x, y);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Controller.grassList.Count / 10; i++)
            {
                Controller.grassListTORemove.Add(Controller.grassList[i]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button6.Hide();
            button5.Show();
            panel4.Hide();
            panel1.Hide();
            button3.Hide();
            button4.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Show();
            button4.Hide();
            button3.Show();
            UpdateTargetInfo();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTargetInfo();
        }

        private void GrassLimit_CB_CheckedChanged(object sender, EventArgs e)
        {
            trackBar7.Enabled = !trackBar7.Enabled;
            label35.Visible = !label35.Visible;
        }
        private void OrgLimit_CB_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = !textBox2.Enabled;
            checkBox5.Enabled = !checkBox5.Enabled;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (Controller.selectedObject != null)
            {
                textBox3.Text = Controller.selectedObject.GetGenotype(Controller.selectedObject);
                textBox3.Text += "\r\n";
                textBox3.Text += "\r";
                textBox3.Text += "\r\nOrganism ID:  ";
                textBox3.Text += Controller.selectedObject.GetID(Controller.selectedObject.myGenWords);
            }
            panel4.Show();
            button5.Hide();
            button6.Show();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            panel4.Hide();
            button5.Show();
            button6.Hide();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            var pp = p == new Point(0, 0) ? new Point(bmp.Width / 2, bmp.Height / 2) : p;
            Controller.cellsList.Add(new Organism(pp, textBox3.Text));
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            label24.Text = trackBar7.Value.ToString();
            MAXgrass = trackBar7.Value;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Controller.CreateGrass(bmp, rand, 100);
        }
        public void UpdateTargetInfo()
        {

            if (panel1.Visible)
            {

                Controller.ListBoxUpdate(Controller.selectedObject);
                Controller.DrawOrganColor(Controller.bmpOrganColor);
                Controller.DrawObservePicture(bmpOrganismPreview);
                if (Controller.selectedObject != null && Controller.selectedObject.bodyTypes.Count == comboBox1.Items.Count)
                {
                    label11.Text = Controller.selectedObject.age.ToString() + "/" + Controller.selectedObject.parameters.maxage;
                    label10.Text = Controller.selectedObject.food.ToString() + "/" + Controller.selectedObject.maxfood.ToString();
                    label6.Text = Controller.selectedObject.canDuplicate.ToString();
                    label3.Text = Controller.selectedObject.hungry.ToString();
                    label9.Text = Controller.selectedObject.point.ToString();
                    progressBar1.Maximum = Controller.selectedObject.parameters.maxFatigue;
                    progressBar1.Value = Controller.selectedObject.fatigue;
                    pictureBox4.Image = bmpOrgansColor;
                }
                else { label10.Text = "NoNe"; label11.Text = "NoNe"; label6.Text = "NoNe"; label3.Text = "NoNe"; ; progressBar1.Value = 0; progressBar1.Maximum = 100; }
                pictureBoxOrganismPrevew.Image = bmpOrganismPreview;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Controller.selectedObject != null)
            {
                //Controller.selectedObject.
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            //управляет процентом автокила
        }

        private void AutoKill_CheckedChanged(object sender, EventArgs e)
        {
            //trackBar3.Enabled = !trackBar3.Enabled;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string pass = Directory.GetCurrentDirectory();
            string rName = Guid.NewGuid().ToString();
            //saveAnyFormat(bmp);
            SaveBitmapAsBmp(bmp, pass + "\\" + rName + ".bmp");
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            bmpPosition.X = pictureBox1.Location.X;
            bmpPosition.Y = pictureBox1.Location.Y;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            label27.Text = e.Location.ToString();

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {

                bmpPosition.X += -(x - e.X);
                bmpPosition.Y += -(y - e.Y);
                bmpPosition.X = bmpPosition.X < 0 ? bmpPosition.X : 0;
                bmpPosition.Y = bmpPosition.Y < 0 ? bmpPosition.Y : 0;
                bmpPosition.X = bmpPosition.X > -pictureBox1.Width * (zoomValue - 1) + pictureBox1.Width / 2 * (zoomValue - 1) ? bmpPosition.X : -pictureBox1.Width * (zoomValue - 1) + pictureBox1.Width / 2 * (zoomValue - 1);
                pictureBox1.Location = bmpPosition;
                x = -(x - e.X);
                y = -(y - e.Y);
                //всегда меньше нуля
                //всегда меньше picturebox1.size.

                x = e.X;
                y = e.Y;
                label26.Text = pictureBox1.Location.ToString();

            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                p = new Point((e.Location.X - pictureBox1.Location.X - panel2.Location.X) / zoomValue, (e.Location.Y - pictureBox1.Location.Y - panel2.Location.Y) / zoomValue);
                Controller.SelectTarget(p);
                UpdateTargetInfo();
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                pictureBox1.Location = new Point(0, 0);
                pictureBox1.Size = size;
                zoomValue = 1;
                label15.Text = "x" + zoomValue.ToString() + "  Zoom";
            }
        }

        private void Form1_Scroll(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 && zoomValue < 10)
            {
                zoomValue++;
                pictureBox1.Size += size;
                pictureBox1.Location = new Point
                (
                    pictureBox1.Location.X - e.X,
                    pictureBox1.Location.Y - e.Y
                );
            }
            else if (e.Delta < 0 && zoomValue > 1)
            {
                zoomValue--;
                pictureBox1.Size -= size;
                pictureBox1.Location = new Point
                    (
                      pictureBox1.Location.X + e.X,
                      pictureBox1.Location.Y + e.Y
                    );

            }
            else if (zoomValue == 1) pictureBox1.Location = new Point(0, 0);
            label15.Text = "x" + zoomValue.ToString() + "  Zoom";
            label34.Text = pictureBox1.Size.ToString();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            if (Controller.selectedObject != null)
            {
                bmp.SetPixel(Controller.selectedObject.bodyTypes[comboBox1.SelectedIndex].globalplace.X, Controller.selectedObject.bodyTypes[comboBox1.SelectedIndex].globalplace.Y, Color.Empty);
                pictureBox1.Image = bmp;
                Controller.selectedObject.bodyTypes.Remove(Controller.selectedObject.bodyTypes[comboBox1.SelectedIndex]);
                ManualOrganDeleteCellWorkUpdate(Controller.selectedObject);
                Controller.ComboBoxUpdate(Controller.selectedObject);
            }
            pictureBox1.Image = bmp;
            pictureBox2.Image = bmp;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            OrgLimit_CB.Enabled = !OrgLimit_CB.Enabled;
            textBox2.Enabled = !textBox2.Enabled;
        }

        //private void Form1_Paint(object sender, PaintEventArgs e)
        //{

        //}
    }

}