using System.Drawing;

namespace MicroLife_Simulator
{
    public partial class Form1
    {
        class Controller
        {
            public List<Grass> grassList = new List<Grass>();
            public List<Grass> grassListTEMP = new List<Grass>();
            public List<Grass> grassListTORemove = new List<Grass>();
            public Dictionary<Point, Grass> grassDictionary = new Dictionary<Point, Grass>();
            public List<Organism> cellsList = new List<Organism>();
            public List<Organism> cellsListTEMP = new List<Organism>();
            public List<Organism> cellsListTORemove = new List<Organism>();
            public Dictionary<Point, Organism> cellDictionary = new Dictionary<Point, Organism>();
            public Dictionary<Point, int> infectionLVL = new Dictionary<Point, int>();
            public List<Point> obstacles = new List<Point>();
            public int sunLVL;
            public int radiationLVL;
            public Bitmap? bmpOrganColor;
            public ComboBox? comboBox;
            public ListBox? listBox;
            //------------------------------Drawing
            int grassCurrent = 1;
            int cellCurrent = 1;
            //-------------------------------------------------------------
            
            ZoneType? activeType;
            //-------------------------------------------------------------
            public Controller(int sun, int radiation)
            {
                sunLVL = sun;
                radiationLVL = radiation;
            }

            public void CreateLive(Bitmap bmp, Random rand, PictureBox pictureBox1, int grass, int cells)
            {
                Point point;
                while (grassList.Count < grass)
                {
                    point = new Point(rand.Next(pictureBox1.Width), rand.Next(pictureBox1.Height));
                    Color color = bmp.GetPixel(point.X, point.Y);
                    if (color.G == 0 && color.R == 0 && color.B == 0)
                    {
                        grassList.Add(new Grass(point));
                    }
                }

                while (cellsList.Count <= cells)
                {
                    point = new Point(rand.Next(pictureBox1.Width), rand.Next(pictureBox1.Height));
                    cellsList.Add(new Organism(point));
                }

                for (int i = 0; i < 5000; i++)
                {
                    point = new Point(rand.Next(pictureBox1.Width), rand.Next(pictureBox1.Height));
                    if (!infectionLVL.ContainsKey(point)) { infectionLVL.Add(point, 2000); }
                }
            }
            public void CreateGrass(Bitmap bmp, Random rand, int grass)
            {
                Point point;
                for (int i = 0; i < grass; i++)
                {
                    point = new Point(rand.Next(bmp.Width), rand.Next(bmp.Height));
                    Color color = bmp.GetPixel(point.X, point.Y);
                    if (color.G == 0 && color.R == 0 && color.B == 0)
                    {
                        grassList.Add(new Grass(point));
                    }
                }
                
            }
            public void CreateOsticles(Bitmap bmp)
            {
                for (int i = bmp.Width / 2 - 30; i < bmp.Width / 2 + 30; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        obstacles.Add(new Point(i, j));
                    }
                }
            }
            public void Draw(Bitmap bmp)
            {
                DrawInfection(bmp);
                foreach (Grass grass in grassList) { grass.Draw(bmp); }
                foreach (Organism cell in cellsList) { cell.Draw(bmp); }
            }
            List<Point> points = new List<Point>();
            void DrawInfection(Bitmap bmp)
            {
                points.Clear();
                foreach (var item in infectionLVL)
                {
                    if (item.Value == 0) { bmp.SetPixel(item.Key.X, item.Key.Y, Color.Empty); points.Add(item.Key); }
                    bmp.SetPixel(item.Key.X, item.Key.Y, Color.FromArgb(255, ColorNormalizator(item.Value), ColorNormalizator(item.Value), 0));
                }
                foreach (var item in points)
                {
                    infectionLVL.Remove(item);
                }
                points.Clear();
            }
            public int ColorNormalizator(int val)
            {
                return val < 65025 ? val / 255 : 255;
            }
            //-----------------------------------------------------------------------------------отрисовка организма в обзорной картинке
            public void DrawObservePicture(Bitmap bmp)
            {
                {
                    Point point = new Point(bmp.Width / 2, bmp.Height / 2);
                    for (int i = point.X - 10; i < point.X + 10; i++)
                    {
                        for (int j = point.Y - 10; j < point.Y + 10; j++)
                        {
                            bmp.SetPixel(i, j, Color.Empty);
                        }
                    }
                }
                if (selectedObject != null)
                {
                    foreach (var item in selectedObject.bodyTypes)
                    {
                        Point point = new Point(bmp.Width / 2 + item.localplace.X, bmp.Height / 2 + item.localplace.Y);
                        bmp.SetPixel(point.X, point.Y, item.color);
                    }
                    Point pointPixel = new Point(selectedObject.bodyTypes[comboBox.SelectedIndex].localplace.X + bmp.Width / 2,
                                                 selectedObject.bodyTypes[comboBox.SelectedIndex].localplace.Y + bmp.Height / 2);
                    bmp.SetPixel(pointPixel.X, pointPixel.Y, Color.White);
                }
            }
            //----------------------------------------------------------------------------------цвет органа организма в информационной таблице
            public void DrawOrganColor(Bitmap? bmp)
            {
                if (selectedObject != null)
                {
                    for (int i = 0; i < bmp.Width; i++)
                    {
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            bmp.SetPixel(i, j, selectedObject.bodyTypes[comboBox.SelectedIndex].color);
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < bmp.Width; i++)
                    {
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            bmp.SetPixel(i, j, Color.Empty);
                        }
                    }

                }

            }
            //----------------------------------------------------------------------------------выбор организма по щелчку ЛКМ-----------доделать перевыбор на одном месте
            public Organism? selectedObject = null;
            public void SelectTarget(Point selectedPoint)
            {
                for (int i = -3; i < 4; i++)
                {
                    for (int j = -3; j < 4; j++)
                    {
                        Point p = new Point(selectedPoint.X + i, selectedPoint.Y + j);
                        if (cellDictionary.ContainsKey(p) && (selectedObject != cellDictionary[p]) )
                        {
                            selectedObject = cellDictionary[p];
                            selectedPoint = new Point(p.X, p.Y);
                            break;
                        }
                        else { selectedObject = null; }
                        
                    }
                    if (selectedObject != null) { break; }
                }
                if (!cellDictionary.ContainsKey(selectedPoint))
                {
                    selectedObject = null;
                    selectedPoint = new Point(-1, -1);
                }
                
                ComboBoxUpdate(selectedObject);
            }
            
