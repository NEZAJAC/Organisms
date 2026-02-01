using System.Drawing;

namespace MicroLife_Simulator
{
    public partial class Form1
    {
        static class Controller
        {
            static public List<Grass> grassList = new List<Grass>();
            static public List<Grass> grassListTEMP = new List<Grass>();
            static public List<Grass> grassListTORemove = new List<Grass>();
            static public Dictionary<Point, Grass> grassDictionary = new Dictionary<Point, Grass>();
            //---------------------------------------------------------------------------------------------------
            static public List<Organism> cellsList = new List<Organism>();
            static public List<Organism> cellsListTEMP = new List<Organism>();
            static public List<Organism> cellsListTORemove = new List<Organism>();
            static public Dictionary<Point, Organism> cellDictionary = new Dictionary<Point, Organism>();
            //---------------------------------------------------------------------------------------------------
            static public List<Egg> eggList = new List<Egg>();
            static public List<Egg> eggListTEMP = new List<Egg>();
            static public List<Egg> eggListTORemove = new List<Egg>();
            static public Dictionary<Point, Egg> eggDictionary = new Dictionary<Point, Egg>();
            //---------------------------------------------------------------------------------------------------
            static public List<Obstacles> obstaclesList = new List<Obstacles>();
            static public Dictionary<Point, int> infectionLVL = new Dictionary<Point, int>();
            //public List<Point> obstacles = new List<Point>();
            static public List<int> attackMarkerTime = new List<int>();
            static public List<Point> attackPoint = new List<Point>();
            static public int sunLVL;
            static public int radiationLVL;
            static public Bitmap? bmpOrganColor;
            static public ComboBox? comboBox;
            static public ListBox? listBox;
            //------------------------------Drawing
            static int grassCurrent = 1;
            static int cellCurrent = 1;
            //-------------------------------------------------------------
            static ZoneType? activeType;
            //-------------------------------------------------------------
            static List<Point> infectionToClear = new List<Point>();
            static public Organism? selectedObject = null;
            static int frameSize = 7;
            static List<Point> coloredPoints = new List<Point>();

