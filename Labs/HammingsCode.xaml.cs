using System;
using System.Windows;

namespace Labs
{
    public partial class HammingsCode
    {
        private static bool _f1;
        private static bool _f2;
        private static bool _f3;

        public HammingsCode()
        {
            InitializeComponent();

            Label1.Visibility = Visibility.Hidden;
            Label2.Visibility = Visibility.Hidden;
            Label3.Visibility = Visibility.Hidden;
            Label4.Visibility = Visibility.Hidden;
            EnterTextBox.Visibility = Visibility.Hidden;
            CodeTextBox.Visibility = Visibility.Hidden;
            Enterbutton.Visibility = Visibility.Hidden;
            Clearbutton.IsEnabled = false;
            PositionMistakeTextBox.Visibility = Visibility.Hidden;
            MessWithourErrorTextBox.Visibility = Visibility.Hidden;
        }

        //кнопка "закодировать сообщение"
        private void codebutton_Click(object sender, RoutedEventArgs e)
        {
            Label1.Content = "Сообщение, которое необходимо закодировать: ";
            Label2.Content = "Закодированное сообщение: ";

            ClearMethod();
            Label1.Visibility = Visibility.Visible;
            Label2.Visibility = Visibility.Visible;
            Label3.Visibility = Visibility.Hidden;
            Label4.Visibility = Visibility.Hidden;
            EnterTextBox.Visibility = Visibility.Visible;
            CodeTextBox.Visibility = Visibility.Visible;
            Enterbutton.Visibility = Visibility.Visible;
            MessWithourErrorTextBox.Visibility = Visibility.Hidden; 
            PositionMistakeTextBox.Visibility = Visibility.Hidden;
            Clearbutton.IsEnabled = true;
            Codebutton.IsEnabled = false;
            Mistakebutton.IsEnabled = true;
            Decodebutton.IsEnabled = true;

            _f1 = true;
            _f2 = false;
            _f3 = false;
        }

        //кнопка "исправить одиночную ошибку"
        private void mistakebutton_Click(object sender, RoutedEventArgs e)
        {
            Label1.Content = "Сообщение, которое необходимо закодировать: ";
            Label2.Content = "Закодированное сообщение, пришедшее с ошибкой: ";

            ClearMethod();
            Label1.Visibility = Visibility.Visible;
            Label2.Visibility = Visibility.Visible;
            Label3.Visibility = Visibility.Visible;
            Label4.Visibility = Visibility.Visible;
            EnterTextBox.Visibility = Visibility.Visible;
            CodeTextBox.Visibility = Visibility.Visible;
            Enterbutton.Visibility = Visibility.Visible;
            MessWithourErrorTextBox.Visibility = Visibility.Visible;
            PositionMistakeTextBox.Visibility = Visibility.Visible;
            Clearbutton.IsEnabled = true;
            Codebutton.IsEnabled = true;
            Mistakebutton.IsEnabled = false;
            Decodebutton.IsEnabled = true;

            _f1 = false;
            _f2 = true;
            _f3 = false;
        }

        //кнопка "раскодировать сообщение"
        private void decodebutton_Click(object sender, RoutedEventArgs e)
        {
            Label1.Content = "Сообщение, которое необходимо раскодировать: ";
            Label2.Content = "Раскодированное сообщение: ";

            ClearMethod();
            Label1.Visibility = Visibility.Visible;
            Label2.Visibility = Visibility.Visible;
            Label3.Visibility = Visibility.Hidden;
            Label4.Visibility = Visibility.Hidden;
            EnterTextBox.Visibility = Visibility.Visible;
            CodeTextBox.Visibility = Visibility.Visible;
            Enterbutton.Visibility = Visibility.Visible;
            MessWithourErrorTextBox.Visibility = Visibility.Hidden;
            PositionMistakeTextBox.Visibility = Visibility.Hidden;
            Clearbutton.IsEnabled = true;
            Codebutton.IsEnabled = true;
            Mistakebutton.IsEnabled = true;
            Decodebutton.IsEnabled = false;

            _f1 = false;
            _f2 = false;
            _f3 = true;
        }

