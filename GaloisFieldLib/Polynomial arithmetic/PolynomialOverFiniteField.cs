using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GaloisFieldLib.Integer_arithmetic;

namespace GaloisFieldLib.Polynomial_arithmetic
{
    public class PolynomialOverFiniteField
    {
        private FactorRing _field;
        private int Size;
        private int _deg;
        private Polynomial _mod;
        private Polynomial[] _coefficients;
    

        public PolynomialOverFiniteField(Polynomial[] arr,FactorRing field)
        {
            Size = 4;
            _field = field;
            _mod = field.Mod;
            _deg = arr.Length - 1;
            for (int i = _deg; i >= 0; i--)
            {
                if (arr[i] == Polynomial.Zero|| (arr[i] ==null))
                    _deg--;
                else
                    break;
            }



            var length = _deg + 1;
            if (length > Size)
            {
                int pow = (int)Math.Log(length, 2);
                Size = (int)Math.Pow(2, pow);
                if (length > Size)
                {
                    Size *= 2;
                }
                else
                {
                    Size = length;
                }
            }

            _coefficients = new Polynomial[Size];


            for (int i = 0; i <= _deg; i++)
            {
                _coefficients[i] = arr[i] % field.Mod;
            }
        }

        public PolynomialOverFiniteField(Polynomial poly, FactorRing field)
        {
            Size = 4;
            _field = field;
            _mod = field.Mod;
            _deg = poly.Deg;
            for (int i = _deg; i >= 0; i--)
            {
                if (poly[i] == 0 || (poly[i] == null))
                    _deg--;
                else
                    break;
            }



            var length = _deg + 1;
            if (length > Size)
            {
                int pow = (int)Math.Log(length, 2);
                Size = (int)Math.Pow(2, pow);
                if (length > Size)
                {
                    Size *= 2;
                }
                else
                {
                    Size = length;
                }
            }

            _coefficients = new Polynomial[Size];


            for (int i = 0; i <= _deg; i++)
            {
                _coefficients[i] = new Polynomial(new ulong[] { poly[i] % field.Char});
            }
        }

        public int Deg
        {
            get { return _deg; }
        }
        public Polynomial this[int index]
        {
            get { return _coefficients[index]; }
        }
        public void Reduce()
        {
            for (int i = 0; i <= _deg; i++)
            {
                _coefficients[i] %= _mod;
            }
            if (_deg == -1)
                return;
            while (_coefficients[_deg] == Polynomial.Zero|| _coefficients[_deg] == null)
            {
                _deg--;
                if (_deg < 0)
                    return;
            }
        }
        void Resize(int newSize)
        {
            var oldSize = Size;

            int pow = (int)Math.Log(newSize, 2);//ближайшая степень двойки, превосходящая длинну массива
            Size = (int)Math.Pow(2, pow);
            if (newSize > Size)
            {
                Size *= 2;
            }
            var newArray = new Polynomial[Size];
            for (int i = 0; i < Size && i < _coefficients.Length; i++)
            {
                newArray[i] = _coefficients[i];
            }




            if (newSize < oldSize)
            {
                for (int i = Size; i < _coefficients.Length; i++)
                {
                    if (_coefficients[i] != Polynomial.Zero)
                    {
                        throw new InvalidOperationException("При изменении размера произошла потеря информации!");
                    }
                }

            }
            _coefficients = newArray;
        }
        public override string ToString()
        {
            var res = new StringBuilder();
            if (_deg == -1)
            {
                return "0";
            }
            for (int index = _deg; index >= 0; index--)
            {
                if (_coefficients[index] ==Polynomial.Zero)
                    continue;


                if (_coefficients[index] == Polynomial.One)
                {
                    if (index == _deg)
                    {
                        if (index == 0)
                        {
                            res.Append("1");
                        }
                        else if (index == 1)
                        {
                            res.Append("Y");
                        }
                        else
                        {
                            res.Append("Y^" + index);
                        }
                    }
                    else
                    {
                        if (index == 0)
                        {
                            res.Append("+1");
                        }
                        else if (index == 1)
                        {
                            res.Append("+" + "Y");
                        }
                        else
                        {
                            res.Append("+" + "Y^" + index);
                        }

                    }
                }
                else
                {

                    if (index == _deg)
                    {
                        if (index == 0)
                        {
                            res.Append(_coefficients[index]);
                        }
                        else if (index == 1)
                        {
                            res.Append("("+_coefficients[index] + ")Y");
                        }
                        else
                        {
                            res.Append("("+_coefficients[index] + ")Y^" + index);
                        }
                    }
                    else
                    {
                        if (index == 0)
                        {
                            res.Append("+" + _coefficients[index]);
                        }
                        else if (index == 1)
                        {
                            res.Append("+(" + _coefficients[index] + ")Y");
                        }
                        else
                        {
                            res.Append("+(" + _coefficients[index] + ")Y^" + index);
                        }

                    }

                }

            }
            return res.ToString();
        }
        public ulong[] ToArray()
        {
            return (ulong[])_coefficients.Clone();
        }
    
      
      
