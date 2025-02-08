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
                maxage = maxfood * 2;
                pen.Color = Color.Green;
                point = pointIN;

            }

            public bool GrassUpdate(int sunIN)
            {
                food = food >= maxfood ? food - maxfood + 1 : food;
                food = sunIN > 0 ? food + sunIN : food--;
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

            public int dublicateDelay;    //--------------------------------------задержка перед повторным делением(текущая)
            public int dublicateDelayMax; //-----------------------------задержка перед повторным делением(верхнее значение)
            public int dublicateFood;     //--------------------------------------требуемое количество насыщения для деления
            public int dublicateFoodPrice;//--------------------------------------------------------------------цена деления
            public int hungryFoodLVL;     //-------------------------------------значение на котором организм захочет кушать
            public int dublicateAgeMin;   //-----------------------------------------------------минимальный возраст деления
            public int dublicateAgeMax;   //----------------------------------------------------максимальный возраст деления
            public int childs;
        }
        class Organism : Object
        {
            OrganismParameters parameters;
            public Controller? controller = null;
            int radiation = 10;
            //для организма
            public Point newPoint = new Point();
            public Point lastPoint = new Point();

            //---------------------------
            public int bodyTemperatureMax;
            public int bodyTemperatureMin;
            //---------------------------размножение
            public bool canDuplicate = true;
            public int dublicateDelay;    //--------------------------------------задержка перед повторным делением(текущая)
            public int dublicateDelayMax; //-----------------------------задержка перед повторным делением(верхнее значение)
            public int dublicateFood;     //--------------------------------------требуемое количество насыщения для деления
            public int dublicateFoodPrice;//--------------------------------------------------------------------цена деления
            public int hungryFoodLVL;     //-------------------------------------значение на котором организм захочет кушать
            public int dublicateAgeMin;   //-----------------------------------------------------минимальный возраст деления
            public int dublicateAgeMax;   //----------------------------------------------------максимальный возраст деления
            public int childs;          //--------------------------------------------------количество детей на одно деление
            /// <summary>
            /// Сигналы мозгу для обработки входящие
            /// </summary>
            public int HungrySignal;//---------------------------------------------------------------------------------гoлод
            public Dictionary<Point, Color> EyeSignal = new Dictionary<Point, Color>();//---------------------глаза, сенсоры
            public int bodyTemperatureSignal;       //------------------------------------------------------температура тела
            public int WithoutDublicateSignal = 0; //---------------------------------------------------задержка размножения
            /// <summary>
            /// Сигналы из мозга обработанные исходящие
            /// </summary>
            public List<Point> ToLegSignal = new List<Point>();//-------------------------------------------точка для движения
            public List<Point> closestPoiints = new List<Point>();
            public Point ToMouthSignal = new Point();//----------------------------------------------------------цель поедания
            //---------------------------------------------------------Генотип
            public List<BodyPart> bodyTypes = new List<BodyPart>();
            public List<Genome> genList = new List<Genome>();
            //--------------------------------------------------------------------------------------------------------------
            public List<Point> ignorePoints = new List<Point>();//-------------------------------------------------свое тело
            public bool hasGenitals = false;//--------------------------------------------------------есть ли половые органы
            public bool hasMouth = false;//---------------------------------------------------------------------есть ли мозг
            public bool hasLeg = false;
            public bool hasBrain = false;
            public bool hasEye = false;
            public bool hasFats = false;
            public bool hasStomach = false;
            public bool hasMiteHorns = false;
            public bool hasFilter = false;
            //------------------------------
            public bool move = true;
            public Organism(Point pointIN)//используется для первого запуска или дополнительной генерации организмов.
            {
                point = pointIN;
                food = 9000;
                maxage = 2000;
                age = 0;

                //parameters.childs = 1;
                //parameters.dublicateAgeMin = maxage / 8;


                dublicateAgeMin = maxage / 8;
                dublicateAgeMax = maxage - maxage / 8;
                GenGeneration();
                dublicateDelayMax = 150 * genList.Count;
                dublicateDelay = dublicateDelayMax;
                dublicateFood = 2000 * genList.Count;
                dublicateFoodPrice = 2000 * genList.Count;
                maxfood = 2600 * genList.Count;
                hungryFoodLVL = maxfood / 2;
            }
            public Organism(Point pointIN, Organism parent)
            {
                point = pointIN;
                food = parent.food;
                maxfood = parent.maxfood;
                dublicateFood = parent.dublicateFood;
                maxage = parent.maxage;
                age = 0;
                dublicateDelayMax = parent.dublicateDelayMax;
                dublicateDelay = dublicateDelayMax;
                dublicateFoodPrice = parent.dublicateFoodPrice;
                dublicateAgeMin = parent.dublicateAgeMin;
                dublicateAgeMax = parent.dublicateAgeMax;
                genList = GenCopyes(parent.genList);
                GenMutation(genList);
                BodyAddRandomPart(genList);
                BodyChengeRandomPart(genList);
                BodyRemoveRandomPart(genList);
                BodyCreate(genList);
            }
            public Organism(Point pointIN, Organism organismX, Organism organismY) //--------------------------------------------------------Доделать половое размножение
            {
                point = pointIN;

            }
            public Organism(Point pointIN, string genome)
            {
                string[] words = genome.Split(new char[] { '|' });
                if (int.TryParse(words[0], out int id))
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
                    dublicateAgeMax = int.Parse(words[j]);
                    dublicateAgeMin = int.Parse(words[j + 1]);
                    dublicateDelayMax = int.Parse(words[j + 2]);
                    dublicateFoodPrice = int.Parse(words[j + 3]);
                    maxage = int.Parse(words[j + 4]);
                    maxfood = int.Parse(words[j + 5]);
                }

                point = pointIN;
                food = 9000;
                dublicateFood = maxfood;
                age = 0;
                dublicateDelay = dublicateDelayMax;

                BodyCreate(genList);

            }
            void FindIgnorePoints()
            {
                ignorePoints.Clear();
                foreach (var item in bodyTypes)
                {
                    ignorePoints.Add(new Point(point.X + item.localplace.X, point.Y + item.localplace.Y));
                }
            }
            Point Normalizator2()
            {
                List<Point> ignorePoints = new List<Point>();
                List<Point> accessPoints = new List<Point>();
                foreach (var item in genList)
                {
                    ignorePoints.Add(new Point(item.localplace.X, item.localplace.Y));//находим точки которые уже заняты
                }
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
                List<Point> points = new List<Point>();
                for (int i = -genList.Count / 2; i <= genList.Count / 2; i++)//обходим диапазон доступный для органа
                {
                    for (int j = -genList.Count / 2; j <= genList.Count / 2; j++)
                    {
                        if (!ignorePoints.Contains(new Point(i, j)))//пропускаем занятые точки
                        {
                            points.Add(new Point(i, j));//добавляем все доступные места в список
                        }
                    }
                }
                ignorePoints.Clear();
                Point newP = points.Count > 0 ? points[rand.Next(0, points.Count)] : new Point(0, 0);
                return newP;//выбираем случайное место для нового органа
            }

            Point Normalizator()
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
                int times = rand.Next(5, 8);
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
                    switch (rand.Next(1, 10))
                    {
                        case 1: { genList.Add(new Genome { part = "Mouth", localplace = points[i], color = color }); bodyTypes.Add(new Mouth()); hasMouth = true; } break;
                        case 2: { genList.Add(new Genome { part = "Leg", localplace = points[i], color = color }); bodyTypes.Add(new Leg()); hasLeg = true; } break;
                        case 3: { genList.Add(new Genome { part = "Eye", localplace = points[i], color = color }); bodyTypes.Add(new Eye()); hasEye = true; } break;
                        case 4: { genList.Add(new Genome { part = "Brain", localplace = points[i], color = color }); bodyTypes.Add(new Brain()); hasBrain = true; } break;
                        case 5: { genList.Add(new Genome { part = "Fats", localplace = points[i], color = color }); bodyTypes.Add(new Fats()); hasFats = true; } break;
                        case 6: { genList.Add(new Genome { part = "Genitals", localplace = points[i], color = color }); bodyTypes.Add(new Genitals()); hasGenitals = true; } break;
                        case 7: { genList.Add(new Genome { part = "Stomach", localplace = points[i], color = color }); bodyTypes.Add(new Stomach()); hasStomach = true; } break;
                        case 8: { genList.Add(new Genome { part = "MiteHorns", localplace = points[i], color = color }); bodyTypes.Add(new MiteHorns()); hasMiteHorns = true; } break;
                        case 9: { genList.Add(new Genome { part = "Filter", localplace = points[i], color = color }); bodyTypes.Add(new Filter()); hasFilter = true; } break;

                    }
                    bodyTypes[bodyTypes.Count - 1].localplace = genList[genList.Count - 1].localplace;
                    bodyTypes[bodyTypes.Count - 1].color = genList[genList.Count - 1].color;
                }
                points.Clear();
                //-----------------Для теста
                //genList.Add(new Genome { part = "Filter", localplace = Normalizator(), color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) }); hasStomach = true; bodyTypes.Add(new Filter());
                //bodyTypes[bodyTypes.Count - 1].localplace = genList[genList.Count - 1].localplace;
                //bodyTypes[bodyTypes.Count - 1].color = genList[genList.Count - 1].color;
                //genList.Add(new Genome { part = "Eye", localplace = Normalizator(), color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) }); bodyTypes.Add(new Eye());
                //bodyTypes[bodyTypes.Count - 1].localplace = genList[genList.Count - 1].localplace;
                //bodyTypes[bodyTypes.Count - 1].color = genList[genList.Count - 1].color;
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
                if (rand.Next(0, 1011 - radiation) < 3 + radiation / 100)
                {
                    int rnd = rand.Next(0, genotype.Count);
                    genotype[rnd] = new Genome { localplace = Normalizator(), part = genotype[rnd].part, color = genotype[rnd].color };
                }
                if (rand.Next(0, 1011 - radiation) < 5 + radiation / 50) //цвет
                {
                    int rnd = rand.Next(0, genotype.Count);
                    genotype[rnd] = new Genome { localplace = genotype[rnd].localplace, part = genotype[rnd].part, color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) };
                }
                if (rand.Next(0, 1011 - radiation) < 3 + radiation / 50)
                {
                    int rnd = dublicateDelayMax + rand.Next(-radiation / 10, radiation / 10);
                    dublicateDelayMax = rnd < 50 ? rnd : 50;
                }
                if (rand.Next(0, 1011 - radiation) < 3 + radiation / 50)
                {
                    int rnd = dublicateFood + rand.Next(-300, 300);
                    dublicateFood = rnd < maxfood && rnd > maxfood / 10 ? rnd : dublicateFood;
                }
                if (rand.Next(0, 1011 - radiation) < 3 + radiation / 50)
                {
                    int rnd = maxage + rand.Next(-300, 300);
                    maxage = rnd > 0 ? rnd : maxage;
                }
                if (rand.Next(0, 1011 - radiation) < 3 + radiation / 50)
                {
                    int rnd = maxfood + rand.Next(-300, 300);
                    maxfood = rnd > dublicateFood ? rnd : 300;
                }
                if (rand.Next(0, 1011 - radiation) < 3 + radiation / 50)
                {
                    int rnd = dublicateFoodPrice + rand.Next(-300, 300);
                    dublicateFoodPrice = rnd > 0 ? rnd : 100;
                }
                if (rand.Next(0, 1011 - radiation) < 3 + radiation / 50)
                {
                    int rnd = dublicateAgeMin + rand.Next(-300, 300);
                    dublicateAgeMin = rnd > 0 ? rnd : 100;
                }


                //Stage 3 Расшифровка генотипа и создания частей тела

            }
            void BodyAddRandomPart(List<Genome> genotype)
            {
                //Stage 2 new parts
                if (rand.Next(0, 1011 - radiation) < 3 + radiation / 100)
                {
                    switch (rand.Next(1, 10))
                    {
                        case 1: genotype.Add(new Genome { localplace = Normalizator(), part = "Mouth", color = genotype[0].color }); hasMouth = true; break;
                        case 2: genotype.Add(new Genome { localplace = Normalizator(), part = "Leg", color = genotype[0].color }); hasLeg = true; break;
                        case 3: genotype.Add(new Genome { localplace = Normalizator(), part = "Eye", color = genotype[0].color }); hasEye = true; break;
                        case 4: genotype.Add(new Genome { localplace = Normalizator(), part = "Brain", color = genotype[0].color }); hasBrain = true; break;
                        case 5: genotype.Add(new Genome { localplace = Normalizator(), part = "Fats", color = genotype[0].color }); hasFats = true; break;
                        case 6: genotype.Add(new Genome { localplace = Normalizator(), part = "Genitals", color = genotype[0].color }); hasGenitals = true; break;
                        case 7: genotype.Add(new Genome { localplace = Normalizator(), part = "Stomach", color = genotype[0].color }); hasStomach = true; break;
                        case 8: genotype.Add(new Genome { localplace = Normalizator(), part = "MiteHorns", color = genotype[0].color }); hasMiteHorns = true; break;
                        case 9: genotype.Add(new Genome { localplace = Normalizator(), part = "Filter", color = genotype[0].color }); hasFilter = true; break;
                    }
                }
            }
            void BodyChengeRandomPart(List<Genome> genotype)
            {
                //Stage 2 new parts
                if (rand.Next(0, 1011 - radiation) < 3 + radiation / 100)
                {
                    int place = rand.Next(0, genList.Count);
                    switch (rand.Next(1, 10))
                    {
                        case 1: genotype[place] = new Genome { localplace = Normalizator(), part = "Mouth", color = genotype[0].color }; hasMouth = true; break;
                        case 2: genotype[place] = new Genome { localplace = Normalizator(), part = "Leg", color = genotype[0].color }; hasLeg = true; break;
                        case 3: genotype[place] = new Genome { localplace = Normalizator(), part = "Eye", color = genotype[0].color }; hasEye = true; break;
                        case 4: genotype[place] = new Genome { localplace = Normalizator(), part = "Brain", color = genotype[0].color }; hasBrain = true; break;
                        case 5: genotype[place] = new Genome { localplace = Normalizator(), part = "Fats", color = genotype[0].color }; hasFats = true; break;
                        case 6: genotype[place] = new Genome { localplace = Normalizator(), part = "Genitals", color = genotype[0].color }; hasGenitals = true; break;
                        case 7: genotype[place] = new Genome { localplace = Normalizator(), part = "Stomach", color = genotype[0].color }; hasStomach = true; break;
                        case 8: genotype[place] = new Genome { localplace = Normalizator(), part = "MiteHorns", color = genotype[0].color }; hasMiteHorns = true; break;
                        case 9: genotype[place] = new Genome { localplace = Normalizator(), part = "Filter", color = genotype[0].color }; hasFilter = true; break;
                    }
                }
            }
            void BodyRemoveRandomPart(List<Genome> genotype)
            {
                //Stage 2 new parts
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
                Cleary(bmp);
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

            public void DoworkPrepare(Bitmap bmp, Controller controllerIN)//берет движение от части тела, берет все другие действия от других частей тела
            {
                controller ??= controllerIN;
                radiation = controller.radiationLVL;
                lastPoint = point;
                EyeSignal.Clear();
                FindIgnorePoints();
                foreach (var part in bodyTypes)
                {
                    food -= part.energyCost;
                    part.lastGlobalplace = new Point(lastPoint.X + part.localplace.X, lastPoint.Y + part.localplace.Y);
                    part.Dosomething(this, bmp);
                    part.globalplace = point;
                }
                point = newPoint;
                canDuplicate = Duplicate();
                age++;//старение
            }

            public bool Duplicate()
            {
                dublicateDelay = dublicateDelay < dublicateDelayMax ? dublicateDelay + 1 : dublicateDelay;
                return dublicateDelay == dublicateDelayMax && age < dublicateAgeMax && dublicateAgeMin < age ? true : false;
            }
        }

    }
}
