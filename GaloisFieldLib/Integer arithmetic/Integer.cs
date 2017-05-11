using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GaloisFieldLib.Integer_arithmetic;

namespace GaloisFieldLib
{
    public class Integer : IComparable
    {
        private const ulong Base = 3221225473; // 2^32=4294967296
        private int Size = 128; //2^10
        public static readonly Integer Zero = new Integer(0);
        public static readonly Integer One = new Integer(1);
        public static readonly ulong DigitMaxValue = Base - 1;
        public ulong[] _digits;
        public int _digitCount;

        #region Init

        public Integer(ulong[] longInteger)

        {



            _digitCount = longInteger.Length;
            for (int i = _digitCount - 1; i >= 0; i--)
            {
                if (longInteger[i] != 0)
                {
                    break;
                }
                _digitCount--;
            }


            if (_digitCount > Size)
            {
                int pow = (int) Math.Log(_digitCount, 2);
                Size = (int) Math.Pow(2, pow);
                if (_digitCount > Size)
                {
                    Size *= 2;
                }
                else
                {
                    Size = _digitCount;
                }
            }

            _digits = new ulong[Size];



            for (int i = 0; i < _digitCount; i++)
            {
                _digits[i] = longInteger[i];
            }
        }

        public Integer()
        {
            _digits = new ulong[Size];
            _digitCount = 0;
        }

        public Integer(string longInteger)
            : this()
        {
            InitFromString(longInteger);
        }

        public Integer(Integer longInteger)
            : this()
        {
            InitFromLongInteger(longInteger);
        }

        public Integer(ulong longInteger)
            : this()
        {
            InitFromULong(longInteger);
        }


        private void InitFromULong(ulong longInteger)
        {
            if (longInteger < Base && longInteger > 0)
            {
                _digits[0] = longInteger;
                _digitCount++;
            }
            else
            {

                while (longInteger > 0)
                {
                    _digits[_digitCount] = longInteger%Base;
                    _digitCount++;
                    longInteger = longInteger/Base;
                }
            }
        }

        private void InitFromLongInteger(Integer longInteger)
        {
            if (longInteger == null)
            {
                throw new ArgumentException();
            }

            _digitCount = longInteger._digitCount;
            _digits = new ulong[longInteger.Size];
            longInteger._digits.CopyTo(_digits, 0);
        }

        private void InitFromString(string longInteger)
        {
            if (string.IsNullOrEmpty(longInteger))
                throw new ArgumentException();
            int digitCount = 0;
            var result = new Integer(0);
            var ten = new Integer(10);
            var digits = new Dictionary<char, Integer>();
            for (ulong i = '0'; i <= '9'; i++)
            {
                digits.Add(Convert.ToChar(i), new Integer(i - '0'));
            }
            foreach (var chr in longInteger.Reverse())
            {
                if (char.IsDigit(chr))
                {
                    result = result + digits[chr]*IntegerMath.Pow(ten, digitCount);
                    digitCount++;
                }
            }
            _digitCount = result._digitCount;
            _digits = result._digits;
        }

        #endregion

        #region Operations

        public static Integer operator +(Integer firstArg, Integer secondArg)
        {
            if (firstArg == null || secondArg == null)
            {
                throw new ArgumentException();
            }

            if (firstArg._digitCount >= secondArg._digitCount)
            {
                return Add(firstArg, secondArg);
            }
            return Add(secondArg, firstArg);
        }

        public static Integer operator -(Integer firstArg, Integer secondArg)
        {
            if (firstArg == null || secondArg == null)
            {
                throw new ArgumentException();
            }
            if (!(firstArg < secondArg))
            {
                return Sub(firstArg, secondArg);
            }
            throw new InvalidOperationException("Уменьшаемое число меньше вычитаемого");
        }



        public static Integer operator *(Integer firstArg, Integer secondArg)
        {
            if (firstArg == null || secondArg == null)
            {
                throw new ArgumentException();
            }
            if (firstArg._digitCount <= 1)
            {
                return ShortMul(secondArg, (uint) firstArg._digits[0]);
            }
            if (secondArg._digitCount <= 1)
            {
                return ShortMul(firstArg, (uint) secondArg._digits[0]);
            }
            return Mul(firstArg, secondArg);
        }

        public static Integer operator /(Integer firstArg, Integer secondArg)
        {
            if (firstArg == null || secondArg == null || secondArg == Zero)
            {
                throw new ArgumentException();
            }
            if (secondArg._digitCount == 0)
                throw new DivideByZeroException();
            if (firstArg < secondArg)
            {
                return Zero;
            }
            if (secondArg._digitCount == 1)
            {
                return ShortDiv(firstArg, (uint) secondArg._digits[0]);
            }
            return Div(firstArg, secondArg);
        }

