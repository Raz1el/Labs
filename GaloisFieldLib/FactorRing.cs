using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GaloisFieldLib.Integer_arithmetic;
using GaloisFieldLib.Polynomial_arithmetic;

namespace GaloisFieldLib
{
    public class FactorRing
    {
        private Polynomial _mod;
        private ulong _char;
        private ulong _cardinality;

        public ulong Card
        {
            get { return _cardinality; }
        }

        public ulong Char
        {
            get { return _char; }
        }

        public Polynomial Mod
        {
            get { return _mod; }
        }



        public FactorRing(Polynomial polynomial, ulong fieldCharacteristic)
        {
            _mod = polynomial;
            _char = fieldCharacteristic;

            _mod.SetMod(_char);
            _mod.Reduce();

            _cardinality = IntegerMath.Pow(fieldCharacteristic, polynomial.Deg);
        }



        public Polynomial Add(Polynomial firstArg, Polynomial secondArg)
        {
            var result = firstArg + secondArg;
            result = result%_mod;
            result.SetMod(_char);
            result.Reduce();
            return result;

        }

        public Polynomial Inverse(Polynomial poly)
        {
            Polynomial gcd;
            return PolynomialMath.Inverse(poly, _mod, _char, out gcd);
           
        }

    public Polynomial Mul(Polynomial firstArg, Polynomial secondArg)
        {
            var result = firstArg*secondArg;
            result = result%_mod;
            result.SetMod(_char);
            result.Reduce();
            return result;

        }

        public Polynomial Sub(Polynomial firstArg, Polynomial secondArg)
        {
            secondArg.SetMod(_char);
            var result = firstArg - secondArg;

            result.SetMod(_char);
            result.Reduce();
            return result;
        }

        public Polynomial Div(Polynomial firstArg, Polynomial secondArg)
        {
            Polynomial gcd;
            var invert = PolynomialMath.Inverse(secondArg, _mod, _char, out gcd);
            return Mul(firstArg, invert);
        }

        public Polynomial FindMinimalPolynomialOld(Polynomial polynomial)
        {
            var cyclicSet=new List<Polynomial[]>();
          
            cyclicSet.Add(new Polynomial[]{ (_char - 1)*polynomial, new Polynomial(new ulong[] { 1 }) });
            var curr = Pow(polynomial,_char);
            while (curr!=polynomial)
            {
                cyclicSet.Add(new Polynomial[] { (_char-1)*curr, new Polynomial(new ulong[] { 1 }) });
                curr = Pow(curr, _char);
            }


            Polynomial[] result = cyclicSet[0];
            var deg = 1;
            for (int k = 1; k < cyclicSet.Count; k++)
            {
                deg = deg+1;

                Polynomial[] tmp = new Polynomial[deg + 1];
                for (int i = 0; i <= deg-1; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        if (tmp[i + j] == null)
                        {
                            tmp[i + j] = (Mul(result[i], cyclicSet[k][j]));
                        }
                        else
                        {
                            tmp[i + j] = Add(tmp[i + j], (Mul(result[i], cyclicSet[k][j])));
                        }
                       
                    }
                }
                result = tmp;

            }

            
           List<ulong> coefficients=new List<ulong>();
            foreach (var poly in result)
            {
                coefficients.Add(poly[0]);
            }
            return new Polynomial(coefficients.ToArray());
        }

        public Polynomial FindMinimalPolynomial(Polynomial polynomial)
        {
            var cyclicSet = new List<PolynomialOverFiniteField>();

            cyclicSet.Add(new PolynomialOverFiniteField(new Polynomial[] { (_char - 1) * polynomial, new Polynomial(new ulong[] { 1 }) },this));
            var curr = Pow(polynomial, _char);
            while (curr != polynomial)
            {
                cyclicSet.Add(new PolynomialOverFiniteField(new Polynomial[] { (_char - 1) * curr, new Polynomial(new ulong[] { 1 }) },this));
                curr = Pow(curr, _char);
            }


            PolynomialOverFiniteField result = cyclicSet[0];
            var deg = 1;
            for (int k = 1; k < cyclicSet.Count; k++)
            {
                
                result = result*cyclicSet[k];

            }


            List<ulong> coefficients = new List<ulong>();
     
            for (int i = 0; i < result.Deg+1; i++)
            {
                coefficients.Add(result[i][0]);
            }
            return new Polynomial(coefficients.ToArray());
        }

        public Polynomial[,] GetTable()
        {
            Polynomial[,] table=new Polynomial[_cardinality,_cardinality];
            var allElements = GetAllElements();
            for (var i = 0; i < (int) _cardinality; i++)
            {
                table[0, i] = allElements[i];
                table[i, 0] = allElements[i];
            }

            for (var i = 1; i < (int)_cardinality; i++)
            {
                for (var j = 1; j < (int)_cardinality; j++)
                {
                    table[i, j] =Mul( table[i, 0],table[0, j]);
                }
            }
            return table;
        }

        public List<Polynomial> GetAllElements()
        {
            var allPolynomials = new List<Polynomial>();
            for (ulong i = 0; i < _cardinality; i++)
            {
                ulong[] currentPoly = new ulong[_mod.Deg];
                currentPoly[0] = i;
                for (int j = 0; j < _mod.Deg - 1; j++)
                {

                    var carry = currentPoly[j] / _char;
                    currentPoly[j] = currentPoly[j] % _char;
                    currentPoly[j + 1] = carry;
                }
                allPolynomials.Add(new Polynomial(currentPoly));
            }
            return allPolynomials;
        }

        public  Polynomial FindPrimitiveRoot()
        {
            var eulerPhiFactorization = IntegerMath.Factorize(_cardinality - 1);
            var one=new Polynomial(new ulong[]{1});
            var zero = new Polynomial(new ulong[] { 0 });
            var elements = GetAllElements();
            foreach (var g in elements)
            {
                if (g != zero)
                {
                    var tryNextelement = false;
                    foreach (var prime in eulerPhiFactorization.Keys)
                    {
                        if (Pow(g, (_cardinality - 1)/prime) == one)
                        {
                            tryNextelement = true;
                            break;
                        }
                    }
                    if (!tryNextelement)
                    {
                        return g;
                    }
                }
            }
         
            throw new OperationCanceledException("Корень не найден!");

        }

        public List<Polynomial> FindAllPrimitiveRoots()
        {
            List<Polynomial> list = new List<Polynomial>();
            var g = FindPrimitiveRoot();
            list.Add(g);
            for (ulong i = 2; i < _cardinality - 1; i ++)
            {
                var gcd = IntegerMath.Gcd(i, _cardinality - 1);
                if (gcd == 1)
                {
                    var result = Pow(g, i);
                    list.Add(result);
                }
            }
            return list;
        }

        public  Polynomial Pow(Polynomial poly, ulong power)
        {
            if (power == 0)
            {
                return new Polynomial(new ulong[] { 1 });
            }

            var powers = new Dictionary<int, Polynomial>();
            powers.Add(1, poly);
            ulong temp = 2;
            for (int i = 1; (ulong)i < power; i++)
            {
                if (temp > power)
                {
                    break;
                }
                powers.Add(i + 1, Mul(powers.ElementAt((i - 1)).Value , powers.ElementAt((i - 1)).Value));
                temp *= 2;
            }
            var result = new Polynomial(new ulong[] { 1 });
            int powersCount = powers.Count;
            for (int i = 0; i < powersCount; i++)
            {
                if (power % 2 == 1)
                {
                    result = Mul(result , powers.ElementAt(i).Value);
                }
                power = (power / 2);
            }
            return result;

        }

    
    }
}
