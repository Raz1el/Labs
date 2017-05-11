using System;

namespace Labs
{
    public static class BuildMatrix
    {
        //метод построения матрицы Хэмминга
        public static int[,] BuildHMatrix(int check)
        {
            int countColumn = check; //количество столбцов транспонированной матрицы
            int countLines = (int)Math.Pow(2, countColumn) - 1; //количество строк транспонированной матрицы

            //объявляем и заполняем транспонированную матрицу
            int[,] ht = new int[countLines, countColumn];
            for (int i = 1; i <= countLines; i++)
            {
                var binaryNum = BinaryClass.TranslationToBinary(i);
                int lenh = binaryNum.Length;
                int q = 0;

                for (int j = countColumn - 1; j >= 0; j--)
                {
                    if (q < lenh)
                    {
                        ht[i - 1, j] = Convert.ToInt32(Convert.ToString(binaryNum[lenh - q - 1]));
                        q++;
                    }
                    else
                    {
                        ht[i - 1, j] = 0;
                    }
                }
            }

            //объявляем и заполняем нужную нам матрицу
            int[,] h = new int[countColumn, countLines];
            for (int i = 0; i < countColumn; i++)
            {
                for (int j = 0; j < countLines; j++)
                {
                    h[i, j] = ht[j, i];
                }
            }

            return h;
        }
    }
}
