using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace lab2_PersonalTask
{
    //для простенькой стилизации возьмем некоторые элементы формы от MetroForm
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) //выполняется при загрузке нашей формы
        {
            ContextMenu contextMenu = new ContextMenu(); //создание контекстного меню

            MenuItem openFile = new MenuItem(); // кнопка в контексном меню - открыть файл
            MenuItem saveFile = new MenuItem(); // кнопка в контексном меню - сохранить файл
            MenuItem saveAsFile = new MenuItem(); // кнопка в контексном меню - сохранить как

            contextMenu.MenuItems.AddRange( 
                new MenuItem[]
                {
                    openFile,
                    saveFile,
                    saveAsFile
                }); //добавление созданных MenuItem в коллекцию, ранее созданного, контексного меню


            openFile.Index = 0; //положение кнопки в меню
            openFile.Text = "Открыть файл"; //текст кнопки

            saveFile.Index = 1;
            saveFile.Text = "Сохранить файл";

            saveAsFile.Index = 2;
            saveAsFile.Text = "Сохранить как файл";

            richTextBox1.ContextMenu = contextMenu; //привязываем контексное меню к RichTextBox

            //привязка события клика к каждой кнопке контексного меню
            openFile.Click += new EventHandler(openFile_Click); 
            saveFile.Click += new EventHandler(saveFile_Click);
            saveAsFile.Click += new EventHandler(saveAsFile_Click);
        }

        string MyFName = "";
        private void openFile_Click(object sender, EventArgs e) // кнопка открыть файл
        {
            openFileDialog1.Filter = "Текстовые файлы (*.rtf; *.txt; *.dat) | *.rtf; *.txt; *.dat"; //фильтр файлов
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //если нажали кнопку ок
            {
                MyFName = openFileDialog1.FileName; //записываем путь файла
                
                try
                {
                    richTextBox1.LoadFile(MyFName); //пробуем загрузить выбранный файл в RichTextBox
                }
                catch 
                {
                    //если возникнет ошибка, обработаем ее следующим сообщением:
                    MessageBox.Show("Ошибка при открытии файла. Возможно файл используется другим приложением");
                }

                GetVowelIndex(); 
            }
        }

        private void saveFile_Click(object sender, EventArgs e) // кнопка сохранить файл
        {
            if (MyFName != "")
            {
                richTextBox1.SaveFile(MyFName);
            }
            else
            {
                saveFileDialog1.Filter = "Текстовые файлы (*.rtf; *.txt; *.dat) | *.rtf; *.txt; *.dat";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    MyFName = saveFileDialog1.FileName;
                    richTextBox1.SaveFile(MyFName);
                }
            }
        }
        private void saveAsFile_Click(object sender, EventArgs e) // кнопка сохранить как 
        {
            saveFileDialog1.Filter = "Текстовые файлы (*.rtf; *.txt; *.dat) | *.rtf; *.txt; *.dat";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MyFName = saveFileDialog1.FileName;
                richTextBox1.SaveFile(MyFName);
            }
        }

        //массив гласных букв
        readonly char[] vowel = new char[] { 'а', 'е', 'ё', 'и', 'о', 'у', 'э', 'ы', 'ю', 'я', 'ы', 'А', 'Е', 'Ё', 'И', 'О', 'У', 'Э', 'Ю', 'Ы', 'Я' };
       
        //массив некоторых символов
        readonly char[] symbol = new char[] { ' ', ',', ';', ':', '!', '?', ')', '.', '-', '(' };

        private void SelectionWord(int indexStartWord, int indexEndWord) //метод выделения слова в RichTextBox
        {
            richTextBox1.Select(indexStartWord, indexEndWord - indexStartWord); //выбираем от куда до куда выделяем
            richTextBox1.SelectionColor = Color.Red; //цвет выделения
        }


        int countWord = 0;
        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e) //нажатие произвольной клавиши
        {
            try
            {
                if (endIndex[countWord] < richTextBox1.Text.Length) //работаем в пределе длины всего текста
                {
                    SelectionWord(startIndex[countWord], endIndex[countWord] + 1); //выделяем
                    countWord++; //прибавляем кол-во слов

                    textBox3.Text = countWord.ToString(); //выводим кол-во слов в TextBox
                }
            }
            catch 
            {}   
        }

        public List<int> startIndex = new List<int>(); //целочисленный список, включащий в себя положения букв, на которые начинаются слова
        public List<int> endIndex = new List<int>(); //целочисленный список, включащий в себя положения букв, на которые заканчиваются слова
        int tempStartIndex = 0;
        private void GetVowelIndex() //решение
        {
            for (int i = 0; i < richTextBox1.Text.Length; i++)
            {
                //если данная буква является началом слова
                if (IsStartWord(i)){
                    tempStartIndex = i;
                }
                //если данная буква является концом слова 
                if (IsEndWord(i))
                {
                    //проверяем эту букву является ли она гласной
                    if (CheckVowelLetter(richTextBox1.Text[i]))
                    {
                        //записываем в списки позиции букв 
                        startIndex.Add(tempStartIndex);
                        endIndex.Add(i);
                    }
                    else
                    {
                        //если последняя буква не гласная, то удаляем первую букву
                        startIndex.Remove(i);
                    }
                }
            }
        }
        private bool IsEndWord(int pos) //проверяет является ли входящая позиция в RichTextBox концом слова
        {
            if (pos == richTextBox1.Text.Length - 1)
            {
                return true; 
            }

            if (pos < richTextBox1.Text.Length && pos > 0)
            {
                if (!CheckSymbolLetter(richTextBox1.Text[pos]) && CheckSymbolLetter(richTextBox1.Text[pos + 1]))
                {
                    return true;
                }
            }

            return false;
        }
        private bool IsStartWord(int pos) //проверяет является ли входящая позиция в RichTextBox началом слова
        {
            if (pos == 0)
            {
                return true;
            }

            if (pos > 0 && pos < richTextBox1.Text.Length)
            {
                if (!CheckSymbolLetter(richTextBox1.Text[pos]) && CheckSymbolLetter(richTextBox1.Text[pos - 1]))
                {
                    return true;
                }
            }
            
            return false; 
        }
        private bool CheckVowelLetter(char letter) //проверяет есть ли символ в массиве гласных букв 
        {
            for (int i = 0; i < vowel.Length; i++)
            {
                if (letter == vowel[i])
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckSymbolLetter(char letter) //проверяет есть ли символ в массиве символов 
        {
            for (int i = 0; i < symbol.Length; i++)
            {
                if (letter == symbol[i])
                {
                    return true;
                }
            }
            return false;
        }

    }  
}
