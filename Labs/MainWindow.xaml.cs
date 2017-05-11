using System;
using System.Windows;

namespace Labs
{
    public partial class MainWindow
    {
        private static bool _f1, _f2, _f3, _f4, _f5, _f6;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                FalseMethod();
                startButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            FalseMethod();
            _f1 = true;
            
            RefreshMethod(true);
            Button1.IsEnabled = false;
            textBox.Text = "Матричные коды Хэмминга. Исправление одиночных ошибок";
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            FalseMethod();
            _f2 = true;
            
            RefreshMethod(true);
            Button2.IsEnabled = false;
            textBox.Text = "БЧХ-коды";
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            FalseMethod();
            _f3 = true;
            
            RefreshMethod(true);
            Button3.IsEnabled = false;
            textBox.Text = "Нахождение примитивного элемента в конечном поле GF(p), p - простое";
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            FalseMethod();
            _f4 = true;
            
            RefreshMethod(true);
            Button4.IsEnabled = false;
            textBox.Text = "Арифметическое кодирование";
        }
        
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            FalseMethod();
            _f5 = true;

            RefreshMethod(true);
            Button5.IsEnabled = false;
            textBox.Text = "Умножение чисел и многочленов. Арифметика в конечном поле GF(p)";
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            FalseMethod();
            _f6 = true;
            
            RefreshMethod(true);
            Button6.IsEnabled = false;
            textBox.Text = "Алгоритм сжатия RLE";
        }

        //кнопка "очистить"
        private void clearbutton_Click(object sender, RoutedEventArgs e)
        {
            RefreshMethod(false);
        }

        //кнопка "запустить"
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_f1)
                {
                    new HammingsCode().ShowDialog();
                }
                if (_f2)
                {
                    new BchCodding().ShowDialog();
                }
                if (_f3)
                {
                    new PrimitiveRootWindow().ShowDialog();
                }
                if (_f4)
                {
                    new ArithmeticCode().ShowDialog();
                }
                if (_f5)
                {
                    new MultiplicationInField().ShowDialog();
                }
                if (_f6)
                {
                    new AlgorithmRle().ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //кнопка "выход"
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        //метод изменения флагов
        private static void FalseMethod()
        {
            _f1 = false; _f2 = false; _f3 = false; _f4 = false; _f5 = false; _f6 = false;
        }

        //метод вскрытия кнопок
        private void EnabledMethod()
        {
            Button1.IsEnabled = true;
            Button2.IsEnabled = true;
            Button3.IsEnabled = true;
            Button4.IsEnabled = true;
            Button5.IsEnabled = true;
            Button6.IsEnabled = true;
        }

        //метод очистки, обновки
        private void RefreshMethod(bool value)
        {
            EnabledMethod();
            startButton.IsEnabled = value;
            textBox.Clear();
        }
    }
}
