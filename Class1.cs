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
        Bitmap? bmp;
        Bitmap? bmp2;
        Bitmap? bmpOrgansColor;
        Controller controller = new Controller(15,1);
        Stopwatch sw = Stopwatch.StartNew();
        Size size;
        public Form1()
        {
            InitializeComponent();
            NewRefresh();
        }

        
        void NewRefresh()
        {
            size = pictureBox1.Size;

            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp2 = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            bmpOrgansColor = new Bitmap(pictureBox4.Width, pictureBox4.Height);
            pictureBox3.Size *= 8;
            pictureBox3.Location = new Point(-pictureBox3.Width/2 + 108 , -pictureBox3.Height/2 + 105); 
            //------------------------------
            MAXgrass = int.Parse(textBox1.Lines[0]);
            MAXorganis = int.Parse(textBox2.Lines[0]);
            //------------------------------
            //controller = 
            controller.CreateLive(bmp,rand, pictureBox1, 100, 100);
            controller.comboBox = comboBox1;
            controller.listBox = listBox1;
            controller.bmpOrganColor = bmpOrgansColor;
            //------------------------------
            trackBar1.Value = 1;
            timer1.Interval = trackBar1.Value;
            label12.Text = timer1.Interval.ToString();
            //------------------------------
            trackBar2.Value = controller.sunLVL;
            label14.Text = controller.sunLVL.ToString();
            //------------------------------
            panel1.Hide();
            
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            controller.DrawSelectedTargetFrame(bmp);
            label13.Text = sw.ElapsedMilliseconds.ToString(); 
            sw.Restart();
            sw.Start();
            controller.Draw(bmp);
            controller.Draw2(bmp2);

            pictureBox1.Image = bmp;
            pictureBox2.Image = bmp;
            pictureBox3.Image = bmp2;
            pictureBox4.Image = bmpOrgansColor;
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
                    if (organism.food >= organism.dublicateFood && !OrgLimit_CB.Checked && controller.cellsList.Count < MAXorganis && organism.canDuplicate)
                    {
                        organism.food = organism.food / 2;
                        controller.cellsListTEMP.Add(new Organism(organism.point, organism.genList));
                        organism.canDuplicate = false;
                        organism.dublicateDelay = 0;
                    }
                    else
                    if (organism.food >= organism.dublicateFood && OrgLimit_CB.Checked && organism.canDuplicate)
                    {
                        organism.food = organism.food / 2;
                        controller.cellsListTEMP.Add(new Organism(organism.point, organism.genList));
                        organism.canDuplicate = false;
                        organism.dublicateDelay = 0;
                    }
                    else
                    if (organism.food >= organism.dublicateFood)
                    {
                        organism.food = organism.food / 2;
                        organism.canDuplicate = false;
                        organism.dublicateDelay = 0;
                    }
                    if (organism.food <= 0 || organism.age >= organism.maxage)
                    {
                        controller.cellsListTORemove.Add(organism);
                    }
                }
                controller.cellDictionary.Clear();
                foreach (Organism organism in controller.cellsList)
                {
                    if (!controller.cellDictionary.ContainsKey(organism.point))
                    {
                        controller.cellDictionary.Add(organism.point, organism);
                    }

                }
                foreach (var item in controller.cellsListTEMP)
                {
                    controller.cellsList.Add(item);
                }
                controller.cellsListTEMP.Clear();

                foreach (var item in controller.cellsListTORemove)
                {
                    if(controller.selectedObject == item)
                    {
                        controller.selectedObject = null;
                    }
                    item.Cleary(bmp);
                    controller.cellsList.Remove(item);
                }
                controller.cellsListTORemove.Clear();

            }
            
            label8.Text = controller.cellsList.Count.ToString();
            if (controller.selectedObject != null)
            {
                label11.Text = controller.selectedObject.age.ToString() + "/" + controller.selectedObject.maxage;
                label10.Text = controller.selectedObject.food.ToString() + "/" + controller.selectedObject.maxfood.ToString();
            }
            else { label10.Text = "NoNe"; label11.Text = "NoNe"; }
            
            sw.Stop();
        }

        
        class Object
        {
            public int food;
            public int maxfood = 100;
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
                point = BorderChecker(point.X, point.Y, bmp);
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
            

            public Grass()
            {
                
            }

            public Grass(Point pointIN)
            {
                maxfood = 100;
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
                Point newpoint = BorderChecker(x, y, bmp);
                Color color = bmp.GetPixel(newpoint.X, newpoint.Y);
                grass = color.G == 0 && color.R == 0 && color.B == 0 ? new Grass(newpoint) : null;//РАЗОБРАТЬСЯ
                grassnoresp1 = grass != null ? grassnoresp : grassnoresp + 1;
                return grass == null ? false : true;
            }

        }

        struct Genome
        {
            //для частай
            public String part;            //название органа
            public Point localplace;       //локальное положение органа в организме
            public Color color;            //цвет органа(от цвета зависит могут ли его скушать)(для глаза определяет какие цвета он видит)
            //--------------------

        }

        class Organism : Object
        {
            int radiation = 10;
            //для организма
            public int dublicateDelay;    //задержка перед повторным делением
            public int dublicateDelayMax;
            public bool canDuplicate = true;
            public int dublicateFood;     //требуемое количество насыщения для деления

            //public int dublicateAgeMin;   //минимальный возраст деления
            //public int dublicateAgeMax;   //максимальный
            //public int maxage;            //долгожительство
            //public int childs;            //количество детей на одно деление
            //---------------------
            public Dictionary<Point, Grass> ?dictionaryOfGrass;

            public List<BodyPart> bodyTypes = new List<BodyPart>();
            public List<Genome> genList = new List<Genome>();

            public Point lastPoint = new Point();
            public Point eatTarget = new Point();
            public List<Point> findingPoints = new List<Point>();//объекты которые нашли глазки, обновляется каждый тик?

            public int scale = 10000;

            public bool hasGenitals = false;
            public bool hasBrain = false;
            public bool move = true;
            public Organism(Point pointIN)//используется для первого запуска или дополнительной генерации организмов.
            {
                point = pointIN;
                GenGeneration();

                food = 5000;
                maxfood = 10000;
                
                maxage = scale / 5;
                age = 0;

                dublicateDelayMax = scale / 10;
                dublicateDelay = dublicateDelayMax;
            }
            public Organism(Point pointIN, List<Genome> genomes)
            {
                point = pointIN;
                genList = GenCopyes(genomes);
                genList = GenMutation(genList);

                
                food = scale / 2;
                maxfood = scale;

                maxage = scale / 5;
                age = 0;

                dublicateDelayMax = scale / 100;
                dublicateDelay = dublicateDelayMax;
                dublicateFood = scale;
            }
            void GenGeneration()
            {
                
                List<Point> points = new List<Point>();
                int times = rand.Next(2, 4);
                while (points.Count != times)
                {
                    Point point = new Point(rand.Next(-times/2, times/2), rand.Next(-times / 2, times/2));
                    if (!points.Contains(point))
                    {
                        points.Add(point);
                    }
                }
                for (int i = 0; i < times; i++)
                {
                    Color color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
                    switch (rand.Next(1, 7))
                    {
                        case 1: { genList.Add(new Genome { part = "Mouth", localplace = points[i], color = color }); bodyTypes.Add(new Mouth()); } break;
                        case 2: { genList.Add(new Genome { part = "Leg", localplace = points[i], color = color }); bodyTypes.Add(new Leg()); } break;
                        case 3: { genList.Add(new Genome { part = "Eye", localplace = points[i], color = color }); bodyTypes.Add(new Eye()); } break;
                        case 4: { genList.Add(new Genome { part = "Brain", localplace = points[i], color = color }); bodyTypes.Add(new Brain()); hasBrain = true; } break;
                        case 5: { genList.Add(new Genome { part = "Fats", localplace = points[i], color = color }); bodyTypes.Add(new Fats()); } break;
                        case 6: { genList.Add(new Genome { part = "Genitals", localplace = points[i], color = color }); bodyTypes.Add(new Genitals()); hasGenitals = true; } break;

                    }
                    bodyTypes[i].localplace = genList[i].localplace;
                    bodyTypes[i].color = genList[i].color;
                }
                points.Clear();
                dublicateFood = scale;
            }
            List<Genome> GenCopyes(List<Genome> genomes)
            {
                List<Genome> copy = new List<Genome>();
                for (int i = 0; i < genomes.Count; i++)
                {
                    if (genomes[i].part == "Brain") { hasBrain = true; }
                    if (genomes[i].part == "Genitals") { hasGenitals = true; }
                    copy.Add(new Genome { part = genomes[i].part, localplace = genomes[i].localplace, color = genomes[i].color });
                }
                return copy;
            }
            List<Genome> GenMutation(List<Genome> genotype)
            {
                //любое число для сравнения обязано быть меньше 100 иначе при уровне радиации 900 результат никогда не будет положительным
                //Stage 1 some chenges
                    if (rand.Next(0, 1011 - radiation) == 20 || rand.Next(0, 1011 - radiation) == 21 || rand.Next(0, 1011 - radiation) == 22)
                    {
                        int rnd = rand.Next(0, genotype.Count);
                        genotype[rnd] = new Genome { localplace = Normalizator(), part = genotype[rnd].part, color = genotype[rnd].color };
                    }
                    if (rand.Next(0, 1011 - radiation) < 5) //цвет
                    {
                        int rnd = rand.Next(0, genotype.Count);
                        genotype[rnd] = new Genome { localplace = genotype[rnd].localplace, part = genotype[rnd].part, color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) };
                    }

                //Stage 2 new parts
                if (rand.Next(0, 1011 - radiation) == 50)
                {
                    switch (rand.Next(1, 7))
                    {
                        case 1: genotype.Add(new Genome { localplace = Normalizator(), part = "Mouth", color = genotype[0].color }); break;
                        case 2: genotype.Add(new Genome { localplace = Normalizator(), part = "Leg", color = genotype[0].color }); break;
                        case 3: genotype.Add(new Genome { localplace = Normalizator(), part = "Eye", color = genotype[0].color }); break;
                        case 4: genotype.Add(new Genome { localplace = Normalizator(), part = "Brain", color = genotype[0].color }); hasBrain = true; break;
                        case 5: genotype.Add(new Genome { localplace = Normalizator(), part = "Fats", color = genotype[0].color }); break;
                        case 6: genotype.Add(new Genome { localplace = Normalizator(), part = "Genitals", color = genotype[0].color }); hasGenitals = true; break;
                    }
                }
                //Normalizator
                Point Normalizator()
                {
                    List<Point> ignorePoints = new List<Point>();
                    foreach (var item in genotype)
                    {
                        ignorePoints.Add(new Point(item.localplace.X, item.localplace.Y));//находим точки которые уже заняты
                    }
                    List<Point> points = new List<Point>();
                    for (int i = -1; i < 2; i++)//обходим диапазон доступный для нового органа
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (!ignorePoints.Contains(new Point(i, j)))//пропускаем занятые точки
                            {
                                points.Add(new Point(i, j));//добавляем все доступные места в список
                            }
                        }
                    }
                    return points[rand.Next(0, points.Count)];//выбираем случайное место для нового органа
                }

                //Stage 3 Apply all changes
                for (int i = 0; i < genList.Count; i++)
                {
                    BodyPart bodyPart;
                    switch (genList[i].part)
                    {
                        case "Mouth": bodyPart = new Mouth(); break;
                        case "Leg": bodyPart = new Leg(); break;
                        case "Eye": bodyPart = new Eye(); break;
                        case "Brain": bodyPart = new Brain(); break;
                        case "Fats": bodyPart = new Fats(); break;
                        case "Genitals": bodyPart = new Genitals(); break;
                        default: bodyPart = new Fats(); break;
                    }
                    bodyPart.localplace = genList[i].localplace;
                    bodyPart.color = genList[i].color;
                    bodyTypes.Add(bodyPart);
                }

                return genotype;
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
                radiation = controller.radiationLVL;
                lastPoint = point;
                foreach (var part in bodyTypes)
                {
                    food -= part.energyCost;
                    part.Dosomething(this, bmp);
                    part.globalplace = point;
                }
                
                canDuplicate = Duplicate();
                age++;//старение
            }

            public void EatTarget(Dictionary<Point,Grass> dictionaryOfGrass, int toeatStrength)
            {
                if (eatTarget.X != -1 && eatTarget.Y != -1 && dictionaryOfGrass.ContainsKey(eatTarget) && food < maxfood)
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
                dublicateDelay = dublicateDelay < dublicateDelayMax ? dublicateDelay+1 : dublicateDelay;
                return dublicateDelay == dublicateDelayMax ? true : false;
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
            public List<string> partsData = new List<string>();
            public BodyPart()
            {
                

            }
            public virtual string UpdateMyData()
            {
                partsData.Clear();
                partsData.Add("localplace X\t" + localplace.X.ToString());
                partsData.Add("localplace Y\t" + localplace.Y.ToString());
                partsData.Add("energyCost\t" + energyCost.ToString());
                partsData.Add(color.ToString());
                return name;
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
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("EatStrength\t" + EatStrength.ToString());
                partsData.Add("eatTarget X\t" + eatTarget.X.ToString());
                partsData.Add("eatTarget Y\t" + eatTarget.Y.ToString());
                foreach (var item in ignorePoints)
                {
                    partsData.Add("ignorePoints X\t" + item.X.ToString() + " " + item.Y.ToString());
                }
                
                return "";
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
                            if (bmp.GetPixel(p.X, p.Y).G == Color.Green.G)
                            { 
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
                if(!Eat(out eatTarget, bmp)) { body.move = true; } else { body.move = false; }// рот решает что делать организму??? НЕТ!!!//только пока нет мозга
                //Eat(out eatTarget, bmp);
                body.eatTarget = eatTarget;
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
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                return "";
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
            public Leg()
            {
                name = "Leg";
                color = Color.DarkOrange;
                energyCost = 3;
            }

            public override string UpdateMyData()
            {
                base.UpdateMyData(); ;
                return "";
            }
            public Point MoveResult(Bitmap bmp)
            {
                List<Point> points = new List<Point>();
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        Point p = BorderChecker(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j, bmp);
                        points.Add(bmp.GetPixel(p.X, p.Y).G == Color.Empty.G ? new Point(localplace.X + i, localplace.Y + j) : new Point(0, 0));
                    }
                }
                for (int i = points.Count-1; i > -1; i--)
                {
                    if (points[i].X == 0 && points[i].Y == 0) { points.Remove(points[i]); }
                }
                return points.Count > 0 ? points[rand.Next(0, points.Count)] : new Point(0, 0); ;
            }


            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (body.move)
                {
                    moveResult = MoveResult(bmp);

                    body.point = BorderChecker(body.point.X + this.moveResult.X, body.point.Y + this.moveResult.Y, bmp);
                }
            }
        }
        class Stomach : BodyPart//желудок сохраняет часть скушанного и переводит в еду с увечиченной скоростью
        {
            int capacity = 2000;
            int transferStrength = 100;
            int transferScale = 2;
            public Stomach()
            {
                name = "Stomach";
                color = Color.RosyBrown;
                energyCost = 3;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("Capacity\t" + capacity.ToString());
                partsData.Add("Transfer\t" + transferStrength.ToString());
                return "";
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
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("Range\t" + eyeRange.ToString());
                return "";
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
        class Sensors : BodyPart //видит зараженные органикой территории
        {
            public Sensors()
            {
                name = "Sensors";
                color = Color.DarkOliveGreen;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                return "";
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {

            }
        }
        class Fats : BodyPart//сохраняет часть съеденного, но не переваренного в свой запас и при дифиците еды высвобождает запасы не давая организму умереть
        {//жиры не используются для определения возможности размножиться, что дает организму дольше жить без еды после деления
            int fats = 0;
            int maxFats = 5000;
            int exchangeStrength = 3;

            public Fats()
            {
                name = "Fats";
                color = Color.LightGray;
                energyCost = 1;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("Fats \t" + fats.ToString() + "/" + maxFats.ToString());
                partsData.Add("Fat intake\t" + exchangeStrength.ToString());
                return "";
            }
            void FoodExchange(Organism body)
            {
                if(body.food >= body.maxfood/2 && fats < maxFats - exchangeStrength && !body.canDuplicate)
                {
                    fats += exchangeStrength;
                    body.food -= exchangeStrength;
                }
                else if(fats - exchangeStrength >= 0)
                {
                    fats -= exchangeStrength;
                    body.food += exchangeStrength;
                }
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                FoodExchange(body);
            }
        }
        class Gills : BodyPart//жарбы для жизни под водой, как только зоны будут добавлены//так же будут и легкие
        {
            public Gills()
            {
                name = "Jabres";
                color = Color.DeepPink;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                return "";
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {

            }
        }
        class Genitals : BodyPart //определяют схему размножения
        {
            public Genitals()
            {
                name = "Genitals";
                color = Color.LavenderBlush;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                return "";
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {

            }
        }
        class Filter : BodyPart //позволяет поедать(фильтровать) зараженные органикой территории
        {
            public Filter()
            {
                name = "Filter";
                color = Color.DimGray;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                return "";
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

        

        
    }
}
