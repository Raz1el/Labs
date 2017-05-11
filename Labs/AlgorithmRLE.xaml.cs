using System;
using System.Globalization;
using System.Windows;

namespace Labs
{
    public partial class AlgorithmRle
    {
        public AlgorithmRle()
        {
            InitializeComponent();
        }

        //кнопка "закодировать"
        private void CodeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str1 = InputCodeTextBox.Text, str = "";
                OutputCodeTextBox.Clear();

                if (str1 == String.Empty)
                {
                    throw new Exception("Пустая входная строка!");
                }
                int count = 1;
                char current = str1[0];
                if (char.IsDigit(current))
                    MessageBox.Show("Введён некорректный текст!");
                else
                {
                    for (int i = 1; i < str1.Length; i++)
                    {
                        char c = str1[str1.Length - 1];
                        if (char.IsDigit(c))
                        {
                            MessageBox.Show("Введён некорректный текст!");
                            return;
                        }
                        else
                        {
                            if (current == str1[i])
                            {
                                count++;
                            }
                            else if (count != 1)
                            {
                                str += Convert.ToString(count) + Convert.ToString(current);
                                count = 1;
                                current = str1[i];
                            }
                            else
                            {
                                str += Convert.ToString(current);
                                current = str1[i];
                            }
                        }
                    }
                    if (!char.IsDigit(current))
                    {
                        if (count != 1)
                            str += Convert.ToString(count) + Convert.ToString(current);
                        else
                            str += Convert.ToString(current);
                        OutputCodeTextBox.Text = str;

                        double kf = (double) str1.Length/str.Length;
                        KF.Text = kf.ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //кнопка "декодировать"
        private void DecodeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str1 = InputDecodeTextBox.Text;
                OutputDecodeTextBox.Clear();
                if (str1 == String.Empty)
                {
                    throw new Exception("Пустая входная строка!");
                }

                string a = "";
                for (int i = 0; i < str1.Length; i++)
                {
                    var current = str1[i];
                    if (!char.IsDigit(current) && a == "")
                        OutputDecodeTextBox.Text += current.ToString();
                    
                    else if (char.IsDigit(current))
                        a += current;
                    else
                    {
                        var count = int.Parse(a);
                        a = "";
                        for (int j = 0; j < count; j++)
                            OutputDecodeTextBox.Text += current.ToString();
                    }
                }
                if (a != "")
                {
                    OutputDecodeTextBox.Clear();
                    throw new Exception("Введён некорректный текст!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //кнопка "выход"
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //кнопка "очистить всё"
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearMethod();
        }

        //метод очистки
        private void ClearMethod()
        {
            InputCodeTextBox.Clear();
            OutputCodeTextBox.Clear();
            InputDecodeTextBox.Clear();
            OutputDecodeTextBox.Clear();
            KF.Clear();
        }
    }
}
