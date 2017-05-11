using System;
using System.Text;
using System.Windows;
using GaloisFieldLib.Integer_arithmetic;

namespace Labs
{
    public partial class PrimitiveRootWindow
    {
        public PrimitiveRootWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ulong mod = ulong.Parse(modTb.Text);
                var opr = checkBox1.IsChecked;
                if (opr == true)
                {
                    var roo = IntegerMath.FindPrimitiveRoot(mod);
                    var strBuilder = new StringBuilder();
                    strBuilder.Append(roo + ";" + Environment.NewLine);
                    ResultTb.Text = strBuilder + Environment.NewLine;
                    ResultTb.Text += "Найден один примитивный элемент!";
                }
                else
                {
                    var roots = IntegerMath.FindAllPrimitiveRoots(mod);
                    var strBuilder = new StringBuilder();
                    int count = 0;
                    foreach (var root in roots)
                    {
                        count++;
                        strBuilder.Append(root + ";" + Environment.NewLine);
                    }
                    ResultTb.Text = strBuilder + Environment.NewLine;
                    ResultTb.Text += "Всего примитивных элементов в GF(" + mod + ") = " + count;
                }
            }
            catch (Exception ex)
            {
                ResultTb.Text = "";
                MessageBox.Show(ex.Message);
            }
        
        }

        private void exitbutton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ResultTb.Clear();
            modTb.Clear();
        }
    }
}
