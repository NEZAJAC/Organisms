using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.InteropServices;

namespace grass
{

    public partial class Form1 : Form
    {
        public static Point BorderChecker(Point point, Bitmap bmp)
        {
            int x = point.X;
            int y = point.Y;
            int newX = x >= bmp.Width ? x - bmp.Width + 1 : x < 1 ? x + bmp.Width - 1 : x;
            int newY = y >= bmp.Height ? y - bmp.Height + 1 : y < 1 ? y + bmp.Height - 1 : y;
            Point newpoint = new Point(newX, newY);
            return newpoint;
        }

        public static Point BorderChecker(int xx, int yy, Bitmap bmp)
        {
            int x = xx;
            int y = yy;
            int newX = x >= bmp.Width ? x - bmp.Width + 1 : x < 1 ? x + bmp.Width - 1 : x;
            int newY = y >= bmp.Height ? y - bmp.Height + 1 : y < 1 ? y + bmp.Height - 1 : y;
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
                controller.sunLVL = trackBar2.Value;
            }
            label14.Text = controller.sunLVL.ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Size = size * trackBar3.Value;
            pictureBox1.Location = PictureBorders(pictureBox1.Width / pictureBox2.Width + pictureBox1.Width / 4, pictureBox1.Height / pictureBox2.Height + pictureBox1.Height / 4);
            label15.Text = "x" + trackBar3.Value.ToString() + "  Zoom";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 0)
            {
                MAXgrass = int.Parse(textBox1.Lines[0]);
            }

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.TextLength > 0)
            {
                MAXorganis = int.Parse(textBox2.Lines[0]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Boolean boo = false;
            foreach (var item in controller.cellsList)
            {
                if (boo) { controller.cellsListTORemove.Add(item); }
                boo = !boo;
            }
            foreach (var item in controller.cellsListTORemove)
            {
                item.Cleary(bmp);
                controller.cellsList.Remove(item);
            }
            controller.cellsListTORemove.Clear();
        }
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int x = -e.X * pictureBox1.Width / pictureBox2.Width + pictureBox1.Width / 4;
                int y = -e.Y * pictureBox1.Height / pictureBox2.Height + pictureBox1.Height / 4;

                pictureBox1.Location = PictureBorders(x, y);
                label11.Text = pictureBox1.Location.ToString();
            }
        }
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int x = -e.X * pictureBox1.Width / pictureBox2.Width + pictureBox1.Width / 4;
                int y = -e.Y * pictureBox1.Height / pictureBox2.Height + pictureBox1.Height / 4;

                pictureBox1.Location = PictureBorders(x, y);
                label11.Text = pictureBox1.Location.ToString();
            }
        }
        Point PictureBorders(int x, int y)
        {
            if (trackBar3.Value == 1)
            {
                x = x < 5 ? 5 : x > 5 ? 5 : x;
                y = y < 12 ? 12 : y > 12 ? 12 : y;
            }
            if (trackBar3.Value == 2)
            {
                x = x < -1070 ? -1070 : x > 5 ? 5 : x;
                y = y < -620 ? -620 : y > 12 ? 12 : y;
            }
            if (trackBar3.Value == 3)
            {
                x = x < -2148 ? -2148 : x > 5 ? 5 : x;
                y = y < -1251 ? -1251 : y > 12 ? 12 : y;
            }
            return new Point(x, y);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //controller.grassList.Clear();
            //bmp.Dispose();
            //bmp = new Bitmap(pictureBox1.Height, pictureBox1.Width);
            //NewRefresh();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.Location.X / trackBar3.Value , e.Location.Y / trackBar3.Value);
            controller.SelectTarget(p);
            label20.Text = p.ToString();// new Point(e.Location.X, e.Location.Y).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Show();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // comboBox1.SelectedItem.ToString();
            controller.DrawOrganColor(controller.bmpOrganColor);
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            if ((trackBar5.Value != 0))
            {
                controller.radiationLVL = trackBar5.Value;
            }
            label17.Text = controller.radiationLVL.ToString();
        }
    }

}