using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using GaloisFieldLib;

namespace Labs
{
    public partial class MultiplicationInFieldPolinom
    {
        public MultiplicationInFieldPolinom()
        {
            InitializeComponent();

            label1.IsEnabled = false;
            label2.IsEnabled = false;
            label3.IsEnabled = false;
            label4.IsEnabled = false;
            label5.IsEnabled = false;
            label6.Visibility = Visibility.Hidden;
            label7.IsEnabled = false;
            label8.IsEnabled = false;
            label9.IsEnabled = false; 
            textBox1.IsEnabled = false;
            textBox2.IsEnabled = false;
            textBox3.IsEnabled = false;
            textBox4.IsEnabled = false;
            textBox5.IsEnabled = false;
            textBox6.IsEnabled = false;
            textBox7.IsEnabled = false;
            textBox8.IsEnabled = false;
            EnterButton.IsEnabled = false;
            EnterAndMulButton.IsEnabled = false;
        }
        
        //кнопка - перемножить для ручного ввода
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ulong[] figure1;
                ulong[] figure2;
                var list1 = new List<ulong>();
                var list2 = new List<ulong>();
                char[] delim = {' ', ','};
                var opr = checkBox1.IsChecked; //определяем, поставлена ли галочка в чек-боксе
                textBox1.IsReadOnly = false;
                textBox2.IsReadOnly = false;

                if (textBox1.Text != string.Empty && textBox2.Text != string.Empty)
                {
                    string vvod1 = Convert.ToString(textBox1.Text);
                    string[] word1 = vvod1.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var w in word1)
                    {
                        list1.Add(Convert.ToUInt64(w));
                    }
                    figure1 = list1.ToArray();

