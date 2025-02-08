using System.Drawing;
using System.Reflection;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace MicroLife_Simulator
{
    public partial class Form1
    {
        /// <summary>
        /// Родительский объект для всех частей тела
        /// </summary>
        class BodyPart
        {
            public Random rand = new Random();
            public int health = 500;
            public Color color = Color.White;
            public Point localplace = new Point();
            public Point globalplace = new Point();
            public Point lastGlobalplace = new Point();
            public Color lastGlobalplaceColor = Color.Empty;
            public int energyCost = 0;
            public string name = "";
            public List<string> partsData = new List<string>();//---------для вывода информации
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
            
            public int EatStrength = 25;
            public int eatResult = 0;
            int transferStrength = 15;
            public Point moveResult = new Point(0, 0);
            public Point eatTarget = new Point(0, 0);

            public Universal()
            {
                name = "Universal";
                color = Color.DarkRed;
                energyCost = 1;
            }

            public bool Eat(out Point targetEat, Bitmap bmp)
            {
                List<Point> points = new List<Point>();
                bool haveToEat = false;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        Point p = BorderChecker(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j, bmp);
                        if (bmp.GetPixel(p.X, p.Y).G == Color.Green.G)
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
                for (int i = points.Count - 1; i > -1; i--)
                {
                    if (points[i].X == 0 && points[i].Y == 0) { points.Remove(points[i]); }
                }
                targetPoint = points.Count > 0 ? points[rand.Next(0, points.Count)] : new Point(0, 0);
                return targetPoint;
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (!Eat(out eatTarget, bmp)) { moveResult = SenseMove(bmp); } else { moveResult = new Point(0, 0); }

                //body.point = BorderChecker(body.point.X + this.moveResult.X, body.point.Y + this.moveResult.Y, bmp);//двигает тело
                //body.ToMouthSignal = this.eatTarget.X != -1 && this.eatTarget.Y != -1 ? this.eatTarget : new Point(-1, -1);
                //body.EatTarget(body.controller.grassDictionary, this.EatStrength);
            }
        }
        /// <summary>
        /// Рот организма
        /// </summary>
        class Mouth : BodyPart
        {
            public int EatStrength = 50;
            public bool predator = false;

            public int eatResult = 0;
            public Point eatTarget = new Point(0, 0);
            public List<Point> ignorePoints = new List<Point>();
            
            public Mouth()
            {
                name = "Mouth";
                color = Color.Red;
                energyCost = 1;
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

            public bool Eat(out Point targetEat, Bitmap bmp)
            {
                List<Point> points = new List<Point>();
                if (!predator)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
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
                else
                {
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (!ignorePoints.Contains(new Point(i, j)))
                            {
                                Point p = BorderChecker(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j, bmp);
                                var temp = bmp.GetPixel(p.X, p.Y);
                                if (bmp.GetPixel(p.X, p.Y).G > 0 || bmp.GetPixel(p.X, p.Y).R > 0 || bmp.GetPixel(p.X, p.Y).B > 0 || bmp.GetPixel(p.X, p.Y).G != Color.Green.G)
                                {
                                    points.Add(new Point(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j));
                                }
                            }
                        }
                    }
                }
                targetEat = points.Count > 0 ? points[rand.Next(0, points.Count)] : new Point(-1, -1);
                return targetEat.X != -1 && targetEat.Y != -1 ? true : false;
            }

            public void EatTarget(Organism body)
            {
                if (eatTarget.X != -1 && eatTarget.Y != -1 && body.controller.grassDictionary.ContainsKey(eatTarget) && body.food < body.maxfood)
                {
                    //------------------------------------------------------Доработать математику
                    int difference = body.controller.grassDictionary[eatTarget].food - EatStrength;
                    int amount = difference <= 0 ? body.controller.grassDictionary[eatTarget].food : EatStrength;
                    body.controller.grassDictionary[eatTarget].food -= amount;
                    body.food += amount;
                }
            }
            public void ToStomach(Organism body)
            {
                int overEating = 0;
                if (eatTarget.X != -1 && eatTarget.Y != -1 && body.controller.grassDictionary.ContainsKey(eatTarget) && body.food < body.maxfood)
                {
                    int stomaches = 0;
                    foreach (var item in body.bodyTypes)
                    {
                        if (item.name == "Stomach")
                        { 
                            stomaches++;
                        }
                    }
                    foreach (var item in body.bodyTypes)
                    {
                        if (item.name == "Stomach")
                        {
                            Stomach stomach = (Stomach)item;
                            int difference = body.controller.grassDictionary[eatTarget].food - EatStrength;
                            int amount = difference <= 0 ? body.controller.grassDictionary[eatTarget].food : EatStrength;//----сколько может откусить
                            body.controller.grassDictionary[eatTarget].food -= amount/stomaches;
                            var overFood = stomach.currentCapacity + amount / stomaches > stomach.capacity ? stomach.currentCapacity + amount / stomaches - stomach.capacity : 0;   
                            stomach.currentCapacity = overFood > 0 ? stomach.capacity : stomach.currentCapacity + amount / stomaches;
                            overEating += overFood;
                        }
                    }
                    
                }
                if(overEating >= 20)
                {
                    if (!body.controller.infectionLVL.ContainsKey(eatTarget)) { body.controller.infectionLVL.Add(eatTarget, 0); } else
                    { 
                        body.controller.infectionLVL[eatTarget] += overEating; 
                    }
                    
                }
                //------------------------------------------------------------сделать выбрасывание излишков и создание мусора(отравление почвы)
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (!Eat(out eatTarget, bmp)) { body.move = true; } else { body.move = false; }// рот решает что делать организму??? НЕТ!!!//только пока нет мозга
                if(!body.hasStomach){ EatTarget(body); }{ ToStomach(body); }
                //body.ToMouthSignal = eatTarget;
                //body.EatTarget(body.controller.grassDictionary, EatStrength);
            }
        }
        /// <summary>
        /// Мозг организма
        /// </summary>
        class Brain : BodyPart
        {
            enum Tasks {Move,Eat,Duplicate }
            //мозг решает куда он хочет идти сейчас, дает указ ногам, ноги проверяют доступные места куда могут пойти, если есть совпаения то идут туда
            //входным параметром может быть стадия голода организма
            //сигналы от "глаз"
            //сигналы от "ферамонов"
            //разработать систему корректировки движия
            //{Skip}{DO}{Take}{Give}
            //{ничего не делать, пропуск частью тела своей задачи}
            //{выполнять свою задачу, если задача требует данных то применяется следущее}
            //{мозг передает необходимую информацию части тела для выполнения задачи}
            //{мазг получает информацию от части тела для обработки}
            //   от органов чувств(глаза, сенсоры) получает словарь точек с цветами(для глаз) или уровень заражения почвы(для сенсоров),
            //   через Analize разделяется набор точек на группы с явными типами {Еда}{Свой}{Чужой}{Свободный путь}{Заражение}
            //   части тела (рты, ноги, жабры, гениталии, фильтры, рожки, зацепки) возвращают результат выполнения операции {success}{fault}{half}
            //   берет свою информацию о потребностях (голод, размножение, температура тела)
            //   разность действия(результата) и ожидания(результата) дает обратную ошибку,
            //   которая меняет множитель в нужную сторону (если совпала то в +, если нет то в -) после N числа несовпадений или совпадений,
            //   где N это жесткость правил. удачная N будет тренеровать сеть лучше
            //   point.X
            //Сначала мы выбираем ближайшие объекты. Для глаз ближайшую пищу, для ног ближайший шаг до этой пиши. В целом определяем цель и как до нее дойти
            //Это не требует работы нейросети?
            List<Point> points = new List<Point>();
            public Brain()
            {
                name = "Brain";
                color = Color.Pink;
                energyCost = 1;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                return "";
            }
            void NeuroWork()
            {
                //от 0 до 8(0-24)(0-48) входных точек - координаты куда можно пойти
                // точка состоит в диапазоне -1, 1 можно представить как х = -1 , у = 1
                
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
               points.Clear();
                points = body.closestPoiints;

                //нихуя не понятно как делать
                //понятно как думать но не понятно как делать
            }
        }
        /// <summary>
        /// Нога организма
        /// </summary>
        class Leg : BodyPart//ножка двигает всю клетку в одну сторону 
        {
            Random rand = new Random();
            public Point moveResult = new Point(0, 0);
            public int speed = 1;
            public Leg()
            {
                name = "Leg";
                color = Color.DarkOrange;
                energyCost = 1;
            }

            public override string UpdateMyData()
            {
                base.UpdateMyData(); ;
                partsData.Add("Speed X\t" + moveResult.X.ToString());
                partsData.Add("Speed Y\t" + moveResult.Y.ToString());
                return "";
            }
            public Point MoveResult(Bitmap bmp) //-------------------------------------------Добавить точки игнорирования//Оставить эту фукнцию для движения без органов осязания
            {
                List<Point> points = new List<Point>();
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        Point p = BorderChecker(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j, bmp);
                        points.Add(bmp.GetPixel(p.X, p.Y).A == 0 ? new Point(localplace.X + i, localplace.Y + j) : new Point(0, 0));
                    }
                }
                for (int i = points.Count - 1; i > -1; i--)
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
                    

                    //body.newPoint = BorderChecker(body.point.X + moveResult.X, body.point.Y + moveResult.Y, bmp);  //------------------------------------ноги решают что пора идти? НЕТ!!
                    body.newPoint = BorderChecker(body.point.X + moveResult.X, body.point.Y + moveResult.Y, bmp);
                }
            }
        }
        /// <summary>
        /// Желудок организма
        /// </summary>
        class Stomach : BodyPart//желудок сохраняет часть скушанного и переводит в еду с увечиченной скоростью
        {
            public int capacity = 3000;
            public int currentCapacity = 0;
            int transferStrength = 25;
            int transferPlus = 2;
            int transferDelay { get; }
            int transferDelayNow = 0;
            public Stomach()
            {
                name = "Stomach";
                color = Color.RosyBrown;
                energyCost = 1;

                transferDelay = 7;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("Capacity\t" + currentCapacity.ToString() + "/" +capacity.ToString());
                partsData.Add("Transfer\t" + transferStrength.ToString());
                partsData.Add("transferPlus\t" + transferPlus.ToString());
                partsData.Add("transferDelayNow\t" + transferDelayNow.ToString());
                return "";
            }
            void DigestFood(Organism body)
            {
                int amount = currentCapacity - transferStrength >= 0 ? transferStrength : currentCapacity;
                if (body.food + amount <= body.maxfood)
                {
                    currentCapacity -= amount;
                    body.food += amount + transferPlus;
                }
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (transferDelayNow < transferDelay){ transferDelayNow++; }
                if (currentCapacity > 0 && transferDelayNow == transferDelay)
                {
                    transferDelayNow = 0;
                    DigestFood(body);
                }
            }
        }
        /// <summary>
        /// Глаз организма
        /// </summary>
        class Eye : BodyPart//проверяет бОльшую область чем сенсоры других органов, отличает цвет, имеет выход нервного окончания для влияния на действия
        {
            
            //--------------------------
            public int eyeRange = 3;
            //--------------------------
            public int redColor = 0;
            public int greenColor = 130;
            public int blueColor = 0;


            public Eye()
            {
                name = "Eye";
                color = Color.White;
                energyCost = 1;
            }
            
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("Range        \t" + eyeRange.ToString());
                partsData.Add("redColor     \t" + redColor.ToString());
                partsData.Add("greenColor   \t" + greenColor.ToString());
                partsData.Add("blueColor    \t" + blueColor.ToString());
                //partsData.Add("blueColor    \t" + blueColor.ToString());
                return "";
            }
            void SenseEye(Organism body, Bitmap bmp)
            {
                for (int i = -eyeRange; i < eyeRange; i++)
                {
                    for (int j = -eyeRange; j < eyeRange; j++)
                    {
                        Point p = BorderChecker(new Point(body.point.X + localplace.X + i, body.point.Y + localplace.Y + j), bmp);
                        if (!body.ignorePoints.Contains(new Point(p.X, p.Y)) && !body.EyeSignal.ContainsKey(p))
                        {
                             Color color = bmp.GetPixel(p.X, p.Y);
                             if (color.G <= greenColor && color.G > 0 || color.A <= redColor && color.A > 0 || color.B <= blueColor && color.B > 0) 
                            { 
                                body.EyeSignal.Add(p, color);
                            }
                        }
                        
                    }
                }
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        Point p = BorderChecker(new Point(body.point.X + localplace.X + i, body.point.Y + localplace.Y + j), bmp);
                        Color color = bmp.GetPixel(p.X, p.Y);
                        if (body.ignorePoints.Contains(new Point(p.X, p.Y)) || color.A == 255)
                        {
                            body.closestPoiints.Add(p);
                        }
                    }
                }
            }

            public override void Dosomething(Organism body, Bitmap bmp)
            {
                SenseEye(body, bmp);
            }
        }
        /// <summary>
        /// Сенсоры организма//видит зараженные органикой территории
        /// </summary>
        class Sensors : BodyPart 
        {
            public Sensors()
            {
                name = "Sensors";
                color = Color.DarkOliveGreen;
                energyCost = 1;
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
        /// <summary>
        /// Жировые запасы организма
        /// </summary>
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
                if (body.food >= body.maxfood / 2 && fats < maxFats - exchangeStrength)
                {
                    fats += exchangeStrength;
                    body.food -= exchangeStrength;
                }
                else if (fats - exchangeStrength >= 0)
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
        /// <summary>
        /// Жабры организма, позволяют жить в водной среде
        /// </summary>
        class Gills : BodyPart//жарбы для жизни под водой, как только зоны будут добавлены//так же будут и легкие
        {
            public Gills()
            {
                name = "Jabres";
                color = Color.DeepPink;
                energyCost = 1;
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
        /// <summary>
        /// Гениталии позволяют "приманивать" родичей и давать потомство при половом размножении
        /// </summary>
        class Genitals : BodyPart //определяют схему размножения
        {
            public Genitals()
            {
                name = "Genitals";
                color = Color.LavenderBlush;
                energyCost = 1;
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
        /// <summary>
        /// Фильтры организма позволяют очищать зараженные органикой местности, получая еду
        /// </summary>
        class Filter : BodyPart //позволяет поедать(фильтровать) зараженные органикой территории
        {
            public int cleanStrength = 100;
            public int foodConvert = 500;
            public int amountClean = 0;
            List<Point> points = new List<Point>();
            Point target = new Point();
            public Filter()
            {
                name = "Filter";
                color = Color.DimGray;
                energyCost = 1;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("cleanStrength\t" + cleanStrength.ToString());
                partsData.Add("foodConvert\t" + foodConvert.ToString());
                partsData.Add("target\t" + target.ToString());
                partsData.Add("amountClean\t" + amountClean.ToString());
                return "";
            }
            void FindInfection(Bitmap bmp)
            {
                points.Clear();
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        Point p = BorderChecker(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j, bmp);
                        Color color = bmp.GetPixel(p.X, p.Y);
                        Point point = color.A != 0 && color.R > 0 && color.G > 0 && color.B == 0 ? new Point(localplace.X + i, localplace.Y + j) : new Point(0, 0);
                        points.Add(point);
                    }
                }
                for (int i = points.Count - 1; i > -1; i--)
                {
                    if (points[i].X == 0 && points[i].Y == 0) { points.Remove(points[i]); }
                }
                if(points.Count > 0) { target = points[rand.Next(0, points.Count)]; }
            }
            void CleanInfection(Organism body)
            {
                Point point = new Point(body.point.X + target.X, body.point.Y + target.Y);
                if (body.controller.infectionLVL.ContainsKey(point))
                {
                    body.controller.infectionLVL[point] = body.controller.infectionLVL[point] - cleanStrength >= 0 ? body.controller.infectionLVL[point] - cleanStrength : 0;
                    body.food += foodConvert;
                    amountClean += foodConvert;
                }
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                FindInfection(bmp);
                CleanInfection(body);
            }
        }
        /// <summary>
        /// Аппарат для общения с другими организмами
        /// </summary>
        class SpeechApparatus : BodyPart
        {
            public SpeechApparatus()
            {
                name = "SpeechApparatus";
                color = Color.DimGray;
                energyCost = 1;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                return "";
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                //брать словарь организмов, искать ближайших и передавать им какие то данные
            }
        }
        /// <summary>
        /// клещевые рожки для ведения паразитической жизни
        /// </summary>
        class MiteHorns : BodyPart
        {
            int amountDrinking = 0;
            int bloodIntake = 15;
            int biteRange = 2;
            public Organism? target = null;
            public MiteHorns()
            {
                name = "MiteHorns";
                color = Color.DimGray;
                energyCost = 2;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                if (target != null)
                {
                    partsData.Add("target X\t" + target.point.X.ToString());
                    partsData.Add("target Y\t" + target.point.Y.ToString());
                }
                partsData.Add("amountDrinking\t" + amountDrinking.ToString());
                return "";
            }
            void FindTargets(Organism myOrganism, Bitmap bmp)
            {
                for (int i = -biteRange; i <= biteRange; i++)
                {
                    for (int j = -biteRange; j <= biteRange; j++)
                    {
                        Point p = BorderChecker(new Point(myOrganism.point.X + localplace.X + i, myOrganism.point.Y + localplace.Y + j), bmp);
                        if (myOrganism.controller.cellDictionary.ContainsKey(p) &&
                            myOrganism.controller.cellDictionary[p].point != myOrganism.point &&
                            myOrganism.controller.cellDictionary[p].genList[0].localplace != myOrganism.genList[0].localplace &&
                            myOrganism.controller.cellDictionary[p].genList[0].color.ToArgb != myOrganism.genList[0].color.ToArgb)
                        {
                            target = myOrganism.controller.cellDictionary[p];
                            break;
                        }
                    }
                    if (target != null) break; 
                }
            }
            void BiteTarget(Organism myOrganism)
            {
                if (Math.Abs(target.point.X - (globalplace.X + localplace.X)) > biteRange && Math.Abs(target.point.Y - (globalplace.Y + localplace.Y)) > biteRange)
                {
                    target = null;
                }
                else
                {
                    target.food -= bloodIntake;
                    myOrganism.food += bloodIntake;
                    amountDrinking += bloodIntake;
                }
            }
            public override void Dosomething(Organism myOrganism, Bitmap bmp)
            {
                if (target == null) {FindTargets(myOrganism, bmp); } else { BiteTarget(myOrganism); }
            }
        }
        /// <summary>
        /// зацепки для прикрепления к другим организмам
        /// </summary>
        class Clues : BodyPart
        {
            public Clues()
            {
                name = "Clues";
                color = Color.LavenderBlush;
                energyCost = 1;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                return "";
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                //брать словарь организмов, искать ближайших и передавать им какие то данные
            }
        }
        /// <summary>
        /// позволяют получать питание от солнца
        /// </summary>
        class Chlorophylls : BodyPart
        {
            public Chlorophylls()
            {
                name = "Chlorophylls";
                color = Color.LightGreen;
                energyCost = 1;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                return "";
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                //брать словарь организмов, искать ближайших и передавать им какие то данные
            }
        }
    }
}
