using System;
using System.Windows.Forms;

namespace лаба_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e) 
        {
            int matrixSize, maxValueNumberMatrix, minValueNumberMatrix; //обьявляем 3 целочисленные переменные

            Random r = new Random(); //создаем экземпляр класса Random

            //считываем значения из numericUpDown с формы
            maxValueNumberMatrix = Convert.ToInt32(numericUpDown7.Value);  //максимальное значение элементов в массиве
            minValueNumberMatrix = Convert.ToInt32(numericUpDown8.Value);  //минимальное значение элементов в массиве
            matrixSize = Convert.ToInt32(numericUpDown6.Value); //длина массива 

            int[,] M = new int[matrixSize,matrixSize]; //инциализируем двумерный массив, квадратная матрица размерностью matrixSize

            for (int i = 0; i < matrixSize; i++) //обход по строкам массива
            {
                for (int j = 0; j < matrixSize; j++) //обход по столбцам массива
                {
                    M[i, j] = r.Next(minValueNumberMatrix, maxValueNumberMatrix); //присваивание каждому элементу массива случайное значение
                }
            }

            dataGridView2.RowCount = M.GetLength(0); //задаём элементу формы DataGridView количество строк
            dataGridView2.ColumnCount = M.GetLength(1); //задаём элементу формы DataGridView количество столбцов

            for (int i = 0; i < M.GetLength(0); i++) //обход по строкам
            {
                for (int j = 0; j < M.GetLength(1); j++) //обход по столбцам
                {
                    dataGridView2
                        .Rows[i]
                        .Cells[j]
                        .Value = M[i, j]; //присваивание каждой ячейке DataGridView значение из массива
                }
            }

            int[,] N; //обьявление двумерного массива
            int maxNegativeCount1 = Convert.ToInt32(numericUpDown9.Value); //считываем с формы число по условию

            int CountNegativeElementsInColumn(int[,] A, int column, int row = 0) //рекурсивный метод по нахождению отритательных чисел в каждой строчке массива
            {
                if (row >= A.GetLength(0)) //работа в пределе массива
                    return 0;

                if (A[row, column] < 0)  //если число отрицательное
                    return CountNegativeElementsInColumn(A, column, row + 1) + 1; //вызываем этот же метод, но переходим к следующей строке и инкрементируем кол-во отрицательных чисел

                else //если число положительное
                    return CountNegativeElementsInColumn(A, column, row + 1);
            }

            void CopyColumn(int[,] source, int columnFrom, int[,] target, int columnTo, int row = 0) //метод по переносу столбцов
            {
                if (row >= source.GetLength(0))
                    return;

                target[row, columnTo] = source[row, columnFrom];
                CopyColumn(source, columnFrom, target, columnTo, row + 1);
            }

            int[,] DeleteSomeColumn(int[,] A, int maxNegativeCount, int column = 0, int savedColumnsCount = 0) //метод по удалению некоторого столбца
            {
                if (column >= A.GetLength(1))
                    return new int[A.GetLength(0), savedColumnsCount + 1];

                if (CountNegativeElementsInColumn(A, column) > maxNegativeCount)
                    return DeleteSomeColumn(A, maxNegativeCount, column + 1, savedColumnsCount);

                int[,] newArray = DeleteSomeColumn(A, maxNegativeCount, column + 1, savedColumnsCount + 1);
                CopyColumn(A, column, newArray, savedColumnsCount + 1);

                return newArray;
            }

            int MaxElementInRow(int[,] A, int row, int column = 0) //метод по нахождению максимального элемента в строках
            {
                if (column == A.GetLength(1) - 1)
                    return A[row, column];

                int tempMax = MaxElementInRow(A, row, column + 1);

                if (tempMax > A[row, column])
                    return tempMax;

                else return A[row, column];
            }

            void FindMaxInRows(int[,] A, int row = 0) //запись в первый столбец максимальных элементов
            {
                if (row >= A.GetLength(0))
                    return;

                A[row, 0] = MaxElementInRow(A, row);

                FindMaxInRows(A, row + 1);
            }

            int[,] Solve(int[,] A, int maxNegativeCount) //метод решения
            {
                int[,] result = DeleteSomeColumn(A, maxNegativeCount);

                FindMaxInRows(result);

                return result;
            }
           
            N = Solve(M, maxNegativeCount1);

            dataGridView3.RowCount = N.GetLength(0);
            dataGridView3.ColumnCount = N.GetLength(1);

            for (int i = 0; i < N.GetLength(0); i++)
            {
                for (int j = 0; j < N.GetLength(1); j++)
                {
                    dataGridView3
                        .Rows[i]
                        .Cells[j]
                        .Value = N[i, j]; //запись обработаного массива в другой DataGridView

                }
            }
        }

        private void button4_Click(object sender, EventArgs e) //кнопка сброса
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView3.Rows.Clear();
            dataGridView3.Columns.Clear();
            toolStripStatusLabel1.Text = "";
            button3.Enabled = true;
            numericUpDown6.Value = 1;
            numericUpDown8.Value = 0;
            numericUpDown7.Value = 1;
            numericUpDown9.Value = 1;
        }

        private void button5_Click(object sender, EventArgs e) //кнопка заполнить для тестирования
        {
            Random r = new Random();
            numericUpDown6.Value = r.Next(5,10);
            numericUpDown8.Value = r.Next(-5, -1);
            numericUpDown7.Value = r.Next(4, 10);
            numericUpDown9.Value = r.Next(1, 5);
        }
    }
}