        public static Integer operator %(Integer firstArg, Integer secondArg)
        {
            if (firstArg == null || secondArg == null || secondArg == Zero)
            {
                throw new ArgumentException();
            }
            return Mod(firstArg, secondArg);
        }

        public static bool operator <(Integer firstArg, Integer secondArg)
        {
            if (firstArg == null || secondArg == null)
            {
                throw new ArgumentException();
            }
            if (firstArg._digitCount < secondArg._digitCount)
            {
                return true;
            }
            else if (firstArg._digitCount == secondArg._digitCount)
            {
                for (int i = firstArg._digitCount - 1; i >= 0; i--)
                {
                    if (firstArg._digits[i] > secondArg._digits[i])
                    {
                        return false;
                    }
                    if (firstArg._digits[i] < secondArg._digits[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool operator >(Integer firstArg, Integer secondArg)
        {
            if (firstArg == null || secondArg == null)
            {
                throw new ArgumentException();
            }
            if (firstArg._digitCount > secondArg._digitCount)
            {
                return true;
            }
            if (firstArg._digitCount == secondArg._digitCount)
            {
                for (int i = firstArg._digitCount - 1; i >= 0; i--)
                {
                    if (firstArg._digits[i] > secondArg._digits[i])
                    {
                        return true;
                    }
                    if (firstArg._digits[i] < secondArg._digits[i])
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public static bool operator ==(Integer firstArg, Integer secondArg)
        {
            if (ReferenceEquals(firstArg, null) && ReferenceEquals(secondArg, null))
            {
                return true;
            }
            if (ReferenceEquals(firstArg, null) || ReferenceEquals(secondArg, null))
            {
                return false;
            }


            if (firstArg._digitCount != secondArg._digitCount)
            {
                return false;
            }
            for (int i = 0; i < firstArg._digitCount; i++)
            {
                if (firstArg._digits[i] != secondArg._digits[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator !=(Integer firstArg, Integer secondArg)
        {
            return !(firstArg == secondArg);
        }

        public static bool operator >=(Integer firstArg, Integer secondArg)
        {
            if (firstArg == null || secondArg == null)
            {
                throw new ArgumentException();
            }
            return !(firstArg < secondArg);
        }

        public static bool operator <=(Integer firstArg, Integer secondArg)
        {
            if (firstArg == null || secondArg == null)
            {
                throw new ArgumentException();
            }
            return !(firstArg > secondArg);
        }


        public static Integer Sub(Integer a, Integer b)
        {
            ulong carry = 1;
            ulong temp;
            var aSize = a._digits.Length - 1;
            var bSize = b._digits.Length - 1;
            var aDigitsCount = a._digitCount;
            var bDigitsCount = b._digitCount;
            var newSize = aDigitsCount - 1;

            ulong[] result = new ulong[aDigitsCount];
            for (int i = 0; i < bDigitsCount; i ++)
            {
                temp = DigitMaxValue + a._digits[i] + carry - b._digits[i];
                if (temp >= Base)
                {
                    result[i] = temp - Base;
                    carry = 1;
                }
                else
                {
                    result[i] = temp;
                    carry = 0;
                }

            }
            for (int i = bDigitsCount; i < aDigitsCount; i++)
            {
                temp = DigitMaxValue + a._digits[i] + carry;
                if (temp >= Base)
                {
                    result[i] = temp - Base;
                    carry = 1;
                }
                else
                {
                    result[i] = temp;
                    carry = 0;
                }

            }
            return new Integer(result);
        }

        public static Integer Add(Integer a, Integer b)
        {
            ulong carry = 0;
            ulong temp;
            var aSize = a._digits.Length - 1;
            var bSize = b._digits.Length - 1;
            var aDigitsCount = a._digitCount;
            var bDigitsCount = b._digitCount;
            var newSize = aDigitsCount;

            ulong[] result = new ulong[aDigitsCount + 1];
            for (int i = 0; i < bDigitsCount; i++)
            {
                temp = a._digits[i] + b._digits[i] + carry;
                if (temp >= Base)
                {
                    result[i] = temp - Base;
                    carry = 1;
                }
                else
                {
                    result[i] = temp;
                    carry = 0;
                }
            }
            for (int i = bDigitsCount; i < aDigitsCount; i++)
            {
                temp = a._digits[i] + carry;
                if (temp >= Base)
                {
                    result[i] = temp - Base;
                    carry = 1;
                }
                else
                {
                    result[i] = temp;
                    carry = 0;
                }
            }

            result[a._digitCount] = carry;
            return new Integer(result);


        }

        public static Integer Increase(Integer arg)
        {


            ulong[] result = new ulong[arg._digits.Length + 1];
            arg._digits.CopyTo(result, 0);
            ulong temp = arg._digits[0] + 1;
            if (temp < Base)
            {
                result[0] = temp;
                return new Integer(result);
            }
            result[0] = (temp - Base);
            ulong carry = 1;
            for (int i = 1; i < arg._digitCount; i = i + 1)
            {
                temp = arg._digits[i] + carry;
                if (temp < Base)
                {
                    result[i] = temp;
                    return new Integer(result);
                }
                result[i] = (temp - Base);
                carry = 1;
            }
            result[arg._digitCount] = carry;
            return new Integer(result);

        }


        public static Integer Mul(Integer a, Integer b)
        {


            ulong temp;
            ulong carry;
            var aSize = a._digits.Length - 1;
            var bSize = b._digits.Length - 1;
            var aDigitsCount = a._digitCount;
            var bDigitsCount = b._digitCount;
            var newSize = a._digitCount + b._digitCount - 1;
            var result = new ulong[newSize + 1];
            int index = 0;
            for (int i = 0; i < aDigitsCount; i++)
            {
                carry = 0;
                for (int j = 0; j < bDigitsCount; j++)
                {
                    index = (i + j);
                    temp = result[index] + b._digits[j]*a._digits[i] + carry;
                    carry = temp/Base;
                    result[index] = temp - (carry*Base);
                }

                index = index + 1;
                result[index] = carry;


            }
            return new Integer(result);
        }

        public static Integer ShortMul(Integer a, uint b)
        {

            ulong temp;
            ulong carry = 0;
            var aSize = a._digits.Length - 1;
            var aDigitsCount = a._digitCount;
            var newSize = a._digitCount;
            var result = new ulong[newSize + 1];

            for (int i = 0; i < aDigitsCount; i++)
            {
                temp = b*a._digits[i] + carry;
                carry = temp/Base;
                result[i] = temp - (carry*Base);
            }

            result[aDigitsCount] = carry;
            return new Integer(result);
        }

        private static Integer Div(Integer a, Integer b)
        {
            throw new NotImplementedException();

        }



        public static Integer ShortDiv(Integer a, uint b)
        {
            if (b == 0)
                throw new DivideByZeroException();
            var aSize = a._digits.Length - 1;
            var aDigitsCount = a._digitCount;
            var newSize = aSize;
            var result = new ulong[newSize + 1];


            ulong res = 0;
            for (int i = a._digitCount - 1; i >= 0; i--)
            {
                var temp = a._digits[i] + (res*Base);
                result[i] = temp/b;
                res = temp%b;
            }
            return new Integer(result);
        }

        public static ulong ShortMod(Integer a, uint b)
        {
            if (b == 0)
                throw new DivideByZeroException();
            var aSize = a._digits.Length - 1;
            var aDigitsCount = a._digitCount;




            ulong res = 0;
            for (int i = aDigitsCount - 1; i >= 0; i--)
            {
                var temp = a._digits[i] + (res*Base);
                res = temp%b;
            }
            return res;
        }

        private static Integer Mod(Integer a, Integer b)
        {
            throw new NotImplementedException();
        }



        #endregion




        public override string ToString()
        {
            var result = new StringBuilder();
            var number = this;
            if (number == Zero)
            {
                return "0";
            }
            do
            {
                var r = ShortMod(number, 10);
                result.Append(r.ToString());
                number = ShortDiv(number, 10);
            } while ((number != Zero));

            return new string(result.ToString().ToCharArray().Reverse().ToArray());
        }








        public static implicit operator Integer(uint arg)
        {
            var res = new Integer(arg);
            return res;
        }

        public static implicit operator Integer(int arg)
        {
            if (arg > 0)
            {
                return new Integer((ulong) arg);
            }
            else
            {
                throw new ArgumentException("Число не может быть отрицательным");
            }

        }

        public static implicit operator Integer(ulong arg)
        {
            var res = new Integer(arg);
            return res;
        }

        public static implicit operator Integer(double arg)
        {
            if (arg > 0)
            {
                return new Integer((ulong) arg);
            }
            else
            {
                throw new ArgumentException("Число не может быть отрицательным");
            }
        }




        protected bool Equals(Integer other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Integer) obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var integer = (Integer) obj;
            if (this > integer)
                return 1;
            if (this < integer)
                return -1;
            return 0;
        }

    }
}