        //кнопка "ввести"
        private void Enterbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_f1)
                {
                    CodeMessage();
                }
                if (_f2)
                {
                    MistakeCorrection();
                }
                if (_f3)
                {
                    DecodeMessage();
                }
                if (!_f1 && !_f2 && !_f3)
                {
                    MessageBox.Show("Неизвестная ошибка!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //кнопка "очистить все поля"
        private void clearbutton_Click(object sender, RoutedEventArgs e)
        {
            ClearMethod();
        }

        //кнопка "выход"
        private void exitbutton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //метод кодирования сообщения по методу Хаффмана
        private void CodeMessage()
        {
            try
            {
                string binaryNumber;
                if (EnterTextBox.Text != string.Empty)
                {
                    binaryNumber = EnterTextBox.Text; //вводим двоичное число из текстбокса
                    for (int i = 0; i < binaryNumber.Length; i++)
                    {
                        int temp = Convert.ToInt32(Convert.ToString(binaryNumber[i]));
                        if ((temp != 0) && (temp != 1))
                        {
                            MessageBox.Show("Сообщение должно быть двоичным, попробуйте ещё раз!");
                            EnterTextBox.Clear();
                            CodeTextBox.Clear();
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Не введено сообщение!");
                    CodeTextBox.Clear();
                    return;
                }

                int n = binaryNumber.Length; //длина введённого числа в двоичной форме
                int m = BinaryClass.SearchM(n); //для заданного n ищем длину закодированного слова
                int countControlSymbols = m - n;
                //количество контрольных символов в зашифрованном слове, передаём это в матрицу

                int[] code = new int[m]; //массив для закодированного слова
                int deg = 0, count = 0;

                //по введённым данным строим матрицу Хэмминга
                var hammingsMatrix = BuildMatrix.BuildHMatrix(countControlSymbols);

                //на контрольные символы ставим временно "9", а на информационные - исходное слово
                for (int i = 0; i < m; i++)
                {
                    //если выполняется это условие - значит нашли контрольный символ
                    if ((i + 1) == Math.Pow(2, deg))
                    {
                        deg++;
                        code[i] = 9;
                    }
                    //иначе вставляем исходное слово
                    else
                    {
                        code[i] = int.Parse(Convert.ToString(binaryNumber[count]));
                        count++;
                    }
                }

                int hammingsMatrixLines = hammingsMatrix.GetLength(0); //количество строк в матрице Хэмминга
                int hammingsMatrixColumn = hammingsMatrix.GetLength(1); //количество столбцов в матрице Хэмминга
                int countcontrol = -1;

                //вычисляем итоговые контрольные символы
                for (int i = 0; i < m; i++)
                {
                    int flag = 1;
                    int sum = 0;

                    if (code[i] == 9)
                    {
                        countcontrol++;
                        int line = hammingsMatrixLines - 1 - countcontrol;

                        for (int j = 0; j < hammingsMatrixColumn; j++)
                        {
                            if ((hammingsMatrix[line, j] == 1) && (flag == 1))
                            {
                                flag = 0;
                                continue;
                            }
                            if ((hammingsMatrix[line, j] == 1) && (flag == 0))
                            {
                                if (j <= code.Length - 1)
                                {
                                    sum = sum + code[j];
                                }
                            }
                        }
                        code[i] = sum%2;
                    }
                }

                //переводим массив в строковую переменную и выводим в текстбокс
                string vyvod = "";
                foreach (var v in code)
                {
                    vyvod = vyvod + Convert.ToString(v);
                }
                CodeTextBox.Text = vyvod;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //метод исправления ошибок
        private void MistakeCorrection()
        {
            try
            {
                string binaryNumber;
                if (EnterTextBox.Text != string.Empty)
                {
                    binaryNumber = EnterTextBox.Text; //вводим двоичное число из текстбокса
                    for (int i = 0; i < binaryNumber.Length; i++)
                    {
                        int temp = Convert.ToInt32(Convert.ToString(binaryNumber[i]));
                        if ((temp != 0) && (temp != 1))
                        {
                            MessageBox.Show("Сообщение должно быть двоичным, попробуйте ещё раз!");
                            EnterTextBox.Clear();
                            CodeTextBox.Clear();
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Не введено сообщение!");
                    CodeTextBox.Clear();
                    return;
                }

                int n = binaryNumber.Length; //длина введённого числа в двоичной форме
                int m = BinaryClass.SearchM(n); //для заданного n ищем длину закодированного слова
                int countControlSymbols = m - n;
                //количество контрольных символов в зашифрованном слове, передаём это в матрицу

                int[] code = new int[m]; //массив для закодированного слова
                int deg = 0, count = 0;

                //по введённым данным строим матрицу Хэмминга
                var hammingsMatrix = BuildMatrix.BuildHMatrix(countControlSymbols);

                //на контрольные символы ставим временно "9", а на информационные - исходное слово
                for (int i = 0; i < m; i++)
                {
                    //если выполняется это условие - значит нашли контрольный символ
                    if ((i + 1) == Math.Pow(2, deg))
                    {
                        deg++;
                        code[i] = 9;
                    }
                    //иначе вставляем исходное слово
                    else
                    {
                        code[i] = int.Parse(Convert.ToString(binaryNumber[count]));
                        count++;
                    }
                }

                int hammingsMatrixLines = hammingsMatrix.GetLength(0); //количество строк в матрице Хэмминга
                int hammingsMatrixColumn = hammingsMatrix.GetLength(1); //количество столбцов в матрице Хэмминга
                int countcontrol = -1;

                //вычисляем итоговые контрольные символы
                for (int i = 0; i < m; i++)
                {
                    int flag = 1;
                    int sum = 0;

                    if (code[i] == 9)
                    {
                        countcontrol++;
                        int line = hammingsMatrixLines - 1 - countcontrol;

                        for (int j = 0; j < hammingsMatrixColumn; j++)
                        {
                            if ((hammingsMatrix[line, j] == 1) && (flag == 1))
                            {
                                flag = 0;
                                continue;
                            }
                            if ((hammingsMatrix[line, j] == 1) && (flag == 0))
                            {
                                if (j <= code.Length - 1)
                                {
                                    sum = sum + code[j];
                                }
                            }
                        }
                        code[i] = sum%2;
                    }
                }

                //переводим массив в строковую переменную
                string vernyVyvod = "";
                foreach (var v in code)
                {
                    vernyVyvod = vernyVyvod + Convert.ToString(v);
                }

                var rnd = new Random();
                var mistake = rnd.Next(1, m + 1);
                code[mistake - 1] = BinaryClass.ReverseSym(code[mistake - 1]); //генерируем ошибку

                //переводим массив в строковую переменную и выводим в текстбокс
                string vyvod = "";
                foreach (var v in code)
                {
                    vyvod = vyvod + Convert.ToString(v);
                }
                CodeTextBox.Text = vyvod;

                //считаем, сколько единиц в закодированном сообщении
                int countN = 0;
                for (int i = 0; i < m; i++)
                {
                    if (code[i] == 1) countN++;
                }

                //вспомогательный массив для вычисления синдрома и его длина
                int countAuxArray = countControlSymbols*countN;
                int[] auxiliaryArray = new int[countAuxArray];

                //в вспомогательный массив записываем те столбцы матрицы Хэмминга, 
                //которые соответствуют единице в закодированном сообщении длины m
                int index = 0;
                for (int i = 0; i < m; i++)
                {
                    if (code[i] == 1)
                    {
                        for (int j = 0; j < hammingsMatrixLines; j++)
                        {
                            auxiliaryArray[index] = hammingsMatrix[j, i];
                            index++;
                        }
                    }
                }

                //ищем сумму цифр на определённых позициях (синдром по сути)
                //результат - номер позиции ошибки в двоичной сс
                int positionmistake = 0;
                for (int i = 0; i < countControlSymbols; i++)
                {
                    var result = 0;
                    for (int t = 0; t < countN; t++)
                    {
                        if ((i + countControlSymbols*t) < countAuxArray)
                        {
                            result = result + auxiliaryArray[i + countControlSymbols*t];
                        }
                    }
                    positionmistake = positionmistake*10 + result%2;
                }

                //переводим номер ошибки в десятичную сс и выводим в текстбокс
                positionmistake = BinaryClass.TranslationOfBinary(positionmistake);
                PositionMistakeTextBox.Text = Convert.ToString(positionmistake);
                MessWithourErrorTextBox.Text = vernyVyvod;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //метод раскодировки сообщений
        private void DecodeMessage()
        {
            try
            {
                string binaryNumber;
                if (EnterTextBox.Text != string.Empty)
                {
                    binaryNumber = EnterTextBox.Text; //вводим двоичное число из текстбокса
                    for (int i = 0; i < binaryNumber.Length; i++)
                    {
                        int temp = Convert.ToInt32(Convert.ToString(binaryNumber[i]));
                        if ((temp != 0) && (temp != 1))
                        {
                            MessageBox.Show("Сообщение должно быть двоичным, попробуйте ещё раз!");
                            EnterTextBox.Clear();
                            CodeTextBox.Clear();
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Не введено сообщение!");
                    CodeTextBox.Clear();
                    return;
                }

                int m = binaryNumber.Length; //длина введённого закодированного сообщения
                if (m < 3)
                {
                    MessageBox.Show("Длина сообщения должна быть больше 3, иначе неверно!");
                    EnterTextBox.Clear();
                    CodeTextBox.Clear();
                    return;
                }
                if (BinaryClass.IsPowTwo(m))
                {
                    MessageBox.Show("Длина закодирванного сообщения не должна быть равна степени двойки!");
                    EnterTextBox.Clear();
                    CodeTextBox.Clear();
                    return;
                }
                int n = (int) Math.Truncate(m - Math.Log(m + 1, 2)); //длина раскодированного сообщения
                int[] decode = new int[n]; //массив для закодированного сообщения

                int count = 0;
                for (int i = 0; i < m; i++)
                {
                    if ((i + 1) == 1) continue;
                    if (BinaryClass.IsPowTwo(i + 1)) continue;

                    decode[count] = binaryNumber[i];
                    count++;
                }

                //переводим массив в строковую переменную и выводим в текстбокс
                string vyvod = "";
                foreach (var v in decode)
                {
                    vyvod = vyvod + Convert.ToString(Convert.ToChar(v));
                }
                CodeTextBox.Text = vyvod;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //метод очистки
        private void ClearMethod()
        {
            EnterTextBox.Clear();
            CodeTextBox.Clear();
            MessWithourErrorTextBox.Clear();
            PositionMistakeTextBox.Clear();
        }
    }
}
