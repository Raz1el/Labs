using System;
using System.Collections.Generic;
using System.Windows;
using GaloisFieldLib.Integer_arithmetic;

namespace Labs
{
    public partial class BchCodding
    {
        public static ulong CharacteristicofField; //характеристика
        public static int CountMistake; //количество ошибок
        public static ulong[] Figure; //массив

        public BchCodding()
        {
            try
            {
                InitializeComponent();
                ContinueButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //ввести данные
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int degree;

                //ввод характеристики
                if (CharTextBox.Text != string.Empty)
                {
                    //проверяем введённые данные
                    string entervalue = Convert.ToString(CharTextBox.Text);
                    foreach (var t in entervalue)
                    {
                        if (!Char.IsDigit(t))
                        {
                            MessageBox.Show("Введены недопустимые символы!");
                            ClearMethod();
                            return;
                        }
                    }
                    CharacteristicofField = (ulong) Convert.ToInt64(entervalue);
                    var simpleornot = IntegerMath.IsStrongPseudoprime(CharacteristicofField, 50);

                    if (CharacteristicofField < 1)
                    {
                        MessageBox.Show("Характеристика поля должна быть больше нуля!");
                        ClearMethod();
                        return;
                    }
                    if (!simpleornot)
                    {
                        MessageBox.Show("Характеристика поля простым числом!");
                        ClearMethod();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Введите характеристику!");
                    return;
                }

                //ввод полинома
                if (PolinomTextBox.Text != string.Empty)
                {
                    var list = new List<ulong>();
                    char[] delim = {' ', ','};

                    string entervalue = Convert.ToString(PolinomTextBox.Text);
                    string[] arrayofnumber = entervalue.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var w in arrayofnumber)
                    {
                        list.Add(Convert.ToUInt64(w));
                    }
                    Figure = list.ToArray();
                    degree = Figure.Length - 1;
                }
                else
                {
                    MessageBox.Show("Введите полином!");
                    return;
                }

                //ввод количества ошибок
                if (CountMistakeTextBox.Text != string.Empty)
                {
                    //проверяем введённые данные
                    string entervalue = Convert.ToString(CountMistakeTextBox.Text);
                    foreach (var t in entervalue)
                    {
                        if (!Char.IsDigit(t))
                        {
                            MessageBox.Show("Введены недопустимые символы!");
                            ClearMethod();
                            return;
                        }
                    }
                    CountMistake = Convert.ToInt32(entervalue);
                    if (CountMistake < 0)
                    {
                        MessageBox.Show("Количество ошибок должно быть больше 0!");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Введите количество ошибок!");
                    return;
                }

                //проверка неравенства 2*t<q^m-1 (q-характеристика, m-степень полинома, t- кол-во ошибок)
                if (2*CountMistake >= Math.Pow(CharacteristicofField, degree) - 1)
                {
                    MessageBox.Show(
                        "Должно выполняться неравенство 2*t<q^m-1,\n где q-характеристика поля, m-степень полинома, t- кол-во ошибок");
                    return;
                }

                HidButton(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        //очистить
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearMethod();
            HidButton(false);
        }

        //выход
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //продолжить
        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            new BchCodding_Code(CharacteristicofField, CountMistake, Figure).ShowDialog();
        }

        //метод очистки
        private void ClearMethod()
        {
            CharTextBox.Clear();
            PolinomTextBox.Clear();
            CountMistakeTextBox.Clear();
        }

        //скрытие кнопок
        private void HidButton(bool value)
        {
            EnterButton.IsEnabled = !value;
            CharTextBox.IsReadOnly = value;
            PolinomTextBox.IsReadOnly = value;
            CountMistakeTextBox.IsReadOnly = value;
            ContinueButton.IsEnabled = value;
        }

    }
}
