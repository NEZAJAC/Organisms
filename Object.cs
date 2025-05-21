namespace MicroLife_Simulator
{
    public partial class Form1
    {
        abstract class Object
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

            public Color GetPixelUnder(Bitmap bmp, Point point)
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

        }
        struct Genome
        {
            //для частай
            public String part { get; set; }            //название органа
            public Point localplace { get; set; }      //локальное положение органа в организме
            public Color color { get; set; }          //цвет органа(от цвета зависит увидят ли его глаза )
        }
        struct OrganismParameters
        {
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
        }
        class Organism : Object
        {
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
                "Keratin"
            };
            public string[] myGenWords;
            public Organism? globalTarget;
            public int fatigue = 0;//-------------------------------------------------------------------------усталость
            public int exhaustionLvl = 5;
            public bool exhaustion = false;
            public int pooopas = 0;
            public OrganismParameters parameters;
            public Controller? controller = null;
            int radiation = 10;
            //для организма
            public Point newPoint = new Point();
            public Point lastPoint = new Point();
            //--------------------------------------------------количество детей на одно деление
            /// <summary>
            /// Сигналы мозгу для обработки входящие
            /// </summary>
            // public int HungrySignal;//---------------------------------------------------------------------------------гoлод
            public Dictionary<Point, Color> EyeSignal = new Dictionary<Point, Color>();//------------------------------глаза
            public Dictionary<Point, Color> SenseSignal = new Dictionary<Point, Color>();//--------------------------сенсоры
            public int bodyTemperatureSignal;       //------------------------------------------------------температура тела
            public int WithoutDublicateSignal = 0; //---------------------------------------------------задержка размножения
            /// <summary>
            /// Сигналы из мозга обработанные исходящие
            /// </summary>
            public List<Point> ToLegSignal = new List<Point>();//-------------------------------------------точка для движения
            public Point ToMouthSignal = new Point();//----------------------------------------------------------цель поедания
            //---------------------------------------------------------Генотип
            public List<BodyPart> bodyTypes = new List<BodyPart>();
            public List<Genome> genList = new List<Genome>();
            //public List<(string part, Point localplace, Color color)> genList;
            //-----------------------------------------------------
            public bool canDuplicate = false;
            public bool hungry = true;
            //--------------------------------------------------------------------------------------------------------------
            public List<Point> ignorePoints = new List<Point>();//-------------------------------------------------свое тело
            //------------------------------
            public bool move = true;
            public Organism(Point pointIN)//используется для первого запуска или дополнительной генерации организмов.
            {
                point = pointIN;
                food = 9000;
                maxage = 4000;
                age = 0;

                //parameters.childs = 1;
                //parameters.dublicateAgeMin = maxage / 8;

                parameters.dublicateAgeMin = maxage / 8;
                parameters.dublicateAgeMax = maxage - maxage / 8;
                GenGeneration();
                parameters.dublicateDelayMax = 150 * genList.Count / 2;
                parameters.dublicateDelay = parameters.dublicateDelayMax;
                parameters.dublicateFood = 2300 * genList.Count;
                parameters.dublicateFoodPrice = parameters.dublicateFood/2;
                maxfood = 2600 * genList.Count;
                parameters.hungryFoodLVL = maxfood - 2600;
                myGenWords = GetGenotype(this).Split(new char[] { '|' });
                parameters.maxFatigue = rand.Next(70,200);
                fatigue = rand.Next(0, parameters.maxFatigue);
            }
            public Organism(Point pointIN, Organism parent)
            {
                parent.food -= parent.parameters.dublicateFoodPrice;
                point = pointIN;
                food = 2000 * genList.Count;
                maxfood = parent.maxfood;
                parameters.dublicateFood = parent.parameters.dublicateFood;
                maxage = parent.maxage;
                age = 0;
                parameters.dublicateDelayMax = parent.parameters.dublicateDelayMax;
                parameters.dublicateDelay = parameters.dublicateDelayMax;
                parameters.dublicateFoodPrice = parent.parameters.dublicateFoodPrice;
                parameters.dublicateAgeMin = parent.parameters.dublicateAgeMin;
                parameters.dublicateAgeMax = parent.parameters.dublicateAgeMax;
                parameters.maxFatigue = parent.parameters.maxFatigue;
                parameters.exhaustionLvl = parent.parameters.exhaustionLvl;
                parameters.hungryFoodLVL = parent.parameters.hungryFoodLVL;
                fatigue = rand.Next(0, parameters.maxFatigue);
                radiation = parent.radiation;
                genList = GenCopyes(parent.genList);
                GenMutation(genList);
                BodyAddRandomPart(genList);
                BodyChengeRandomPart(genList);
                //BodyRemoveRandomPart(genList);
                BodyCreate(genList);
                myGenWords = GetGenotype(this).Split(new char[] { '|' });
            }
            public Organism(Point pointIN, Organism organismX, Organism organismY) //--------------------------------------------------------Доделать половое размножение
            {
                point = pointIN;
                food = (organismX.food + organismY.food) / 2;
                age = 0;
                parameters.dublicateDelay = parameters.dublicateDelayMax;
                myGenWords = Crossingover(organismX.myGenWords, organismY.myGenWords);
            }
            public Organism(Point pointIN, string genome)
            {
                DecodeGenotype(genome, out myGenWords);
                point = pointIN;
                food = 9000;
                parameters.dublicateFood = maxfood;
                age = 0;

                BodyCreate(genList);

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
                for (int i = biggerInt; i < maxString; i++)
                {
                    myGenWords[i] = rand.Next(0, 2) == 1 ? myGenome[i] : otherGenome[i];
                }
                return myGenWords;
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
                    + selected.maxage.ToString() + "|"
                    + selected.maxfood.ToString() + "|"
                    + selected.parameters.maxFatigue.ToString() + "|";
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
                    maxage = int.Parse(words[j + 7]);
                    maxfood = int.Parse(words[j + 8]);
                    parameters.maxFatigue = int.Parse(words[j + 9]);
                }
            }
            
            /// <summary>
            /// если генотип различается то false иначе true
            /// </summary>
            /// <param name="myGenome"></param>
            /// <param name="otherGenome"></param>
            /// <returns></returns>
            public bool CompireGenotype(string[] myGenome, string[] otherGenome)
            {
                int length = myGenome.Length >= otherGenome.Length ? otherGenome.Length : myGenome.Length;
                int difference = Math.Abs(myGenome.Length - otherGenome.Length);
                for (int i = 0; i < length; i++)
                {
                    difference += myGenome[i] == otherGenome[i] ? 0 : 1;
                }
                return difference <= 10;
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
            Point Normalizator()
            {
                //обходим все наши части тела, выбираем случайную, смотрим пустые места вокруг выбранной части заглядывая в геном организма, запоминаем свободную точку случайную, передаем точку дальше
                
                List<Point> points = new List<Point>();//все точки организма
                foreach (var item in genList)
                {
                    points.Add(item.localplace);
                }
                Point pp = points[rand.Next(0, points.Count)];//случайная точка вокруг которой пляшем
                List<Point> p = new List<Point>();
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if(x != 0 && y != 0 && !points.Contains(new Point(pp.X + x, pp.Y + y)))
                        {
                            p.Add(new Point(pp.X + x, pp.Y + y));
                        }
                    }
                }
                return p[rand.Next(0,p.Count)];
            }

            Point Normalizator1()
            {
                List<Point> accessPoints = new List<Point>();
                foreach (var item in genList)
                {
                    for (int i = -1; i < 1; i++)//обходим диапазон доступный для органа
                    {
                        for (int j = -1; j < 1; j++)
                        {
                            if (!accessPoints.Contains(new Point(item.localplace.X + i, item.localplace.Y + j)))//пропускаем занятые точки
                            {
                                accessPoints.Add(new Point(item.localplace.X + i, item.localplace.Y + j));//добавляем все доступные места в список
                            }
                        }
                    }
                }
                Point newP = accessPoints.Count > 0 ? accessPoints[rand.Next(0, accessPoints.Count)] : new Point(0, 0);
                return newP;//выбираем случайное место для нового органа
            }
            void GenGeneration()
            {
                List<Point> points = new List<Point>();
                int times = rand.Next(2, 5);
                while (points.Count != times)
                {
                    Point point = new Point(rand.Next(-times / 2, times / 2), rand.Next(-times / 2, times / 2));
                    if (!points.Contains(point))
                    {
                        points.Add(point);
                    }
                }

                for (int i = 0; i < points.Count; i++)
                {
                    Color color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
                    switch (rand.Next(1, 13))
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

                    }
                    bodyTypes[bodyTypes.Count - 1].localplace = genList[genList.Count - 1].localplace;
                    bodyTypes[bodyTypes.Count - 1].color = genList[genList.Count - 1].color;
                }
                points.Clear();
                //-----------------Для теста
                //genList.Add(new Genome { part = "Stomach", localplace = Normalizator(), color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) }); hasStomach = true; bodyTypes.Add(new Stomach());
                //bodyTypes[bodyTypes.Count - 1].localplace = genList[genList.Count - 1].localplace;
                //bodyTypes[bodyTypes.Count - 1].color = genList[genList.Count - 1].color;
                genList.Add(new Genome { part = "Leg", localplace = Normalizator(), color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) }); bodyTypes.Add(new Leg());
                bodyTypes[bodyTypes.Count - 1].localplace = genList[genList.Count - 1].localplace;
                bodyTypes[bodyTypes.Count - 1].color = genList[genList.Count - 1].color;
                //-----------------

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
            void GenMutation(List<Genome> genotype)
            {
                //любое число для сравнения обязано быть меньше 100 иначе при уровне радиации 900 результат никогда не будет положительным
                //Stage 1 some chenges
                if (rand.Next(0, 311 - radiation) == 1)
                {
                    int rnd = rand.Next(0, genotype.Count);
                    genotype[rnd] = new Genome { localplace = Normalizator(), part = genotype[rnd].part, color = genotype[rnd].color };
                }
                if (rand.Next(0, 311 - radiation) == 2) //цвет
                {
                    int rnd = rand.Next(0, genotype.Count);
                    genotype[rnd] = new Genome { localplace = genotype[rnd].localplace, part = genotype[rnd].part, color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) };
                }
                if (rand.Next(0, 311 - radiation) == 3)
                {
                    int rnd = parameters.dublicateDelayMax + rand.Next(-radiation / 10, radiation / 10);
                    parameters.dublicateDelayMax = rnd < 50 ? rnd : 50;
                }
                if (rand.Next(0, 311 - radiation) == 4)
                {
                    int rnd = parameters.dublicateFood + rand.Next(-300, 300);
                    parameters.dublicateFood = rnd < maxfood && rnd > maxfood / 10 ? rnd : parameters.dublicateFood;
                }
                if (rand.Next(0, 311 - radiation) == 5)
                {
                    int rnd = maxage + rand.Next(-300, 300);
                    maxage = rnd > 0 ? rnd : maxage;
                }
                if (rand.Next(0, 311 - radiation) == 6)
                {
                    int rnd = maxfood + rand.Next(-300, 300);
                    maxfood = rnd > parameters.dublicateFood ? rnd : maxfood;
                }
                if (rand.Next(0, 311 - radiation) == 7)
                {
                    int rnd = parameters.dublicateFoodPrice + rand.Next(-300, 300);
                    parameters.dublicateFoodPrice = rnd > 0 ? rnd : parameters.dublicateFoodPrice;
                }
                if (rand.Next(0, 311 - radiation) == 8)
                {
                    int rnd = parameters.dublicateAgeMin + rand.Next(-300, 300);
                    parameters.dublicateAgeMin = rnd > 0 ? rnd : parameters.dublicateAgeMin;
                }
                if (rand.Next(0, 311 - radiation) == 9)
                {
                    int rnd = parameters.maxFatigue + rand.Next(-10, 11);
                    parameters.maxFatigue = rnd > 0 ? rnd : parameters.maxFatigue;
                }
                if (rand.Next(0, 311 - radiation) == 10)
                {
                    int rnd = parameters.exhaustionLvl + rand.Next(-1, 2);
                    parameters.exhaustionLvl = rnd > 0 && rnd <= 10 ? rnd : parameters.exhaustionLvl;
                }
                if (rand.Next(0, 311 - radiation) == 11)
                {
                    int rnd = parameters.hungryFoodLVL + rand.Next(-200, 200);
                    parameters.hungryFoodLVL = rnd > 0 && rnd <= 500 ? rnd : parameters.hungryFoodLVL;
                }


                //Stage 3 Расшифровка генотипа и создания частей тела

            }
            void BodyAddRandomPart(List<Genome> genotype)
            {
                //Stage 2 new parts
                if (rand.Next(0, 311 - radiation) == 21)
                {
                    genotype.Add(new Genome { localplace = Normalizator(), part = partsNames[rand.Next(0, partsNames.Count)], color = genotype[0].color });
                }
            }
            void BodyChengeRandomPart(List<Genome> genotype)
            {
                //Stage 2 new parts
                if (rand.Next(0, 311 - radiation) == 22)
                {
                    int place = rand.Next(0, genList.Count);
                    genotype[place] = new Genome { localplace = Normalizator(), part = partsNames[rand.Next(0, partsNames.Count)], color = genotype[place].color };
                }
            }
            void BodyRemoveRandomPart(List<Genome> genotype)//доработать тут(проблема что часть тела может быть убрана но к ней будут "прикреплены" другие и образуется дырка)
            {
                if (rand.Next(0, 1011 - radiation) < 3 + radiation / 100)
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
                    Point p = new Point(lastPoint.X + item.localplace.X, lastPoint.Y + item.localplace.Y);
                    p = BorderChecker(p, bmp);
                    bmp.SetPixel(p.X, p.Y, Color.Empty);
                }
                foreach (var item in bodyTypes)//Draw
                {
                    Point p = new Point(point.X + item.localplace.X, point.Y + item.localplace.Y);
                    p = BorderChecker(p, bmp);
                    bmp.SetPixel(p.X, p.Y, item.color);
                }
            }
            public void Cleary(Bitmap bmp)
            {
                foreach (var item in bodyTypes)//Clean
                {
                    Point p = new Point(lastPoint.X + item.localplace.X, lastPoint.Y + item.localplace.Y);
                    p = BorderChecker(p, bmp);
                    bmp.SetPixel(p.X, p.Y, Color.Empty);
                }
            }
            public void DoworkPrepare(Bitmap bmp, Controller controllerIN)
            {
                controller ??= controllerIN;
                radiation = controller.radiationLVL;
                lastPoint = point;
                Cleary(bmp);
                //TurnRL2();
                EyeSignal.Clear();
                SenseSignal.Clear();
                FindIgnorePoints();

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
                canDuplicate = Duplicate();
                age++;//старение
                PoopasAdd();

            }
            void PoopasAdd()
            {
                if (pooopas >= 1000)
                {
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

    }
}
