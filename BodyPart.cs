using System;
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
            public string type = "";
            public string name = "";
            public List<string> partsData = new List<string>();//---------для вывода информации
            public BodyPart()
            {


            }
            public virtual string UpdateMyData()
            {
                partsData.Clear();
                partsData.Add(color.ToString());
                partsData.Add("localplace       \t" + localplace.ToString());
                partsData.Add("energyCost\t" + energyCost.ToString());
                partsData.Add("poopasGlobal\t" );
                return name;
            }
            void FoodConsume(Organism body)
            {
                body.food -= energyCost;//--------------------------------------------------------------------------------------сделать отсутствие потребления если часть тела не работала}
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
            public int EatStrength = 25;
            public int EatRange = 2;
            List<BodyPart> stomaches = new List<BodyPart>();

            int alreadyEaten = 0;
            public Point eatTarget = new Point(0, 0);
            //public List<Point> ignorePoints = new List<Point>();
            List<Point> points = new List<Point>();
            public Mouth()
            {
                name = "Mouth";
                type = "EatOrgan";
                color = Color.Red;
                energyCost = 3;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("EatStrength\t" + EatStrength.ToString());
                partsData.Add("AlreadyEaten    \t" + alreadyEaten.ToString());
                partsData.Add("EatTarget    \t" + (eatTarget != new Point(-1,-1) ? eatTarget.ToString() : "NoNe"));
                partsData.Add("Stomaches\t" + stomaches.Count.ToString());
                partsData.Add("---------------------------------");
                //foreach (var item in ignorePoints)
                //{
                //    partsData.Add("IgnorePoints\t" + item.ToString());
                //}
                //partsData.Add("---------------------------------");
                foreach (var item in points)
                {
                    partsData.Add("AvaliblePoints  \t" + item.ToString());
                }

                return "";
            }

            public bool Eat(out Point targetEat, Bitmap bmp)
            {
                points.Clear();
                for (int i = -EatRange; i <= EatRange; i++)
                {
                    for (int j = -EatRange; j <= EatRange; j++)
                    {
                        Point p = BorderChecker(globalplace.X + localplace.X + i, globalplace.Y + localplace.Y + j, bmp);
                        var temp = bmp.GetPixel(p.X, p.Y);
                        if (temp.G == Color.Green.G)
                        {
                            points.Add(new Point(p.X, p.Y));
                        }
                    }
                }
                targetEat = points.Count > 0 ? points[rand.Next(0, points.Count)] : new Point(-1, -1);
                return targetEat != new Point(-1,-1);
            }

            public void ToBody(Organism body)
            {

                if (body.controller.grassDictionary.ContainsKey(eatTarget) && body.food < body.maxfood)
                {
                    //------------------------------------------------------Доработать математику
                    int difference = body.controller.grassDictionary[eatTarget].food - EatStrength;
                    int amount = difference <= 0 ? body.controller.grassDictionary[eatTarget].food : EatStrength;
                    body.controller.grassDictionary[eatTarget].food -= amount;
                    body.food += amount;
                    alreadyEaten += amount;
                }
            }
            public void ToStomach(Organism body)
            {
                if (body.controller.grassDictionary.ContainsKey(eatTarget) && body.food < body.maxfood)
                {
                    foreach (var item in body.bodyTypes)
                    {
                        if (item.name == "Stomach")
                        {
                            if (!stomaches.Contains(item)) stomaches.Add(item);
                        }
                    }
                    if (stomaches.Count > 0)
                    {
                        int amount = body.controller.grassDictionary[eatTarget].food - EatStrength < 0 ? body.controller.grassDictionary[eatTarget].food : EatStrength;
                        body.controller.grassDictionary[eatTarget].food -= amount;
                        alreadyEaten += amount;
                        int pieceOfFood = amount / stomaches.Count;
                        foreach (Stomach stomach in stomaches)
                        {
                            if (stomach.currentCapacity + pieceOfFood <= stomach.capacity)
                            {
                                stomach.currentCapacity += pieceOfFood;
                            }
                            else { stomach.currentCapacity = stomach.capacity; }

                        }
                    }
                }
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                //ignorePoints = body.ignorePoints;
                if (body.food >= body.maxfood) { body.hungry = false; }
                if (body.food <= body.parameters.hungryFoodLVL) { body.hungry = true; }//-----------------------------------------------починить голод, желудок не учитывается наполненным
                if (body.hungry)
                {
                    if (!Eat(out eatTarget, bmp)) { body.move = true; }
                    else
                    {
                        body.move = false;
                        if (body.myGenWords.Contains("Stomach")) { ToStomach(body); } else { ToBody(body); }
                    }// рот решает что делать организму??? НЕТ!!!//только пока нет мозга
                }
                else { body.move = true; }
            }
        }
        /// <summary>
        /// Мозг организма
        /// </summary>
        class Brain : BodyPart
        {
            public int brainLessDelay = 0;
            public int brainLessDelayMax = 100;
            int randoms;
            string? side;
            
            
            Point? moveResult;
            public Brain()
            {
                name = "Brain";
                color = Color.Pink;
                energyCost = 1;

            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("EyeCount     \t" + eyeCount.ToString());
                partsData.Add("EyeSignals     \t" + eyeSig.ToString());
                partsData.Add("legCount     \t" + legCount.ToString());
                partsData.Add("LegSignals     \t" + legSig.ToString());

                partsData.Add("MoveResult     \t" + legSig.ToString());
                partsData.Add("lessLength     \t" + lessLength.ToString());
                return "";
            }

            public void TurnRL(string axis, Organism organism)
            {
                if (axis == "Y")
                {
                    foreach (var part in organism.bodyTypes) { part.localplace.Y *= -1; }
                }
                if (axis == "X")
                {
                    foreach (var part in organism.bodyTypes) { part.localplace.X *= -1; }
                }
            }
            public void TurnRL2(Organism organism)
            {
                if (brainLessDelay >= brainLessDelayMax)
                {
                    randoms = rand.Next(0, 3);
                    side = randoms == 0 ? "" : randoms == 1 ? "X" : "Y";
                    TurnRL(side,organism);
                    brainLessDelay = 0;
                }
                brainLessDelay++;
            }
            int lessLength = 99;
            int legCount = 0;
            int eyeCount = 0;
            Point legSig;
            Point eyeSig;
            public void Calculate(Organism body)
            {
                lessLength = 99;
                for (int i = 0; i < legCount; i++)
                {
                    for (int j = 0; j < eyeCount; j++)
                    {
                        if(Math.Abs(body.LegSignal[i].X - body.EyeSignal[j].X) + Math.Abs(body.LegSignal[i].Y - body.EyeSignal[j].Y) < lessLength)
                        {
                            lessLength = Math.Abs(body.LegSignal[i].X - body.EyeSignal[j].X) + Math.Abs(body.LegSignal[i].Y - body.EyeSignal[j].Y);
                            body.ToLegSignal = body.LegSignal[i];


                            legSig = body.LegSignal[i];
                            eyeSig = body.EyeSignal[j];
                            moveResult = body.LegSignal[i];
                        }

                    }
                }

            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                
                legCount = body.LegSignal.Count;
                eyeCount = body.EyeSignal.Count;
                Calculate(body);
                TurnRL2(body);
            }
        }
        /// <summary>
        /// Нога организма
        /// </summary>
        class Leg : BodyPart//ножка двигает всю клетку в одну сторону 
        {
            public Point moveResult = new Point(0, 0);
            public int speedX = 1;
            public int speedY = 1;
            List<Point> points = new List<Point>();
            public int restCost = 2;
            public Leg()
            {
                name = "Leg";
                type = "MoveOrgan";
                color = Color.DarkOrange;
                energyCost = 1;
            }

            public override string UpdateMyData()
            {
                base.UpdateMyData(); ;
                partsData.Add("RestCost     \t" + restCost.ToString());
                partsData.Add("---------------------------------");
                partsData.Add("Speed X      \t" + moveResult.X.ToString());
                partsData.Add("Speed Y      \t" + moveResult.Y.ToString());

                foreach (var item in points)
                {
                    partsData.Add("Avalible point\t" + item.ToString());
                }
                return "";
            }
            public Point MoveResult(Bitmap bmp, Organism body) //-------------------------------------------Добавить точки игнорирования//Оставить эту фукнцию для движения без органов осязания
            {
                points.Clear();
                body.LegSignal.Clear();
                for (int i = -speedX; i <= speedX; i++)
                {
                    for (int j = -speedY; j <= speedY; j++)
                    {
                        Point p = BorderChecker(body.point.X + localplace.X + i + rand.Next(-1,1), body.point.Y + localplace.Y + j + rand.Next(-1, 1), bmp);
                        Color color = bmp.GetPixel(p.X, p.Y);
                        if (!body.ignorePoints.Contains(new Point(p.X, p.Y)))
                        {
                            if (color.B == 0 && color.A == 0 && color.G == 0)
                            {
                                points.Add(new Point(localplace.X + i, localplace.Y + j));
                                body.LegSignal.Add(new Point(localplace.X + i, localplace.Y + j));
                            }
                        }
                    }
                }
                
                return points.Count > 0 ? points[rand.Next(0, points.Count)] : new Point(0, 0);//-----------------------------------------сюда можно вмешаться органами чувств или мозгом и решить куда идти
            }


            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (body.fatigue == 0) { body.exhaustion = true; }
                if (body.fatigue > body.parameters.maxFatigue/body.parameters.exhaustionLvl) { body.exhaustion = false; }
                if (body.move && !body.exhaustion)//---------------------------------------------------------------------------------------------------------------------------вот здесь мы это фиксим, ноги теперь случаются
                {
                    if (body.ToLegSignal == null) 
                    {
                        moveResult = MoveResult(bmp, body);
                        body.newPoint = BorderChecker(body.point.X + moveResult.X, body.point.Y + moveResult.Y, bmp);
                        body.fatigue += body.fatigue > 0 ? -1 : 0;
                    }
                    else
                    if (body.ToLegSignal != null)
                    {
                        moveResult = (Point)body.ToLegSignal;
                        body.newPoint = BorderChecker(body.point.X + moveResult.X, body.point.Y + moveResult.Y, bmp);
                        body.ToLegSignal = null;
                        body.fatigue += body.fatigue > 0 ? -1 : 0;
                    }
                }
                else
                {
                    if (body.fatigue != body.parameters.maxFatigue) { body.fatigue++; body.food -= restCost; }
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
            int transferPlus = 3;
            int amountTransfered = 0;
            int transferDelay { get; }
            int transferDelayNow = 0;
            public Stomach()//---------------------------------------------------------------------------баг с желудком, переработать логику обмена и разделения между несколькими
            {
                name = "Stomach";
                color = Color.RosyBrown;
                energyCost = 1;

                transferDelay = 3;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("Capacity\t" + currentCapacity.ToString() + "/" + capacity.ToString());
                partsData.Add("Transfer\t" + transferStrength.ToString());
                partsData.Add("TransferPlus\t" + transferPlus.ToString());
                partsData.Add("TransferDelayNow\t" + transferDelayNow.ToString());
                partsData.Add("AmountTransfered\t" + amountTransfered.ToString());
                return "";
            }
            void DigestFood(Organism body)
            {
                int amount = currentCapacity - transferStrength >= 0 ? transferStrength : currentCapacity;
                if (body.food + amount <= body.maxfood)
                {
                    currentCapacity -= amount;
                    body.food += amount + transferPlus <= body.maxfood ? amount + transferPlus : body.maxfood;
                    amountTransfered += amount + transferPlus;
                }
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (transferDelayNow < transferDelay) { transferDelayNow++; }
                if (currentCapacity > 0 && transferDelayNow == transferDelay)
                {
                    transferDelayNow = 0;
                    DigestFood(body);
                }
                if (currentCapacity == capacity) { body.hungry = false; } else if(currentCapacity == 0 && body.food <= body.parameters.hungryFoodLVL) { body.hungry = true; }
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
                type = "SenseOrgan";
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
                for (int i = -eyeRange; i <= eyeRange; i++)
                {
                    for (int j = -eyeRange; j <= eyeRange; j++)
                    {
                        Point p = BorderChecker(new Point(body.point.X + localplace.X + i, body.point.Y + localplace.Y + j), bmp);
                        if (!body.ignorePoints.Contains(new Point(p.X, p.Y)) && !body.EyeSignal.Contains(p))
                        {
                            Color color = bmp.GetPixel(p.X, p.Y);
                            if ((color.G != 0) && color.R < 100)//if ((color.G <= greenColor && color.G > 0) || (color.A <= redColor && color.A > 0) || (color.B <= blueColor && color.B > 0))
                            {
                                body.EyeSignal.Add(new Point(localplace.X + i, localplace.Y + j));
                            }
                        }
                    }
                }
            }

            public override void Dosomething(Organism body, Bitmap bmp)
            {

                body.EyeSignal.Clear();
                SenseEye(body, bmp);
            }
        }
        /// <summary>
        /// Сенсоры организма//видит зараженные органикой территории
        /// </summary>
        class Sensors : BodyPart
        {
            //--------------------------
            public int sensorRange = 2;
            int targets = 0;
            public Sensors()
            {
                name = "Sensors";
                type = "SenseOrgan";
                color = Color.DarkOliveGreen;
                energyCost = 1;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("Targets    \t" + targets.ToString());
                return "";
            }

            void Sense(Organism body, Bitmap bmp)
            {
                for (int i = -sensorRange; i <= sensorRange; i++)
                {
                    for (int j = -sensorRange; j <= sensorRange; j++)
                    {
                        Point p = BorderChecker(new Point(body.point.X + localplace.X + i, body.point.Y + localplace.Y + j), bmp);
                        if (!body.ignorePoints.Contains(new Point(p.X, p.Y)) && !body.SenseSignal.ContainsKey(p))
                        {
                            Color color = bmp.GetPixel(p.X, p.Y);
                            if (body.controller.infectionLVL.ContainsKey(p))
                            {
                                body.SenseSignal.Add(p, color);
                                targets = body.SenseSignal.Count;
                            }
                        }
                    }
                }
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                Sense(body, bmp);
            }
        }
        /// <summary>
        /// Жировые запасы организма
        /// </summary>
        class Fats : BodyPart//сохраняет часть съеденного, но не переваренного в свой запас и при дифиците еды высвобождает запасы не давая организму умереть
        {//жиры не используются для определения возможности размножиться, что дает организму дольше жить без еды после деления
            int fats = 0;
            int maxFats = 5000;
            int exchangeStrength = 10;

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
                name = "Gills";
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
            Organism? friend;
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

            Organism? FindFriend(Organism body)
            {
                
                return null;
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                friend = FindFriend(body); 
            }
        }
        /// <summary>
        /// Клока позволяет откладывать яички и оплодотворять их
        /// </summary>
        class Cloaca : BodyPart
        {
            public int amountEggsAtTime = 1;
            int delay = 3;
            string? myID = null;
            public int fertilizeCount = 0;
            public int LayeredEggsCount = 0;
            Controller? controller;
            public Cloaca()
            {
                name = "Cloaca";
                color = Color.LavenderBlush;
                energyCost = 1;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("amountEggsAtTime\t" + amountEggsAtTime.ToString());
                partsData.Add("myID\t" + myID.ToString());
                partsData.Add("LayeredEggsCount\t" + LayeredEggsCount.ToString());
                partsData.Add("fertilizeCount\t" + fertilizeCount.ToString());
                return "";
            }
            void LayEgg(Organism body)
            {
                body.controller.eggListTEMP.Add(new Egg(body));
            }
            void FindEgg(Organism body,Bitmap bmp)
            {
                Point point;

                for (int i = -2; i <= 3; i++)
                {
                    for (int j = -2; j <= 3; j++)
                    {
                        point = new Point(body.point.X + localplace.X + i, body.point.Y + localplace.Y + j);
                        if (controller.eggDictionary.ContainsKey(point) )
                        {
                            if (controller.eggDictionary[point].ID2 == "" && controller.eggDictionary[point].myGuid != body.myGuid)
                            {
                                if (controller.eggDictionary[point].ID1 == myID)//не стал делать сразу, вместо ID вставить метод сравнения генотипных слов, при этом в яйцо передавать не ID  а слова напрямую
                                {
                                    controller.eggDictionary[point].parametersParent2 = body.parameters;
                                    body.GenCopyes(controller.eggDictionary[point].genListParent2,body.genList);
                                    controller.eggDictionary[point].ID2 = myID;
                                    controller.eggDictionary[point].age = 0;
                                    fertilizeCount++;
                                }
                            }
                        }
                    }
                }
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {

                myID ??= body.GetID(body.myGenWords);
                controller ??= body.controller;

                body.canDuplicate = false;
                if ( body.food >=  body.parameters.dublicateFood )
                {
                    for (int i = 0; i < amountEggsAtTime; i++)
                    {
                        body.food -= body.parameters.dublicateFoodPrice / amountEggsAtTime;
                        LayEgg(body);
                        LayeredEggsCount++;
                    }
                }
                if ((delay = delay == -1 ? 3 : delay - 1) == 0) { FindEgg(body, bmp); }
            }
        }
        /// <summary>
        /// Фильтры организма позволяют очищать зараженные органикой местности, получая еду
        /// </summary>
        class Filter : BodyPart //позволяет поедать(фильтровать) зараженные органикой территории
        {
            public int cleanStrength = 50;
            public int cleanRange = 3;
            public int foodConvert = 40;
            public int amountClean;
            List<Point> points = new List<Point>();
            Point target;
            public Filter()
            {
                name = "Filter";
                type = "EatOrgan";
                color = Color.DimGray;
                energyCost = 3;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                partsData.Add("CleanStrength\t" + cleanStrength.ToString());
                partsData.Add("FoodConvert  \t" + foodConvert.ToString());
                partsData.Add("AmountClean  \t" + amountClean.ToString());
                partsData.Add("CleanTarget       \t" + (target != new Point(-1, -1) ? target.ToString() : "NoNe"));
                foreach (Point p in points)
                {
                    partsData.Add("AvalibleTargets\t" + target.ToString());
                }
                return "";
            }
            void FindInfection(Bitmap bmp, Organism body)
            {
                target = new Point(-1, -1);
                points.Clear();
                for (int i = -cleanRange; i <= cleanRange; i++)
                {
                    for (int j = -cleanRange; j <= cleanRange; j++)
                    {

                        Point pG = BorderChecker(body.point.X + localplace.X + i, body.point.Y + localplace.Y + j, bmp);
                        //Color color = bmp.GetPixel(p.X, p.Y);//ЗАМЕНИТЬ поиск по цвету на поиск в словаре
                        //Point point = color.A != 0 && color.R > 0 && color.G > 0 && color.B == 0 ? new Point(localplace.X + i, localplace.Y + j) : new Point(0, 0);
                        //points.Add(point);
                        if (body.controller.infectionLVL.ContainsKey(pG)) { points.Add(pG); }
                    }
                }
                //for (int i = points.Count - 1; i > -1; i--)
                //{
                //    if (points[i].X == 0 && points[i].Y == 0) { points.Remove(points[i]); }
                //}
                if (points.Count > 0) { target = points[rand.Next(0, points.Count)]; }
            }
            void CleanInfection(Organism body)
            {
                //Point point = new Point(body.point.X + target.X, body.point.Y + target.Y);
                if (body.controller.infectionLVL.ContainsKey(target))
                {
                    body.controller.infectionLVL[target] = body.controller.infectionLVL[target] - cleanStrength >= 0 ? body.controller.infectionLVL[target] - cleanStrength : 0;
                    body.food += foodConvert;
                    amountClean += foodConvert;
                }
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (body.hungry)
                {
                    FindInfection(bmp,body);
                    CleanInfection(body);
                }
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
            int bloodIntake = 25;
            int bloodDrink = 35;
            int biteRange = 3;
            public Organism? target = null;
            public MiteHorns()
            {
                name = "MiteHorns";
                type = "EatOrgan";
                color = Color.DimGray;
                energyCost = 3;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                if (target != null)
                {
                    partsData.Add("Target         \t" + target.point.ToString());
                    partsData.Add("TargetEnergy   \t" + target.food.ToString());
                    partsData.Add("TargetID       \t" + target.GetID(target.myGenWords).ToString());
                }
                else
                {
                    partsData.Add("Target         \t" + "NoNe");
                    partsData.Add("TargetEnergy   \t" + "NoNe");
                    partsData.Add("TargetID       \t" + "NoNe");
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
                        Organism? findTarget = myOrganism.controller.cellDictionary.ContainsKey(p) ? myOrganism.controller.cellDictionary[p] : null;
                        if (findTarget != null && !myOrganism.GetIDdiff(myOrganism.myGenWords, findTarget.myGenWords))
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
                if(target.food - bloodDrink > 0)
                {
                    target.food -= bloodDrink;
                    myOrganism.food += bloodIntake;
                    amountDrinking += bloodIntake;
                }
                RangeToTarget();
                
            }
            bool RangeToTarget()
            {
                if (Math.Abs(target.point.X - (globalplace.X + localplace.X)) > biteRange * 2 || Math.Abs(target.point.Y - (globalplace.Y + localplace.Y)) > biteRange * 2)
                {
                    target = null;
                    return false;
                }
                else
                    return true;
            }
            public override void Dosomething(Organism myOrganism, Bitmap bmp)
            {
                if (target == null) { FindTargets(myOrganism, bmp); } else { BiteTarget(myOrganism); }
                if (target != null && target.food <= 500) { target = null; }
            }
        }
        /// <summary>
        /// зацепки для прикрепления к другим организмам
        /// </summary>
        class Clues : BodyPart
        {
            int holdOnRange = 4;
            bool myMove = true;
            public Organism? target = null;
            public Clues()
            {
                name = "Clues";
                color = Color.LavenderBlush;
                energyCost = 0;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                if (target != null)
                {
                    partsData.Add("Target         \t" + target.point.ToString());
                    partsData.Add("TargetEnergy   \t" + target.food.ToString());
                    partsData.Add("TargetID       \t" + target.GetID(target.myGenWords).ToString());

                }
                else
                {
                    partsData.Add("Target         \t" + "NoNe");
                    partsData.Add("TargetEnergy   \t" + "NoNe");
                    partsData.Add("TargetID       \t" + "NoNe");
                }
                partsData.Add("HookDistance   \t" + holdOnRange.ToString());
                partsData.Add("CanMove        \t" + myMove.ToString());
                return "";
            }
            void FindTargets(Organism myOrganism, Bitmap bmp)
            {
                for (int i = -holdOnRange; i <= holdOnRange; i++)
                {
                    for (int j = -holdOnRange; j <= holdOnRange; j++)
                    {
                        Point p = BorderChecker(new Point(myOrganism.point.X + localplace.X + i, myOrganism.point.Y + localplace.Y + j), bmp);
                        Organism? findTarget = myOrganism.controller.cellDictionary.ContainsKey(p) ? myOrganism.controller.cellDictionary[p] : null;
                        if (findTarget != null && !myOrganism.GetIDdiff(myOrganism.myGenWords, findTarget.myGenWords))
                        {
                            target = findTarget;
                            findTarget = null;
                            break;
                        }
                    }
                    if (target != null) break;
                }
            }
            void Hook(Organism body,Bitmap bmp)
            {
                if (target != null)
                    body.newPoint = BorderChecker(target.newPoint.X, target.newPoint.Y - 1, bmp);
                    body.globalTarget = target;
            }
            bool CheckDistance(Organism body, Organism? target)
            {
                if (target == null) return false;
                return Math.Abs(body.point.X - target.point.X) <= holdOnRange * 2 || Math.Abs(body.point.Y - target.point.Y) <= holdOnRange * 2;
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (target == null) { FindTargets(body, bmp); body.move = true; } else { Hook(body, bmp); body.move = false; }
                if ((target != null && !CheckDistance(body, target)) || (target != null && target.food <= 0)) { target = null; }
                myMove = body.move;
            }
        }
        /// <summary>
        /// когти для удержание другого организма
        /// </summary>
        class Claws : BodyPart
        {
            int holdOnRange = 4;
            bool myMove = true;
            public Organism? target = null;
            public Claws()
            {
                name = "Claws";
                color = Color.WhiteSmoke;
                energyCost = 1;
            }
            public override string UpdateMyData()
            {
                base.UpdateMyData();
                if (target != null)
                {
                    partsData.Add("Target         \t" + target.point.ToString());
                    partsData.Add("TargetEnergy   \t" + target.food.ToString());
                    partsData.Add("TargetID       \t" + target.GetID(target.myGenWords).ToString());
                }
                else
                {
                    partsData.Add("Target         \t" + "NoNe");
                    partsData.Add("TargetEnergy   \t" + "NoNe");
                    partsData.Add("TargetID       \t" + "NoNe");
                }
                partsData.Add("HoldOnDistance   \t" + holdOnRange.ToString());
                partsData.Add("TargetCanMove    \t" + myMove.ToString());
                return "";
            }
            void FindTargets(Organism myOrganism, Bitmap bmp)
            {
                for (int i = -holdOnRange; i <= holdOnRange; i++)
                {
                    for (int j = -holdOnRange; j <= holdOnRange; j++)
                    {
                        Point p = BorderChecker(new Point(myOrganism.point.X + localplace.X + i, myOrganism.point.Y + localplace.Y + j), bmp);
                        Organism? findTarget = myOrganism.controller.cellDictionary.ContainsKey(p) ? myOrganism.controller.cellDictionary[p] : null;
                        if (findTarget != null && !myOrganism.GetIDdiff(myOrganism.myGenWords, findTarget.myGenWords))
                        {
                            target = findTarget;
                            findTarget = null;
                            break;
                        }
                    }
                    if (target != null) break;
                }
            }
            void holdOn(Organism body, Bitmap bmp)
            {
                if (target != null)
                    target.newPoint = BorderChecker( body.newPoint.X, body.newPoint.Y - 1, bmp);
                    body.globalTarget = target;
            }
            bool CheckDistance(Organism body, Organism? target)
            {
                if (target == null) return false;
                return Math.Abs(body.point.X - target.point.X) <= holdOnRange * 2 || Math.Abs(body.point.Y - target.point.Y) <= holdOnRange * 2;
            }
            public override void Dosomething(Organism body, Bitmap bmp)
            {
                if (target == null) { myMove = false; FindTargets(body, bmp); } else if (CheckDistance(body, target)) { myMove = target.move; holdOn(body,bmp); } else
                if ((target != null && !CheckDistance(body, target)) || (target != null && target.food <= 0)) { myMove = target.move; target = null; }
                
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
        /// <summary>
        /// его не могут скушать рты и мешают скушать органы которые около кератина
        /// </summary>
        class Keratin : BodyPart
        {
            public Keratin()
            {
                name = "Keratin";
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
        /// <summary>
        /// Шип наносит урон тому кто его кусает(сам шип тоже получает урон) (регенерация тканей?)
        /// </summary>
        class Spike: BodyPart
        {
            public Spike()
            {
                name = "Spike";
                color = Color.LightGray;
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
        /// Позволяет ускоряться на короткое время
        /// </summary>
        class Tail
        {

        }
        /// <summary>
        /// метаморфизин это особая часть тела, которая не является физическим органом и не выполняющая физических действий, но позволяет организму переходить в другую фазу, позволяя менять расположение частей тела
        /// может использоваться как аналогия матаморфоз у гусеницы
        /// </summary>
        class Metamorfizin : BodyPart
        {
            public Metamorfizin()
            {
                name = "Metamorfizin";
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
