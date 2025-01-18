using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace grass
{
    public partial class Form1 : Form
    {
        //List<GrassTree> grassTrees = new List<GrassTree>();
        //List<PictureBox> pictureBoxes;
        //int groundlvl = 0;
        int MAXgrass = 10000;
        int MAXorganis = 100;
        List<Object> LObjects = new List<Object>();
        //Graphics g;
        Random rand = new Random();
        Bitmap bmp;
        Bitmap bmp2;
        Bitmap bmpReserve;
        Controller controller;
        public Form1()
        {
            InitializeComponent();
            NewRefresh();
            bmpReserve = new Bitmap(pictureBox1.Width, pictureBox1.Height);

        }

        
        void NewRefresh()
        {
            pictureBox1.BackColor = Color.White;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp2 = bmpReserve;
            MAXgrass = int.Parse(textBox1.Lines[0]);
            MAXorganis = int.Parse(textBox2.Lines[0]);
            controller = new Controller(1);
            controller.CreateLive(rand, pictureBox1, 100, 100);
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            bmp = bmpReserve;
            controller.Draw(bmp);
            pictureBox1.Image = bmp;

            if (checkBox1.Checked)
            {
                foreach (Grass grass in controller.grassList)
                {
                    if (grass.GrassUpdate(controller.sunLVL))
                    {
                        if (controller.grassList.Count < MAXgrass && !GrassLimit_CB.Checked)
                        {
                            if (grass.Duplicate(out Grass? grass1, out controller.grassNoresp, bmp, controller.grassNoresp))
                            {
                                controller.grassListTEMP.Add(grass1);
                            }
                        }
                        else 
                        if (GrassLimit_CB.Checked)
                        {
                            if (grass.Duplicate(out Grass? grass1, out controller.grassNoresp, bmp, controller.grassNoresp))
                            {
                                controller.grassListTEMP.Add(grass1);
                            }
                        }
                        if (grass.age >= grass.maxage)
                        {
                            grass.Clear(bmp);
                            controller.grassListTORemove.Add(grass);
                        }

                    }


                }

                foreach (Grass grass in controller.grassListTEMP)
                {
                    controller.grassList.Add(grass);
                }
                controller.grassListTEMP.Clear();

                foreach (Grass grass in controller.grassListTORemove)
                {
                    controller.grassList.Remove(grass);
                }
                controller.grassListTORemove.Clear();

                label1.Text = controller.grassList.Count.ToString();
                label4.Text = controller.grassNoresp.ToString();
                
                
                
            }

            if (checkBox2.Checked)
            {
                foreach (Organism organism in controller.cellsList)
                {
                    organism.Dowork(bmp);
                }
                label10.Text = controller.cellsList[0].point.ToString();
            }
            

        }




        class Controller
        {
            public int grassNoresp = 0;
            public List<Grass> grassList = new List<Grass>();
            public List<Grass> grassListTEMP = new List<Grass>();
            public List<Grass> grassListTORemove = new List<Grass>();
            public List<Organism> cellsList = new List<Organism>();
            public List<Organism> cellsListTEMP = new List<Organism>();
            public List<Organism> cellsListTORemove = new List<Organism>();
            public int sunLVL;
            //-------------------------------------------------------------
            ZoneType activeType;
            Point mousePoint;
            //-------------------------------------------------------------
            public Controller(int sunlvl)
            {
                sunLVL = sunlvl;
            }

            public void CreateLive(Random rand, PictureBox pictureBox1, int grass, int cells)
            {

                for (int i = 0; i < grass; i++)
                {
                    grassList.Add(new Grass(new Point(rand.Next(pictureBox1.Width), rand.Next(pictureBox1.Height))));

                }

                for (int i = 0; i < cells; i++)
                {
                    cellsList.Add(new Organism(new Point(rand.Next(pictureBox1.Width), rand.Next(pictureBox1.Height))));  

                }


            }

            public void Draw(Bitmap bmp)
            {
                foreach (var item in grassList) item.Draw(bmp);
                foreach (var item in cellsList) item.Draw(bmp);
            }

            

            public void CreateZones_auto()//случаная местность
            {

            }
            public void CreateZones_manual(Point mousePoint, ZoneType activeType)//ручное создание зоны
            {
                //создавать зону типа activeType в точке курсора mousePoint
            }

        }

        class Object
        {
            public int food;
            public int age;
            public int maxage;
            public Point point;
            public Random rand = new Random();
            public Pen pen = new Pen(Color.White);
            public Color pixelunder = Color.White;
            bool alreadyDraw = false;
            public Object()
            {

            }
            public Object(int age, Point point)
            {
                this.age = age;
                this.point = point;
            }

            public Color GetPixelUnder(Bitmap bmp,Point point)
            {
                Color color = bmp.GetPixel(point.X, point.Y);
                return color;
            }
            public void Draw(Bitmap bmp)
            {
                if (!alreadyDraw)
                {
                    pixelunder = GetPixelUnder(bmp, point);
                    bmp.SetPixel(point.X, point.Y, pen.Color);
                    alreadyDraw = true;
                }
            }

            public void Clear(Bitmap bmp)
            {
                bmp.SetPixel(point.X, point.Y, Color.Empty);
            }



        }

        class Grass : Object
        {
            int sun;
            public int maxfood = 100;

            public Grass()
            {

            }

            public Grass(Point pointIN)
            {
                food = rand.Next(maxfood / 10);
                maxfood = rand.Next(500, 1000);
                maxage = maxfood * 2;
                pen.Color = Color.Green;
                point = pointIN;

            }

            public bool GrassUpdate(int sunIN)
            {
                
                    age++;
                    food = food >= maxfood ? food - maxfood : food;
                    food = sunIN > 0 ? food + sunIN : food--;
                    return food >= maxfood ? true : false;
                
            }

            public bool Duplicate(out Grass? grass, out int grassnoresp1, Bitmap bmp, int grassnoresp)
            {
                int x = point.X + rand.Next(-10, 11);
                int y = point.Y + rand.Next(-10, 11);
                int newX = x >= bmp.Width ? x - bmp.Width : x <= 0 ? x + bmp.Width - 1 : x;
                int newY = y >= bmp.Height ? y - bmp.Height : y <= 0 ? y + bmp.Height - 1 : y;
                Point newpoint = new Point(newX, newY);
                Color color = bmp.GetPixel(newX, newY);
                grass = color.G == 0 && color.R == 0 && color.B == 0 ? new Grass(newpoint) : null;//РАЗОБРАТЬСЯ
                grassnoresp1 = grass != null ? grassnoresp : grassnoresp + 1;
                return grass == null ? false : true;
            }

        }

        struct Genome
        {
            public BodyPart part;
            public Point localplace;
            
        }

        class Organism : Object
        {
            List<BodyPart> bodyTypes = new List<BodyPart>();
            List<Genome> genList = new List<Genome>();
            Point lastPoint = new Point();



            public Organism(Point pointIN)
            {
                food = 1000;
                point = pointIN;
                genList.Add(new Genome { part = new Universal(), localplace = new Point(0,-1) });
                bodyTypes.Add(genList[0].part);
                bodyTypes[0].localplace = genList[0].localplace;
            }

            public new void Draw(Bitmap bmp)
            {
                Clear(bmp);
                Point pp = new Point(point.X, point.Y);
                pp = BorderChecker(pp, bmp);
                bmp.SetPixel(pp.X, pp.Y, Color.Red);
                foreach (var item in bodyTypes)
                {
                    Point p = new Point(point.X + item.localplace.X, point.Y + item.localplace.Y);
                    p = BorderChecker(p, bmp);
                    bmp.SetPixel(p.X,p.Y, item.color);
                }
               
            }

            new void  Clear(Bitmap bmp)
            {
                Point pp = new Point(lastPoint.X, lastPoint.Y);
                pp = BorderChecker(pp, bmp);
                bmp.SetPixel(pp.X, pp.Y, Color.Empty);
                foreach (var item in bodyTypes)
                {
                    Point p = new Point(lastPoint.X + item.localplace.X, lastPoint.Y + item.localplace.Y);
                    p = BorderChecker(p, bmp);
                    bmp.SetPixel(p.X,p.Y, Color.Empty);
                }
            }

            public void Dowork(Bitmap bmp)//берет движение от части тела, берет все другие действия от других частей тела
            {
                lastPoint = point;
                //point = BorderChecker(point.X + 1, point.Y + 1, bmp);
                foreach (var part in bodyTypes)
                {
                    food -= part.energyCost;
                    //part.Dosomething(bmp);
                    if(part.name == "Universal") 
                    {
                        Universal? newPart = (Universal)part;
                        newPart.Dosomething(bmp);
                        point = BorderChecker(new Point(point.X + newPart.moveResult.X, point.Y + newPart.moveResult.Y),bmp);
                        food -= newPart.energyCost;
                        food += newPart.eatResult;
                    }
                }
            }


        }

        class BodyPart
        {
            public Color color = Color.White;
            public Point localplace = new Point();
            public int energyCost = 0;
            public string name = "";
            public BodyPart()
            {

            }

            public virtual void Dosomething(Bitmap bmp)
            {
                //делать тут потребление питательных веществ из организма
            }
        }

        class Universal : BodyPart
        {

            Random rand = new Random();
            int EatStrength = 10;
            public int eatResult = 0;
            public Point moveResult = new Point(0,0);
            public bool duplicateResult = false; 
            public Universal()
            {
                name = "Universal";
                color = Color.Black;
                energyCost = 5;
            }

            public bool Eat(Bitmap bmp) 
            {
                bool haveToEat = false;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                      Point p = BorderChecker(new Point(localplace.X + i, localplace.Y + j),bmp);
                      haveToEat = bmp.GetPixel(p.X,p.Y) == Color.Green ? true : false;
                      if(haveToEat == true) { eatResult = EatStrength; break; } else { eatResult = 0; }
                    }
                }
                return haveToEat; 
            }
            public Point SenseMove(Bitmap bmp) 
            {
                
                List<Point> points = new List<Point>();
                Point targetPoint = new Point();
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        Point p = BorderChecker(new Point(localplace.X + i, localplace.Y + j), bmp);
                        points.Add(bmp.GetPixel(p.X, p.Y) == Color.Empty ? new Point(p.X, p.Y) : new Point(0, 0));
                    }
                }
                for (int i = points.Count-1; i > 0; i--)
                {
                    if (points[i] == new Point(0,0)) { points.Remove(points[i]); }
                }
                targetPoint = points.Count > 0 ? points[rand.Next(0,points.Count-1)] : new Point(0,0);
                return targetPoint;// = new Point(rand.Next(-1,2), rand.Next(-1,2));//---------------------Clean
            }
            public bool Duplicate() { return false; }

            public override void Dosomething(Bitmap bmp)
            {
                if (Eat(bmp) == false) { moveResult = SenseMove(bmp); }
                
                Duplicate();
                
            }
        }

        class Mouth : BodyPart
        {
            public Mouth()
            {
                color = Color.Red;
            }

            public override void Dosomething(Bitmap bmp)
            {
                
            }
        }

        class Brain : BodyPart
        {
            public Brain()
            {
                color = Color.Pink;
            }

            public override void Dosomething(Bitmap bmp)
            {
                
            }
        }

        class Leg : BodyPart
        {
            public Leg()
            {
                color = Color.Gray;
            }

            public override void Dosomething(Bitmap bmp)
            {
               
            }
        }

        class Stomach : BodyPart
        {
            public Stomach()
            {
                color = Color.RosyBrown;
            }

            public override void Dosomething(Bitmap bmp)
            {
                
            }
        }

        class Eye : BodyPart
        {
            public Eye()
            {
                color = Color.Yellow;
            }

            public override void Dosomething(Bitmap bmp)
            {
                
            }
        }

        class Fats : BodyPart
        {
            public Fats()
            {
                color = Color.LightGray;
            }

            public override void Dosomething(Bitmap bmp)
            {
                
            }
        }


        class ZoneType
        {
            Pen pen;
            Rectangle radius;
            Effect effect;
            public virtual void CreateZone() { }

        }

        class Effect//оказывает влияние на клетки находящие в зоне
        {

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            controller.grassList.Clear();
            bmp.Dispose();
            bmp = new Bitmap(pictureBox1.Height, pictureBox1.Width);
            NewRefresh();
            
        }



        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
        static public Color GetPixelColor(Point p)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, p.X, p.Y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF), (int)(pixel & 0x0000FF00) >> 8, (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }
    }
}
