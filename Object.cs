using System.Drawing.Drawing2D;

namespace grass
{
    public partial class Form1
    {
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
                food = food >= maxfood ? food - maxfood + 1 : food;
                food = sunIN > 0 ? food + sunIN : food--;
                return food >= maxfood;
            }

            public bool Duplicate(Bitmap bmp, out Point pointt)
            {
                int x = point.X + rand.Next(-10, 11);
                int y = point.Y + rand.Next(-10, 11);
                Point newpoint = BorderChecker(x, y, bmp);
                Color color = bmp.GetPixel(newpoint.X, newpoint.Y);
                if (color.G == 0 && color.B == 0 && color.R == 0) { pointt = newpoint; return true; } else pointt = new Point(-1,-1);  return false;
            }

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
            public Dictionary<Point, Grass>? dictionaryOfGrass;
            public Dictionary<Point, Organism>? cellDictionary;

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

                food = 9000;
                maxfood = scale;

                maxage = scale / 5;
                age = 0;

                dublicateDelayMax = scale / 40;
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

                dublicateDelayMax = scale / 40;
                dublicateDelay = dublicateDelayMax;
                dublicateFood = scale;
            }
            public Organism(Point pointIN, Organism organismX, Organism organismY) //--------------------------------------------------------Доделать половое размножение
            {
                point = pointIN;

                food = scale / 2;
                maxfood = scale;

                maxage = scale / 5;
                age = 0;

                dublicateDelayMax = scale / 40;
                dublicateDelay = dublicateDelayMax;
                dublicateFood = scale;
            }
            Point Normalizator()
            {
                List<Point> ignorePoints = new List<Point>();
                foreach (var item in genList)
                {
                    ignorePoints.Add(new Point(item.localplace.X, item.localplace.Y));//находим точки которые уже заняты
                }
                List<Point> points = new List<Point>();
                for (int i = -1; i < 2; i++)//обходим диапазон доступный для органа
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (!ignorePoints.Contains(new Point(i, j)))//пропускаем занятые точки
                        {
                            points.Add(new Point(i, j));//добавляем все доступные места в список
                        }
                    }
                }
                Point newP = points.Count > 0 ? points[rand.Next(0, points.Count)] : new Point(0, 0);
                return newP;//выбираем случайное место для нового органа
            }
            void GenGeneration()
            {

                List<Point> points = new List<Point>();
                int times = rand.Next(2, 4);
                while (points.Count != times)
                {
                    Point point = new Point(rand.Next(-times / 2, times / 2), rand.Next(-times / 2, times / 2));
                    if (!points.Contains(point))
                    {
                        points.Add(point);
                    }
                }
                for (int i = 0; i < times; i++)
                {
                    Color color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
                    switch (rand.Next(1, 8))
                    {
                        case 1: { genList.Add(new Genome { part = "Mouth", localplace = points[i], color = color }); bodyTypes.Add(new Mouth()); } break;
                        case 2: { genList.Add(new Genome { part = "Leg", localplace = points[i], color = color }); bodyTypes.Add(new Leg()); } break;
                        case 3: { genList.Add(new Genome { part = "Eye", localplace = points[i], color = color }); bodyTypes.Add(new Eye()); } break;
                        case 4: { genList.Add(new Genome { part = "Brain", localplace = points[i], color = color }); bodyTypes.Add(new Brain()); hasBrain = true; } break;
                        case 5: { genList.Add(new Genome { part = "Fats", localplace = points[i], color = color }); bodyTypes.Add(new Fats()); } break;
                        case 6: { genList.Add(new Genome { part = "Genitals", localplace = points[i], color = color }); bodyTypes.Add(new Genitals()); hasGenitals = true; } break;
                        case 7: { genList.Add(new Genome { part = "Stomach", localplace = points[i], color = color }); bodyTypes.Add(new Stomach()); } break;

                    }
                    bodyTypes[i].localplace = genList[i].localplace;
                    bodyTypes[i].color = genList[i].color;
                }
                //-----------------Для теста
                genList.Add(new Genome { part = "MiteHorns", localplace = Normalizator(), color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) }); bodyTypes.Add(new MiteHorns());
                bodyTypes[bodyTypes.Count - 1].localplace = genList[genList.Count - 1].localplace;
                bodyTypes[bodyTypes.Count - 1].color = genList[genList.Count - 1].color;
                //-----------------
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
                if (rand.Next(0, 1011 - radiation) < 3)
                {
                    int rnd = rand.Next(0, genotype.Count);
                    genotype[rnd] = new Genome { localplace = Normalizator(), part = genotype[rnd].part, color = genotype[rnd].color };
                }
                if (rand.Next(0, 1011 - radiation) < 5) //цвет
                {
                    int rnd = rand.Next(0, genotype.Count);
                    genotype[rnd] = new Genome { localplace = genotype[rnd].localplace, part = genotype[rnd].part, color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)) };
                }
                if (rand.Next(0, 1011 - radiation) <3)
                {
                    int rnd = dublicateDelayMax + rand.Next(-radiation / 10, radiation / 10);
                    dublicateDelayMax = rnd > 50 ? rnd : 50;
                }
                if (rand.Next(0, 1011 - radiation) < 3)
                {
                    int rnd = dublicateFood + rand.Next(-300, 300);
                    dublicateFood = rnd < maxfood && rnd > maxfood / 10 ? rnd : dublicateFood;
                }
                if (rand.Next(0, 1011 - radiation) < 3)
                {
                    int rnd = maxage + rand.Next(-300, 300);
                    maxage = rnd > 0 ? rnd : maxage;
                }
                if (rand.Next(0, 1011 - radiation) < 3)
                {
                    int rnd = maxfood + rand.Next(-300, 300);
                    maxfood = rnd > 0 ? rnd : 100;
                }

                //Stage 2 new parts
                if (rand.Next(0, 1011 - radiation) == 50)
                {
                    switch (rand.Next(1, 9))
                    {
                        case 1: genotype.Add(new Genome { localplace = Normalizator(), part = "Mouth", color = genotype[0].color }); break;
                        case 2: genotype.Add(new Genome { localplace = Normalizator(), part = "Leg", color = genotype[0].color }); break;
                        case 3: genotype.Add(new Genome { localplace = Normalizator(), part = "Eye", color = genotype[0].color }); break;
                        case 4: genotype.Add(new Genome { localplace = Normalizator(), part = "Brain", color = genotype[0].color }); hasBrain = true; break;
                        case 5: genotype.Add(new Genome { localplace = Normalizator(), part = "Fats", color = genotype[0].color }); break;
                        case 6: genotype.Add(new Genome { localplace = Normalizator(), part = "Genitals", color = genotype[0].color }); hasGenitals = true; break;
                        case 7: genotype.Add(new Genome { localplace = Normalizator(), part = "Stomach", color = genotype[0].color }); break;
                        case 8: genotype.Add(new Genome { localplace = Normalizator(), part = "MiteHorns", color = genotype[0].color }); break;
                    }//MiteHorns
                }
                //Stage 3 Apply all changes for PARTS
                for (int i = 0; i < genList.Count; i++)
                {
                    BodyPart bodyPart;
                    switch (genList[i].part)
                    {
                        case "Mouth":
                            bodyPart = new Mouth
                            {
                                localplace = genList[i].localplace,
                                globalplace = point,
                                color = genList[i].color
                            };
                            break;
                        case "Leg":
                            bodyPart = new Leg
                            {
                                localplace = genList[i].localplace,
                                globalplace = point,
                                color = genList[i].color
                            };
                            break;
                        case "Eye":
                            bodyPart = new Eye
                            {
                                localplace = genList[i].localplace,
                                globalplace = point,
                                color = genList[i].color
                            };
                            break;
                        case "Brain":
                            bodyPart = new Brain
                            {
                                localplace = genList[i].localplace,
                                globalplace = point,
                                color = genList[i].color
                            };
                            break;
                        case "Fats":
                            bodyPart = new Fats
                            {
                                localplace = genList[i].localplace,
                                globalplace = point,
                                color = genList[i].color
                            };
                            break;
                        case "Genitals":
                            bodyPart = new Genitals
                            {
                                localplace = genList[i].localplace,
                                globalplace = point,
                                color = genList[i].color
                            };
                            break;
                        case "Stomach":
                            bodyPart = new Stomach
                            {
                                localplace = genList[i].localplace,
                                globalplace = point,
                                color = genList[i].color
                            };
                            break;
                        case "MiteHorns":
                            bodyPart = new MiteHorns
                            {
                                localplace = genList[i].localplace,
                                globalplace = point,
                                color = genList[i].color
                            };
                            break;
                        default:
                            bodyPart = new Fats
                            {
                                localplace = genList[i].localplace,
                                globalplace = point,
                                color = genList[i].color
                            };
                            break;
                    }//MiteHorns

                    bodyTypes.Add(bodyPart);
                }
                //Stage 4 Mutate parts

                //------------------------------------------------------
                return genotype;
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
            
            public void DoworkPrepare(Bitmap bmp, Controller controller)//берет движение от части тела, берет все другие действия от других частей тела
            {
                dictionaryOfGrass = controller.grassDictionary;
                cellDictionary = controller.cellDictionary;
                radiation = controller.radiationLVL;
                lastPoint = point;
                foreach (var part in bodyTypes)
                {
                    part.lastGlobalplace = new Point(lastPoint.X + part.localplace.X, lastPoint.Y + part.localplace.Y);
                    food -= part.energyCost;
                    part.Dosomething(this, bmp);
                    part.globalplace = point;
                }

                canDuplicate = Duplicate();
                age++;//старение
            }

            public void EatTarget(Dictionary<Point, Grass> dictionaryOfGrass, int toeatStrength)
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
                dublicateDelay = dublicateDelay < dublicateDelayMax ? dublicateDelay + 1 : dublicateDelay;
                return dublicateDelay == dublicateDelayMax ? true : false;
            }
        }

    }
}
