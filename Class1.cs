using System.Diagnostics;

namespace MicroLife_Simulator
{
    public partial class Form1 : Form
    {
        int MAXgrass = 10000;
        int MAXorganis = 100;
        Random rand = new Random();
        Bitmap? bmp;
        Bitmap? bmpObservePicture;
        Bitmap? bmpOrgansColor;
        Controller controller = new Controller(15, 10);
        Stopwatch sw = Stopwatch.StartNew();
        Size size;
        public Form1()
        {
            InitializeComponent();
            NewStart();
        }
        void NewStart()
        {
            size = pictureBox1.Size;
            panel1.AutoScroll = true;
            //pictureBox1.Size = size*3;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmpObservePicture = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            bmpOrgansColor = new Bitmap(pictureBox4.Width, pictureBox4.Height);
            pictureBox3.Size *= 8;//------------------------------------------------------------------------ увеличиваю картинку что бы видеть организм лучше
            pictureBox3.Location = new Point(-pictureBox3.Width / 2 + 108, -pictureBox3.Height / 2 + 105);
            //------------------------------
            MAXgrass = int.Parse(label24.Text);
            MAXorganis = int.Parse(textBox2.Lines[0]);
            //------------------------
            controller.CreateLive(bmp, rand, pictureBox1, 100, 100);
            controller.CreateOsticles(bmp);
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
            label13.Text = sw.ElapsedMilliseconds.ToString();
            int power = ((int)sw.ElapsedMilliseconds) < 256 ? ((int)sw.ElapsedMilliseconds) : 255;
            label13.ForeColor = Color.FromArgb(255, power, 255 - power, 0);
            sw.Restart();
            sw.Start();

            UpdateTargetInfo();

            ControllerGrassWork();
            ControllerCellWork();
            ControllerEggWork();

			controller.Draw(bmp);
            pictureBox1.Image = bmp;
            pictureBox2.Image = bmp;//----------------------------------------Minimap
            //TESTobstaclesDraw(bmp);

            controller.grassListTORemove.Clear();
            controller.cellsListTORemove.Clear();
			controller.eggListTORemove.Clear();

			label1.Text = controller.grassList.Count.ToString();
            label8.Text = controller.cellsList.Count.ToString();
			label21.Text = controller.eggList.Count.ToString();

			controller.DrawSelectedTargetFrame(bmp);
            sw.Stop();
        }
        private void ControllerCellWork()
        {
            if (AutoKill.Checked && controller.cellsList.Count >= MAXorganis) { AutoKillProcent(); }
            if (checkBox1.Checked)
            {
                foreach (Organism organism in controller.cellsList)
                {
                    organism.DoworkPrepare(bmp, controller);//----------------------------------------------------------------------------------------------каждый делает свою работу

                    if (organism.food >= organism.parameters.dublicateFood && !OrgLimit_CB.Checked && controller.cellsList.Count + controller.cellsListTEMP.Count < MAXorganis && organism.canDuplicate)
                    {
                        //organism.food -= organism.parameters.dublicateFoodPrice;
                        controller.cellsListTEMP.Add(new Organism(organism.point, organism));
                        organism.WithoutDublicateSignal = 0;
                        organism.canDuplicate = false;
                        organism.parameters.dublicateDelay = 0;
                    }
                    else
                    if (organism.food >= organism.parameters.dublicateFood && OrgLimit_CB.Checked && organism.canDuplicate)
                    {
                        //organism.food -= organism.parameters.dublicateFoodPrice;
                        controller.cellsListTEMP.Add(new Organism(organism.point, organism));
                        organism.WithoutDublicateSignal = 0;
                        organism.canDuplicate = false;
                        organism.parameters.dublicateDelay = 0;
                    }
                    else
                    {
                        organism.WithoutDublicateSignal++;
                    }
                    if (organism.food <= 0 || organism.age >= organism.parameters.maxage)
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
                    if (controller.selectedObject == item)
                    {
                        controller.selectedObject = null;
                    }
                    item.Cleary(bmp);
                    if (controller.infectionLVL.ContainsKey(item.point)) { controller.infectionLVL[item.point] += 500 * item.bodyTypes.Count + item.food / 500; } else { controller.infectionLVL.Add(item.point, 500 * item.bodyTypes.Count + item.food / 500); }
                    controller.cellsList.Remove(item);
                }

                
            }
        }
        private void ControllerGrassWork()
        {
            if (checkBox1.Checked)
            {
                foreach (Grass grass in controller.grassList)
                {
                    if (grass.GrassUpdate(controller.sunLVL))
                    {
                        if (controller.grassList.Count + controller.grassListTEMP.Count < MAXgrass && !GrassLimit_CB.Checked)
                        {
                            if (grass.Duplicate(bmp, out Point grPoint))
                            {
                                controller.grassListTEMP.Add(new Grass(grPoint));
                                grass.maxfood += 50;
                            }
                        }
                        else
                        if (GrassLimit_CB.Checked)
                        {
                            if (grass.Duplicate(bmp, out Point grPoint))
                            {
                                controller.grassListTEMP.Add(new Grass(grPoint));
                                grass.maxfood += 50;
                            }
                        }
                    }
                    if (grass.food <= controller.sunLVL)
                    {
                        controller.grassListTORemove.Add(grass);
                    }
                }

				foreach (Grass grass in controller.grassListTEMP)
				{
					controller.grassList.Add(grass);
				}
				controller.grassListTEMP.Clear();

				foreach (Grass grass in controller.grassList)//---------------------------------------------------------вот тут разбить на чанки
				{
					if (!controller.grassDictionary.ContainsKey(grass.point))
					{
						controller.grassDictionary.Add(grass.point, grass);
					}
					if (grass.food <= 0 || grass.age >= grass.maxage)
					{
						controller.grassListTORemove.Add(grass);
					}
				}
				foreach (Grass grass in controller.grassListTORemove)
				{
					grass.Clear(bmp);
					controller.grassList.Remove(grass);
					controller.grassDictionary.Remove(grass.point);
				}
			}
            
        }
        private void ControllerEggWork()
        {
            if (checkBox1.Checked)
            {
                foreach (Egg egg in controller.eggList)
                {
                    egg.Dosomething();//----------------------------------------------------------------------------------------------каждый делает свою работу
                    if (egg.incubation >= egg.incubationTime && !OrgLimit_CB.Checked && controller.cellsList.Count + controller.cellsListTEMP.Count < MAXorganis)
                    {
                        controller.cellsListTEMP.Add(new Organism(egg.point, egg.parametersParent1, egg.genListParent1, egg.parametersParent2, egg.genListParent2));
                        controller.eggListTORemove.Add(egg);
                    }
                    else if(egg.age >= 3900 && !OrgLimit_CB.Checked && controller.cellsList.Count + controller.cellsListTEMP.Count < MAXorganis)
                    {
                        label20.Text = egg.myGuid.ToString();
                        controller.cellsListTEMP.Add(new Organism(egg.point, egg.parametersParent1, egg.genListParent1, egg.parametersParent1, egg.genListParent1));
                        controller.eggListTORemove.Add(egg);
                    }
                    if (egg.age >= 4000 || controller.eggList.Count - controller.eggListTORemove.Count >= 500)
                    {
                        controller.eggListTORemove.Add(egg);
                    }
                }
                foreach (var egg in controller.eggListTEMP)
				{
					controller.eggList.Add(egg);
				}
				controller.eggListTEMP.Clear();
                foreach (var item in controller.eggListTORemove)
                {
                    item.Clear(bmp);
                    controller.eggList.Remove(item);    
                }
                controller.eggDictionary.Clear();
				foreach (var egg in controller.eggList)
				{
					if (!controller.eggDictionary.ContainsKey(egg.point))
					{
						controller.eggDictionary.Add(egg.point, egg);
					}

				}
			}

		}
        void TESTobstaclesDraw(Bitmap bmp)
        {
            //------------------------------------------------------------------------------------------------рисую тут тестовые заграждения
            //controller = 
            foreach (var item in controller.obstacles)
            {
                bmp.SetPixel(item.X, item.Y, Color.Aqua);
            }

        }
        class ZoneType
        {
            Pen pen = new Pen(Color.Red);
            Rectangle radius;
            Effect? effect;
            public virtual void CreateZone() { }

        }
        class Effect//оказывает влияние на клетки находящие в зоне
        {

        }
    }
}
