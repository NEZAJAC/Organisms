using System.Drawing;
using System.Reflection;
using System.Xml.Linq;

namespace grass
{
    public partial class Form1
    {
        /// <summary>
        /// Родительский объект для всех частей тела
        /// </summary>
        class BodyPart
        {
            public Random rand = new Random();
            public int health = 200;
            public Color color = Color.White;
            public Point localplace = new Point();
            public Point globalplace = new Point();
            public Point lastGlobalplace = new Point();
            public Color lastGlobalplaceColor = Color.Empty;
            public int energyCost = 0;
            public string name = "";
            public List<string> partsData = new List<string>();//---------для вывода информации
            Organism? myBody;
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

                body.point = BorderChecker(body.point.X + this.moveResult.X, body.point.Y + this.moveResult.Y, bmp);//двигает тело
                body.eatTarget = this.eatTarget.X != -1 && this.eatTarget.Y != -1 ? this.eatTarget : new Point(-1, -1);
                body.EatTarget(body.dictionaryOfGrass, this.EatStrength);
            }
        }
        /// <summary>
        /// Рот организма
        /// </summary>
        class Mouth : BodyPart
        {
            public int EatStrength = 100;
            public int eatResult = 0;
            int transferStrength = 25;
            public Point eatTarget = new Point(0, 0);
            public List<Point> ignorePoints = new List<Point>();
            bool predator = false;

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
                                if (bmp.GetPixel(p.X, p.Y).G > 0 || bmp.GetPixel(p.X, p.Y).R > 0 || bmp.GetPixel(p.X, p.Y).B > 0)
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

            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (!Eat(out eatTarget, bmp)) { body.move = true; } else { body.move = false; }// рот решает что делать организму??? НЕТ!!!//только пока нет мозга
                //Eat(out eatTarget, bmp);
                body.eatTarget = eatTarget;
                body.EatTarget(body.dictionaryOfGrass, EatStrength);
            }
        }
        /// <summary>
        /// Мозг организма
        /// </summary>
        class Brain : BodyPart
        {
            int amountFoodSignal = 0;
            Dictionary<Point, Color> pointsEyeSignal = new Dictionary<Point, Color>();//Глаза дают мозгу сигнал в виде точек с цветами
            int pheromoniusStrengthSignal = 0;
            Point sidePointMoveSignal = new Point();
            int foodSignal = 0;
            //--------------------------
            Point brainWantMoveToSignalOut = new Point();//мозг решает куда он хочет идти сейчас, дает указ ногам, ноги проверяют доступные места куда могут пойти, если есть совпаения то идут туда
            //входным параметром может быть стадия голода организма
            //сигналы от "глаз"
            //сигналы от "ферамонов"
            //разработать систему корректировки движия
            List<BodyPart> myParts = new List<BodyPart>();
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
            void Thinking(List<BodyPart> myParts, out int FoodSignal, out List<Point> EyeSignal, out int pheromoniusSignal)
            {
                foreach (var item in myParts)
                {
                    if (item.name == "")
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
        /// <summary>
        /// Нога организма
        /// </summary>
        class Leg : BodyPart//ножка двигает всю клетку в одну сторону 
        {
            Random rand = new Random();
            public Point moveResult = new Point(0, 0);
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
            public Point MoveResult(Bitmap bmp) //-------------------------------------------Добавить точки игнорирования
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
                    

                    body.point = BorderChecker(body.point.X + moveResult.X, body.point.Y + moveResult.Y, bmp);  //------------------------------------ноги решают что пора идти? НЕТ!!
                }
            }
        }
        /// <summary>
        /// Желудок организма
        /// </summary>
        class Stomach : BodyPart//желудок сохраняет часть скушанного и переводит в еду с увечиченной скоростью
        {
            int capacity = 2000;
            int transferStrength = 100;
            int transferScale = 2;
            public Stomach()
            {
                name = "Stomach";
                color = Color.RosyBrown;
                energyCost = 1;
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
        /// <summary>
        /// Глаз организма
        /// </summary>
        class Eye : BodyPart//проверяет бОльшую область чем сенсоры других органов, отличает цвет, имеет выход нервного окончания для влияния на действия
        {
            List<Point> points = new List<Point>();
            List<Point> ignorePoints = new List<Point>();
            public Dictionary<Point, Color> findingTargets = new Dictionary<Point, Color>();
            //--------------------------
            public int eyeRange = 5;
            //--------------------------
            public int redColor = 0;
            public int greenColor = 130;
            public int blueColor = 0;
            public Eye()
            {
                name = "Eye";
                color = Color.Yellow;
                energyCost = 1;
            }
            
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("Range        \t" + eyeRange.ToString());
                partsData.Add("redColor     \t" + redColor.ToString());
                partsData.Add("greenColor   \t" + greenColor.ToString());
                partsData.Add("blueColor    \t" + blueColor.ToString());
                foreach (Point p in points)
                {
                partsData.Add("SeeingAt     \t" + "x" + p.X.ToString() + " \ty" + p.Y.ToString());
                }
                return "";
            }
            public void CheckMyBody(Organism organism)
            {
                ignorePoints.Clear();
                foreach (var item in organism.bodyTypes)
                {
                    ignorePoints.Add(new Point(item.globalplace.X + item.localplace.X,item.globalplace.Y + item.localplace.Y));
                }
            }
            List<Point> SenseEye(Organism body, Bitmap bmp)
            {
                points.Clear();
                CheckMyBody(body);
                for (int i = -eyeRange; i < eyeRange; i++)
                {
                    for (int j = -eyeRange; j < eyeRange; j++)
                    {
                        Point p = BorderChecker(new Point(body.point.X + localplace.X + i, body.point.Y + localplace.Y + j), bmp);
                        if (!ignorePoints.Contains(new Point(p.X, p.Y)))
                        {
                             Color color = bmp.GetPixel(p.X, p.Y);
                             if (color.G <= greenColor && color.G > 0 ) { points.Add(p); }
                             else
                             if (color.A <= redColor && color.A > 0 ) { points.Add(p); }
                             else
                             if (color.B <= blueColor && color.B > 0 ) { points.Add(p); }
                        }
                        
                    }
                }
                return points;
            }

            public override void Dosomething(Organism body, Bitmap bmp)
            {
                //body.findingPoints.Clear(); 
                //body.findingPoints = SenseEye(body,bmp);
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
                if (body.food >= body.maxfood / 2 && fats < maxFats - exchangeStrength && !body.canDuplicate)
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
            public Filter()
            {
                name = "Filter";
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

            }
        }
        /// <summary>
        /// клещевые рожки для ведения паразитической жизни
        /// </summary>
        class MiteHorns : BodyPart
        {
            int amountDrinking = 0;
            int bloodIntake = 10;
            int biteRange = 2;
            public Point trackingRange = new Point(6, 6);
            List<Point> ignorePoints = new List<Point>();
            Organism? target = null;
            public Dictionary<Point, Organism>? cellDictionary;
            public MiteHorns()
            {
                name = "MiteHorns";
                color = Color.DimGray;
                energyCost = 1;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                if(target != null )partsData.Add("target X\t" + target.point.X.ToString());
                if (target != null) partsData.Add("target Y\t" + target.point.Y.ToString());
                partsData.Add("amountDrinking\t" + amountDrinking.ToString());
                return "";
            }
            void FindTargets(Organism mybody, Bitmap bmp)
            {
                for (int i = -biteRange; i <= biteRange; i++)
                {
                    for (int j = -biteRange; j <= biteRange; j++)
                    {
                        Point p = BorderChecker(new Point(mybody.point.X + localplace.X + i, mybody.point.Y + localplace.Y + j), bmp);
                        Color color = bmp.GetPixel(p.X, p.Y);
                        if ((color.R > 0 || color.B > 0) && cellDictionary.ContainsKey(p) && cellDictionary[p] != mybody)
                        {
                            target = cellDictionary[p];
                        }
                    }
                }
            }
            void Tracking(Organism myOrganism)
            {
                if(target != null)
                {
                    if(Math.Abs(target.point.X - (globalplace.X + localplace.X)) <= biteRange && Math.Abs(target.point.Y - (globalplace.Y + localplace.Y)) <= biteRange)
                    {
                        BiteTarget(target, myOrganism);
                    }
                    else
                    {
                        target = null;

                    }
                }
            }

            void BiteTarget(Organism targetOrganism,Organism myOrganism)
            {
                if (targetOrganism != myOrganism)
                {
                    targetOrganism.food -= bloodIntake;
                    myOrganism.food += bloodIntake;
                    amountDrinking += bloodIntake;
                }
                else { target = null; }
            }
            public override void Dosomething(Organism myOrganism, Bitmap bmp)
            {
                if (target == null) { cellDictionary = myOrganism.cellDictionary; FindTargets(myOrganism, bmp); } else { Tracking(myOrganism); }
            }
        }
    }
}
