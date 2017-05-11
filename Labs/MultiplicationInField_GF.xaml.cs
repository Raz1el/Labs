using System;
using System.Collections.Generic;
using System.Windows;
using GaloisFieldLib;
using GaloisFieldLib.Integer_arithmetic;

namespace Labs
{
    public partial class MultiplicationInFieldGf
    {
        public static Polynomial Polinom; //полином
        public static ulong Characteristic; //модуль

        public MultiplicationInFieldGf()
        {
            InitializeComponent();
        }

        //кнопка построить таблицу
        private void BuildGridButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ulong[] figure; //массив чисел
                var list = new List<ulong>();
                char[] delim = {' ', ','};

                //ввод полинома
                if (PolinomTextBox.Text != string.Empty)
                {
                    string entervalue = Convert.ToString(PolinomTextBox.Text);
                    string[] arrayofnumber = entervalue.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var w in arrayofnumber)
                    {
                        list.Add(Convert.ToUInt64(w));
                    }
                    figure = list.ToArray();
                }
                else
                {
                    MessageBox.Show("Введите полином!");
                    return;
                }

                //ввод характеристики
                if (CharacteristicTextBox.Text != string.Empty)
                {
                    //проверяем введённые данные
                    string entervalue = Convert.ToString(CharacteristicTextBox.Text);
                    foreach (var t in entervalue)
                    {
                        if (!Char.IsDigit(t))
                        {
                            MessageBox.Show("Введены недопустимые символы!");
                            ClearMethod();
                            return;
                        }
                    }
                    Characteristic = (ulong) Convert.ToInt32(entervalue);
                    var simpleornot = IntegerMath.IsStrongPseudoprime(Characteristic, 50);

                    if (Characteristic < 1)
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

                Polinom = new Polynomial(figure);
                new GfTable().ShowDialog();
            }
            catch (Exception ex)
            {
                ClearMethod();
                MessageBox.Show(ex.Message);
            }
        }

        //кнопка очистить поля
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearMethod();
        }

        //метод очистки
        private void ClearMethod()
        {
            PolinomTextBox.Clear();
            CharacteristicTextBox.Clear();
        }

        //кнопка выход
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
