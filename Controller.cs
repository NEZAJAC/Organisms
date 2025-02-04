using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace MicroLife_Simulator
{
    public partial class Form1
    {
        class Controller
        {
            public List<Grass> grassList = new List<Grass>();
            public List<Grass> grassListTEMP = new List<Grass>();
            public List<Grass> grassListTORemove = new List<Grass>();
            public Dictionary<Point,Grass> grassDictionary = new Dictionary<Point,Grass>();
            public List<Organism> cellsList = new List<Organism>();
            public List<Organism> cellsListTEMP = new List<Organism>();
            public List<Organism> cellsListTORemove = new List<Organism>();
            public Dictionary<Point,Organism> cellDictionary = new Dictionary<Point,Organism>();
            public int sunLVL;
            public int radiationLVL;
            public Bitmap? bmpOrganColor;
            public ComboBox? comboBox;
            public ListBox? listBox;
            //-------------------------------------------------------------
            ZoneType? activeType;
            //-------------------------------------------------------------
            public Controller(int sun, int radiation)
            {
                sunLVL = sun;
                radiationLVL = radiation;
            }

            public void CreateLive(Bitmap bmp,Random rand, PictureBox pictureBox1, int grass, int cells)
            {

                for (int i = 0; i < grass; i++)
                {
                    Point point = new Point(rand.Next(pictureBox1.Width), rand.Next(pictureBox1.Height));
                    Color color = bmp.GetPixel(point.X, point.Y);
                    if (color.G == 0 && color.R == 0 && color.B == 0)
                    {
                        Grass? tempGrass = new Grass(point);
                        grassList.Add(tempGrass);
                        tempGrass = null;
                    }

                }

                for (int i = 0; i < cells; i++)
                {
                    cellsList.Add(new Organism(new Point(rand.Next(pictureBox1.Width), rand.Next(pictureBox1.Height))));  

                }


            }

            public void Draw(Bitmap bmp)
            {
                foreach (var item in grassList) item.Draw(bmp);
                foreach (var item in cellsList) item.Draw(bmp);
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
                        bmp.SetPixel(point.X,point.Y,item.color);
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
            //----------------------------------------------------------------------------------выбор организма по щелчку ЛКМ
            public Organism? selectedObject = null;
            public void SelectTarget(Point selectedPoint)
            {
                for (int i = -3; i < 4; i++)
                {
                    for (int j = -3; j < 4; j++)
                    {
                        Point p = new Point(selectedPoint.X + i, selectedPoint.Y + j);
                        if (cellDictionary.ContainsKey(p))
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
                else {listBox.Items.Clear(); }
            }
            //----------------------------------------------------------------------------

            public void CreateZones_auto()//случаная местность
            {

            }
            public void CreateZones_manual(Point mousePoint, ZoneType activeType)//ручное создание зоны
            {
                //создавать зону типа activeType в точке курсора mousePoint
            }

            public string GetGenotype(Organism? selected)
            {
                string result = "";
                result += selected.genList.Count + "|";
                foreach (var item in selected.genList)
                {
                    result += item.part + "|" 
                        + item.localplace.X.ToString() + "|" 
                        + item.localplace.Y.ToString() + "|" 
                        + item.color.R.ToString()  + "|" 
                        + item.color.G.ToString() + "|" 
                        + item.color.B.ToString() + "|";
                }
                result += selected.dublicateAgeMax.ToString() + "|" 
                    + selected.dublicateAgeMin.ToString() + "|" 
                    + selected.dublicateDelayMax.ToString() + "|" 
                    + selected.dublicateFoodPrice.ToString() + "|"
                    + selected.maxage.ToString() + "|" 
                    + selected.maxfood.ToString() + "|";
                return result;
            }
        }
    }
}