        #region Operators
        public static bool operator ==(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            if (ReferenceEquals(firstArg, null) && ReferenceEquals(secondArg, null))
            {
                return true;
            }
            if (ReferenceEquals(firstArg, null) || ReferenceEquals(secondArg, null))
            {
                return false;
            }
            if (firstArg.Deg != secondArg.Deg)
            {
                return false;
            }
            for (int i = 0; i <= firstArg.Deg; i++)
            {
                if (firstArg._coefficients[i] != secondArg._coefficients[i])
                {
                    return false;
                }
            }
            return true;
        }
        public static bool operator !=(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            return !(firstArg == secondArg);
        }
        public static PolynomialOverFiniteField operator +(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            return Add(firstArg, secondArg);
        }
        public static PolynomialOverFiniteField operator -(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            return Sub(firstArg, secondArg);
        }
        public static PolynomialOverFiniteField operator *(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            return Mul(firstArg, secondArg);
        }

   
        public static PolynomialOverFiniteField operator /(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            if (secondArg._deg == -1)
                throw new DivideByZeroException();
            if (firstArg._deg < secondArg._deg)
                return new PolynomialOverFiniteField(new Polynomial[] {new Polynomial(), },secondArg._field );

            return Div(firstArg, secondArg);
        }
        public static PolynomialOverFiniteField operator %(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            if (secondArg._deg == -1)
                throw new DivideByZeroException();
            if (firstArg._deg < secondArg._deg)
                return new PolynomialOverFiniteField(firstArg._coefficients,firstArg._field);

            return Rem(firstArg, secondArg);
        }
        public static PolynomialOverFiniteField Mul(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            int firstDeg = firstArg._deg;
            int secondDeg = secondArg._deg;
            var field = secondArg._field;
            if (firstDeg == -1 || secondDeg == -1)
                return new PolynomialOverFiniteField(new Polynomial[] { Polynomial.Zero },secondArg._field);
            int maxdegree = firstDeg + secondDeg;
            Polynomial[] result = new Polynomial[maxdegree + 1];
            for (int i = 0; i <= firstDeg; i++)
            {
                for (int j = 0; j <= secondDeg; j++)
                {
                    if (result[i + j] != null)
                    {
                        result[i + j] = field.Add(result[i + j], (field.Mul(firstArg[i], secondArg[j])));
                    }
                    else
                    {
                        result[i + j] = (field.Mul(firstArg[i], secondArg[j]));
                    }
                }
            }
            return new PolynomialOverFiniteField(result,field);
        }
        public static PolynomialOverFiniteField Div(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            var field = secondArg._field;
            var firstDeg = firstArg.Deg;
            var secondDeg = secondArg.Deg;
            var newDeg = firstDeg - secondDeg;
            var result = new Polynomial[newDeg + 1];
            var reminder = (Polynomial[])firstArg._coefficients.Clone();
            var mod = secondArg._mod;
            Polynomial firstLc;
            Polynomial secondLc = secondArg[secondDeg];

            for (int i = 0; i <= newDeg; i++)
            {
                firstLc = reminder[firstDeg - i];
                result[newDeg - i] = field.Div(firstLc ,secondLc);
                for (int j = 0; j <= secondDeg; j++)
                {
                    if (reminder[firstDeg - secondDeg + j - i] != null)
                    {
                        reminder[firstDeg - secondDeg + j - i] = field.Add
                            (reminder[firstDeg - secondDeg + j - i], (field.Mul(result[newDeg - i], secondArg[j])));
                    }
                    else
                    {
                        reminder[firstDeg - secondDeg + j - i] =  (field.Mul(result[newDeg - i], secondArg[j]));
                    }
                }
            }
            return new PolynomialOverFiniteField(result,field);

        }
        public static PolynomialOverFiniteField Rem(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {

            var field = secondArg._field;
            var firstDeg = firstArg.Deg;
            var secondDeg = secondArg.Deg;
            var newDeg = firstDeg - secondDeg;
            var result = new Polynomial[newDeg + 1];
            var reminder = (Polynomial[])firstArg._coefficients.Clone();
            var mod = secondArg._mod;
            Polynomial firstLc;
            Polynomial secondLc = secondArg[secondDeg];

            for (int i = 0; i <= newDeg; i++)
            {
                firstLc = reminder[firstDeg - i];
                result[newDeg - i] = field.Div(firstLc, secondLc);
                for (int j = 0; j <= secondDeg; j++)
                {
                    if (reminder[firstDeg - secondDeg + j - i] != null)
                    {
                        reminder[firstDeg - secondDeg + j - i] = field.Add
                            (reminder[firstDeg - secondDeg + j - i], (field.Mul(result[newDeg - i], secondArg[j])));
                    }
                    else
                    {
                        reminder[firstDeg - secondDeg + j - i] = (field.Mul(result[newDeg - i], secondArg[j]));
                    }
                }
            }
            return new PolynomialOverFiniteField(reminder,field);

        }
        public static PolynomialOverFiniteField Sub(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            int firstDeg = firstArg._deg;
            int secondDeg = secondArg._deg;
            var field = secondArg._field;
       

            var maxDeg = Math.Max(firstDeg, secondDeg);
            if (maxDeg == -1)
            {
                return new PolynomialOverFiniteField(new Polynomial[] {Polynomial.Zero},field);
            }

            var result = new Polynomial[maxDeg + 1];
            if (firstDeg > secondDeg)
            {
                int i;
                for (i = 0; i <= secondDeg; i++)
                {
                    result[i] = field.Sub(firstArg[i] , secondArg[i]);
                }
                for (; i <= firstDeg; i++)
                {
                    result[i] = firstArg[i];
                }
            }
            else
            {
                int i;
                for (i = 0; i <= firstDeg; i++)
                {
                    result[i] = field.Sub( firstArg[i] , secondArg[i]);
                }
                for (; i <= secondDeg; i++)
                {
                    result[i] = field.Sub(Polynomial.Zero, secondArg[i]);
                }

            }

            return new PolynomialOverFiniteField(result,field);
        }
        public static PolynomialOverFiniteField Add(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            int firstDeg = firstArg._deg;
            int secondDeg = secondArg._deg;
            var field = secondArg._field;
            var maxDeg = Math.Max(firstDeg, secondDeg);
            if (maxDeg == -1)
            {
                return new PolynomialOverFiniteField(new Polynomial[] { Polynomial.Zero }, field);
            }
            var result = new Polynomial[maxDeg + 1];
            if (firstDeg > secondDeg)
            {
                int i;
                for (i = 0; i <= secondDeg; i++)
                {
                    result[i] = field.Add(firstArg[i] , secondArg[i]);
                }
                for (; i <= firstDeg; i++)
                {
                    result[i] = firstArg[i];
                }
            }
            else
            {
                int i;
                for (i = 0; i <= firstDeg; i++)
                {
                    result[i] = field.Add(firstArg[i] , secondArg[i]);
                }
                for (; i <= secondDeg; i++)
                {
                    result[i] = secondArg[i];
                }

            }

            return new PolynomialOverFiniteField(result,field);
        }

        public Polynomial Value(Polynomial point)
        {
            Polynomial bn, bn_1=_coefficients[0];

            bn = this[Deg];
            for (int i = 1; i <= Deg; i++)
            {
                bn_1 = _field.Add(_field.Mul(point,bn),this[Deg-i]);
                bn = bn_1;
            }
            return bn_1;
        }

        #endregion

        public override bool Equals(object obj)
        {
            return this == (PolynomialOverFiniteField)obj;
        }
    }
}