            static public void CreateLive(Bitmap bmp, Random rand, PictureBox pictureBox1, int grass, int cells, int obstacles, int infection, int sun, int radiation)
            {
                sunLVL = sun;
                radiationLVL = radiation;
                Point point;
                for (int i = 0; i < obstacles; i++)
                {
                    Point[] p = new Point[2];
                    p[0] = new Point(rand.Next(pictureBox1.Width), rand.Next(pictureBox1.Height));
                    p[1] = new Point(p[0].X + rand.Next(10, 30), p[0].Y + rand.Next(10, 30));
                    obstaclesList.Add(new Obstacles(p[0], p[1], bmp));
                }
                int times = 0;
                while (times < infection)
                {
                    point = new Point(rand.Next(pictureBox1.Width), rand.Next(pictureBox1.Height));
                    if (infectionLVL.ContainsKey(point)) { infectionLVL[point] += 500; }
                    point = new Point(rand.Next(pictureBox1.Width), rand.Next(pictureBox1.Height));
                    if (!infectionLVL.ContainsKey(point)) { infectionLVL.Add(point, 2000); }
                    times++;
                }
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

            }
            static public void CreateGrass(Bitmap bmp, Random rand, int grass)
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
            static public void Draw(Bitmap bmp)
            {
                //if (checkBox4.Checked) DrawInfection(bmp);
                //if (checkBox3.Checked) DrawFoodGrass(bmp);
                DrawInfection(bmp);
                DrawFoodGrass(bmp);
                foreach (Obstacles obstacles in obstaclesList) { obstacles.Draw(bmp); }

                foreach (Organism cell in cellsList) { cell.Draw(bmp); }
                foreach (Egg egg in eggList) { egg.Draw(bmp); }
                DrawRectangleOnOrganism(bmp);
                DrawCursorOnTheField(bmp);
            }
            static void DrawCursorOnTheField(Bitmap bmp)
            {

            }
            static void DrawRectangleOnOrganism(Bitmap bmp)
            {

            }
            static void DrawFoodGrass(Bitmap bmp)
            {
                foreach (Grass grass in grassList) { grass.Draw(bmp); }
            }
            static void DrawInfection(Bitmap bmp)
            {
                //points.Clear();
                foreach (var item in infectionLVL)
                {
                    if (item.Value == 0) { bmp.SetPixel(item.Key.X, item.Key.Y, Color.Empty); infectionToClear.Add(item.Key); }
                    bmp.SetPixel(item.Key.X, item.Key.Y, Color.FromArgb(255, ColorNormalizator(item.Value), ColorNormalizator(item.Value), 0));//Перестать рисовать каждый такт!!!!!!!!!!!!!!!!
                }
                foreach (var item in infectionToClear)
                {
                    infectionLVL.Remove(item);
                }
                infectionToClear.Clear();
            }
            static public int ColorNormalizator(int val)
            {
                return val < 65025 ? val / 255 : 255;
            }
            //-----------------------------------------------------------------------------------отрисовка организма в обзорной картинке
            static public void DrawObservePicture(Bitmap bmp)
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
                if (selectedObject != null && comboBox.Visible)
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
            static public void DrawOrganColor(Bitmap? bmp)
            {
                if (selectedObject != null && comboBox.Visible)
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
            static public void SelectTarget(Point selectedPoint)
            {
                for (int i = -3; i < 4; i++)
                {
                    for (int j = -3; j < 4; j++)
                    {
                        Point p = new Point(selectedPoint.X + i, selectedPoint.Y + j);
                        if (cellDictionary.ContainsKey(p) && (selectedObject != cellDictionary[p]))
                        {
                            selectedObject = cellDictionary[p];
                            foreach (var item in selectedObject.bodyTypes)
                            {
                                comboBox.Items.Add(item);
                            }
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
            static public void DrawSelectedTargetFrame(Bitmap bmp)
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
            //-------------------------------------------------------------------------рисует положение "камеры" на миникарте(сделать)
            static List<Point> cameraOnMinimapPoints = new List<Point>();
            static public void DrawCameraOnMinimap(Bitmap bmp)
            {
                foreach (var item in cameraOnMinimapPoints)
                {
                    bmp.SetPixel(item.X, item.Y, Color.Empty);
                }
                cameraOnMinimapPoints.Clear();
                for (int x = -frameSize; x <= frameSize; x++)
                {
                    for (int y = -frameSize; y <= frameSize; y++)
                    {
                        if ((x == -frameSize || x == frameSize) || (y == -frameSize || y == frameSize))
                        {
                            if ((selectedObject.point.X + x > 1) && (selectedObject.point.X + x < bmp.Width) && (selectedObject.point.Y + y > 1) && (selectedObject.point.Y + y < bmp.Height))
                            {
                                bmp.SetPixel(selectedObject.point.X + x, selectedObject.point.Y + y, Color.White);
                                cameraOnMinimapPoints.Add(new Point(selectedObject.point.X + x, selectedObject.point.Y + y));
                            }
                        }
                    }
                }
            }//---------------------------НЕ СДЕЛАНО
            //------------------------------------------------------------------------обновление информации по организму
            static public void ComboBoxUpdate(Organism? selected)
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
            static public void ListBoxUpdate(Organism? selected)
            {
                listBox.Items.Clear();
                if (selected != null)
                {
                    
                    foreach (string item in selected.bodyTypes[comboBox.SelectedIndex].partsData)
                    {
                        listBox.Items.Add(item);
                    }
                    selected.bodyTypes[comboBox.SelectedIndex].UpdateMyData();

                }
                else { listBox.Items.Clear(); }
            }
            //---------------------------------------------------------------------------
            static public void CreateZones_auto()//случаная местность
            {

            }
            static public void CreateZones_manual(Point mousePoint, ZoneType activeType)//ручное создание зоны
            {
                //создавать зону типа activeType в точке курсора mousePoint
            }
        }
    }
}