                    string vvod2 = Convert.ToString(textBox2.Text);
                    string[] word2 = vvod2.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var w in word2)
                    {
                        list2.Add(Convert.ToUInt64(w));
                    }
                    figure2 = list2.ToArray();
                }
                else
                {
                    MessageBox.Show("Введите коэффициенты многочленов в поля!");
                    return;
                }

                if (opr == true) //значит галочка стоит
                {
                    label3.IsEnabled = true;
                    label4.IsEnabled = true;
                    label5.IsEnabled = true;
                    label9.IsEnabled = true;
                    textBox3.IsEnabled = true;
                    textBox4.IsEnabled = true;
                    textBox5.IsEnabled = true;
                    textBox6.IsEnabled = true;

                    //обычное умножение
                    Stopwatch time1 = new Stopwatch();
                    time1.Start();
                    var root1 = Polynomial.Mul(new Polynomial(figure1), new Polynomial(figure2));
                    textBox3.Text = Convert.ToString(root1);
                    time1.Stop();
                    textBox4.Text = Convert.ToString(time1.Elapsed);

                    //бпф-умножение
                    Stopwatch time2 = new Stopwatch();
                    time2.Start();
                    var root2 = Polynomial.FftMul(new Polynomial(figure1), new Polynomial(figure2));
                    textBox6.Text = Convert.ToString(root2);
                    time2.Stop();
                    textBox5.Text = Convert.ToString(time2.Elapsed);
                }
                else
                {
                    label3.IsEnabled = true;
                    label4.IsEnabled = true;
                    textBox3.IsEnabled = true;
                    textBox4.IsEnabled = true;
                    textBox5.Clear();
                    textBox5.IsEnabled = false;
                    textBox6.Clear();
                    textBox6.IsEnabled = false;
                    label9.IsEnabled = false;
                    label5.IsEnabled = false;

                    //обычное умножение
                    Stopwatch time = new Stopwatch();
                    time.Start();
                    var root1 = Polynomial.Mul(new Polynomial(figure1), new Polynomial(figure2));
                    textBox3.Text = Convert.ToString(root1);
                    time.Stop();
                    textBox4.Text = Convert.ToString(time.Elapsed);
                }
            }
            catch (Exception ex)
            {
                textBox4.Text = "";
                textBox5.Text = "";
                textBox3.Text = "";
                textBox6.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        //кнопка - перемножить для рандомного ввода
        private void EnterAndMulButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                label1.IsEnabled = true;
                label2.IsEnabled = true;
                textBox1.IsEnabled = true;
                textBox1.IsReadOnly = true;
                textBox2.IsEnabled = true;
                textBox2.IsReadOnly = true;

                ulong[] figure1;
                ulong[] figure2;
                var list1 = new List<ulong>();
                var list2 = new List<ulong>();
                string str1 = String.Empty;
                string str2 = String.Empty;
                Random rnd = new Random();
                var opr = checkBox1.IsChecked; //определяем, поставлена ли галочка в чек-боксе

                if (textBox7.Text != string.Empty && textBox8.Text != string.Empty)
                {
                    int deg1 = int.Parse(textBox7.Text);
                    int deg2 = int.Parse(textBox8.Text);
                    if (deg1 <= 0 || deg2 <= 0)
                    {
                        MessageBox.Show("Неверно введена степень!");
                        return;
                    }
                    for (int i = 0; i < deg1 + 1; i++)
                    {
                        int temp = rnd.Next(1, 20);
                        list1.Add((ulong) temp);
                        str1 = str1 + " " + Convert.ToString(temp);
                    }
                    for (int i = 0; i < deg2 + 1; i++)
                    {
                        int temp = rnd.Next(1, 20);
                        list2.Add((ulong) temp);
                        str2 = str2 + " " + Convert.ToString(temp);
                    }
                    figure1 = list1.ToArray();
                    figure2 = list2.ToArray();

                    textBox1.Text = str1;
                    textBox2.Text = str2;
                }
                else
                {
                    MessageBox.Show("Введите степени многочленов!");
                    return;
                }

                if (opr == true) //значит галочка стоит
                {
                    label3.IsEnabled = true;
                    label4.IsEnabled = true;
                    label5.IsEnabled = true;
                    label9.IsEnabled = true;
                    textBox3.IsEnabled = true;
                    textBox4.IsEnabled = true;
                    textBox5.IsEnabled = true;
                    textBox6.IsEnabled = true;

                    //обычное умножение
                    Stopwatch time1 = new Stopwatch();
                    time1.Start();
                    var root1 = Polynomial.Mul(new Polynomial(figure1), new Polynomial(figure2));
                    textBox3.Text = Convert.ToString(root1);
                    time1.Stop();
                    textBox4.Text = Convert.ToString(time1.Elapsed);

                    //бпф-умножение
                    Stopwatch time2 = new Stopwatch();
                    time2.Start();
                    var root2 = Polynomial.FftMul(new Polynomial(figure1), new Polynomial(figure2));
                    textBox6.Text = Convert.ToString(root2);
                    time2.Stop();
                    textBox5.Text = Convert.ToString(time2.Elapsed);
                }
                else
                {
                    label3.IsEnabled = true;
                    label4.IsEnabled = true;
                    textBox3.IsEnabled = true;
                    textBox4.IsEnabled = true;
                    textBox5.Clear();
                    textBox5.IsEnabled = false;
                    textBox6.Clear();
                    textBox6.IsEnabled = false;
                    label9.IsEnabled = false;
                    label5.IsEnabled = false;

                    //обычное умножение
                    Stopwatch time = new Stopwatch();
                    time.Start();
                    var root1 = Polynomial.Mul(new Polynomial(figure1), new Polynomial(figure2));
                    textBox3.Text = Convert.ToString(root1);
                    time.Stop();
                    textBox4.Text = Convert.ToString(time.Elapsed);
                }
            }
            catch (Exception ex)
            {
                textBox4.Text = "";
                textBox5.Text = "";
                textBox3.Text = "";
                textBox6.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        //ручное задание коэффициентов
        private void EnterHandButton_Click(object sender, RoutedEventArgs e)
        {
            ClearMethod();

            EnterRandomButton.IsEnabled = true;
            label7.IsEnabled = false;
            label8.IsEnabled = false;
            label3.IsEnabled = false;
            label4.IsEnabled = false;
            label5.IsEnabled = false;
            label9.IsEnabled = false;
            textBox3.IsEnabled = false;
            textBox6.IsEnabled = false;
            textBox7.IsEnabled = false;
            textBox8.IsEnabled = false;
            textBox4.IsEnabled = false;
            textBox5.IsEnabled = false;
            EnterAndMulButton.IsEnabled = false;
            textBox1.IsReadOnly = false;
            textBox2.IsReadOnly = false;

            EnterHandButton.IsEnabled = false;
            label1.IsEnabled = true;
            label2.IsEnabled = true;
            label6.Visibility = Visibility.Visible;
            textBox1.IsEnabled = true;
            textBox2.IsEnabled = true;
            EnterButton.IsEnabled = true;
        }

        //рандомное задание коэффициентов
        private void EnterRandomButton_Click(object sender, RoutedEventArgs e)
        {
            ClearMethod();

            EnterHandButton.IsEnabled = true;
            label1.IsEnabled = false;
            label2.IsEnabled = false;
            label6.Visibility = Visibility.Hidden;
            textBox1.IsEnabled = false;
            textBox2.IsEnabled = false;
            EnterButton.IsEnabled = false;
            label3.IsEnabled = false;
            label4.IsEnabled = false;
            label5.IsEnabled = false;
            label9.IsEnabled = false;
            textBox4.IsEnabled = false;
            textBox5.IsEnabled = false;
            textBox3.IsEnabled = false;
            textBox6.IsEnabled = false;
            
            EnterRandomButton.IsEnabled = false;
            label7.IsEnabled = true;
            label8.IsEnabled = true;
            textBox7.IsEnabled = true;
            textBox8.IsEnabled = true;
            EnterAndMulButton.IsEnabled = true;
        }

        //очистить все поля
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearMethod();
        }
        
        //метод очистки
        private void ClearMethod()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
        }

        //кнопка - выход
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