            //--------------------------------------------------------------------------отрисовка рамки вокруг организма
            int frameSize = 7;
            List<Point> coloredPoints = new List<Point>();
            public void DrawSelectedTargetFrame(Bitmap bmp)
            {
                foreach (var item in coloredPoints)
                {
                    bmp.SetPixel(item.X, item.Y, Color.Empty);
                }
                coloredPoints.Clear();
                if (selectedObject != null && cellsList.Contains(selectedObject))
                {
                    for (int x = -frameSize; x <= frameSize; x++)
                    {
                        for (int y = -frameSize; y <= frameSize; y++)
                        {
                            if ((x == -frameSize || x == frameSize) || (y == -frameSize || y == frameSize))
                            {
                                if ((selectedObject.point.X + x > 1) && (selectedObject.point.X + x < bmp.Width) && (selectedObject.point.Y + y > 1) && (selectedObject.point.Y + y < bmp.Height))
                                {
                                    bmp.SetPixel(selectedObject.point.X + x, selectedObject.point.Y + y, Color.White);
                                    coloredPoints.Add(new Point(selectedObject.point.X + x, selectedObject.point.Y + y));
                                }
                            }
                        }
                    }
                }
            }
            //------------------------------------------------------------------------обновление информации по организму
            public void ComboBoxUpdate(Organism? selected)
            {

                comboBox.Items.Clear();
                if (selected != null)
                {
                    foreach (var item in selected.bodyTypes)
                    {
                        comboBox.Items.Add(item.name);
                    }
                    comboBox.Text = selected.bodyTypes[0].name;

                }
                DrawOrganColor(bmpOrganColor);
            }
            public void ListBoxUpdate(Organism? selected)
            {
                listBox.Items.Clear();
                if (selected != null)
                {
                    selected.bodyTypes[comboBox.SelectedIndex].UpdateMyData();
                    foreach (string item in selected.bodyTypes[comboBox.SelectedIndex].partsData)
                    {
                        listBox.Items.Add(item);
                    }

                }
                else { listBox.Items.Clear(); }
            }
            //---------------------------------------------------------------------------
            public void CreateZones_auto()//случаная местность
            {

            }
            public void CreateZones_manual(Point mousePoint, ZoneType activeType)//ручное создание зоны
            {
                //создавать зону типа activeType в точке курсора mousePoint
            }
        }
    }
}
