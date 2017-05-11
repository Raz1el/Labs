using System;

namespace Labs
{
    static class BinaryClass
    {
        //перевод в двоичное число
        public static string TranslationToBinary(int num)
        {
            int number = num;
            string binaryNumber = "";

            while (number > 1)
            {
                int temp = number;
                number = number / 2;
                number = number * 2;
                var rank = temp - number;
                number = (number + rank) / 2;
                binaryNumber += Convert.ToString(rank);
            }
            binaryNumber += Convert.ToString(number);
            binaryNumber = ReverseToLine(binaryNumber);
            return binaryNumber;
        }

        //перевод из двоичного числа
        public static int TranslationOfBinary(int num)
        {
            string strnumber = Convert.ToString(num);
            int number = 0;

            for (int i = 0; i < strnumber.Length; i++)
            {
                int a = Convert.ToInt32(Convert.ToString(strnumber[i]));
                int b = (int)Math.Pow(2, strnumber.Length - i - 1);
                number += a * b;
            }
            return number;
        }

        //метод-перевёртыш
        private static string ReverseToLine(string str)
        {
            string line = str;
            string newline = "";

            for (int i = line.Length - 1; i >= 0; i--)
            {
                newline = newline + Convert.ToString(line[i]);
            }

            return newline;
        }

        //поиск длины закодированного сообщения
        public static int SearchM(int num)
        {
            int n = num;
            int m = n;
            bool flag = true;

            while (flag)
            {
                if (Math.Pow(2, m - n) - m - 1 >= 0)
                {
                    flag = false;
                }
                else
                {
                    m++;
                }
            }
            return m;
        }

        //метод для замены символа в закодированном слове
        public static int ReverseSym(int num)
        {
            if (num == 1) return 0;
            if (num == 0) return 1;
            return 2;
        }

        //метод проверки, является ли число степенью двойки
        public static bool IsPowTwo(int a)
        {
            if (a == 2) return true;
            if (a%2 == 0) return IsPowTwo(a/2);
            return false;
        }
    }
}
