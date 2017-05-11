using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GaloisFieldLib.Integer_arithmetic;

namespace GaloisFieldLib.Polynomial_arithmetic
{
    public class PolynomialMath
    {

        public static Polynomial Gcd(Polynomial firstArg, Polynomial secondArg, ulong fieldCharacteristic)
        {
            firstArg.SetMod(fieldCharacteristic);
            secondArg.SetMod(fieldCharacteristic);
            firstArg.Reduce();
            secondArg.Reduce();
            if(firstArg.Deg==-1)
            {
                if(secondArg.Deg==-1)
                {
                    throw new InvalidOperationException("Gcd не определен");
                }
                return secondArg;
            }

            if (secondArg.Deg == -1)
            {
                if (firstArg.Deg == -1)
                {
                    throw new InvalidOperationException("Gcd не определен");
                }
                return firstArg;
            }
            while (firstArg.Deg != -1)
            {
                firstArg.SetMod(fieldCharacteristic);
                secondArg.SetMod(fieldCharacteristic);
                firstArg.Reduce();
                secondArg.Reduce();
                Polynomial temp = firstArg;
                firstArg = secondArg % firstArg;
                secondArg = temp;
            }
            return secondArg;
        }


        public static Polynomial Inverse(Polynomial polynomial, Polynomial mod, ulong fieldCharacteristic, out Polynomial gcd)
        {
            polynomial.SetMod(fieldCharacteristic);
            mod.SetMod(fieldCharacteristic);
            polynomial.Reduce();
            mod.Reduce();
            if (mod.Deg == -1 || polynomial.Deg == -1)
            {
                    throw new InvalidOperationException("Обратный элемент не определен");
            }
            Polynomial bezoutCoefficient = new Polynomial(new ulong[]{1});
            Polynomial nextBezoutCoefficient = new Polynomial();
            gcd = polynomial % mod;
            Polynomial reminder = mod;
            Polynomial temp = new Polynomial();
            Polynomial quotient;
            gcd.SetMod(fieldCharacteristic);
            gcd.Reduce();
            bezoutCoefficient.SetMod(fieldCharacteristic);
            bezoutCoefficient.Reduce();
            nextBezoutCoefficient.SetMod(fieldCharacteristic);
            nextBezoutCoefficient.Reduce();
            while (reminder.Deg != -1)
            {
                temp = reminder;
                quotient = gcd / reminder;
                quotient.SetMod(fieldCharacteristic);
                quotient.Reduce();
                var qxr = quotient * reminder;
                qxr.SetMod(fieldCharacteristic);
                qxr.Reduce();
                reminder = gcd - qxr;
                reminder.SetMod(fieldCharacteristic);
                reminder.Reduce();
                gcd = temp;
                temp = nextBezoutCoefficient;
                var qxu = quotient * nextBezoutCoefficient;
                qxu.SetMod(fieldCharacteristic);
                qxu.Reduce();
                nextBezoutCoefficient = bezoutCoefficient - qxu;
                nextBezoutCoefficient.SetMod(fieldCharacteristic);
                nextBezoutCoefficient.Reduce();
                bezoutCoefficient = temp;
            }
            gcd = gcd % mod;
            gcd.SetMod(fieldCharacteristic);
            gcd.Reduce();
            var c = gcd[0];
            var inverse = IntegerMath.Inverse(c, fieldCharacteristic);
            if (c != 1)
            {
                gcd = inverse * gcd;
                gcd.SetMod(fieldCharacteristic);
                gcd.Reduce();
            }
            if (gcd != new Polynomial(new ulong[] {1}))
                throw new ArgumentException(string.Format("Обратный элемент к {0} не существует!", polynomial));
            var res= (inverse * bezoutCoefficient) % mod;
            res.SetMod(fieldCharacteristic);
            res.Reduce();
            return res;
        }


       
    }
}
