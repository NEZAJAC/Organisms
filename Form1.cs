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

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

    }

}