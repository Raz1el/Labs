using System.Windows;

namespace Labs
{
    public partial class MultiplicationInField
    {
        public MultiplicationInField()
        {
            InitializeComponent();
        }

        private void MultiplicationNumbersButton_Click(object sender, RoutedEventArgs e)
        {
            new MultiplicationInFieldNumber().ShowDialog();
        }

        private void MultiplicationPolinomsButton_Click(object sender, RoutedEventArgs e)
        {
            new MultiplicationInFieldPolinom().ShowDialog();
        }

        private void GFArithmeticButton_Click(object sender, RoutedEventArgs e)
        {
            new MultiplicationInFieldGf().ShowDialog();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
