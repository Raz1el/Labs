using System;
using System.Windows;
using GaloisFieldLib;

namespace Labs
{
    public partial class MultiplicationInFieldNumber 
    {
        public MultiplicationInFieldNumber()
        {
            InitializeComponent();
        }
        
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NumOneTextBox.Text != string.Empty && NumTwoTextBox.Text != string.Empty)
                {
                    //проверка на ввод чисел
                    string enter1 = NumOneTextBox.Text;
                    string enter2 = NumTwoTextBox.Text;
                    foreach (var t in enter1)
                    {
                        if (!Char.IsDigit(t))
                        {
                            MessageBox.Show("Введены недопустимые символы!");
                            ClearMethod();
                            return;
                        }
                    }
                    foreach (var t in enter2)
                    {
                        if (!Char.IsDigit(t))
                        {
                            ClearMethod();
                            MessageBox.Show("Введены недопустимые символы!");
                            return;
                        }
                    }

                    Integer figure1 = new Integer(enter1);
                    Integer figure2 = new Integer(enter2);
                    var result = Integer.Mul(new Integer(figure1), new Integer(figure2));
                    ResultTextBox.Text = Convert.ToString(result);
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все поля для умножения чисел!");
                }
            }
            catch (Exception ex)
            {
                ClearMethod();
                MessageBox.Show(ex.Message);
            }
        }
        
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearMethod();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClearMethod()
        {
            NumOneTextBox.Clear();
            NumTwoTextBox.Clear();
            ResultTextBox.Clear();
        }
    }
}
