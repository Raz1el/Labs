using System;
using System.Collections.Generic;
using System.Linq;

namespace GaloisFieldLib.Integer_arithmetic
{
    public static class IntegerMath
    {
        private static Random rnd = new Random();

        public static Integer Pow(Integer number, int power)
        {
            if (power == 0)
            {
                return Integer.One;
            }

            var powers = new Dictionary<int, Integer>();
            powers.Add(1, number);
            int temp = 2;
            for (int i = 1; i < power; i++)
            {
                if (temp > power)
                {
                    break;
                }
                powers.Add(i + 1, powers.ElementAt((i - 1)).Value*powers.ElementAt((i - 1)).Value);
                temp *= 2;
            }
            var result = Integer.One;
            int powersCount = powers.Count;
            for (int i = 0; i < powersCount; i++)
            {
                if (power%2 == 1)
                {
                    result = result*powers.ElementAt(i).Value;
                }
                power = (power/2);
            }
            return result;

        }


        public static ulong Pow(ulong number, int power)
        {
            if (power == 0)
            {
                return 1;
            }

            var powers = new Dictionary<int, ulong>();
            powers.Add(1, number);
            int temp = 2;
            for (int i = 1; i < power; i++)
            {
                if (temp > power)
                {
                    break;
                }
                powers.Add(i + 1, powers.ElementAt((i - 1)).Value * powers.ElementAt((i - 1)).Value);
                temp *= 2;
            }
            ulong result = 1;
            int powersCount = powers.Count;
            for (int i = 0; i < powersCount; i++)
            {
                if (power % 2 == 1)
                {
                    result = result * powers.ElementAt(i).Value;
                }
                power = (power / 2);
            }
            return result;

        }

        public static bool IsStrongPseudoprime(ulong number, ulong iteratios)
        {
            if (number < 2)
                return false;
            if (number == 2 || number == 3)
                return true;

            ulong q = number - 1;
            uint s = 0;
            while (q%2 == 0)
            {
                s++;
                q = q/2;
            }
            for (ulong i = 0; i < iteratios; i++)
            {
                var b = GenerateNumber(1, number - 1);
                b = ModPow(b, q, number);
                if (b == 1 || b == number - 1)
                    continue;
                bool nextStep = false;
                for (int j = 0; j < s; j++)
                {
                    b = ModPow(b, 2, number);
                    if (b == number - 1)
                    {
                        nextStep = true;
                        break;
                    }
                }
                if (nextStep)
                    continue;
                return false;


            }
            return true;


        }

        public static ulong ModPow(ulong number, ulong power, ulong modulo)
        {
            if (power == 0)
                return 1;
            ulong res = 1;
            ulong a = number%modulo;
            while (power != 0)
            {

                if (power%2 != 0)
                {
                    res = (res*a)%modulo;
                }
                a = (a*a)%modulo;
                power >>= 1;
            }
            return res;
        }

        private static ulong GenerateNumber(ulong lowerBound, ulong upperBound)
        {
            var res = (ulong) rnd.Next()%(upperBound + 1);
            while (res < lowerBound)
            {
                res = (ulong) rnd.Next()%(upperBound + 1);
            }

            return res;
        }

        public static Dictionary<ulong, ulong> Factorize(ulong number)
        {
            var result = new Dictionary<ulong, ulong>();


            while (!IsStrongPseudoprime(number, 50))
            {
                var factor = FindFactor(number);
                if (result.ContainsKey(factor))
                {
                    result[factor]++;
                }
                else
                {
                    result.Add(factor, 1);
                }
                number /= factor;
            }
            if (result.ContainsKey(number))
            {
                result[number]++;
            }
            else
            {
                result.Add(number, 1);
            }


            return result;
        }

        private static ulong FindFactor(ulong number)
        {
            if (number%2 == 0)
            {
                return 2;
            }
            for (ulong i = 3; i*i <= number; i = i + 2)
            {
                if (number%i == 0)
                {
                    return i;
                }
            }
            return number;
        }

        public static ulong FindPrimitiveRoot(ulong modulo)
        {

            if (!IsStrongPseudoprime(modulo, 50))
            {
                throw new OperationCanceledException("Модуль не простой!");
            }
            if (modulo == 2)
                return 1;
            var eulerPhiFactorization = Factorize(modulo - 1);
            for (ulong g = 2; g < modulo; g++)
            {
                var tryNextelement = false;
                foreach (var prime in eulerPhiFactorization.Keys)
                {
                    if (ModPow(g, (modulo - 1)/prime, modulo) == 1)
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
            throw new OperationCanceledException("Корень не найден!");

        }

        public static List<ulong> FindAllPrimitiveRoots(ulong modulo)
        {
            List<ulong> list = new List<ulong>(); 
           var g = FindPrimitiveRoot(modulo);
            list.Add(g);
            for (ulong i = 3; i < modulo - 1; i+=2)
            {
                var gcd=Gcd(i, modulo-1);
                if (gcd == 1)
                {
                    var result = ModPow(g, i, modulo);
                    list.Add(result);
                }
            }
            return list;
        }

        public static ulong Gcd(ulong firstArg, ulong secondArg)
        {
            while (firstArg != 0)
            {
                ulong temp = firstArg;
                firstArg = secondArg % firstArg;
                secondArg = temp;
            }
            return secondArg;
        }

        public static Integer Inverse(Integer number, Integer mod, out Integer gcd)
        {

            Integer bezoutCoefficient = Integer.One;
            Integer nextBezoutCoefficient = Integer.Zero;
            gcd = number % mod;
            Integer reminder = mod;
            Integer temp = Integer.Zero;
            Integer quotient;


            while (reminder != Integer.Zero)
            {
                temp = reminder;
                quotient = gcd / reminder;
                reminder = (gcd + (mod - (quotient * reminder) % mod)) % mod;
                gcd = temp;
                temp = nextBezoutCoefficient;
                nextBezoutCoefficient = (bezoutCoefficient + (mod - (quotient * nextBezoutCoefficient) % mod)) % mod;
                bezoutCoefficient = temp;


            }
            gcd = gcd % mod;
            return bezoutCoefficient % mod;



        }

        public static ulong Inverse(ulong number, ulong mod)
        {

            ulong bezoutCoefficient = 1;
            ulong nextBezoutCoefficient = 0;
            ulong gcd = number % mod;
            ulong reminder = mod;
            ulong temp = 0;
            ulong quotient;
               

            while (reminder != 0)
            {
                temp = reminder;
                quotient = gcd / reminder;
                reminder = (gcd+(mod- (quotient * reminder)%mod))% mod;
                gcd = temp;
                temp = nextBezoutCoefficient;
                nextBezoutCoefficient = (bezoutCoefficient +( mod - (quotient * nextBezoutCoefficient) % mod)) % mod;
                bezoutCoefficient = temp;


            }
            gcd = gcd % mod;
            if(gcd!=1)
                throw new ArgumentException(string.Format("Аргумент mod не простой! Обратный элемент к {0} не существует!",number));
            return bezoutCoefficient % mod;



        }
    }
}