using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

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
            panel1.AutoScroll = true;
            
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmpObservePicture = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            bmpOrgansColor = new Bitmap(pictureBox4.Width, pictureBox4.Height);
            pictureBox3.Size *= 8;//------------------------------------------------------------------------ увеличиваю картинку что бы видеть организм лучше
            pictureBox3.Location = new Point(-pictureBox3.Width/2 + 108 , -pictureBox3.Height/2 + 105); 
            //------------------------------
            MAXgrass = int.Parse(textBox1.Lines[0]);
            MAXorganis = int.Parse(textBox2.Lines[0]);
            //------------------------------
            //controller = 
            /*for (int i = bmp.Width / 2 - 30; i < bmp.Width / 2 + 30; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    bmp.SetPixel(i, j, Color.Wheat);
                }
            }
            for (int i = bmp.Width / 2 - 30; i < bmp.Width / 2 + 30; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    bmp.SetPixel(j, i, Color.Wheat);
                }
            }
            */
            //------------------------
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
            //List<List<Grass>> chunkGrassList = controller.grassList.Chunk(10000);
            

        }
        public void UpdateInfo()
        {
            
            if (panel1.Visible && bmpObservePicture != null)
                {
                if (checkBox1.Checked)
                {
                    controller.ListBoxUpdate(controller.selectedObject);
                    controller.DrawOrganColor(controller.bmpOrganColor);
                }
                    controller.DrawObservePicture(bmpObservePicture);
                    pictureBox3.Image = bmpObservePicture;
                    if (controller.selectedObject != null)
                    {
                        label11.Text = controller.selectedObject.age.ToString() + "/" + controller.selectedObject.maxage;
                        label10.Text = controller.selectedObject.food.ToString() + "/" + controller.selectedObject.maxfood.ToString();
                        label6.Text = controller.selectedObject.canDuplicate.ToString();
                        label9.Text = controller.selectedObject.point.ToString();
                    pictureBox4.Image = bmpOrgansColor;
                }
                    else { label10.Text = "NoNe"; label11.Text = "NoNe"; }
                }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label13.Text = sw.ElapsedMilliseconds.ToString(); 
            sw.Restart();
            sw.Start();
            controller.DrawSelectedTargetFrame(bmp);
            controller.Draw(bmp);
            UpdateInfo();
            pictureBox1.Image = bmp;
            pictureBox2.Image = bmp;//----------------------------------------Minimap
            if (checkBox1.Checked)
            {
                foreach (Grass grass in controller.grassList)
                {
                    if (grass.GrassUpdate(controller.sunLVL))
                    {
                        if (controller.grassList.Count < MAXgrass && !GrassLimit_CB.Checked)
                        {
                            if (grass.Duplicate(bmp, out Point grPoint))
                            {
                               controller.grassListTEMP.Add(new Grass(grPoint));
                            }
                        }
                        else
                        if (GrassLimit_CB.Checked)
                        {
                            if (grass.Duplicate(bmp, out Point grPoint))
                            {
                               controller.grassListTEMP.Add(new Grass(grPoint));
                            }
                        }
                    }
                    if (grass.food <= controller.sunLVL)
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
            //label4.Text = controller.grassNoresp.ToString();
            //-----------------------------------------------------------------------------------------
            if (checkBox1.Checked)
            {
                foreach (Organism organism in controller.cellsList)
                {
                    organism.DoworkPrepare(bmp, controller);//----------------------------------------------------------------------------------------------каждый делает свою работу

                    if (organism.food >= organism.dublicateFood && !OrgLimit_CB.Checked && controller.cellsList.Count < MAXorganis && organism.canDuplicate)
                    {
                        organism.food -= organism.dublicateFoodPrice;
                        controller.cellsListTEMP.Add(new Organism(organism.point, organism));
                        organism.canDuplicate = false;
                        organism.dublicateDelay = 0;
                    }
                    else
                    if (organism.food >= organism.dublicateFood && OrgLimit_CB.Checked && organism.canDuplicate)
                    {
                        organism.food -= organism.dublicateFoodPrice;
                        controller.cellsListTEMP.Add(new Organism(organism.point, organism));
                        organism.canDuplicate = false;
                        organism.dublicateDelay = 0;
                    }
                    else
                    if (organism.food >= organism.dublicateFood)
                    {
                        //organism.food -= organism.dublicateFoodPrice;
                        //organism.canDuplicate = false;
                        //organism.dublicateDelay = 0;
                    }
                    if (organism.food <= 0 || organism.age >= organism.maxage || organism.point.X < 0 || organism.point.X > bmp.Width || organism.point.Y < 0 || organism.point.Y > bmp.Height)
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
                    if (controller.infectionLVL.ContainsKey(item.point)) { controller.infectionLVL[item.point] += 500 * item.bodyTypes.Count + item.food/500; } else { controller.infectionLVL.Add(item.point, 500 * item.bodyTypes.Count + item.food / 500); }
                    controller.cellsList.Remove(item);
                }
                controller.cellsListTORemove.Clear();

                label8.Text = controller.cellsList.Count.ToString();
            }
            sw.Stop();
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
