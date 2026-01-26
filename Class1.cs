using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MicroLife_Simulator
{
    public partial class Form1 : Form
    {
        int MAXgrass = 10000;
        int MAXorganis = 100;
        Random rand = new Random();
        public Bitmap? bmp;
        public Bitmap? bmpGrass;
        Bitmap? bmpObservePicture;
        Bitmap? bmpOrgansColor;
        //Controller Controller = new Controller(10, 10);
        
        Stopwatch sw = Stopwatch.StartNew();
        Size size;
        public Form1()
        {
            InitializeComponent();
            //this.DoubleBuffered = true;
            //this.Paint += new PaintEventHandler(Form1_Paint);
            NewStart();
        }
        void NewStart()
        {
            size = pictureBox1.Size;
            panel1.AutoScroll = true;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmpGrass = new Bitmap(pictureBox1.Height, pictureBox1.Width);
            bmpObservePicture = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            bmpOrgansColor = new Bitmap(pictureBox4.Width, pictureBox4.Height);
            pictureBox3.Size *= 8;//------------------------------------------------------------------------ увеличиваю картинку что бы видеть организм лучше
            pictureBox3.Location = new Point(-pictureBox3.Width / 2 + 108, -pictureBox3.Height / 2 + 105);
            //------------------------------
            MAXgrass = int.Parse(label24.Text);
            MAXorganis = int.Parse(textBox2.Lines[0]);
            //------------------------
            Controller.CreateLive(bmp, rand, pictureBox1, 100, 100, 0, 500, 10, 10);

            Controller.comboBox = comboBox1;
            Controller.listBox = listBox1;
            Controller.bmpOrganColor = bmpOrgansColor;
            //------------------------------
            trackBar1.Value = 1;
            timer1.Interval = trackBar1.Value;
            label12.Text = timer1.Interval.ToString();
            //------------------------------
            trackBar2.Value = Controller.sunLVL;
            label14.Text = Controller.sunLVL.ToString();
            //------------------------------
            panel1.Hide();
        }
        //Сделать таймер времени эмуляции
        //сделать кнопку выборки всех существ с одним генотипом/ подсчет этих особей/кнопка для удаления всех существ этого генотипа
        //Систему автоматического сохранения генома и автоматическую загрузку
        //Добавить возможность именовать комбинации генов при сохранении
        //Добавить краткие описания органоидов ✔
        //Расширение карты(до 9 пнг)
        //Переработать автокил
        //движение карты с помощью мыши тыкая по карте а не по миникарте ✔
        //проработать зону игнорирования возможного перемещения для ноги, подготовка к препятствиям
        //добавить кнопку рестарт
        //динамическая смена дня ночи по галочке
        //-------------------------------Из сложного
        //обобщить поиск организма, убрав его из органов, в органах использовать уже готовые результаты поиска, поиск основывать на имеющихся органов нуждающихся в поиске проверяя их условия.
        //переписать рисовальщик убрав возможность рисовать у всех организмов. Организмы добавляют цвет и положение пикселя, рисовальщик рисует все сразу
        //пофиксить генерацию организмов, что бы было без дыр, все органы должны стоять рядом. Возможно нужно переработать поиск возможных мест.
        //СОХРАНЕНИЕ ЭМУЛЯЦИИ В ТЕКСТОВЫЙ ФАЙЛ(БУДЕТ МНОГО) ДЛЯ ДАЛЬНЕЙШЕГО ПРОДОЛЖЕНИЯ И ПЕРЕДАЧИ 
        private void timer1_Tick(object sender, EventArgs e)
        {
            label13.Text = sw.ElapsedMilliseconds.ToString();
            int power = ((int)sw.ElapsedMilliseconds) < 256 ? ((int)sw.ElapsedMilliseconds) : 255;
            label13.ForeColor = Color.FromArgb(255, power, 255 - power, 0);
            sw.Restart();
            sw.Start();
            if (checkBox1.Checked)
            {
                ControllerGrassWork();
                ControllerCellWork();
                ControllerEggWork();
                UpdateTargetInfo();
            }



            Controller.Draw(bmp);
            Controller.DrawSelectedTargetFrame(bmp);

            pictureBox1.Image = bmp;
            pictureBox2.Image = bmp;//----------------------------------------Minimap
            //TESTobstaclesDraw(bmp);

            Controller.grassListTORemove.Clear();
            Controller.cellsListTORemove.Clear();
            Controller.eggListTORemove.Clear();

            label1.Text = Controller.grassList.Count.ToString();
            label8.Text = Controller.cellsList.Count.ToString();
            label21.Text = Controller.eggList.Count.ToString();

            
            sw.Stop();
        }
        private void ManualOrganDeleteCellWorkUpdate(Organism organism)
        {
            if (organism.bodyTypes.Count < 1)
            {
                Controller.selectedObject = null;
                UpdateTargetInfo();
                organism.Cleary(bmp);
                Controller.cellsList.Remove(organism);
            }
            
        }
        private void ControllerCellWork()
        {
            if (AutoKill.Checked && Controller.cellsList.Count >= MAXorganis) { AutoKillProcent(); }

            Controller.cellDictionary.Clear();
            foreach (Organism organism in Controller.cellsList)
            {
                organism.DoworkPrepare(bmp);//----------------------------------------------------------------------------------------------каждый делает свою работу

                if (organism.food >= organism.parameters.dublicateFood && !OrgLimit_CB.Checked && Controller.cellsList.Count + Controller.cellsListTEMP.Count < MAXorganis && organism.canDuplicate)
                {
                    //organism.food -= organism.parameters.dublicateFoodPrice;
                    Controller.cellsListTEMP.Add(new Organism(organism.point, organism));
                    organism.WithoutDublicateSignal = 0;
                    organism.canDuplicate = false;
                    organism.parameters.dublicateDelay = 0;
                }
                else
                if (organism.food >= organism.parameters.dublicateFood && OrgLimit_CB.Checked && organism.canDuplicate)
                {
                    //organism.food -= organism.parameters.dublicateFoodPrice;
                    Controller.cellsListTEMP.Add(new Organism(organism.point, organism));
                    organism.WithoutDublicateSignal = 0;
                    organism.canDuplicate = false;
                    organism.parameters.dublicateDelay = 0;
                }
                else
                {
                    organism.WithoutDublicateSignal++;
                }
                if (organism.food <= 0 || organism.age >= organism.parameters.maxage || organism.bodyTypes.Count < 2)
                {
                    Controller.cellsListTORemove.Add(organism);
                }

                if (!Controller.cellDictionary.ContainsKey(organism.point))
                {
                    Controller.cellDictionary.Add(organism.point, organism);
                }
            }
            //foreach (Organism organism in Controller.cellsList)
            //{
            //    if (!Controller.cellDictionary.ContainsKey(organism.point))
            //    {
            //        Controller.cellDictionary.Add(organism.point, organism);
            //    }
            //}
            foreach (var item in Controller.cellsListTEMP)
            {
                Controller.cellsList.Add(item);
            }
            Controller.cellsListTEMP.Clear();

            foreach (var item in Controller.cellsListTORemove)
            {
                if (Controller.selectedObject == item)
                {
                    Controller.selectedObject = null;
                }
                item.Cleary(bmp);
                if (Controller.infectionLVL.ContainsKey(item.point)) { Controller.infectionLVL[item.point] += 500 * item.bodyTypes.Count + item.food / 500; } else { Controller.infectionLVL.Add(item.point, 500 * item.bodyTypes.Count + item.food / 500); }
                Controller.cellsList.Remove(item);
            }



        }
        private void ControllerGrassWork()
        {

            if (checkBox1.Checked)
            {
                foreach (Grass grass in Controller.grassListTEMP)
                {
                    Controller.grassList.Add(grass);
                }
                Controller.grassListTEMP.Clear();

                foreach (Grass grass in Controller.grassList)
                {
                    if (grass.GrassUpdate(Controller.sunLVL))
                    {
                        if (Controller.grassList.Count + Controller.grassListTEMP.Count < MAXgrass && !GrassLimit_CB.Checked)
                        {
                            if (grass.Duplicate(bmp, out Point grPoint))
                            {
                                Controller.grassListTEMP.Add(new Grass(grPoint));
                                grass.food = grass.food / 2;
                                grass.maxfood += 50;
                            }
                        }
                        else
                        if (GrassLimit_CB.Checked)
                        {
                            if (grass.Duplicate(bmp, out Point grPoint))
                            {
                                Controller.grassListTEMP.Add(new Grass(grPoint));
                                grass.maxfood += 50;
                            }
                        }
                    }
                    if (grass.food <= Controller.sunLVL)
                    {
                        Controller.grassListTORemove.Add(grass);
                    }


                    if (!Controller.grassDictionary.ContainsKey(grass.point))
                    {
                        Controller.grassDictionary.Add(grass.point, grass);
                    }
                    if (grass.food <= 0 || grass.age >= grass.maxage)
                    {
                        Controller.grassListTORemove.Add(grass);
                    }
                }



                //foreach (Grass grass in Controller.grassList)//---------------------------------------------------------вот тут разбить на чанки
                //{
                //	if (!Controller.grassDictionary.ContainsKey(grass.point))
                //	{
                //		Controller.grassDictionary.Add(grass.point, grass);
                //	}
                //	if (grass.food <= 0 || grass.age >= grass.maxage)
                //	{
                //		Controller.grassListTORemove.Add(grass);
                //	}
                //}
                foreach (Grass grass in Controller.grassListTORemove)
                {
                    grass.Clear(bmp);
                    Controller.grassList.Remove(grass);
                    Controller.grassDictionary.Remove(grass.point);
                }
            }

        }
        private void ControllerEggWork()
        {
            if (checkBox1.Checked)
            {
                foreach (Egg egg in Controller.eggList)
                {
                    egg.Dosomething();//----------------------------------------------------------------------------------------------каждый делает свою работу
                    if (egg.incubation >= egg.incubationTime && !OrgLimit_CB.Checked && Controller.cellsList.Count + Controller.cellsListTEMP.Count < MAXorganis)
                    {
                        Controller.cellsListTEMP.Add(new Organism(egg.point, egg.parametersParent1, egg.genListParent1, egg.parametersParent2, egg.genListParent2));
                        Controller.eggListTORemove.Add(egg);
                    }
                    else if (egg.age >= 3900 && !OrgLimit_CB.Checked && Controller.cellsList.Count + Controller.cellsListTEMP.Count < MAXorganis)
                    {
                        //label20.Text = egg.myGuid.ToString();
                        Controller.cellsListTEMP.Add(new Organism(egg.point, egg.parametersParent1, egg.genListParent1, egg.parametersParent1, egg.genListParent1));
                        Controller.eggListTORemove.Add(egg);
                    }
                    if (egg.age >= 4000 || Controller.eggList.Count - Controller.eggListTORemove.Count >= 500)
                    {
                        Controller.eggListTORemove.Add(egg);
                    }
                }
                foreach (var egg in Controller.eggListTEMP)
                {
                    Controller.eggList.Add(egg);
                }
                Controller.eggListTEMP.Clear();
                foreach (var item in Controller.eggListTORemove)
                {
                    item.Clear(bmp);
                    Controller.eggList.Remove(item);
                }
                Controller.eggDictionary.Clear();
                foreach (var egg in Controller.eggList)
                {
                    if (!Controller.eggDictionary.ContainsKey(egg.point))
                    {
                        Controller.eggDictionary.Add(egg.point, egg);
                    }

                }
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

        void saveAnyFormat(Bitmap bmp1)
        {
            Bitmap bmp = (Bitmap)bmp1.Clone();
            // Создаем диалоговое окно для выбора места сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp";
            saveFileDialog.DefaultExt = "bmp";
            saveFileDialog.AddExtension = true;

            //if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            try
            {
                // Получаем выбранный формат
                ImageFormat format = ImageFormat.Jpeg;
                switch (Path.GetExtension(saveFileDialog.FileName).ToLower())
                {
                    case ".png":
                        format = ImageFormat.Png;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }

                // Сохраняем Bitmap

                bmp.Save(saveFileDialog.FileName, format);
                MessageBox.Show("Изображение сохранено", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //}
        }

        public static void SaveBitmapAsBmp(Bitmap bmp, string filePath)
        {
            try
            {
                bmp.Save(filePath, ImageFormat.Bmp);
                MessageBox.Show("Изображение успешно сохранено в BMP.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
