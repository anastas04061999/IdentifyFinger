using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
//static System.Windows.Forms.DataVisualization.Charting.Chart;

namespace Picture2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //dataGridSize();

            panel1.Visible = false;
            panel2.Visible = false;
            groupBox3.Visible = false;
            button5.Visible = false;
            groupBox5.Visible = false;
            menuStrip1.Items[1].Enabled = false;
            menuStrip1.Items[2].Enabled = false;
            menuStrip1.Items[3].Enabled = false;
            button5.Visible = false;

        }

        private Bitmap bmpOrigin;
        private Bitmap bmp;
        // bmpImage !ТОЛЬКО! для нормального отображения рисунка в pictureBox
        private Bitmap bmpImage;
        private Bitmap bmp2;


        private void dataGridSize()
        {
            dataGridView1.ColumnCount = 3;
            dataGridView1.RowCount = 3;
            dataGridView1.Enabled = true;

            dataGridView1[0, 0].Value = 1;
            dataGridView1[1, 0].Value = 2;
            dataGridView1[2, 0].Value = 1;
            dataGridView1[0, 1].Value = 2;
            dataGridView1[1, 1].Value = 4;
            dataGridView1[2, 1].Value = 2;
            dataGridView1[0, 2].Value = 1;
            dataGridView1[1, 2].Value = 2;
            dataGridView1[2, 2].Value = 1;
        }

        private void dataGridSize2()
        {
            dataGridView1.ColumnCount = 3;
            dataGridView1.RowCount = 3;
            dataGridView1.Enabled = true;

            dataGridView1[0, 0].Value = 1;
            dataGridView1[1, 0].Value = 1;
            dataGridView1[2, 0].Value = 1;
            dataGridView1[0, 1].Value = 1;
            dataGridView1[1, 1].Value = 1;
            dataGridView1[2, 1].Value = 1;
            dataGridView1[0, 2].Value = 1;
            dataGridView1[1, 2].Value = 1;
            dataGridView1[2, 2].Value = 1;
        }

        private int checkEnlightenment(int enlightenment, int k1)
        {
            if (enlightenment + k1 > 255)
            {
                enlightenment = 255;
                return enlightenment;
            }
            else if (enlightenment + k1 < 0)
            {
                enlightenment = 0;
                return enlightenment;
            }

            return enlightenment + k1;
        }

        private int ToGray(Bitmap bmp2, int i, int j)
        {
            // Извлекаем в R значение красного цвета 
            int R = bmp2.GetPixel(i, j).R;
            // Извлекаем в G значение зеленого цвета 
            int G = bmp2.GetPixel(i, j).G;
            // Извлекаем в B значение синего цвета 
            int B = bmp2.GetPixel(i, j).B;
            // Высчитываем среднее арифметическое 
            double GrayDouble = 0.2125 * R + 0.7154 * G + 0.0721 * B;
            int Gray = Convert.ToInt32(GrayDouble);
            return Gray;
        }

        private void Light()
        {
            if (bmp2 == null)
                bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);
            int k = Convert.ToInt32(textBox1.Text);
            // Циклы для перебора всех пикселей на изображении КРАСНЫЙ
            
            for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    // Извлекаем в R значение красного цвета 
                    int R = bmp2.GetPixel(i, j).R;
                    // Извлекаем в G значение зеленого цвета 
                    int G = bmp2.GetPixel(i, j).G;
                    // Извлекаем в B значение синего цвета 
                    int B = bmp2.GetPixel(i, j).B;
                    // Переводим число в значение цвета. 
                    // 255 – показывает степень прозрачности. 
                    R = checkEnlightenment(R, k);
                    G = checkEnlightenment(G, k);
                    B = checkEnlightenment(B, k);
                    Color p = Color.FromArgb(255, R, G, B);
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, p);
                }
                
            pictureBox2.Image = new Bitmap(bmp2, pictureBox2.Width, pictureBox2.Height); //перерисовка окна
        }

        private void Negative(int grayInt)
        {
            if (bmp2 == null)
                bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);
            // Циклы для перебора всех пикселей на изображении 
            for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    int Gray = ToGray(bmp2, i, j);
                    // Переводим число в значение цвета. 
                    // 255 – показывает степень прозрачности. 
                    // Остальные значения одинаковы 
                    //для перевода в градацию серого
                    Color p = Color.FromArgb(255, Gray, Gray, Gray); //255 - непрозрачный цвет

                    // Для негатива
                    if (grayInt == 255)
                    {
                        p = Color.FromArgb(255, grayInt - Gray, grayInt - Gray, grayInt - Gray);
                    }
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, p);
                }
            pictureBox2.Image = new Bitmap(bmp2, pictureBox2.Width, pictureBox2.Height); //перерисовка окна
        }

        private void Binary()
        {
            if (bmp2 == null)
                bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);
            int step = Convert.ToInt32(textBox2.Text);
            // Циклы для перебора всех пикселей на изображении 
            for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    int Gray = ToGray(bmp2, i, j);
                    // Переводим число в значение цвета. 
                    // 255 – показывает степень прозрачности. 
                    // Остальные значения одинаковы 
                    Color blackColor = Color.FromArgb(255, 0, 0, 0);
                    Color whiteColor = Color.FromArgb(255, 255, 255, 255);
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, Gray < step ? blackColor : whiteColor);
                }
            pictureBox2.Image = new Bitmap(bmp2, pictureBox2.Width, pictureBox2.Height); //перерисовка окна
        }

        private void BinaryLocal()
        {
            if (bmp2 == null)
                bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);
            int step = Convert.ToInt32(textBox2.Text);
            // Циклы для перебора всех пикселей на изображении 
            for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    int Gray = ToGray(bmp2, i, j);
                    // Переводим число в значение цвета. 
                    // 255 – показывает степень прозрачности. 
                    // Остальные значения одинаковы 
                    Color blackColor = Color.FromArgb(255, 0, 0, 0);
                    Color whiteColor = Color.FromArgb(255, 255, 255, 255);
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, Gray < step ? blackColor : whiteColor);
                }
            pictureBox2.Image = new Bitmap(bmp2, pictureBox2.Width, pictureBox2.Height); //перерисовка окна
        }

        private void psevdoColor()
        {
            if (bmp2 == null)
                bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);
            // Циклы для перебора всех пикселей на изображении КРАСНЫЙ
            for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    int Gray = ToGray(bmp2, i, j);
                    // Переводим число в значение цвета. 
                    // 255 – показывает степень прозрачности. 
                    Color p = Color.FromArgb(255, Gray,255, 255);
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, p);
                }
            // Вызываем функцию перерисовки окна 
            //Refresh();
            //pictureBox2.Image = bmpTemp;
            pictureBox2.Image = new Bitmap(bmp2, pictureBox2.Width, pictureBox2.Height); //перерисовка окна
        
        }

        private void DrawHisto(Bitmap bmpTemp, Chart MyChart)
        {
            MyChart.Series[0].Points.Clear();

            int[] Med = new int[256];
            for (int z = 0; z < 256; z++) Med[z] = 0;

            var res = new Bitmap(bmpTemp);
            for (int i = 0; i < res.Width; i++)
                for (int j = 0; j < res.Height; j++)
                {
                    int R = res.GetPixel(i, j).R;
                    int G = res.GetPixel(i, j).G;
                    int B = res.GetPixel(i, j).B;
                    double GrayDouble = 0.2125 * R + 0.7154 * G + 0.0721 * B;
                    int Gray = Convert.ToInt32(GrayDouble);

                    int b = 0;
                    while (b < 256)
                    {
                        if (Gray == b)
                        {
                            Med[b]++;
                            break;
                        }
                        else b++;
                    }

                }
          
            int max = 0;
            for (int i = 0; i < Med.Length; i++)
            {
                if (Med[i] > max) max = Med[i];
            }

            // Сместить ось OY на влево (чтобы было видно 0 значения)
            //chart1.Series[0].Points.AddXY(-1, 0);
            MyChart.ChartAreas[0].AxisX.Minimum = -1;
            MyChart.ChartAreas[0].AxisY.Minimum = 0;

            //можно удалить?
            MyChart.ChartAreas[0].AxisX.IsLabelAutoFit = false;

            //  Выключить сетку
            MyChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            MyChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            // Повернуть надпись значений на оси OX
            MyChart.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
            // Интервал по оси OY
            MyChart.ChartAreas[0].AxisY.Interval = max/10;
            // Интервал по оси OX
            MyChart.ChartAreas[0].AxisX.Interval = 16;
            // Максимум по оси OY 
            MyChart.ChartAreas[0].AxisY.Maximum = max + 100;
            // Максимум по оси OX
            MyChart.ChartAreas[0].AxisX.Maximum = 256;
            
            //chart1.ChartAreas[0].AxisY.Maximum = max + 100;

            int c = 0;
            while (c < 256)
            {
                
                MyChart.Series[0].Points.AddXY(c, Med[c]);
                //chart1.Series[0].Points.Add(Med[c]);
                c++;
            }
        }

        private void DrawEqualHisto(Bitmap bmpTemp)
        {
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox7.Clear();
            groupBox5.Visible = false;

            int[,] imageMas = new int[bmpTemp.Width, bmpTemp.Height];
            int[] Med = new int[256];
            double[] Ver = new double[256];
            double[] Rasp = new double[257]; //изменила
          //  for (int z = 0; z < 256; z++) Med[z] = 0;

            //Считаем пиксели дял первого изобр
            for (int i = 0; i < bmpTemp.Width; i++)
                for (int j = 0; j < bmpTemp.Height; j++)
                {
                    // Получаем значение цвета изображения
                    int colorInt = Convert.ToInt32(0.2125 * bmpTemp.GetPixel(i, j).R
                        + 0.7154 * bmpTemp.GetPixel(i, j).G
                        + 0.0721 * bmpTemp.GetPixel(i, j).B);
                    // прибавляем +1 к переменной в массиве для посчета
                    Med[colorInt]++;
                }

            double H = bmpTemp.Height;
            double W = bmpTemp.Width;

            for (int i=0; i<256; i++)
            {
                double p = Med[i] / (H * W);
                Ver[i] = Med[i] / (H * W); //Чтобы было дробное число, один из делителей должен быть double 
                textBox4.Text += Med[i].ToString() + Environment.NewLine;
            }

            for (int i = 0; i < 257; i++)
            {
                Rasp[i] = 0;
            }


            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    Rasp[i] += 255 * Ver[j]; //число - максимальаня интенсивность
                }
                Rasp[i] = Math.Round(Rasp[i]);
                
                textBox3.Text += i.ToString() + " - " +Rasp[i].ToString() + Environment.NewLine;
            }


            //Rasp[i] - новый i, элементы уже упорядочены
            //Вывод гистограммы
            int c = 0;
            int k = 1;
            while (c < 255)
            {
                //Если Rasp[i] имеет повторяющиеся значения, то нужно для этого значения 
                //интенсивности сложить соотсвествующие  Med[i]. Еще нужно перескочить эти повторяющиеся значения
                //Как это реализовать? 
                //Необходимо ввести 256ой элемент, чтобы на конечном значении интенсивности(255) работал нормально.
                for (int j = c; j < 256; j++)
                {
                    if (Rasp[j + 1] == Rasp[j])
                    {
                        Med[c] += Med[j + 1];
                        k++;
                        
                    }
                    else
                    {
                        chart2.Series[0].Points.AddXY(Rasp[c], Med[c]); 
                        //Rasp[c] - уровень интенсивности
                        textBox5.Text += Rasp[c].ToString() +" - " + Med[c].ToString() + Environment.NewLine;
                        c = c + k;
                        k = 1;
                        break;
                    } 
                }

            }

            //Переопределяю тип элементов массива и создаю новый
            int[] Rasp2 = new int[256];
            for (int i = 0; i < 256; i++)
            {
                Rasp2[i] = Convert.ToInt32(Rasp[i]);
                textBox7.Text += i.ToString() + " - " + Rasp2[i].ToString() + Environment.NewLine;
            }


            //double[] b = Med.Select(x => Convert.ToDouble(x)).ToArray(); - перевод тпиа массива, но сосздавтаь новый не вариант

            //Для некоторых изображение ругается при отражении массива в таблицу
            //dataGridView3.ColumnCount = bmp.Height;
            //dataGridView3.RowCount = bmp.Width;

            //Считаем пиксели дял первого изобр
            for (int i = 0; i < bmpTemp.Width; i++)
                for (int j = 0; j < bmpTemp.Height; j++)
                {
                    // Получаем значение цвета изображения
                   int colorInt2 = Convert.ToInt32(0.2125 * bmpTemp.GetPixel(i, j).R
                        + 0.7154 * bmpTemp.GetPixel(i, j).G
                        + 0.0721 * bmpTemp.GetPixel(i, j).B);
                    // прибавляем +1 к переменной в массиве для посчета
                    //Med[colorInt]++;
                    for (int intensive = 0; intensive < 256; intensive++)
                    {
                        if (colorInt2 == intensive)
                        {
                            colorInt2 = Rasp2[intensive];
                            imageMas[i, j] = colorInt2;
                            //dataGridView3.Rows[i].Cells[j].Value = imageMas[i, j];
                            break;
                        }
                    }

                }
   
                bmp2 = new Bitmap(bmp.Width, bmp.Height); //Начинаешь цвета устаналивать, отступив с начала, но зачем крайние с конца ячейки заполняешь?
                //ИЗМЕНИЛА ЗДЕСЬ!
                for (int i = 1; i < bmp2.Width; i++)
                    for (int j = 1; j < bmp2.Height; j++)
                    {
                    Color p = Color.FromArgb(255, imageMas[i, j], imageMas[i, j], imageMas[i, j]);
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, p);
                    }
            // Вызываем функцию перерисовки окна
            pictureBox2.Image = new Bitmap(bmp2, pictureBox2.Width, pictureBox2.Height);

            //Возвращение изображения через ее гистограмму
            //groupBox5.Visible = true;
            //Tabl(dataGridView2); 
        }

        private int[,] Extend(int [,] imageMas, int tempBmpWidth, int tempBmpHeight)
        {
            // 1....2
            // ......
            // 3....4

            //1 
            imageMas[0, 0] = imageMas[1, 1];
            //2
            imageMas[0, tempBmpHeight - 1] = imageMas[1, tempBmpHeight - 2];
            //3
            imageMas[tempBmpWidth - 1, 0] = imageMas[tempBmpWidth - 2, 1];
            //4
            imageMas[tempBmpWidth - 1, tempBmpHeight - 1] = imageMas[tempBmpWidth - 2, tempBmpHeight - 2];

            // .1111.
            // ......
            // ......
            for (int j = 1; j < bmp.Height + 1; j++)
                imageMas[0, j] = imageMas[1, j];

            // ......
            // ......
            // .1111.
            for (int j = 1; j < bmp.Height + 1; j++)
                imageMas[tempBmpWidth - 1, j] = imageMas[tempBmpWidth - 2, j];

            // ......
            // 1.....  
            // ......
            for (int i = 1; i < bmp.Width + 1; i++)
                imageMas[i, 0] = imageMas[i, 1];

            // ......
            // .....1
            // ......
            for (int i = 1; i < bmp.Width + 1; i++)
                imageMas[i, tempBmpHeight - 1] = imageMas[i, tempBmpHeight - 2];

            return imageMas;
        }

        private void Smoothing()
        {
            if (bmp2 == null)
                bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);

                int tempBmpWidth = bmp2.Width + 2;
                int tempBmpHeight = bmp2.Height + 2;
                int[,] imageMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];
                int[,] newMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];


                for (int i = 0; i < bmp2.Width; i++) 
                {
                    for (int j = 0; j < bmp2.Height; j++) 
                {
                        int Gray = ToGray(bmp2,i,j);
                        imageMas[i + 1, j + 1] = Gray;
                    }
                }

            Extend(imageMas, tempBmpWidth, tempBmpHeight);

                // Получаем новые значения с применения маски
                int t1 = Convert.ToInt32(dataGridView1[0, 0].Value);
                int t2 = Convert.ToInt32(dataGridView1[1, 0].Value);
                int t3 = Convert.ToInt32(dataGridView1[2, 0].Value);
                int t4 = Convert.ToInt32(dataGridView1[0, 1].Value);
                int t5 = Convert.ToInt32(dataGridView1[1, 1].Value);
                int t6 = Convert.ToInt32(dataGridView1[2, 1].Value);
                int t7 = Convert.ToInt32(dataGridView1[0, 2].Value);
                int t8 = Convert.ToInt32(dataGridView1[1, 2].Value);
                int t9 = Convert.ToInt32(dataGridView1[2, 2].Value);

                for (int i = 1; i < tempBmpWidth; i++)
                {
                    for (int j = 1; j < tempBmpHeight; j++)
                    {
                        newMas[i, j] = t1 * imageMas[i - 1, j - 1] +
                            t2 * imageMas[i - 1, j] +
                            t3 * imageMas[i - 1, j + 1] +
                            t4 * imageMas[i, j - 1] +
                            t5 * imageMas[i, j] +
                            t6 * imageMas[i, j + 1] +
                            t7 * imageMas[i + 1, j - 1] +
                            t8 * imageMas[i + 1, j] +
                            t9 * imageMas[i + 1, j + 1];
                        //тут добавила -1 -1
                        //12,10,20 - убрала
                        newMas[i, j] = Convert.ToInt32(newMas[i, j] / Convert.ToDouble(t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9));
                    }
            }


            //bmp2 = new Bitmap(bmp.Width + 1, bmp.Height + 1);
            bmp2 = new Bitmap(bmp.Width, bmp.Height);
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color p = Color.FromArgb(255, newMas[i+1, j+1], newMas[i+1, j+1], newMas[i+1, j+1]); //+1 12/10/2020
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, p);
                }
            // Вызываем функцию перерисовки окна
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            // Создаем и загружаем изображение в формате bmp 
            bmpImage = new Bitmap(bmp2, width, height);
            pictureBox2.Image = bmpImage;

        }

        private void Roberts()
        {
            int tempBmpWidth = bmp.Width + 2;
            int tempBmpHeight = bmp.Height + 2;
            int[,] imageMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];
            int[,] newMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];


            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int Gray = ToGray(bmp, i, j);
                    imageMas[i + 1, j + 1] = Gray;
                }
            }

            Extend(imageMas, tempBmpWidth, tempBmpHeight);


            for (int i = 1; i < tempBmpWidth; i++) //здесь разве не должны отпить по строче(столбцу) с каждой стороны?
            {
                for (int j = 1; j < tempBmpHeight; j++)
                {
                    newMas[i, j] = Math.Abs(imageMas[i, j] - imageMas[i, j]) + Math.Abs(imageMas[i + 1, j] - imageMas[i, j + 1]);
                    if (newMas[i, j] > 255)
                    {
                        newMas[i, j] = 255;
                    }
                    else if (newMas[i, j] < 0)
                    {
                        newMas[i, j] = 0;
                    }
                }
            }

            bmp2 = new Bitmap(bmp.Width, bmp.Height); //Начинаешь цвета устаналивать, отступив с начала, но зачем крайние с конца ячейки заполняешь?
            //ИЗМЕНИЛА ЗДЕСЬ!
            for (int i = 1; i < bmp2.Width; i++)
                for (int j = 1; j < bmp2.Height; j++)
                {
                    Color p = Color.FromArgb(255, newMas[i, j], newMas[i, j], newMas[i, j]);
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, p);
                }
           
            // Вызываем функцию перерисовки окна
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            // Создаем и загружаем изображение в формате bmp 
            bmpImage = new Bitmap(bmp2, width, height);
            pictureBox2.Image = bmpImage;
        }

        private void Sobel()
        {
            int tempBmpWidth = bmp.Width + 2;
            int tempBmpHeight = bmp.Height + 2;
            int[,] imageMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];
            int[,] newMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];


            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int Gray = ToGray(bmp, i, j);
                    imageMas[i + 1, j + 1] = Gray;
                }
            }

            Extend(imageMas, tempBmpWidth, tempBmpHeight); Extend(imageMas, tempBmpWidth, tempBmpHeight);


            for (int i = 1; i < tempBmpWidth; i++) //здесь разве не должны отпить по строче(столбцу) с каждой стороны?
            {
                for (int j = 1; j < tempBmpHeight; j++)
                {
                    int X = (imageMas[i - 1, j + 1] + 2 * imageMas[i, j + 1] + imageMas[i + 1, j + 1]) - (imageMas[i - 1, j - 1] + 2 * imageMas[i, j - 1] + imageMas[i + 1, j - 1]);
                    int Y = (imageMas[i - 1, j - 1] + 2 * imageMas[i-1, j] + imageMas[i - 1, j + 1]) - (imageMas[i + 1, j - 1] + 2 * imageMas[i+1, j] + imageMas[i + 1, j + 1]);
                    double XY = Math.Sqrt(X*X+Y*Y);
                    newMas[i, j] = Convert.ToInt32(XY);
                    if (newMas[i, j] > 255)
                    {
                        newMas[i, j] = 255;
                    }
                    else if (newMas[i, j] < 0)
                    {
                        newMas[i, j] = 0;
                    }

                    //newMas[i, j] = Math.Abs(imageMas[i, j] - imageMas[i+1, j+1]) + Math.Abs(imageMas[i + 1, j] - imageMas[i, j + 1]);
                }
            }

            bmp2 = new Bitmap(bmp.Width, bmp.Height); //Начинаешь цвета устаналивать, отступив с начала, но зачем крайние с конца ячейки заполняешь?
            //ИЗМЕНИЛА ЗДЕСЬ!
            for (int i = 1; i < bmp2.Width; i++)
                for (int j = 1; j < bmp2.Height; j++)
                {
                    Color p = Color.FromArgb(255, newMas[i, j], newMas[i, j], newMas[i, j]);
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, p);
                }
           
            // Вызываем функцию перерисовки окна
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            // Создаем и загружаем изображение в формате bmp 
            bmpImage = new Bitmap(bmp2, width, height);
            pictureBox2.Image = bmpImage;
        }

        private void kirsh()
        {
            int tempBmpWidth = bmp.Width + 2;
            int tempBmpHeight = bmp.Height + 2;
            int[,] imageMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];
            int[,] newMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int Gray = ToGray(bmp, i, j);
                    imageMas[i + 1, j + 1] = Gray;
                }
            }

            Extend(imageMas, tempBmpWidth, tempBmpHeight);

            for (int i = 1; i < tempBmpWidth; i++)
            {
                for (int j = 1; j < tempBmpHeight; j++)
                {
                    // Создаем новые массивы S и T
                    int[] S = new int[7];
                    int[] T = new int[7];
                    int[] A = new int[8];

                    // Заносим значения для A
                    A[0] = imageMas[i - 1, j - 1];
                    A[1] = imageMas[i - 1, j];
                    A[2] = imageMas[i - 1, j + 1];
                    A[3] = imageMas[i, j + 1];
                    A[4] = imageMas[i + 1, j + 1];
                    A[5] = imageMas[i + 1, j];
                    A[6] = imageMas[i + 1, j - 1];
                    A[7] = imageMas[i, j - 1];

                    // Считаем S и T
                    for (int k = 0; k < 7; k++)
                    {
                        S[k] = A[sumEight(k, 0)] + A[sumEight(k, 1)] + A[sumEight(k, 2)];
                        T[k] = A[sumEight(k, 3)] + A[sumEight(k, 4)] + A[sumEight(k, 5)] +
                            A[sumEight(k, 6)] + A[sumEight(k, 7)];
                    }

                    // Присваеваем максимуму Первый элемент
                    double max = Math.Abs(5 * S[0] - 3 * T[0]);
                    for (int k = 1; k < 7; k++)
                    {
                        int t = Math.Abs(5 * S[k] - 3 * T[k]);
                        if (max < t)
                            max = t;
                    }

                    newMas[i, j] = Convert.ToInt32(max);
                    if (newMas[i, j] > 255)
                    {
                        newMas[i, j] = 255;
                    }
                    else if (newMas[i, j] < 0)
                    {
                        newMas[i, j] = 0;
                    }

                }
            }

            bmp2 = new Bitmap(bmp.Width, bmp.Height);

            for (int i = 1; i < bmp2.Width; i++)
                for (int j = 1; j < bmp2.Height; j++)
                {
                    Color p = Color.FromArgb(255, newMas[i, j], newMas[i, j], newMas[i, j]);
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, p);
                }
            // Вызываем функцию перерисовки окна
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            // Создаем и загружаем изображение в формате bmp 
            bmpImage = new Bitmap(bmp2, width, height);
            pictureBox2.Image = bmpImage;
        }

        private int sumEight(int x, int y)
        {
            int sum = x + y;
            if (sum > 7)
                sum -= 8;
            return sum;
        }

        private void Laplas()
        {
            int tempBmpWidth = bmp.Width + 2;
            int tempBmpHeight = bmp.Height + 2;
            int[,] imageMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];
            int[,] newMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];


            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int Gray = ToGray(bmp, i, j);
                    imageMas[i + 1, j + 1] = Gray;
                }
            }

            Extend(imageMas, tempBmpWidth, tempBmpHeight);

            // Получаем новые значения с применения маски
            int t1 = -1;
            int t2 = -2;
            int t3 = -1;
            int t4 = -2;
            int t5 = 12;
            int t6 = -2;
            int t7 = -1;
            int t8 = -2;
            int t9 = -1;

            for (int i = 1; i < tempBmpWidth; i++) //здесь разве не должны отпить по строче(столбцу) с каждой стороны?
            {
                for (int j = 1; j < tempBmpHeight; j++)
                {
                    newMas[i, j] = t1 * imageMas[i - 1, j - 1] +
                        t2 * imageMas[i - 1, j] +
                        t3 * imageMas[i - 1, j + 1] +
                        t4 * imageMas[i, j - 1] +
                        t5 * imageMas[i, j] +
                        t6 * imageMas[i, j + 1] +
                        t7 * imageMas[i + 1, j - 1] +
                        t8 * imageMas[i + 1, j] +
                        t9 * imageMas[i + 1, j + 1];

                    if (newMas[i, j] > 255)
                    {
                        newMas[i, j] = 255;
                    }
                    else if (newMas[i, j] < 0)
                    {
                        newMas[i, j] = 0;
                    }
                    //int sum = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9;
                    //newMas[i, j] = Convert.ToInt32(newMas[i, j] / sum);
                }
            }

            bmp2 = new Bitmap(bmp.Width, bmp.Height); //Начинаешь цвета устаналивать, отступив с начала, но зачем крайние с конца ячейки заполняешь?
            //ИЗМЕНИЛА ЗДЕСЬ!
            for (int i = 1; i < bmp2.Width; i++)
                for (int j = 1; j < bmp2.Height; j++)
                {
                    Color p = Color.FromArgb(255, newMas[i, j], newMas[i, j], newMas[i, j]);
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, p);
                }

            // Вызываем функцию перерисовки окна
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            // Создаем и загружаем изображение в формате bmp 
            bmpImage = new Bitmap(bmp2, width, height);
            pictureBox2.Image = bmpImage;
        }

        private void Tabl(DataGridView data)
        {

            int[,] imageMas = new int [bmp.Width, bmp.Height];

            // Вывод исходного изображения в таблицу
            data.ColumnCount = bmp.Height;
            data.RowCount = bmp.Width;

            /*
            // Вывод измененного схображения в таблицу
             dataGridView3.ColumnCount = bmp.Height;
            dataGridView3.RowCount = bmp.Width;
            */

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int R = bmp.GetPixel(i, j).R;
                    int G = bmp.GetPixel(i, j).G;
                    int B = bmp.GetPixel(i, j).B;
                    double GrayDouble = 0.2125 * R + 0.7154 * G + 0.0721 * B;
                    int Gray = Convert.ToInt32(GrayDouble);
                    imageMas[i, j] = Gray;
                    data.Rows[i].Cells[j].Value = imageMas[i, j];
                }
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
                // Описываем объект класса OpenFileDialog
                OpenFileDialog dialog = new OpenFileDialog();
                // Задаем расширения файлов  
                dialog.Filter = "Image files (*.BMP, *.JPG, " +
                "*.GIF, *.PNG)|*.bmp;*.jpg;*.gif;*.png";
                // Вызываем диалог и проверяем выбран ли файл 
                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    // Загружаем изображение из выбранного файла 
                    Image image = Image.FromFile(dialog.FileName);
                    int width = pictureBox1.Width;
                    int height = pictureBox1.Height;
                    int width2 = pictureBox2.Width;
                    int height2 = pictureBox2.Height;


                    // Создаем и загружаем изображение в формате bmp
                    // bmpImage — в него заносим копию изображения, которую будем показывать
                    bmpImage = new Bitmap(image, width, height);
                    bmp2 = new Bitmap(image, width2, height2);
                    // Переманная с изображением, с которой будем работать
                    bmp = new Bitmap(image, image.Width, image.Height);
                    // Оригинальное изображение, которое НЕИЗМЕНЯЕТСЯ!!! Изменять его запрещено
                    bmpOrigin = new Bitmap(bmp, bmp.Width, bmp.Height);
                    // Записываем изображение в pictureBox1 
                    pictureBox1.Image = bmpImage;

                    menuStrip1.Items[1].Enabled = true;
                    menuStrip1.Items[2].Enabled = true;
                    menuStrip1.Items[3].Enabled = true;

                }
                bmp2 = null;
                if (pictureBox2.Image != null) pictureBox2.Image = null;
                groupBox3.Visible = false;
                chart1.Series[0].Points.Clear();

            }
            catch
            {
                MessageBox.Show("Что-то пошло не так...", "Ошибка");
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null) MessageBox.Show("Откройте изображение", "Предупреждение");
                else
                {
                    if (pictureBox2.Image == null) MessageBox.Show("Измените изображение", "Предупреждение");
                    else
                    { 
                    // Описываем и порождаем объект savedialog 
                    SaveFileDialog savedialog = new SaveFileDialog();
                    // Задаем свойства для savedialog 
                    savedialog.Title = "Сохранить картинку как ...";
                    savedialog.OverwritePrompt = true;
                    savedialog.CheckPathExists = true;
                    savedialog.Filter =
                    "Bitmap File(*.bmp)|*.bmp|" +
                    "GIF File(*.gif)|*.gif|" +
                    "JPEG File(*.jpg)|*.jpg|" +
                    "PNG File(*.png)|*.png";
                    // Показываем диалог и проверяем задано ли имя файла 
                    if (savedialog.ShowDialog() == DialogResult.OK)
                        {
                        string fileName = savedialog.FileName;
                        // Убираем из имени расширение файла 
                        string strFilExtn = fileName.Remove(0, fileName.Length - 3);
                        // Сохраняем файл в нужном формате 
                        switch (strFilExtn)
                            {
                            case "bmp":
                                bmp2.Save(fileName,
                                System.Drawing.Imaging.ImageFormat.Bmp);
                                break;
                            case "jpg":
                                bmp2.Save(fileName,
                                System.Drawing.Imaging.ImageFormat.Jpeg);
                                break;
                            case "gif":
                                bmp2.Save(fileName,
                                System.Drawing.Imaging.ImageFormat.Gif);
                                break;
                            case "tif":
                                bmp2.Save(fileName,
                                System.Drawing.Imaging.ImageFormat.Tiff);
                                break;
                            case "png":
                                bmp2.Save(fileName,
                                System.Drawing.Imaging.ImageFormat.Png);
                                break;
                            default:
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Измените изображение перед его сохранением", "Предупреждение");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, если введенное значение выходит за рамки, 
                // то останавливаем выполнение и выводим сообщение
                int text = Convert.ToInt32(textBox1.Text);
                if (text < 1 || text > 255)
                {
                    //textBox1.Text = "150";
                    MessageBox.Show("Введите значение от 1 до 255", "Предупреждение");
                }
                else
                {
                    // Функиция просветления изображения
                    Light();
                }
            }
            catch
            {
                errorMessage2();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Negative(255);
            }
            catch
            {
                errorMessage2();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Если введеное значение выходит за рамки, 
                // то выводим соотвествующее сообщение
                int text = Convert.ToInt32(textBox2.Text);
                if (text < -255 || text > 255)
                {
                    //textBox2.Text = "10";
                    MessageBox.Show("Введите значение от -255 до 255", "Предупреждение");
                }
                else
                {
                    // Функиция бинаризации
                    Binary();
                }
            }
            catch
            {
                errorMessage2();
            }
            
        }

        private void точечныеМетодыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            groupBox3.Visible = false;
            button5.Visible = true;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                psevdoColor();
            }
            catch
            {
                errorMessage2();
            }
        }

        private void button5_Click(object sender, EventArgs e) //Вернуться к исходному
        {
            bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);
            pictureBox2.Image = new Bitmap(bmp2, pictureBox2.Width, pictureBox2.Height);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Laplas();
            }
            catch
            {
                errorMessage2();
            }
        }

        private void пРострансвенныеМетодыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button5.Visible = true;
            panel1.Visible =false;
            panel2.Visible = true;
            groupBox3.Visible = false;
            dataGridView1.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridSize();

                Smoothing();
            }
            catch
            {
                errorMessage2();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                kirsh();
            }
            catch
            {
                errorMessage2();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                Roberts();
            }
            catch
            {
                errorMessage2();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                Sobel();
            }
            catch
            {
                errorMessage2();
            } 
        }

        private void гистограммноеПреобразованиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                button5.Visible = true;
                groupBox3.Visible = true;
                chart1.Visible = true;
                chart2.Visible = false;
                label10.Visible = false;
                DrawHisto(bmp, chart1); 
            }
            catch
            {
                errorMessage();
            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                chart2.Visible = true;
                label10.Visible = true;
                chart2.Series[0].Points.Clear();
                DrawEqualHisto(bmp);
            }
            catch
            {
                errorMessage();
            }
    }


        private void errorMessage()
        {
            if (bmp != null)
                MessageBox.Show("Ошибка при построении гистаграммы", "Ошибка");
            else
                MessageBox.Show("Загрузите изображение", "Предупреждение");
        }

        private void errorMessage2()
        {
            if (bmp != null)
                MessageBox.Show("При применении фильтров произошла ошибка", "Ошибка");
            else
                MessageBox.Show("Сначала нужно загрузить изображение", "Предупреждение");
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && (number != 8) && (number != 45))
            {
                e.Handled = true;
            }
        }


        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(Cell_KeyPress);
        }

        private void Cell_KeyPress(object Sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.KeyChar = Convert.ToChar("\0");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridSize2();
                Smoothing();
            }
            catch
            {
                errorMessage2();
            }
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (bmp2 == null)
                bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);

            int[,] imageMas = new int[bmp2.Width, bmp2.Height];
            int[,] newMas = new int[bmp2.Width, bmp2.Height];


            // Циклы для перебора всех пикселей на изображении 
            for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    int Gray = ToGray(bmp2, i, j);
                    imageMas[i, j] = Gray;
                }
        

            int maxint = 0;
            int minint = 0;


            int r = Convert.ToInt32(textBox6.Text);
            //int r = 5;
            int k = bmp2.Width / r;

            //int r1 = bmp2.Width % k;
            int r1 = r*k;

            r1 = bmp2.Width - r1;
            
            for (int i = 0; i < r*k; i++)
            {
                int n = bmp2.Height/r;
                for (int j = 0; j < r*n; j++)
                {

                    maxint = imageMas[i, j];
                    minint = imageMas[i, j];

                    for (int u = 0; u < r; u++) //10
                    {
                        for (int v = 0; v < r; v++) //10
                        {
                            if (imageMas[i + u, j+v] > maxint) maxint = imageMas[i + u, j + v];
                            else if (imageMas[i + u, j + v] < minint) minint = imageMas[i + u, j + v];
                        }
                    }

                    double step2 = 1 / 2.0 * (maxint+minint);

                    for (int u = 0; u < r; u++) //10
                    {
                        for (int v = 0; v < r; v++) //10

                        {

                            if (imageMas[i + u, j + v] < step2) newMas[i + u, j + v] = 0;
                            else newMas[i + u, j + v] = 255;

                        }
                    }

                    j = j + r-1;
                }

                i = i + r-1;
            }

            //bmp2 = new Bitmap(bmp.Width + 1, bmp.Height + 1);
            bmp2 = new Bitmap(bmp.Width, bmp.Height);
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color p = Color.FromArgb(255, newMas[i, j], newMas[i, j], newMas[i, j]); //+1 12/10/2020
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, p);
                }
            // Вызываем функцию перерисовки окна
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            // Создаем и загружаем изображение в формате bmp 
            bmpImage = new Bitmap(bmp2, width, height);
            pictureBox2.Image = bmpImage;


            BinaryLocal();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (bmp2 == null)
                bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);

            int[,] imageMas = new int[bmp2.Width, bmp2.Height];
            int[,] newMas = new int[bmp2.Width, bmp2.Height];


            // Циклы для перебора всех пикселей на изображении 
            for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    int Gray = ToGray(bmp2, i, j);
                    imageMas[i, j] = Gray;
                }


            int maxint = imageMas[0, 0];
            int minint = imageMas[0, 0];

            for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    if (imageMas[i, j] > maxint) maxint = imageMas[i, j];
                        else if (imageMas[i, j] < minint) minint = imageMas[i, j];
                }

            int sred = (maxint - minint) / 2;
            int sum = 0;

            for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    if ((imageMas[i, j] >= minint)&&(imageMas[i, j] <=sred )) sum=sum+1;
 
                }

            int step =  sum / (sred-minint+1);


                for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    int Gray = ToGray(bmp2, i, j);
                    // Переводим число в значение цвета. 
                    // 255 – показывает степень прозрачности. 
                    // Остальные значения одинаковы 
                    Color blackColor = Color.FromArgb(255, 0, 0, 0);
                    Color whiteColor = Color.FromArgb(255, 255, 255, 255);
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, Gray < step ? blackColor : whiteColor);
                }
            pictureBox2.Image = new Bitmap(bmp2, pictureBox2.Width, pictureBox2.Height); //перерисовка окна

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (bmp2 == null)
                bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);

            //одномерный массив 
            int[] image = new int[bmp2.Width * bmp2.Height];
            int k = 0;

            for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    int Gray = ToGray(bmp2, i, j);
                    image[k] = Gray;
                    k++;  
                }

                  
            // Посчитаем минимальную и максимальную яркость всех пикселей
            int min = image[0];
            int max = image[0];

            for (int i = 1; i < image.Length; i++)
            {
                if (image[i] < min)
                    min = image[i];

                if (image[i] > max)
                    max = image[i];
            }

            // Гистограмма будет ограничена снизу и сверху значениями min и max,
            // поэтому нет смысла создавать гистограмму размером 256 бинов
            int histSize = max - min + 1;
            int[] hist = new int[histSize];

            // Заполним гистограмму нулями
            for (int i = 0; i < histSize; i++)
                hist[i] = 0;

            // И вычислим высоту бинов
            for (int i = 0; i < image.Length; i++)
                hist[image[i] - min]++;

            // Введем два вспомогательных числа:
            int m = 0; // m - сумма высот всех бинов, домноженных на положение их середины
            int n = 0; // n - сумма высот всех бинов
            for (int t = 0; t <= max - min; t++)
            {
                m += t * hist[t];
                n += hist[t];
            }

            float maxSigma = -1; // Максимальное значение межклассовой дисперсии
            int threshold = 0; // Порог, соответствующий maxSigma

            int alpha1 = 0; // Сумма высот всех бинов для класса 1
            int beta1 = 0; // Сумма высот всех бинов для класса 1, домноженных на положение их середины

            // Переменная alpha2 не нужна, т.к. она равна m - alpha1
            // Переменная beta2 не нужна, т.к. она равна n - alpha1

            // t пробегается по всем возможным значениям порога
            for (int t = 0; t < max - min; t++)
            {
                alpha1 += t * hist[t];
                beta1 += hist[t];

                // Считаем вероятность класса 1.
                float w1 = (float)beta1 / n;
                // Нетрудно догадаться, что w2 тоже не нужна, т.к. она равна 1 - w1

                // a = a1 - a2, где a1, a2 - средние арифметические для классов 1 и 2
                float a = (float)alpha1 / beta1 - (float)(m - alpha1) / (n - beta1);

                // Наконец, считаем sigma
                float sigma = w1 * (1 - w1) * a * a;

                // Если sigma больше текущей максимальной, то обновляем maxSigma и порог
                if (sigma > maxSigma)
                {
                    maxSigma = sigma;
                    threshold = t;
                }
            }

            // Не забудем, что порог отсчитывался от min, а не от нуля
            threshold += min;
            textBox2.Text = threshold.ToString();

            for (int i = 0; i < bmp2.Width; i++)
                for (int j = 0; j < bmp2.Height; j++)
                {
                    int Gray = ToGray(bmp2, i, j);
                    // Переводим число в значение цвета. 
                    // 255 – показывает степень прозрачности. 
                    // Остальные значения одинаковы 
                    Color blackColor = Color.FromArgb(255, 0, 0, 0);
                    Color whiteColor = Color.FromArgb(255, 255, 255, 255);
                    // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, Gray <=  threshold ? blackColor : whiteColor);
                }
            pictureBox2.Image = new Bitmap(bmp2, pictureBox2.Width, pictureBox2.Height); //перерисовка окна

        }

        private void Mediana()
        {
            if (bmp2 == null)
                bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);

            int tempBmpWidth = bmp2.Width + 2;
            int tempBmpHeight = bmp2.Height + 2;
            int[,] imageMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];
            int[,] newMas = new int[tempBmpWidth + 1, tempBmpHeight + 1];


            for (int i = 0; i < bmp2.Width; i++)
            {
                for (int j = 0; j < bmp2.Height; j++)
                {
                    int Gray = ToGray(bmp2, i, j);
                    imageMas[i + 1, j + 1] = Gray;
                }
            }

            Extend(imageMas, tempBmpWidth, tempBmpHeight);

            int[] nums = new int[9];

            for (int i = 5; i < tempBmpWidth; i++)
            {
                for (int j = 5; j < tempBmpHeight; j++)
                {
                    nums[0] = imageMas[i - 1, j - 1];
                    nums[1] = imageMas[i - 1, j];
                    nums[2] = imageMas[i - 1, j + 1];
                    nums[3] = imageMas[i, j - 1];
                    nums[4] = imageMas[i, j];
                    nums[5] = imageMas[i, j + 1];
                    nums[6] = imageMas[i + 1, j - 1];
                    nums[7] = imageMas[i + 1, j];
                    nums[8] = imageMas[i + 1, j + 1];


                    // сортировка
                    int temp = 0;
                    for (int n = 0; n < nums.Length - 1; n++)
                    {
                        for (int m = i + 1; m < nums.Length; m++)
                        {
                            if (nums[n] > nums[m])
                            {
                                temp = nums[n];
                                nums[n] = nums[m];
                                nums[m] = temp;
                            }
                        }
                    }

                    newMas[i, j] = nums[4];
                }
            }



            //bmp2 = new Bitmap(bmp.Width + 1, bmp.Height + 1);
            bmp2 = new Bitmap(bmp.Width, bmp.Height);
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color p = Color.FromArgb(255, newMas[i + 1, j + 1], newMas[i + 1, j + 1], newMas[i + 1, j + 1]); //+1 12/10/2020
                                                                                                                     // Записываем цвет в текущую точку 
                    bmp2.SetPixel(i, j, p);
                }


            // Вызываем функцию перерисовки окна
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            // Создаем и загружаем изображение в формате bmp 
            bmpImage = new Bitmap(bmp2, width, height);
            pictureBox2.Image = bmpImage;
        }

            private void button15_Click(object sender, EventArgs e)
        {

            //Mediana();
          
            //Записать в переменную изображение из файла
            Image<Bgr, Byte> img1 = new Image<Bgr, Byte>("C:\\Users\\Анастасия\\Desktop\\Диплом\\Обработка изобржений_1\\Изображения_лаба1\\Картинки\\1.png");

            Image<Bgr, Byte> img2 = new Image<Bgr, Byte>("C:\\Users\\Анастасия\\Desktop\\Диплом\\Обработка изобржений_1\\Изображения_лаба1\\Картинки\\2.png");
            //Image<Bgr, Byte> img3 = img2 - img1; //Так разница не работает, быть может через функцию

            imageBox1.Image = img2;

            /*
            
            bmpImage = new Bitmap(bmp, pictureBox1.Width, pictureBox1.Height);
            Image<Bgr, Byte> myImage = new Image<Bgr, Byte>(new Bitmap(bmp, pictureBox1.Width, pictureBox1.Height));
            imageBox1.Image = myImage;
            */


            // ошибка  - область растрового изображения заблокирвоана(уже используется чем-то)
            //Для версии 4,3
            //var img2 = new Image<Bgr, Byte>((Bitmap)pictureBox1.Image);
            //imageBox1.Image = img2;

            //Как преобразовать ImageBox.Image в изображение<Bgr, Byte>
            //Image<Bgr, Byte> imgeOrigenal = new Image<Bgr, Byte>(ImageBoxbOrigenal.Image.Bitmap);

            /*
             Добавление a Bitmap в ImageBox действительно приведет к следующей ошибке
            Mat matImage = capture.QueryFrame();
            CamImageBox.Image = matImage; // Directly show Mat object in *ImageBox*
            Image<Bgr, byte> iplImage = matImage.ToImage<Bgr, byte>();
            CamImageBox.Image = iplImage; // Show Image<,> object in *ImageBox*
*/



        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog()==DialogResult.OK)
                {
                    Image<Bgr, byte> imgInput = new Image<Bgr, byte>(ofd.FileName);
                    pictureBox3.Image = imgInput.Bitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
