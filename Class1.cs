using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace grass
{
    public partial class Form1 : Form
    {
        int MAXgrass = 10000;
        int MAXorganis = 100;
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
            pictureBox2.BackColor = Color.White;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp2 = bmpReserve;
            //------------------------------
            MAXgrass = int.Parse(textBox1.Lines[0]);
            MAXorganis = int.Parse(textBox2.Lines[0]);
            //------------------------------
            controller = new Controller(3);
            controller.CreateLive(bmp,rand, pictureBox1, 100, 100);
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
            //bmp2 = bmp ;
            controller.Draw(bmp);
            pictureBox1.Image = bmp;
            //pictureBox2.Image = bmp2;
            
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
                        
                        
                    }

                    if (grass.age >= grass.maxage)
                    {
                        controller.grassListTORemove.Add(grass);
                    }
                }

            }
            //-------------------------------------------------------------------------------------------
            foreach (Grass grass in controller.grassListTEMP)
            {
                controller.grassList.Add(grass);
            }
            controller.grassListTEMP.Clear();

            controller.grassDictionary.Clear();
            foreach (Grass grass in controller.grassList)
            {
                if (!controller.grassDictionary.ContainsKey(grass.point))
                {
                    controller.grassDictionary.Add(grass.point, grass);
                }
                else
                {
                    controller.grassListTORemove.Add(grass);
                }
                if (grass.food <= 0)
                {
                    controller.grassListTORemove.Add(grass);
                }

            }
            foreach (Grass grass in controller.grassListTORemove)
            {
                grass.Clear(bmp);
                controller.grassList.Remove(grass);
            }
            controller.grassListTORemove.Clear();
            label1.Text = controller.grassList.Count.ToString();
            label4.Text = controller.grassNoresp.ToString();
            label11.Text = controller.grassList.Count.ToString();
            label12.Text = controller.grassDictionary.Count.ToString();
            //-----------------------------------------------------------------------------------------
            if (checkBox2.Checked)
            {
                foreach (Organism organism in controller.cellsList)
                {
                    organism.Dowork(bmp, controller.grassDictionary);
                    if(organism.Duplicate())
                    {
                        controller.cellsListTEMP.Add(new Organism(organism.point));
                    }
                }
                foreach (var item in controller.cellsListTEMP)
                {
                    controller.cellsList.Add(item);
                }
                controller.cellsListTEMP.Clear();

                var temp = controller.cellsList[0];
                //label10.Text = temp.eatTarget.ToString();
                //if(temp.eatTarget.X != -1)
                //{
                    //label10.Text = controller.grassDictionary[temp.eatTarget].food.ToString();
                //}
                label10.Text = temp.food.ToString();
            }
            
            label8.Text = controller.cellsList.Count.ToString();
        }




        class Controller
        {
            public int grassNoresp = 0;
            public List<Grass> grassList = new List<Grass>();
            public List<Grass> grassListTEMP = new List<Grass>();
            public List<Grass> grassListTORemove = new List<Grass>();
            public Dictionary<Point,Grass> grassDictionary = new Dictionary<Point,Grass>();
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

            public void CreateLive(Bitmap bmp,Random rand, PictureBox pictureBox1, int grass, int cells)
            {

                for (int i = 0; i < grass; i++)
                {
                    Point point = new Point(rand.Next(pictureBox1.Width), rand.Next(pictureBox1.Height));
                    Color color = bmp.GetPixel(point.X, point.Y);
                    if (color.G == 0 && color.R == 0 && color.B == 0)
                    {
                        Grass? tempGrass = new Grass(point);
                        grassList.Add(tempGrass);
                        tempGrass = null;
                    }

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
                if (bmp.GetPixel(point.X, point.Y) != pen.Color)
                {
                    pixelunder = GetPixelUnder(bmp, point);
                    bmp.SetPixel(point.X, point.Y, pen.Color);
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
            public List<BodyPart> bodyTypes = new List<BodyPart>();
            List<Genome> genList = new List<Genome>();

            Point lastPoint = new Point();
            public Point eatTarget = new Point();

            public int duplicate;

            public Organism(Point pointIN)
            {
                food = 10000;
                point = pointIN;
                genList.Add(new Genome { part = new Universal(), localplace = new Point(0,0) });//-----заменить на генерацию с генотипа
                bodyTypes.Add(genList[0].part);
                bodyTypes[0].localplace = genList[0].localplace;
                duplicate = 20000;
            }

            public new void Draw(Bitmap bmp)
            {
                Clear(bmp);
                //Point pp = new Point(point.X, point.Y);
                //pp = BorderChecker(pp, bmp);
                //bmp.SetPixel(pp.X, pp.Y, Color.Red);
                foreach (var item in bodyTypes)
                {
                    Point p = new Point(point.X + item.localplace.X, point.Y + item.localplace.Y);
                    p = BorderChecker(p, bmp);
                    bmp.SetPixel(p.X,p.Y, item.color);
                }
               
            }

            void GenGeneration()
            {

            }

            new void Clear(Bitmap bmp)
            {
                //Point pp = new Point(lastPoint.X, lastPoint.Y);
                //pp = BorderChecker(pp, bmp);
                //bmp.SetPixel(pp.X, pp.Y, Color.Empty);
                foreach (var item in bodyTypes)
                {
                    Point p = new Point(lastPoint.X + item.localplace.X, lastPoint.Y + item.localplace.Y);
                    p = BorderChecker(p, bmp);
                    bmp.SetPixel(p.X,p.Y, Color.Empty);
                }
            }

            public void Dowork(Bitmap bmp, Dictionary<Point, Grass> readyEat)//берет движение от части тела, берет все другие действия от других частей тела
            {
                lastPoint = point;
                foreach (var part in bodyTypes)
                {
                    food -= part.energyCost;
                    if(part.name == "Universal") 
                    {
                        Universal? newPart = (Universal)part;
                        newPart.Dosomething(bmp);
                        point = BorderChecker(point.X + newPart.moveResult.X, point.Y + newPart.moveResult.Y,bmp);
                        eatTarget = newPart.eatTarget.X != -1 && newPart.eatTarget.Y != -1 ? newPart.eatTarget : new Point(-1,-1);
                        EatTarget(readyEat, newPart.EatStrength);
                        newPart.globalplace = point;
                        food -= newPart.energyCost;
                        food += newPart.eatResult;
                    }
                }


                Duplicate();
            }

            public void EatTarget(Dictionary<Point,Grass> readyEat, int toeatStrength)
            {
                if (eatTarget.X != -1 && eatTarget.Y != -1 && readyEat.ContainsKey(eatTarget) )
                {
                    //------------------------------------------------------Доработать математику
                    int difference = readyEat[eatTarget].food - toeatStrength;
                    int amount = difference <= 0 ? toeatStrength - difference : toeatStrength;
                    readyEat[eatTarget].food -= amount;
                    food += amount;
                }
            }

            public bool Duplicate()
            {
                if (food >= duplicate)
                {
                    food = food / 2;
                    return true;
                }
                else 
                    return false;
            }
        }

        class BodyPart
        {
            public Color color = Color.White;
            public Point localplace = new Point();
            public Point globalplace = new Point();
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
            public int EatStrength = 100;
            public int eatResult = 0;
            public Point moveResult = new Point(0,0);
            public Point eatTarget = new Point(0,0);

            public Universal()
            {
                name = "Universal";
                color = Color.Black;
                energyCost = 5;
            }

            public bool Eat(out Point targetEat, Bitmap bmp) 
            {
                List<Point> points = new List<Point>();
                bool haveToEat = false;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        Point p = BorderChecker(globalplace.X + localplace.X + i,globalplace.Y + localplace.Y + j,bmp);
                        if(bmp.GetPixel(p.X, p.Y).G == Color.Green.G)
                        {
                            points.Add(new Point(globalplace.X + i, globalplace.Y + j));
                        }
                    }
                }
                for (int i = points.Count - 1; i > -1; i--)
                {
                    if (points[i].X == 0 && points[i].Y == 0) { points.Remove(points[i]); }
                }
                targetEat = points.Count > 0 ? points[rand.Next(0, points.Count)] : new Point(-1, -1);
                return haveToEat = targetEat.X != -1 && targetEat.Y != -1 ? true : false;
            }
            public Point SenseMove(Bitmap bmp) 
            {
                List<Point> points = new List<Point>();
                Point targetPoint = new Point();
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        Point p = BorderChecker(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j, bmp);
                        points.Add(bmp.GetPixel(p.X, p.Y).A == Color.Empty.A ? new Point(localplace.X + i, localplace.Y + j) : new Point(0, 0));
                    }
                }
                for (int i = points.Count-1; i > -1; i--)
                {
                    if (points[i].X == 0 && points[i].Y == 0) { points.Remove(points[i]); }
                }
                targetPoint = points.Count > 0 ? points[rand.Next(0,points.Count)] : new Point(0,0);
                return targetPoint;// = new Point(rand.Next(-1,2), rand.Next(-1,2));//---------------------Clean
            }
            public override void Dosomething(Bitmap bmp)
            {
                
                if (!Eat(out eatTarget,bmp)) { moveResult = SenseMove(bmp); } else { moveResult = new Point(0,0); }
                

            }
        }

        class Mouth : BodyPart
        {
            public Mouth()
            {
                name = "Mouth";
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
                name = "Brain";
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
                name = "Leg";
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
                name = "Stomach";
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
                name = "Eye";
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
                name = "Fats";
                color = Color.LightGray;
            }

            public override void Dosomething(Bitmap bmp)
            {
                
            }
        }

        class Jabres : BodyPart
        {
            public Jabres()
            {
                name = "Jabres";
                color = Color.DeepPink;
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
