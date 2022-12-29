using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace lab3_PersonalTask
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e) //кнопка заполнить все для тестирования
        {
            Random r = new Random(); //экземпляр класса генератора псевдослучайных чисел

            numericUpDown1.Value = r.Next(1, 10); 
            numericUpDown2.Value = r.Next(10, 100);
            numericUpDown3.Value = r.Next(1, 10); 
            numericUpDown4.Value = r.Next(0, 10);
            numericUpDown5.Value = r.Next(10, 100); 
        }

        private void button1_Click(object sender, EventArgs e) //кнопка начать расчёт
        {
            int minSize, maxSize, minRange, maxRange, countCompare, countSwap, countOperation;

            minSize = ((int)numericUpDown1.Value); //минимальный размер массива
            maxSize = ((int)numericUpDown2.Value); //максимальный размер массива
            minRange = ((int)numericUpDown4.Value); //диапазон чисел (минимальное значение)
            maxRange = ((int)numericUpDown5.Value); //диапазон чисел (максимальное значение)

            dataGridView1.ColumnCount = 3; //кол-во столбцов в DataGridView

            //присваивание каждой колонке название
            dataGridView1.Columns[0].HeaderText = "Размер";  
            dataGridView1.Columns[1].HeaderText = "Пузырек";
            dataGridView1.Columns[2].HeaderText = "Вставки";

            dataGridView1.AllowUserToAddRows = false; //скрытие параметра добавления строк для пользователя

            Random r = new Random();
            int k = 0;

            label7.Text = "Количество операций";
            label6.Text = "Длина входного массива";
            
            //цикл увеличивает размерность массива
            for (;minSize < maxSize; minSize += (int)numericUpDown3.Value) 
            {
                dataGridView2.RowCount++; //добавляем строку в DataGridView
                int[] numbers = new int[minSize]; //исходный массив
                int[] tempBubbleSort = new int[minSize]; //масив обработанный сортировкой пузырьком
                int[] tempSelectionSort = new int[minSize]; //масив обработанный сортировкой выборкой
                int[] tempQuickSort = new int[minSize]; //масив обработанный сортировкой быстрой


                for (int i = 0; i < minSize; i++)
                    numbers[i] = r.Next(minRange, maxRange); //заполнение случайными числами исходного массива

                Sort Sorting = new Sort(); //создаем экземпляр класса Sort для дальнейшего использования его методов

                //копируем в каждый массив сортировок значения из исходного массива 
                for (int i = 0; i < numbers.Length; i++)
                {
                    tempBubbleSort[i] = numbers[i];
                    tempSelectionSort[i] = numbers[i];
                    tempQuickSort[i] = numbers[i];
                }
                //кол-во сравнений, кол-во обменов, кол-во всех операций
                countCompare = 0; countSwap = 0; countOperation = 0;
                //вывод исходного массива
                OutputOriginalArray(ref numbers);
                //сортировка пузырьком
                Sorting.BubbleSort(ref tempBubbleSort, tempBubbleSort.Length, ref countCompare, ref countSwap, ref countOperation);
                //вывод отсортированного пузырьком массива
                OutputSortedArray(ref tempBubbleSort, "Пузырёк");

                //работа с графиками
                chart1.Series[1].Points.AddXY(minSize, countSwap); //добавляем на график точку с координатами (minSize, countSwap)
                chart1.Series[0].Points.AddXY(minSize, countCompare); //вывод на график кол-ва сравнений
                chart2.Series[0].Points.AddXY(minSize, countCompare + countSwap * 3); //вывод на график кол-во всех операций
                
                dataGridView2.Rows[k].Cells[1].Value = countCompare + countSwap * 3; //вывод в DataGridView кол-во всех операций
                dataGridView2.Rows[k].Cells[0].Value = tempBubbleSort.Length; //вывод в DataGridView

                dataGridView1.Rows.Add(); //добавление строки в DataGridView
                dataGridView1.Rows[k].Cells[0].Value = minSize; //запись в DataGridView размера массива
                dataGridView1.Rows[k].Cells[1].Value = countSwap; //запись в DataGridView кол-во обменов
     
                textBox1.Text += Environment.NewLine + Environment.NewLine; //добавление строки в TextBox


                //аналогично с остальными сортировками
                countCompare = 0; countSwap = 0; countOperation = 0;
                OutputOriginalArray(ref numbers);
                Sorting.SelectionSort(ref tempSelectionSort, 0, ref countCompare, ref countSwap, ref countOperation);
                OutputSortedArray(ref tempSelectionSort, "Выбором");

                chart3.Series[1].Points.AddXY(minSize, countSwap);
                chart3.Series[0].Points.AddXY(minSize, countCompare);
                chart2.Series[1].Points.AddXY(minSize, countCompare + countSwap * 3);

                dataGridView2.Rows.Add();
                dataGridView2.Rows[k].Cells[2].Value = countCompare + countSwap * 3;
                dataGridView2.Rows[k].Cells[3].Value = Math.Round((Convert.ToDouble(dataGridView2.Rows[k].Cells[1].Value) / Convert.ToDouble(dataGridView2.Rows[k].Cells[2].Value)), 2);

                dataGridView1.Rows[k].Cells[2].Value = countSwap;

                textBox1.Text += Environment.NewLine + Environment.NewLine;

                OutputOriginalArray(ref numbers);
                Sorting.QuickSort(ref tempQuickSort, 0, tempQuickSort.Length - 1);
                OutputSortedArray(ref tempQuickSort, "Быстрая");

                textBox1.Text += Environment.NewLine + Environment.NewLine;
                dataGridView2.RowCount++;
                k++; 
            }
            dataGridView2.RowCount = dataGridView2.RowCount / 2 - 3;
        }

        private void OutputOriginalArray(ref int[] array) //вывод исходного массива
        {
            textBox1.Text += "Исходный массив: ";
            for (int i = 0; i < array.Length; i++)
                textBox1.Text += $"{array[i]} ";  
        }
        
        private void OutputSortedArray(ref int[] sortedArray, string typeSort) //вывод отсортированного массива
        {
            textBox1.Text += Environment.NewLine;
            textBox1.Text += $"{typeSort} отсортированный массив: ";
            for (int i = 0; i < sortedArray.Length; i++)
                textBox1.Text += $"{sortedArray[i]} ";
        }
       
        private void button2_Click(object sender, EventArgs e) //кнопка очистить форму
        {
            textBox1.Text = "";
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView1.RowCount = 1;
            dataGridView2.RowCount = 1;
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart3.Series[1].Points.Clear();
            label6.Text = " ";
            label7.Text = " ";

        } 

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e) //кнопка справки
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e) //кнопка сохранить график 1
        {
            SaveToImg(chart1);
            MessageBox.Show("Успешно");
        }

        private void button5_Click(object sender, EventArgs e) //кнопка сохранить график 2
        {
            SaveToImg(chart3);
            MessageBox.Show("Успешно");
        }

        private void button6_Click(object sender, EventArgs e) //кнопка сохранить график 3
        {
            SaveToImg(chart2);
            MessageBox.Show("Успешно");
        }
        private void сохранитьВсеГрафикиToolStripMenuItem_Click(object sender, EventArgs e) //кнопка сохранить все графики
        {
            SaveToImg(chart1);
            SaveToImg(chart2);
            SaveToImg(chart3);
            MessageBox.Show("Успешно");
        }
        private static void SaveToImg(Chart chart) //сохранение графика картинкой
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Сохранить изображение как ...";
                sfd.Filter = "*.bmp|*.bmp;|*.png|*.png;|*.jpg|*.jpg";
                sfd.AddExtension = true;
                sfd.FileName = "graphicImage";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    switch (sfd.FilterIndex)
                    {
                        case 1: chart.SaveImage(sfd.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Bmp); break;
                        case 2: chart.SaveImage(sfd.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png); break;
                        case 3: chart.SaveImage(sfd.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Jpeg); break;
                    }
                }
            }
        }


    }
    }

  
