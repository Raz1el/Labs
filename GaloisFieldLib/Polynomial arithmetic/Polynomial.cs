using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GaloisFieldLib.Integer_arithmetic;

namespace GaloisFieldLib
{
    public class Polynomial
    {
        static ulong Mod = 3221225473;          //основное поле - Z3221225473; (3221225473=3*2^30+1)
        static ulong Root = 125;                 // 5^(3*2^30)=1=>(5^3) - корень 2^30 степени из 1
        static int Power = 1073741824;          //Степень корня из 1 (1073741824=2^30). Для любого числа вида 2^k,  (125)^(2^k) - корень 2^(30-k) степени из 1
        static ulong Root_1 = 2267742733;       // 125^(-1)
        static ulong Inverse = 1610612737;      //2^(-1)
        static int DefaultSize = 4;

        private int Size = DefaultSize;
        private int _deg;
        private ulong _mod =Mod;
        private ulong[] _coefficients;

        public static Polynomial Zero { get { return new Polynomial(new ulong[] { 0}); } }
        public static Polynomial One { get { return new Polynomial(new ulong[] { 1 }); } }

        public Polynomial(ulong[] arr)
        {
            _deg = arr.Length - 1;
            for (int i = _deg; i >=0; i--)
            {
                if (arr[i] == 0)
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

            _coefficients = new ulong[Size];


            for (int i = 0; i <= _deg; i++)
            {
                _coefficients[i] = arr[i] % Mod;
            }
        }
        public Polynomial(int size)
        {
            if (size > Size)
            {
                int pow = (int)Math.Log(size, 2);
                Size = (int)Math.Pow(2, pow);
                if (size > Size)
                {
                    Size *= 2;
                }
                else
                {
                    Size = size;
                }
            }
          
            _deg = -1;
            _coefficients = new ulong[Size];
            
        }
        public Polynomial()
        {
            _deg = -1;
            _coefficients = new ulong[Size];

        }
        public int Deg
        {
            get { return _deg; }
        }
        public ulong this[int index]
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
            while (_coefficients[_deg] == 0)
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
            var newArray = new ulong[Size];
            for (int i = 0; i < Size&&i<_coefficients.Length; i++)
            {
                newArray[i] = _coefficients[i];
            }
          



             if (newSize < oldSize)
            {
                for (int i = Size; i < _coefficients.Length; i++)
                {
                    if( _coefficients[i]!=0)
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
                if (_coefficients[index] == 0)
                    continue;


                if (_coefficients[index] == 1)
                {
                    if (index == _deg)
                    {
                        if (index == 0)
                        {
                            res.Append("1");
                        }
                        else if (index == 1)
                        {
                            res.Append("x");
                        }
                        else
                        {
                            res.Append("x^" + index);
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
                            res.Append("+" + "x");
                        }
                        else
                        {
                            res.Append("+" + "x^" + index);
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
                            res.Append(_coefficients[index] + "x");
                        }
                        else
                        {
                            res.Append(_coefficients[index] + "x^" + index);
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
                            res.Append("+" + _coefficients[index] + "x");
                        }
                        else
                        {
                            res.Append("+" + _coefficients[index] + "x^" + index);
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
        public void SetMod(ulong mod)
        {
            _mod = mod;
        }
        public void ResetMod()
        {
            _mod = Mod;
        }
        #region Fourier transform
        public void SlowFt()
        {
            _coefficients=SlowFt(_coefficients);
        }
        public void SlowInverseFt()
        {
            _coefficients = SlowInverseFt(_coefficients);
        }
        static ulong[] SlowFt(ulong[] vector)
        {
            
            var size = vector.Length;
            var res = new ulong[size];
            ulong pow = (ulong)(Power / size);
            ulong root_n = IntegerMath.ModPow(Root, pow, Mod);
            ulong root_i = 1;
            for (int i = 0; i < size; i++)
            {
                ulong temp = 0;
                ulong root_j = 1;
                for (int j = 0; j < size; j++)
                {
                    temp = (temp + (vector[j] * root_j) % Mod) % Mod;
                    root_j = (root_j * root_i) % Mod;
                }
                res[i] = temp;
                root_i = (root_i * root_n) % Mod;

            }
            return res;
        }
        static ulong[] SlowInverseFt(ulong[] vector)
        {
            var size = vector.Length;
            var res = new ulong[size];
            ulong pow = (ulong)(Power / size);
            ulong root_n = IntegerMath.ModPow(Root_1, pow, Mod);
            ulong root_i = 1;
            for (int i = 0; i < size; i++)
            {
                double temp = 0;
                ulong root_j = 1;
                for (int j = 0; j < size; j++)
                {
                    temp = (temp + (vector[j] * root_j) % Mod) % Mod;
                    root_j = (root_j * root_i) % Mod;
                }
                res[i] =(ulong) (temp*IntegerMath.Inverse((ulong)size,Mod))%Mod;
                root_i = (root_i * root_n) % Mod;

            }
            return res;
        }

        public void Fft()
        {
            _coefficients=Fft(_coefficients);
        }
        public void InverseFft()
        {
            _coefficients=InverseFft(_coefficients);
        }

        static ulong[] Fft(ulong[] vector)
        {
            var size = vector.Length;
            if(size==1)
                return vector;
            var evenVector = new ulong[size/2];
            var oddVector = new ulong[size / 2];
            for (int i = 0; i < size/2; i ++)
            {
                evenVector[i] = vector[2*i];
                oddVector[i] = vector[2 * i + 1];
            }
            evenVector=Fft(evenVector);
            oddVector= Fft(oddVector);
            ulong pow = (ulong)(Power/size);
            ulong root_n = IntegerMath.ModPow(Root,pow,Mod);
            ulong w = 1;
            var result = new ulong[size];
            for (int i = 0; i < size/2; i++)
            {
                result[i] = (evenVector[i] + (w * oddVector[i]) % Mod) % Mod;
                result[i + size / 2] = (evenVector[i] + Mod - (w * oddVector[i]) % Mod) % Mod;

                w = (root_n*w)%Mod;
            }
            return result;
        }

        static ulong[] InverseFft(ulong[] vector)
        {
            var size = vector.Length;
            if (size == 1)
                return vector;
            var evenVector = new ulong[size / 2];
            var oddVector = new ulong[size / 2];
            for (int i = 0; i < size/2; i ++)
            {
                evenVector[i] = vector[2 * i];
                oddVector[i] = vector[2 * i + 1];
            }
            evenVector=InverseFft(evenVector);
            oddVector= InverseFft(oddVector);
            ulong pow = (ulong)(Power / size);
            ulong root_n = IntegerMath.ModPow(Root_1, pow, Mod);
            ulong w = 1;

            var result = new ulong[size];
            for (int i = 0; i < size / 2; i++)
            {
                result[i] = (Inverse * (evenVector[ i] + (w * oddVector[ i]) % Mod) % Mod) % Mod;
                result[i + size / 2] = 
                    (Inverse * (evenVector[i] + Mod - (w * oddVector[i]) % Mod) % Mod) % Mod;

                w = (root_n * w) % Mod;
            }
            return result;
        }

#endregion
        #region Operators
        public static bool operator ==(Polynomial firstArg, Polynomial secondArg)
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
        public static bool operator !=(Polynomial firstArg, Polynomial secondArg)
        {
            return !(firstArg == secondArg);
        }
        public static Polynomial operator +(Polynomial firstArg, Polynomial secondArg)
        {
            return  Add(firstArg, secondArg);
        }
        public static Polynomial operator -(Polynomial firstArg, Polynomial secondArg)
        {
            return Sub(firstArg, secondArg);
        }
        public static Polynomial operator *(Polynomial firstArg,Polynomial secondArg)
        {
            return Mul(firstArg, secondArg);
        }
        public static Polynomial operator *(Polynomial firstArg, ulong secondArg)
        {
            return ConstMul(firstArg,secondArg);
        }
        public static Polynomial operator *( ulong firstArg, Polynomial secondArg)
        {
            return ConstMul(secondArg, firstArg);
        }
        public static Polynomial operator /(Polynomial firstArg, Polynomial secondArg)
        {
            if (secondArg._deg == -1)
                throw new DivideByZeroException();
            if (firstArg._deg < secondArg._deg)
                return new Polynomial();

            return Div(firstArg, secondArg);
        }
        public static Polynomial operator %(Polynomial firstArg, Polynomial secondArg)
        {
            if (secondArg._deg == -1)
                throw new DivideByZeroException();
            if (firstArg._deg < secondArg._deg)
                return new Polynomial(firstArg._coefficients);

            return Rem(firstArg, secondArg);
        }
        public static Polynomial Mul(Polynomial firstArg, Polynomial secondArg)
        {
            int firstDeg = firstArg._deg;
            int secondDeg = secondArg._deg;
            if(firstDeg==-1||secondDeg==-1)
                return new Polynomial(new ulong[] {0});
            int maxdegree = firstDeg + secondDeg;
            ulong[] result = new ulong[maxdegree+1];
            for (int i = 0; i <= firstDeg; i++)
            {
                for (int j = 0; j <= secondDeg; j++)
                {
                    result[i + j] = (result[i + j] + (firstArg[i] * secondArg[j]) % Mod) % Mod;
                }
            }
            return new Polynomial(result);
        }
        public static Polynomial FftMul(Polynomial firstArg, Polynomial secondArg)
        {
            var maxDeg = secondArg.Deg;
            if (secondArg.Deg < firstArg.Deg)
            {
                maxDeg = firstArg.Deg;
            }
            var pow = 0;
            if (maxDeg != 0)
            {
                pow = (int)(Math.Log(2 * maxDeg, 2) + 1);
            }
            var newSize = (int)Math.Pow(2, pow);
            if(newSize<DefaultSize)
            {
                newSize = DefaultSize;
            }
            else
            {
                firstArg.Resize(newSize);
                secondArg.Resize(newSize);
            }
            var firstFt = Fft(firstArg._coefficients);
            var secondFt = Fft(secondArg._coefficients);
            var ftResult = new ulong[newSize];
            for (int i = 0; i < newSize; i++)
            {
                ftResult[i] = firstFt[i] * secondFt[i] % Mod;
            }
            var result = InverseFft(ftResult);
            return new Polynomial(result);
        }
        public static Polynomial Div(Polynomial firstArg, Polynomial secondArg)
        {
            
            var firstDeg = firstArg.Deg;
            var secondDeg = secondArg.Deg;
            var newDeg=firstDeg - secondDeg;
            var result = new ulong[newDeg+1];
            var reminder = (ulong[])firstArg._coefficients.Clone();
            var mod=secondArg._mod;
            ulong firstLc;
            ulong secondLc = secondArg[secondDeg];

            for (int i = 0; i <= newDeg; i++)
            {
                firstLc = reminder[firstDeg - i];
                result[newDeg - i] = (firstLc * IntegerMath.Inverse(secondLc, mod))%mod;
                for (int j = 0; j <= secondDeg; j++)
                {
                    reminder[firstDeg-secondDeg+j-i]=
                        (reminder[firstDeg-secondDeg+j-i]+(mod-(result[newDeg - i]*secondArg[j])%mod))%mod;
                }
            }
            return new Polynomial(result);

        }
        public static Polynomial Rem(Polynomial firstArg, Polynomial secondArg)
        {

            var firstDeg = firstArg.Deg;
            var secondDeg = secondArg.Deg;
            var newDeg = firstDeg - secondDeg;
            var result = new ulong[newDeg + 1];
            var reminder = (ulong[])firstArg._coefficients.Clone();
            var mod = secondArg._mod;


            ulong firstLc;
            ulong secondLc = secondArg[secondDeg];

            for (int i = 0; i <= newDeg; i++)
            {
                firstLc = reminder[firstDeg - i];
                result[newDeg - i] = (firstLc * IntegerMath.Inverse(secondLc, mod)) % mod;
                for (int j = 0; j <= secondDeg; j++)
                {
                    reminder[firstDeg - secondDeg + j - i] = 
                        (reminder[firstDeg - secondDeg + j - i] + (mod - (result[newDeg - i] * secondArg[j]) % mod)) % mod;
                }
            }
            return new Polynomial(reminder);

        }
        public static Polynomial Sub(Polynomial firstArg, Polynomial secondArg)
        {
            int firstDeg = firstArg._deg;
            int secondDeg = secondArg._deg;
            ulong mod = secondArg._mod;

            var maxDeg = Math.Max(firstDeg, secondDeg);
            if (maxDeg == -1)
            {
                return new Polynomial(new ulong[1]);
            }

            var result = new ulong[maxDeg+1];
            if (firstDeg > secondDeg)
            {
                int i;
                for (i = 0; i <= secondDeg; i++)
                {
                    result[i] = (mod + firstArg[i] - secondArg[i]) % mod;
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
                    result[i] = (mod + firstArg[i] - secondArg[i]) % mod;
                }
                for (; i <= secondDeg; i++)
                {
                    result[i] = (mod-secondArg[i])%mod;
                }

            }

            return new Polynomial(result);
        }
        public static Polynomial Add(Polynomial firstArg, Polynomial secondArg)
        {
            int firstDeg = firstArg._deg;
            int secondDeg = secondArg._deg;
            var maxDeg = Math.Max(firstDeg, secondDeg);
            if(maxDeg==-1)
            {
                return new Polynomial(new ulong[1]);
            }
            var result = new ulong[maxDeg+1];
            if (firstDeg > secondDeg)
            {
                int i;
                for (i = 0; i <= secondDeg; i++)
                {
                    result[i] = (firstArg[i] + secondArg[i]) % Mod;
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
                    result[i] = (firstArg[i] + secondArg[i]) % Mod;
                }
                for (; i <= secondDeg; i++)
                {
                    result[i] = secondArg[i];
                }

            }

            return new Polynomial(result);
        }
        public static Polynomial ConstMul(Polynomial firstArg, ulong number)
        {
            ulong[] result = (ulong[])firstArg._coefficients.Clone() ;
            for (int i = 0; i <= firstArg._deg; i++)
            {
                result[i] = (firstArg[i] * number) % Mod;
            }
            return new Polynomial(result);
        }

        #endregion
        public ulong Value(ulong point)
        {
            ulong bn, bn_1=0;

            bn = this[Deg];
            for (int i = 1; i <= Deg; i++)
            {
                bn_1 =point* bn+ this[Deg - i];
                bn = bn_1;
            }
            return bn_1;
        }
        public override bool Equals(object obj)
        {
            return this==(Polynomial)obj;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
