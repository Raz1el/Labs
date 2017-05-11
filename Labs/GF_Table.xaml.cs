using System.Data;
using GaloisFieldLib;

namespace Labs
{
    public partial class GfTable
    {
        public GfTable()
        {
            InitializeComponent();

            var polinom = MultiplicationInFieldGf.Polinom; //полином
            var fieldChar = MultiplicationInFieldGf.Characteristic; //модуль

            var gf = new FactorRing(polinom, fieldChar);
            Polynomial[,] table = gf.GetTable();

            DataTable dataTable = new DataTable();
            
            for (int i = 0; i < table.GetLength(1); i++)
            {
                dataTable.Columns.Add("", typeof(Polynomial));
            }
            for (int i = 0; i < table.GetLength(0); i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataTable.Rows.Add(dataRow);
            }
            DataView dataView;
            using (dataView = new DataView(dataTable))
            {
                for (int i =0; i < table.GetLength(0); i++)
                {
                    for (int j = 0; j < table.GetLength(1); j++)
                    {
                        dataView[i][j] = table[i,j];
                    }
                }
            }

            DataGrid.ItemsSource = dataTable.DefaultView;
        }
    }
}
