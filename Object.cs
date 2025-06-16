using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MicroLife_Simulator
{
    public partial class Form1
    {
        class Obstacles : Object
        {
            Point point1;
            Point point2;
            int difX = 0;
            int difY = 0;
            List<Point> myPoints = new List<Point>();
            public Obstacles(Point point1, Point point2, Bitmap bmp)
            {
                this.point1 = point1;
                this.point2 = point2;
                difX = point2.X - point1.X;
                difY = point2.Y - point1.Y;
                pen = new Pen(Color.Brown);
                for (int i = point1.X; i < point2.X; i++)
                {
                    for (int j = point1.Y; j < point2.Y; j++)
                    {
                        myPoints.Add(BorderChecker(new Point(i, j), bmp));
                    }
                }
             }
            public new void Draw(Bitmap bmp)
            {
                foreach (var item in myPoints)
                {
                    bmp.SetPixel(item.X, item.Y, pen.Color);
                }   
            }
        }
        abstract class Object
        {
            public int food;
            public int maxfood = 100;
            public int age;
            //public int parameters.maxage;
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

            //public Color GetPixelUnder(Bitmap bmp, Point point)
            //{
            //    return bmp.GetPixel(point.X, point.Y);
            //}
            public void Normalize(Bitmap bmp)
            {
                point = BorderChecker(point.X, point.Y, bmp);
            }
            public void Draw(Bitmap bmp)
            {
                Normalize(bmp);
                //if (bmp.GetPixel(point.X, point.Y) != pen.Color)
                //{
                    //pixelunder = GetPixelUnder(bmp, point);
                    bmp.SetPixel(point.X, point.Y, pen.Color);
                //}
            }

            public void Clear(Bitmap bmp)
            {
                bmp.SetPixel(point.X, point.Y, Color.Empty);
            }



        }
        class Grass : Object
        {
            int updateDelay = 10;
            public int maxage;
            public Grass()
            {

            }
            public Grass(Point pointIN)
            {

                maxfood = 100;
                food = rand.Next(maxfood / 10);
                maxfood = rand.Next(500, 1000);
                maxage = 1000;
                age = rand.Next(50, 1000);
                pen.Color = Color.Green;
                point = pointIN;

            }

            public bool GrassUpdate(int sunIN)
            {
                food = food > maxfood ? food - maxfood + 1 : food;
                food = sunIN > 0 ? food + sunIN : food--;
                age++;
                return food >= maxfood;
            }

            public bool Duplicate(Bitmap bmp, out Point pointt)
            {
                int x = point.X + rand.Next(-20, 21);
                int y = point.Y + rand.Next(-20, 21);
                Point newpoint = BorderChecker(x, y, bmp);
                Color color = bmp.GetPixel(newpoint.X, newpoint.Y);
                if (color.G == 0 && color.B == 0 && color.R == 0) { pointt = newpoint; return true; } else pointt = new Point(-1, -1); return false;
            }
            new public void Draw(Bitmap bmp)
            {
                if (updateDelay == 0)
                {
                    bmp.SetPixel(point.X, point.Y, pen.Color);
                    updateDelay = 15;
                }
                updateDelay--;
            }
        }
        struct Genome
        {
            //для частей
            public String part { get; set; }            //название органа
            public Point localplace { get; set; }      //локальное положение органа в организме
            public Color color { get; set; }          //цвет органа(от цвета зависит увидят ли его глаза )
            public Image image { get; set; }
            public string param1;
            public string param2;
            public string param3;
            public string param4;
            public string param5;
        }
        struct OrganismParameters
        {
            //для параметров организма
            public int bodyTemperatureMax;
            public int bodyTemperatureMin;
            public int childs;

            public int dublicateAgeMax;   //----------------------------------------------------максимальный возраст деления
            public int dublicateAgeMin;   //-----------------------------------------------------минимальный возраст деления
            public int dublicateDelayMax; //-----------------------------задержка перед повторным делением(верхнее значение)
            public int dublicateFoodPrice;//--------------------------------------------------------------------цена деления
            public int hungryFoodLVL;     //-------------------------------------значение на котором организм захочет кушать
            public int dublicateDelay;    //--------------------------------------задержка перед повторным делением(текущая)
            public int dublicateFood;     //--------------------------------------требуемое количество насыщения для деления
            public int maxFatigue;
            public int exhaustionLvl;
            public int maxage;
            public int maxFood;
        }
        class Egg : Object
        {
            Controller? controller;
            public Guid myGuid;
            public string ID1 = "";
            public string ID2 = "";
            public int incubationTime;
            public int incubation;
            public OrganismParameters parametersParent1;
            public OrganismParameters parametersParent2;
            public List<Genome> genListParent1 = new List<Genome>();
            public List<Genome> genListParent2 = new List<Genome>();
            public Egg(Organism organism)
            {
                point = new Point(organism.point.X, organism.point.Y);
                age = 0;
                incubationTime = organism.parameters.maxage / 5;
                incubation = 0;
                pen.Color = organism.genList[1].color;
                //------------------------------------------
                parametersParent1 = organism.parameters;
                genListParent1 = GenCopyes(organism.genList);
                //------------------------------------------
                ID1 = organism.GetID(organism.myGenWords);
                myGuid = organism.myGuid;
                //------------------------------------------
                controller = organism.controller;//доделать
            }
            List<Genome> GenCopyes(List<Genome> genomes)
            {
                List<Genome> copy = new List<Genome>();
                for (int i = 0; i < genomes.Count; i++)
                {
                    copy.Add(new Genome { part = genomes[i].part, localplace = genomes[i].localplace, color = genomes[i].color });
                }
                return copy;
            }
            new public void Draw(Bitmap bmp)
            {
                bmp.SetPixel(point.X, point.Y, pen.Color);
            }
            public void Dosomething()
            {
                age++;
                if(ID2 != "")
                {
                    incubation++;
                }
            }
        }
        class Organism : Object
        {
            public Controller? controller = null;

            public Guid myGuid;
            public int radiation = 10;
            public int pooopas = 0;
            public int pooopasGlobal = 0;
            public int fatigue = 0;//-------------------------------------------------------------------------усталость
            public bool exhaustion = false;//-----------------------------------------------------------------переутомление
            public bool canDuplicate = false;
            public bool hungry = true;
            public bool move = true;
            public Organism? globalTarget;
            public Point newPoint = new Point();
            public Point lastPoint = new Point();
            public List<Point> ignorePoints = new List<Point>();//-------------------------------------------------свое тело
            public int HungrySignal;//---------------------------------------------------------------------------------гoлод
            public List<Point> EyeSignal = new List<Point>();//------------------------------глаза
            public List<Point> LegSignal = new List<Point>();
            public Dictionary<Point, Color> SenseSignal = new Dictionary<Point, Color>();//--------------------------сенсоры
            public int bodyTemperatureSignal;       //------------------------------------------------------температура тела
            public int WithoutDublicateSignal = 0; //---------------------------------------------------задержка размножения
            public Point? ToLegSignal = new Point();//-----------------------------------------------------точка для движения
            public Point ToMouthSignal = new Point();//--------------------------------------------------------цель поедания

            List<string> partsNames = new List<string>()
            {
                "Mouth",
                "Brain",
                "Leg",
                "Stomach",
                "Eye",
                "Sensors",
                "Fats",
                "Gills",
                "Genitals",
                "Filter",
                "SpeechApparatus",
                "MiteHorns",
                "Clues",
                "Claws",
                "Chlorophylls",
                "Keratin",
                "Cloaca"
            };
            public string[]? myGenWords;
            public OrganismParameters parameters;
            public List<BodyPart> bodyTypes = new List<BodyPart>(); //-------------------------------список частей тела которые есть у организма
            public List<Genome> genList = new List<Genome>(); //-------------------------------------список генов определяющий цвет и тип частей тела организма

            public Organism(Point pointIN)//используется для первого запуска или дополнительной генерации организмов.
            {
                point = pointIN;
                food = 9000;
                parameters.maxage = 4000;
                age = 0;

                //parameters.childs = 1;
                //parameters.dublicateAgeMin = parameters.maxage / 8;

                parameters.dublicateAgeMin = parameters.maxage / 8;
                parameters.dublicateAgeMax = parameters.maxage - parameters.maxage / 8;
                GenGeneration();
                parameters.dublicateDelayMax = 150 * genList.Count / 2;
                parameters.dublicateDelay = parameters.dublicateDelayMax;
                parameters.dublicateFood = 2300 * genList.Count;
                parameters.dublicateFoodPrice = parameters.dublicateFood/2;
                maxfood = 2600 * genList.Count;
                parameters.maxFood = maxfood;
                parameters.hungryFoodLVL = maxfood - 2600;
                myGenWords = GetGenotype(this).Split(new char[] { '|' });
                parameters.maxFatigue = rand.Next(70,200);
                fatigue = rand.Next(0, parameters.maxFatigue);
                parameters.exhaustionLvl = rand.Next(1,20);
                myGuid = Guid.NewGuid();
            }
            public Organism(Point pointIN, Organism parent)
            {
                parent.food -= parent.parameters.dublicateFoodPrice;
                point = pointIN;
                food = 2000 * genList.Count;
                parameters.maxFood = parent.parameters.maxFood;
                maxfood = parent.maxfood;
                parameters.maxage = parent.parameters.maxage;
                age = 0;
                parameters = parent.parameters;
                fatigue = rand.Next(0, parameters.maxFatigue);
                radiation = parent.radiation;
                GenCopyes(genList,parent.genList);
                GenMutation(genList);
                BodyAddRandomPart(genList);
                BodyChengeRandomPart(genList);
                //BodyRemoveRandomPart(genList);
                BodyCreate(genList);
                myGenWords = GetGenotype(this).Split(new char[] { '|' });
                myGuid = Guid.NewGuid();
            }
            public Organism(Point pointIN, Organism organismX, Organism organismY) //--------------------------------------------------------Доделать половое размножение
            {
                point = pointIN;
                food = (organismX.food + organismY.food) / 2;
                age = 0;
                parameters.dublicateDelay = parameters.dublicateDelayMax;
                myGenWords = Crossingover(organismX.myGenWords, organismY.myGenWords);
                myGuid = Guid.NewGuid();
            }
            public Organism(Point pointIN, OrganismParameters parameter1, List<Genome> genomes1, OrganismParameters parameter2, List<Genome> genomes2)//-------яйцевое размножение
            {
                point = pointIN;
                food = 1500 * genomes1.Count;
                age = 0;
                
                myGuid = Guid.NewGuid();
                this.parameters = parameter1;
                GenCopyes(genList,genomes1);
                GenMutation(genList);
                BodyAddRandomPart(genList);
                BodyChengeRandomPart(genList);
                myGenWords = GetGenotype(this).Split(new char[] { '|' });
                this.parameters = parameter2;
                genList.Clear();
                GenCopyes(genList,genomes2);
                GenMutation(genList);
                BodyAddRandomPart(genList);
                BodyChengeRandomPart(genList);
                string[] myGenWords2 = GetGenotype(this).Split(new char[] { '|' });
                myGenWords = Crossingover(myGenWords, myGenWords2);
                myGenWords2 = new string[0];
                genList.Clear();
                DecodeGenotype(TranslateGenotype(myGenWords), out _);
                BodyCreate(genList);
                maxfood = parameters.maxFood;
                
            }
            public Organism(Point pointIN, string genome)//--------------------------------------------------------------------------------------------------искуственное размножение
            {
                DecodeGenotype(genome, out myGenWords);
                point = pointIN;
                food = 9000;
                maxfood = parameters.maxFood;
                age = 0;

                BodyCreate(genList);
                myGuid = Guid.NewGuid();
            }
            public string[] Crossingover(string[] myGenome, string[] otherGenome)
            {
                int myNumber = int.Parse(myGenome[0]);
                int otherNumber = int.Parse(otherGenome[0]);
                int biggerInt = myNumber >= otherNumber ? myNumber : otherNumber;
                int maxString = myGenome.Length >= otherGenome.Length ? myGenome.Length : otherGenome.Length;
                for (int i = 0; i < biggerInt; i++)
                {
                    myGenWords[i] = rand.Next(0, 2) == 1 ? myGenome[i] : otherGenome[i];
                }
                return myGenWords;
            }
            public string TranslateGenotype(string[] strings)
            {
                string newString = "";
                for (int i = 0; i < strings.Length; i++)
                {
                    newString += strings[i];
                }
                return newString;
            }
            public string GetGenotype(Organism? selected)
            {
                string result = "";
                result += selected.genList.Count + "|";
                foreach (var item in selected.genList)
                {
                    result += item.part + "|"
                        + item.localplace.X.ToString() + "|"
                        + item.localplace.Y.ToString() + "|"
                        + item.color.R.ToString() + "|"
                        + item.color.G.ToString() + "|"
                        + item.color.B.ToString() + "|";
                }
                result += selected.parameters.dublicateAgeMax.ToString() + "|"
                    + selected.parameters.dublicateAgeMin.ToString() + "|"
                    + selected.parameters.dublicateDelayMax.ToString() + "|"
                    + selected.parameters.dublicateFoodPrice.ToString() + "|"
                    + selected.parameters.hungryFoodLVL.ToString() + "|"
                    + selected.parameters.dublicateDelay.ToString() + "|"
                    + selected.parameters.dublicateFood.ToString() + "|"
                    + selected.parameters.maxage.ToString() + "|"
                    + selected.parameters.maxFood.ToString() + "|"
                    + selected.parameters.maxFatigue.ToString() + "|"
                    + selected.parameters.exhaustionLvl.ToString() + "|";
                return result;
            }
            void DecodeGenotype(string genome, out string[] words)
            {
                words = genome.Split(new char[] { '|' });
                //string asd = genome.Split(new char[] { '|' }).Contains("Mouth") ? "asd": " adsa";//test
                if (int.TryParse(words[0], out _))
                {
                    string partt;
                    int localplaceX;
                    int localplaceY;
                    int colorR;
                    int colorG;
                    int colorB;
                    int j = 1;
                    for (int i = 1; i <= int.Parse(words[0]);)
                    {
                        partt = words[j];
                        localplaceX = int.TryParse((words[j + 1]), out int iid) ? int.Parse(words[j + 1]) : 0;
                        localplaceY = int.TryParse(words[j + 2], out _) ? int.Parse(words[j + 2]) : 0;
                        colorR = int.TryParse(words[j + 3], out _) ? int.Parse(words[j + 3]) : 255;
                        colorG = int.TryParse(words[j + 4], out _) ? int.Parse(words[j + 4]) : 255;
                        colorB = int.TryParse(words[j + 5], out _) ? int.Parse(words[j + 5]) : 255;
                        genList.Add(new Genome { part = partt, localplace = new Point(localplaceX, localplaceY), color = Color.FromArgb(255, colorR, colorG, colorB) });
                        j = i * 6 + 1;
                        i++;
                    }
                    parameters.dublicateAgeMax = int.Parse(words[j]);
                    parameters.dublicateAgeMin = int.Parse(words[j + 1]);
                    parameters.dublicateDelayMax = int.Parse(words[j + 2]);
                    parameters.dublicateFoodPrice = int.Parse(words[j + 3]);
                    parameters.hungryFoodLVL = int.Parse(words[j + 4]);
                    parameters.dublicateDelay = int.Parse(words[j + 5]);
                    parameters.dublicateFood = int.Parse(words[j + 6]);
                    parameters.maxage = int.Parse(words[j + 7]);
                    parameters.maxFood = int.Parse(words[j + 8]);
                    parameters.maxFatigue = int.Parse(words[j + 9]);
                    parameters.exhaustionLvl = int.Parse(words[j + 10]);
                }
            }
            
            /// <summary>
            /// если генотип различается то false иначе true
            /// </summary>
            /// <param name="myGenome"></param>
            /// <param name="otherGenome"></param>
            /// <returns></returns>
            public bool CompireGenotype(string[] myGenome, string[] otherGenome, int maxDiff)
            {
                int length = myGenome.Length >= otherGenome.Length ? otherGenome.Length : myGenome.Length;
                int difference = Math.Abs(myGenome.Length - otherGenome.Length);
                for (int i = 0; i < length; i++)
                {
                    difference += myGenome[i] == otherGenome[i] ? 0 : 1;
                }
                return difference <= maxDiff;
            }
            public bool GetIDdiff(string[] mywords, string[] otherwords)
            {
                string myID = mywords[0] + mywords[1];
                string otherID = otherwords[0] + mywords[1];
                for (int i = 1; i < int.Parse(mywords[0]); i++)
                {
                    myID += mywords[i * 6 + 1];
                }
                for (int i = 1; i < int.Parse(otherwords[0]); i++)
                {
                    otherID += otherwords[i * 6 + 1];
                }
                return myID == otherID;
            }
            public string GetID(string[] mywords)
            {
                int len = int.Parse(mywords[0]);
                string myID = mywords[0] + mywords[1];
                for (int i = 1; i < len; i++)
                {
                    myID += mywords[i * 6 + 1];
                }
                return myID;
            }
            void FindIgnorePoints()
            {
                ignorePoints.Clear();
                foreach (var item in bodyTypes)
                {
                    ignorePoints.Add(new Point(point.X + item.localplace.X, point.Y + item.localplace.Y));
                }
            }
            Point Normalizator2()//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Почини!! Где то выход из массива!!!
            {
                //обходим все наши части тела, выбираем случайную, смотрим пустые места вокруг выбранной части заглядывая в геном организма, запоминаем свободную точку случайную, передаем точку дальше
                
                List<Point> points = new List<Point>();//все точки организма
                foreach (var item in genList)
                {
                    points.Add(item.localplace);
                }
                //Point pp = points[rand.Next(0, points.Count)];//случайная точка вокруг которой пляшем
                List<Point> p = new List<Point>();
                foreach (var item in genList)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {    
                           var pp = item.localplace;
                                if (x != 0 && y != 0 && !points.Contains(new Point(pp.X + x, pp.Y + y)))
                                {
                                    p.Add(new Point(pp.X + x, pp.Y + y));
                                }
                         }
                    }
                }
                //if(p.Count ==  0) p.Add(new Point());
                return p[rand.Next(0,p.Count)];
            }

            Point Normalizator()
            {
                List<Point> points = new List<Point>();
                foreach (var item in genList)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if ((item.localplace.X + x != item.localplace.X && item.localplace.Y + y != item.localplace.Y) && !points.Contains(new Point(item.localplace.X + x, item.localplace.Y + y)))
                            {
                                points.Add(new Point(item.localplace.X + x, item.localplace.Y + y));
                            }
                        }
                    }
                }
                return points[rand.Next(0,points.Count)];
            }
            List<Point> PointGen1(List<Point> points)
            {
                int times = rand.Next(2, 5);
                //int times = 9;
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (!points.Contains(new Point(x, y)))
                        {
                            points.Add(new Point(x, y));
                        }
                    }
                }
                while (points.Count != times)
                {
                    points.Remove(points[rand.Next(0,points.Count)]);
                }
                return points;
            }
            List<Point> PointGen2(List<Point> points)
            {
                int times = rand.Next(2, 20);
                while (points.Count != times)
                {
                    Point point = new Point(rand.Next(-points.Count , points.Count +1), rand.Next(-points.Count , points.Count +1));
                    if (!points.Contains(point))
                    {
                        points.Add(point);
                    }
                }
                return points;
            }

            void GenGeneration()
            {
                List<Point> points = new List<Point>();
                PointGen1(points);
                
                for (int i = 0; i < points.Count; i++)
                {
                    Color color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
                    switch (rand.Next(1, 14))
                    {
                        case 1: { genList.Add(new Genome { part = "Mouth", localplace = points[i], color = color }); bodyTypes.Add(new Mouth()); } break;
                        case 2: { genList.Add(new Genome { part = "Leg", localplace = points[i], color = color }); bodyTypes.Add(new Leg()); } break;
                        case 3: { genList.Add(new Genome { part = "Eye", localplace = points[i], color = color }); bodyTypes.Add(new Eye()); } break;
                        case 4: { genList.Add(new Genome { part = "Brain", localplace = points[i], color = color }); bodyTypes.Add(new Brain()); } break;
                        case 5: { genList.Add(new Genome { part = "Fats", localplace = points[i], color = color }); bodyTypes.Add(new Fats()); } break;
                        case 6: { genList.Add(new Genome { part = "Genitals", localplace = points[i], color = color }); bodyTypes.Add(new Genitals()); } break;
                        case 7: { genList.Add(new Genome { part = "Stomach", localplace = points[i], color = color }); bodyTypes.Add(new Stomach()); } break;
                        case 8: { genList.Add(new Genome { part = "MiteHorns", localplace = points[i], color = color }); bodyTypes.Add(new MiteHorns()); } break;
                        case 9: { genList.Add(new Genome { part = "Clues", localplace = points[i], color = color }); bodyTypes.Add(new Clues()); } break;
                        case 10: { genList.Add(new Genome { part = "Filter", localplace = points[i], color = color }); bodyTypes.Add(new Filter()); } break;
                        case 11: { genList.Add(new Genome { part = "Claws", localplace = points[i], color = color }); bodyTypes.Add(new Claws()); } break;
                        case 12: { genList.Add(new Genome { part = "Sensors", localplace = points[i], color = color }); bodyTypes.Add(new Sensors()); } break;
                        case 13: { genList.Add(new Genome { part = "Cloaca", localplace = points[i], color = color }); bodyTypes.Add(new Cloaca()); } break;

                    }
                    bodyTypes[bodyTypes.Count - 1].localplace = genList[genList.Count - 1].localplace;
                    bodyTypes[bodyTypes.Count - 1].color = genList[genList.Count - 1].color;
                }
                points.Clear();
                //-----------------Для теста
                //genList.Add(new Genome { part = "Cloaca", localplace = Normalizator(), color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) }); bodyTypes.Add(new Cloaca());
                //bodyTypes[bodyTypes.Count - 1].localplace = genList[genList.Count - 1].localplace;
                //bodyTypes[bodyTypes.Count - 1].color = genList[genList.Count - 1].color;
                genList.Add(new Genome { part = "Leg", localplace = Normalizator(), color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) }); bodyTypes.Add(new Leg());
                bodyTypes[bodyTypes.Count - 1].localplace = genList[genList.Count - 1].localplace;
                bodyTypes[bodyTypes.Count - 1].color = genList[genList.Count - 1].color;
                //-----------------

            }

            public List<Genome> GenCopyes(List<Genome> meGens, List<Genome> genomes)
            {
                for (int i = 0; i < genomes.Count; i++)
                {
                    meGens.Add(new Genome { part = genomes[i].part, localplace = genomes[i].localplace, color = genomes[i].color });
                }
                return meGens;
            }
            void GenMutation(List<Genome> genotype)
            {
                //любое число для сравнения обязано быть меньше 100 иначе при уровне радиации 200 результат никогда не будет положительным
                //Stage 1 some chenges
                int chance = rand.Next(0, 212 - radiation);
                //1
                if (chance == 1)
                {
                    int rnd = rand.Next(0, genotype.Count);
                    genotype[rnd] = new Genome { localplace = Normalizator(), part = genotype[rnd].part, color = genotype[rnd].color };
                }
                chance = rand.Next(0, 212 - radiation);
                //2
                if (chance == 2) //цвет
                {
                    int rnd = rand.Next(0, genotype.Count);
                    genotype[rnd] = new Genome { localplace = genotype[rnd].localplace, part = genotype[rnd].part, color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) };
                }
                chance = rand.Next(0, 212 - radiation);
                //3
                if (chance == 3)
                {
                    int rnd = parameters.dublicateDelayMax + rand.Next(-radiation / 10, radiation / 10);
                    parameters.dublicateDelayMax = rnd < 50 ? rnd : 50;
                }
                chance = rand.Next(0, 212 - radiation);
                //4
                if (chance == 4)
                {
                    int rnd = parameters.dublicateFood + rand.Next(-300, 300);
                    parameters.dublicateFood = rnd < parameters.maxFood && rnd > parameters.maxFood / 10 ? rnd : parameters.dublicateFood;
                }
                chance = rand.Next(0, 212 - radiation);
                //5
                if (chance == 5)
                {
                    int rnd = parameters.maxage + rand.Next(-300, 300);
                    parameters.maxage = rnd > 0 ? rnd : parameters.maxage;
                }
                chance = rand.Next(0, 212 - radiation);
                //6
                if (chance == 6)
                {
                    int rnd = parameters.maxFood + rand.Next(-300, 300);
                    parameters.maxFood = rnd > parameters.dublicateFood ? rnd : parameters.maxFood;
                }
                chance = rand.Next(0, 212 - radiation);
                //7
                if (chance == 7)
                {
                    int rnd = parameters.dublicateFoodPrice + rand.Next(-300, 300);
                    parameters.dublicateFoodPrice = rnd > 0 ? rnd : parameters.dublicateFoodPrice;
                }
                chance = rand.Next(0, 212 - radiation);
                //8
                if (chance == 8)
                {
                    int rnd = parameters.dublicateAgeMin + rand.Next(-300, 300);
                    parameters.dublicateAgeMin = rnd > 0 ? rnd : parameters.dublicateAgeMin;
                }
                chance = rand.Next(0, 212 - radiation);
                //9
                if (chance == 9)
                {
                    int rnd = parameters.maxFatigue + rand.Next(-10, 11);
                    parameters.maxFatigue = rnd > 0 ? rnd : parameters.maxFatigue;
                }
                chance = rand.Next(0, 212 - radiation);
                //10
                if (chance == 10)
                {
                    int rnd = parameters.exhaustionLvl + rand.Next(-1, 2);
                    parameters.exhaustionLvl = rnd > 0 && rnd <= 10 ? rnd : parameters.exhaustionLvl;
                }
                chance = rand.Next(0, 212 - radiation);
                //11
                if (chance == 11)
                {
                    int rnd = parameters.hungryFoodLVL + rand.Next(-200, 200);
                    parameters.hungryFoodLVL = rnd > 0 && rnd <= 500 ? rnd : parameters.hungryFoodLVL;
                }


                //Stage 3 Расшифровка генотипа и создания частей тела

            }
            void BodyAddRandomPart(List<Genome> genotype)
            {
                //Stage 2 new parts
                if (rand.Next(0, 222 - radiation) == 21)
                {
                    genotype.Add(new Genome { localplace = Normalizator(), part = partsNames[rand.Next(0, partsNames.Count)], color = genotype[0].color });
                }
            }
            void BodyChengeRandomPart(List<Genome> genotype)
            {
                //Stage 2 new parts
                if (rand.Next(0, 223 - radiation) == 22)
                {
                    int place = rand.Next(0, genList.Count);
                    genotype[place] = new Genome { localplace = Normalizator(), part = partsNames[rand.Next(0, partsNames.Count)], color = genotype[place].color };
                }
            }
            void BodyRemoveRandomPart(List<Genome> genotype)//доработать тут(проблема что часть тела может быть убрана но к ней будут "прикреплены" другие и образуется дырка)
            {
                if (rand.Next(0, 311 - radiation) == 23)
                {
                    int place = rand.Next(0, genList.Count);
                    genotype.Remove(genotype[place]);
                }
            }
            void BodyCreate(List<Genome> genList)
            {
                for (int i = 0; i < genList.Count; i++)
                {
                    BodyPart bodyPart = genList[i].part switch
                    {
                        "Mouth" => new Mouth
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "Leg" => new Leg
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "Eye" => new Eye
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "Brain" => new Brain
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "Fats" => new Fats
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "Genitals" => new Genitals
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "Stomach" => new Stomach
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "MiteHorns" => new MiteHorns
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "Filter" => new Filter
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "Clues" => new Clues
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "Claws" => new Claws
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "Sensors" => new Sensors
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        "Cloaca" => new Cloaca
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                        
                        _ => new Fats
                        {
                            localplace = genList[i].localplace,
                            globalplace = point,
                            color = genList[i].color
                        },
                    };
                    bodyTypes.Add(bodyPart);
                    
                }
            }
            public new void Draw(Bitmap bmp)
            {
                Normalize(bmp);
                foreach (var item in bodyTypes)//Clean
                {
                    //Point p = new Point(lastPoint.X + item.localplace.X, lastPoint.Y + item.localplace.Y);
                    Point p = BorderChecker(new Point(lastPoint.X + item.localplace.X, lastPoint.Y + item.localplace.Y), bmp);
                    bmp.SetPixel(p.X, p.Y, Color.Empty);
                    p = BorderChecker(new Point(point.X + item.localplace.X, point.Y + item.localplace.Y), bmp);
                    bmp.SetPixel(p.X, p.Y, item.color);
                }
                //foreach (var item in bodyTypes)//Draw
                //{
                    //Point p = new Point(point.X + item.localplace.X, point.Y + item.localplace.Y);
                //    Point p = BorderChecker(new Point(point.X + item.localplace.X, point.Y + item.localplace.Y), bmp);
                //    bmp.SetPixel(p.X, p.Y, item.color);
                //}
            }
            public void Cleary(Bitmap bmp)
            {
                foreach (var item in bodyTypes)//Clean
                {
                    //Point p = new Point(lastPoint.X + item.localplace.X, lastPoint.Y + item.localplace.Y);
                    Point p = BorderChecker(new Point(lastPoint.X + item.localplace.X, lastPoint.Y + item.localplace.Y), bmp);
                    bmp.SetPixel(p.X, p.Y, Color.Empty);
                }
            }
            public void DoworkPrepare(Bitmap bmp, Controller controllerIN)
            {
                controller ??= controllerIN;
                radiation = controller.radiationLVL;
                lastPoint = point;
                Cleary(bmp);
                SenseSignal.Clear();
                //LegSignal.Clear();
                FindIgnorePoints();
                canDuplicate = Duplicate();

                foreach (var part in bodyTypes)
                {
                    pooopas += part.energyCost;
                    part.lastGlobalplace = new Point(lastPoint.X + part.localplace.X, lastPoint.Y + part.localplace.Y);
                    part.Dosomething(this, bmp);
                    food -= part.energyCost;//--------------------------------------------------------------------------------------сделать отсутствие потребления если часть тела не работала
                    part.globalplace = point;
                    Cleary(bmp);
                }
                
                point = newPoint != new Point(0, 0) ? newPoint : point; //----------------------------------------------------------фикс появления в углу экрана
                age++;//старение
                PoopasAdd();

            }
            void PoopasAdd()
            {
                if (pooopas >= 1000)
                {
                    pooopasGlobal += pooopas;
                    pooopas -= 1000;
                    
                    if (!controller.infectionLVL.ContainsKey(point)) { controller.infectionLVL.Add(point, 500); } else { controller.infectionLVL[point] += 500; }
                }
            }

            public bool Duplicate()
            {
                parameters.dublicateDelay = parameters.dublicateDelay < parameters.dublicateDelayMax ? parameters.dublicateDelay + 1 : parameters.dublicateDelay;
                return parameters.dublicateDelay == parameters.dublicateDelayMax && age < parameters.dublicateAgeMax && parameters.dublicateAgeMin < age ? true : false;
            }
        }

        class Virus:Object
        {

        }
    }

}
