using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
        Stopwatch sw = Stopwatch.StartNew();
        Size size;
        Point pictureBox1Point;
        public Form1()
        {
            InitializeComponent();
            NewRefresh();
        }

        
        void NewRefresh()
        {
            
            size = pictureBox1.Size;
            bmpReserve = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.BackColor = Color.Black;
            pictureBox2.BackColor = Color.Black;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp2 = bmpReserve;
            //------------------------------
            MAXgrass = int.Parse(textBox1.Lines[0]);
            MAXorganis = int.Parse(textBox2.Lines[0]);
            //------------------------------
            controller = new Controller(10);
            controller.CreateLive(bmp,rand, pictureBox1, 100, 100);

            trackBar1.Value = 100;
            timer1.Interval = trackBar1.Value;
            label12.Text = timer1.Interval.ToString();

            trackBar2.Value = controller.sunLVL;
            label14.Text = controller.sunLVL.ToString();
            
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            label13.Text = sw.ElapsedMilliseconds.ToString(); 
            sw.Restart();
            sw.Start();
            //bmp = bmpReserve;
            //bmp2 = bmp ;
            controller.Draw(bmp);
            pictureBox1.Image = bmp;

            pictureBox2.Image = bmp;
            //pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
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
                                if (grass1 != null)
                                    controller.grassListTEMP.Add(grass1);
                            }
                        }
                        else
                        if (GrassLimit_CB.Checked)
                        {
                            if (grass.Duplicate(out Grass? grass1, out controller.grassNoresp, bmp, controller.grassNoresp))
                            {
                                if (grass1 != null)
                                    controller.grassListTEMP.Add(grass1);
                            }
                        }


                    }

                    if (/*grass.age >= grass.maxage || */grass.food <= controller.sunLVL)
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
            
            //-----------------------------------------------------------------------------------------
            if (checkBox2.Checked)
            {
                foreach (Organism organism in controller.cellsList)
                {
                    organism.DoworkPrepare(bmp, controller);
                    if (organism.food >= organism.duplicate && !OrgLimit_CB.Checked && controller.cellsList.Count < MAXorganis)
                    {
                        organism.food = organism.food / 2;
                        controller.cellsListTEMP.Add(new Organism(organism.point, organism.genList));
                    }
                    else
                    if (organism.food >= organism.duplicate && OrgLimit_CB.Checked)
                    {
                        organism.food = organism.food / 2;
                        controller.cellsListTEMP.Add(new Organism(organism.point, organism.genList));
                    }
                    else
                    if (organism.food >= organism.duplicate)
                    {
                        organism.food = organism.food / 2;
                    }
                    if (organism.food <= 0)
                    {
                        controller.cellsListTORemove.Add(organism);
                    }
                }
                foreach (var item in controller.cellsListTEMP)
                {
                    controller.cellsList.Add(item);
                }
                controller.cellsListTEMP.Clear();

                foreach (var item in controller.cellsListTORemove)
                {
                    item.Cleary(bmp);
                    controller.cellsList.Remove(item);
                }
                controller.cellsListTORemove.Clear();

                if (controller.cellsList.Count > 0)
                {
                    var temp = controller.cellsList[0];
                    label10.Text = temp.food.ToString() + "/" + temp.duplicate.ToString();
                }
                else label10.Text = "NoNe";
            }
            
            label8.Text = controller.cellsList.Count.ToString();
            if (controller.grassDictionary.Count > 1 && controller.grassDictionary.ContainsKey(controller.cellsList[0].eatTarget))
            { 
                label11.Text = controller.grassDictionary[controller.cellsList[0].eatTarget].food.ToString(); 
            }
            sw.Stop();
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
                return bmp.GetPixel(point.X, point.Y);
            }
            public void Normalize(Bitmap bmp)
            {
                point.X = point.X <= 0 ? 1 : point.X >= bmp.Width ? bmp.Width - 1 : point.X;
                point.Y = point.Y <= 0 ? 1 : point.Y >= bmp.Height ? bmp.Height - 1 : point.Y;
            }
            public void Draw(Bitmap bmp)
            {
                Normalize(bmp);
                if (bmp.GetPixel(point.X, point.Y) != pen.Color)
                {
                    //pixelunder = GetPixelUnder(bmp, point);
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
                //int newX = x >= bmp.Width ? x - bmp.Width : x <= 0 ? x + bmp.Width - 1 : x;
                //int newY = y >= bmp.Height ? y - bmp.Height : y <= 0 ? y + bmp.Height - 1 : y;
                //Point newpoint = new Point(newX, newY);
                Point newpoint = BorderChecker(x, y, bmp);
                //Color color = bmp.GetPixel(newX, newY);
                Color color = bmp.GetPixel(newpoint.X, newpoint.Y);
                grass = color.G == 0 && color.R == 0 && color.B == 0 ? new Grass(newpoint) : null;//РАЗОБРАТЬСЯ
                grassnoresp1 = grass != null ? grassnoresp : grassnoresp + 1;
                return grass == null ? false : true;
            }

        }

        struct Genome
        {
            public String part;
            public Point localplace;
            public Color color;
            
        }

        class Organism : Object
        {
            public Dictionary<Point, Grass> dictionaryOfGrass;

            public List<BodyPart> bodyTypes = new List<BodyPart>();
            public List<Genome> genList = new List<Genome>();

            public Point lastPoint = new Point();
            public Point eatTarget = new Point();

            public List<Point> findingPoints = new List<Point>();

            public int duplicate;
            public int scale = 10000;
            public bool hasGenitals = false;
            public bool hasBrain = false;
            public bool move = true;
            public Organism(Point pointIN)//используется для первого запуска или дополнительной генерации организмов.
            {
                point = pointIN;
                GenGeneration();
                food = 5000;

                
            }
            public Organism(Point pointIN, List<Genome> genomes)
            {
                point = pointIN;
                genList.Clear();
                GenApplys(genomes);
                food = genList.Count * scale / 2;
            }
            void GenGeneration()
            {
                //если есть генеталии то ставить переменную hasGenitals = true
                //если есть мозг то ставить переменную hasBrain = true
                //первоночальная инициализация организма, генотип генерируется случайным образом
                genList.Add(new Genome { part = "Mouth", localplace = new Point(0, 0), color = Color.Red });//-----заменить на генерацию с генотипа
                bodyTypes.Add(new Mouth());
                bodyTypes[0].localplace = genList[0].localplace;
                bodyTypes[0].color = genList[0].color;
                //----------------------------
                genList.Add(new Genome { part = "Leg", localplace = new Point(1, 0), color = Color.Yellow });
                bodyTypes.Add(new Leg());
                bodyTypes[1].localplace = genList[1].localplace;
                bodyTypes[1].color = genList[1].color;
                //----------------------------
                genList.Add(new Genome { part = "Eye", localplace = new Point(-1, 0), color = Color.AliceBlue });
                bodyTypes.Add(new Eye());
                bodyTypes[2].localplace = genList[2].localplace;
                bodyTypes[2].color = genList[2].color;
                //----------------------------

                //
                //
                duplicate = genList.Count * scale;
            }
            void GenApplys(List<Genome> genomes)
            {
                
                genList = GenCopyes(genomes);
                genList = GenMutation(genList);
                duplicate = genList.Count * scale;
            }
            List<Genome> GenCopyes(List<Genome> genomes)
            {
                List<Genome> copy = new List<Genome>();
                for (int i = 0; i < genomes.Count; i++)
                {
                    if (genomes[i].part == "Universal") { BodyPart newPart = new Universal(); }
                    if (genomes[i].part == "Mouth") { BodyPart newPart = new Mouth(); }
                    if (genomes[i].part == "Leg") { BodyPart newPart = new Leg(); }
                    if (genomes[i].part == "Eye") { BodyPart newPart = new Eye(); }
                    if (genomes[i].part == "Brain") { BodyPart newPart = new Brain(); hasBrain = true; }
                    if (genomes[i].part == "Fats") { BodyPart newPart = new Fats(); }
                    if (genomes[i].part == "Genitals") { BodyPart newPart = new Genitals(); hasGenitals = true; }
                    //if (genomes[i].part.name == "Universal") { newPart = new Universal(); }
                    copy.Add(new Genome { part = genomes[i].part, localplace = genomes[i].localplace, color = genomes[i].color });
                }

                return copy;
            }
            List<Genome> GenMutation(List<Genome> genotype)
            {
                

                List<Genome> newGenome = genotype;
                //Stage 1 some chenges
                if (rand.Next(0,101) == 20) //смещения локально по организму // необходимо добавить првоерку тела, что бы без отступов
                {
                   int rnd = rand.Next(0, genotype.Count);
                    newGenome[rnd] = new Genome { localplace = new Point(genotype[rnd].localplace.X * -1, genotype[rnd].localplace.Y), part = genotype[rnd].part, color = genotype[rnd].color };
                }
                if (rand.Next(0, 101) == 40)
                {
                    int rnd = rand.Next(0, genotype.Count);
                    newGenome[rnd] = new Genome { localplace = new Point(genotype[rnd].localplace.X, genotype[rnd].localplace.Y * -1), part = genotype[rnd].part, color = genotype[rnd].color };
                }
                if (rand.Next(0, 101) == 21)
                {
                    int rnd = rand.Next(0, genotype.Count);
                    newGenome[rnd] = new Genome { localplace = new Point(genotype[rnd].localplace.X - 1, genotype[rnd].localplace.Y), part = genotype[rnd].part, color = genotype[rnd].color };
                }
                if (rand.Next(0, 101) == 41)
                {
                    int rnd = rand.Next(0, genotype.Count);
                    newGenome[rnd] = new Genome { localplace = new Point(genotype[rnd].localplace.X, genotype[rnd].localplace.Y - 1), part = genotype[rnd].part, color = genotype[rnd].color };
                }
                if (rand.Next(0, 101) == 22)
                {
                    int rnd = rand.Next(0, genotype.Count);
                    newGenome[rnd] = new Genome { localplace = new Point(genotype[rnd].localplace.X + 1, genotype[rnd].localplace.Y), part = genotype[rnd].part, color = genotype[rnd].color };
                }
                if (rand.Next(0, 101) == 42)
                {
                    int rnd = rand.Next(0, genotype.Count);
                    newGenome[rnd] = new Genome { localplace = new Point(genotype[rnd].localplace.X, genotype[rnd].localplace.Y + 1), part = genotype[rnd].part, color = genotype[rnd].color };
                }
                if (rand.Next(0, 1001) < 5) //цвет
                {
                    int rnd = rand.Next(0, genotype.Count);
                    newGenome[rnd] = new Genome {localplace = genotype[rnd].localplace, part = genotype[rnd].part, color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) };
                }


                //Stage 2 new parts

                if (rand.Next(0, 1001) == 500)
                {
                    //Check Place
                    //--------------------------
                    Point newPoint = Normalizator();
                    int chance = rand.Next(0, 8);
                    if (chance == 0)//двигает тело и кушает "Universal"
                    {
                        //newGenome.Add(new Genome { localplace = newPoint, part = new Universal(), color = genotype[0].color });
                    }
                    if (chance == 1)//только кушает, но больше"Mouth"
                    {
                        newGenome.Add(new Genome { localplace = newPoint, part = "Mouth", color = genotype[0].color });
                    }
                    if (chance == 2)//двигает тело игнорирует все "Leg"
                    {
                        newGenome.Add(new Genome { localplace = newPoint, part = "Leg", color = genotype[0].color });
                    }
                    if (chance == 3)//смотрит глазками "Eye"
                    {
                        newGenome.Add(new Genome { localplace = newPoint, part = "Eye", color = genotype[0].color });
                    }
                    if (chance == 4)//решает за всех и смотрит глазками и ходит ножками "Brain"
                    {
                        newGenome.Add(new Genome { localplace = newPoint, part = "Brain", color = genotype[0].color });
                    }
                    if (chance == 5)//накопление жиров "Fats"
                    {
                        newGenome.Add(new Genome { localplace = newPoint, part = "Fats", color = genotype[0].color });
                    }


                }
                //Stage 3 Normalizator
                Point Normalizator()
                {
                    List<Point> ignorePoints = new List<Point>();
                    foreach (var item in newGenome)
                    {
                        ignorePoints.Add(new Point(item.localplace.X, item.localplace.Y));//находим точки которые уже заняты
                    }
                    List<Point> points = new List<Point>();
                    for (int i = -2; i < 3; i++)//обходим диапазон доступный для нового органа
                    {
                        for (int j = -2; j < 3; j++)
                        {
                            if (!ignorePoints.Contains(new Point(i, j)))//пропускаем занятые точки
                            {
                                points.Add(new Point(i, j));//добавляем все доступные места в список
                            }
                        }
                    }
                    return points[rand.Next(0, points.Count)];//выбираем случайное место для нового органа
                }

                //Stage 0 Apply all changes
                for (int i = 0; i < genList.Count; i++)
                {
                    BodyPart bodyPart = genList[i].part == "Mouth" ? new Mouth(): 
                                        genList[i].part == "Leg" ? new Leg() : 
                                        genList[i].part == "Eye" ? new Eye() : 
                                        genList[i].part == "Brain" ? new Brain() : 
                                        genList[i].part == "Fats" ? new Fats() : 
                                        new Universal();
                    bodyPart.localplace = genList[i].localplace;
                    bodyPart.color = genList[i].color;
                    bodyTypes.Add(bodyPart);
                }

                return newGenome;
            }
            public new void Draw(Bitmap bmp)
            {
                Normalize(bmp);
                Cleary(bmp);
                
                foreach (var item in bodyTypes)
                {
                    Point p = new Point(point.X + item.localplace.X, point.Y + item.localplace.Y);
                    p = BorderChecker(p, bmp);
                    bmp.SetPixel(p.X,p.Y, item.color);
                    
                }
               
            }
            public void Cleary(Bitmap bmp)
            {
                foreach (var item in bodyTypes)
                {
                    Point p = new Point(lastPoint.X + item.localplace.X, lastPoint.Y + item.localplace.Y);
                    p = BorderChecker(p, bmp);
                    bmp.SetPixel(p.X,p.Y, Color.Empty);
                }
            }

            public void DoworkPrepare(Bitmap bmp, Controller controller)//берет движение от части тела, берет все другие действия от других частей тела
            {
                dictionaryOfGrass = controller.grassDictionary;
                lastPoint = point;
                foreach (var part in bodyTypes)
                {
                    food -= part.energyCost;
                    part.Dosomething(this, bmp);
                }
                
                DoWorkComplete();
                
            }
            public void DoWorkComplete()
            {
                foreach (var part in bodyTypes)
                {
                    part.globalplace = point;
                }

                Duplicate();
            }

            public void EatTarget(Dictionary<Point,Grass> dictionaryOfGrass, int toeatStrength)
            {
                if (eatTarget.X != -1 && eatTarget.Y != -1 && dictionaryOfGrass.ContainsKey(eatTarget) )
                {
                    //------------------------------------------------------Доработать математику
                    int difference = dictionaryOfGrass[eatTarget].food - toeatStrength;
                    int amount = difference <= 0 ? dictionaryOfGrass[eatTarget].food : toeatStrength;
                    dictionaryOfGrass[eatTarget].food -= amount;
                    food += amount;
                }
            }

            public bool Duplicate()
            {
                if (food >= duplicate) return true; else return false;
            }
        }

        class BodyPart
        {
            public int food = 100;
            public Color color = Color.White;
            public Point localplace = new Point();
            public Point globalplace = new Point();
            public int energyCost = 0;
            public string name = "";
            public BodyPart()
            {

            }

            public virtual void Dosomething(Organism body, Bitmap bmp)
            {
                //делать тут потребление питательных веществ из организма
            }
        }
        class Universal : BodyPart//---------------------------------------------------------------------------------------------------Universal
        {
            Random rand = new Random();
            public int EatStrength = 25;
            public int eatResult = 0;
            int transferStrength = 15;
            public Point moveResult = new Point(0,0);
            public Point eatTarget = new Point(0,0);

            public Universal()
            {
                name = "Universal";
                color = Color.DarkRed;
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
                            points.Add(new Point(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j));
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
                        //-------------тут нужно сделать универсальный поиск пути, исключающий себя, а то бежит от своего тела
                    }
                }
                for (int i = points.Count-1; i > -1; i--)
                {
                    if (points[i].X == 0 && points[i].Y == 0) { points.Remove(points[i]); }
                }
                targetPoint = points.Count > 0 ? points[rand.Next(0,points.Count)] : new Point(0,0);
                return targetPoint;
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (!Eat(out eatTarget,bmp)) { moveResult = SenseMove(bmp); } else { moveResult = new Point(0,0); }

                body.point = BorderChecker(body.point.X + this.moveResult.X, body.point.Y + this.moveResult.Y, bmp);//двигает тело
                body.eatTarget = this.eatTarget.X != -1 && this.eatTarget.Y != -1 ? this.eatTarget : new Point(-1, -1);
                body.EatTarget(body.dictionaryOfGrass, this.EatStrength);
            }
        }
        class Mouth : BodyPart
        {
            public int EatStrength = 100;
            public int eatResult = 0;
            int transferStrength = 25;
            public Point eatTarget = new Point(0, 0);
            public List<Point> ignorePoints = new List<Point>();

            Random rand = new Random();
            public Mouth()
            {
                name = "Mouth";
                color = Color.Red;
                energyCost = 4;
                //CheckMyBody();  //проверять после каждого изменения тела
            }
            public void CheckMyBody(List<BodyPart> parts)//----------------при создании смотрим где можно кушать, главное не кушать себя
            {//-------------------------------------------------------рот кушает все в отличии от клеточного всасывания, те могут только Green
                foreach (var item in parts)
                {
                    ignorePoints.Add(new Point(item.localplace.X - localplace.X, item.localplace.Y - localplace.Y));
                }
            }

            public bool Eat(out Point targetEat, Bitmap bmp)
            {
                List<Point> points = new List<Point>();
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (!ignorePoints.Contains(new Point(i, j)))
                        {
                            Point p = BorderChecker(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j, bmp);
                            var temp = bmp.GetPixel(p.X, p.Y);
                            if (bmp.GetPixel(p.X, p.Y).G == Color.Green.G)//----------------------------тут придумать как понимать какой цвет кушать
                            {//рот должен уметь кушать любой подходящий цвет
                                points.Add(new Point(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j));
                            }
                        }
                    }
                }
                targetEat = points.Count > 0 ? points[rand.Next(0, points.Count)] : new Point(-1, -1);
                return targetEat.X != -1 && targetEat.Y != -1 ? true : false;
            }

            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if(!Eat(out eatTarget, bmp)) { body.move = true; } else { body.move = false; }

                body.eatTarget = this.eatTarget.X != -1 && this.eatTarget.Y != -1 ? this.eatTarget : new Point(-1, -1);
                body.EatTarget(body.dictionaryOfGrass, this.EatStrength);
            }
        }
        class Brain : BodyPart
        {
            int amountFoodSignal = 0;
            List<Point> pointsEyeSignal = new List<Point>();
            int pheromoniusStrengthSignal = 0;
            //входным параметром может быть стадия голода организма
            //сигналы от "глаз"
            //сигналы от "ферамонов"
            //разработать систему корректировки движия
            List<BodyPart> myParts = new List<BodyPart>();
            public Brain()
            {
                name = "Brain";
                color = Color.Pink;
                energyCost = 15;
            }

            void Thinking(List<BodyPart> myParts,out int FoodSignal,out List<Point> EyeSignal,out int pheromoniusSignal)
            {
                foreach (var item in myParts)
                {
                    if(item.name == "")
                    {

                    }
                }

                FoodSignal = 0;
                EyeSignal = new List<Point>();
                pheromoniusSignal = 0;
            }

            public override void Dosomething(Organism body, Bitmap bmp)
            {
                
            }
        }

        class Leg : BodyPart//ножка двигает всю клетку в одну сторону 
        {
            Random rand = new Random();
            public Point moveResult = new Point(0, 0);
            int sdvigX = 0;
            int sdvigY = 0;
            public Leg()
            {
                name = "Leg";
                color = Color.DarkOrange;
                energyCost = 3;
                ChengeCourse();
            }
            //метод который будет определять в какую сторону бедт двигать новорожденная ножка
            void ChengeCourse()
            {
                sdvigX = localplace.X > 0 ? 1 : localplace.X < 0 ? -1 : 0;
                sdvigY = localplace.Y > 0 ? 1 : localplace.Y < 0 ? -1 : 0;
            }
            public Point Move(Bitmap bmp)
            {
                List<Point> points = new List<Point>();
                Point targetPoint = new Point();
                for (int i = -1 + sdvigX; i < 2 + sdvigX; i++)
                {
                    for (int j = -1 + sdvigY; j < 2 + sdvigY; j++)
                    {
                        Point p = BorderChecker(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j, bmp);
                        points.Add(new Point(localplace.X + i, localplace.Y + j));
                    }
                }
                targetPoint = points.Count > 0 ? points[rand.Next(0, points.Count)] : new Point(0, 0);
                return targetPoint;
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (body.move)
                {
                    moveResult = Move(bmp);

                    body.point = BorderChecker(body.point.X + this.moveResult.X, body.point.Y + this.moveResult.Y, bmp);
                }
            }
        }

        class Stomach : BodyPart//желудок сохраняет часть скушанного и переводит в еду с увечиченной скоростью
        {
            int capacity = 500;
            int transferStrength = 50;
            public Stomach()
            {
                name = "Stomach";
                color = Color.RosyBrown;
                energyCost = 3;
            }

            public override void Dosomething(Organism body, Bitmap bmp)
            {
                
            }
        }

        class Eye : BodyPart//проверяет бОльшую область чем сенсоры других органов, отличает цвет, имеет выход нервного окончания для влияния на действия
        {
            List<Point> points = new List<Point>();
            public List<Point> findingTargets = new List<Point>();
            int sdvigX = 0;
            int sdvigY = 0;
            int eyeRange = 5;
            public Eye()
            {
                name = "Eye";
                color = Color.Yellow;
                energyCost = 5;
                ChengeCourse();
            }

            //метод который будет определять в какую сторону бeдет смотреть глаз
            void ChengeCourse()
            {
                sdvigX = localplace.X > 0 ? 1 : localplace.X < 0 ? -1 : 0;
                sdvigY = localplace.Y > 0 ? 1 : localplace.Y < 0 ? -1 : 0;
                sdvigX = sdvigX != 0 ? sdvigX * eyeRange : eyeRange;
                sdvigY = sdvigY != 0 ? sdvigY * eyeRange : eyeRange;
            }

            List<Point> SenseEye(Organism body, Bitmap bmp)
            {
                points.Clear();
                for (int i = -eyeRange; i < eyeRange; i++)
                {
                    for (int j = -eyeRange; j < eyeRange; j++)
                    {
                        Point p = BorderChecker(new Point(body.point.X + localplace.X + i + sdvigX / 2, body.point.Y + localplace.Y + j + sdvigY / 2),bmp);
                        if (bmp.GetPixel(p.X, p.Y).G > 100) { points.Add(p); }
                    }
                    
                }
                
                return points;
            }

            public override void Dosomething(Organism body, Bitmap bmp)
            {
                body.findingPoints.Clear(); 
                body.findingPoints = SenseEye(body,bmp);
            }
        }

        class Fats : BodyPart//сохраняет часть съеденного, но не переваренного в свой запас и при дифиците еды высвобождает запасы не давая организму умереть
        {//жиры не используются для определения возможности размножиться, что дает организму дольше жить без еды после деления
            public Fats()
            {
                name = "Fats";
                color = Color.LightGray;
            }

            public override void Dosomething(Organism body, Bitmap bmp)
            {
                
            }
        }

        class Jabres : BodyPart//жарбы для жизни под водой, как только зоны будут добавлены//так же будут и легкие
        {
            public Jabres()
            {
                name = "Jabres";
                color = Color.DeepPink;
            }

            public override void Dosomething(Organism body, Bitmap bmp)
            {

            }
        }

        class Genitals : BodyPart
        {
            public Genitals()
            {
                name = "Genitals";
                color = Color.LavenderBlush;
            }

            public override void Dosomething(Organism body, Bitmap bmp)
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
    }
}
