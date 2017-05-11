using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using GaloisFieldLib;
using GaloisFieldLib.Integer_arithmetic;
using GaloisFieldLib.Polynomial_arithmetic;
using GaloisFieldLib.Сyclic_codes;

namespace Test
{
    static class Program
    {
        static void Main(string[] args)
        {
            //var field=new FactorRing(new Polynomial(new ulong[] { 1, 1, 1 }),3 );
            //Polynomial[,] p=new Polynomial[2,2];
            //p[0,0]=new Polynomial(new ulong[] {2});
            //p[0, 1] = new Polynomial(new ulong[] { 2 });
            //p[1, 0] = new Polynomial(new ulong[] { 2 });
            //p[1, 1] = new Polynomial(new ulong[] { 0 });
            //Matrix m=new Matrix(p,field);
            //var x = m.Solve();
            //Console.ReadKey();
            DivTest(11);
            Dictionary<int, Polynomial> table = new Dictionary<int, Polynomial>()
            {
                {2,new Polynomial(new ulong[] {1,1,1}) },
                 {3,new Polynomial(new ulong[] {1,0,1,1}) },
                  {4,new Polynomial(new ulong[] {1,1,0,0,1}) },
                   {5,new Polynomial(new ulong[] {1,0,0,1,0,1}) },
                    {6,new Polynomial(new ulong[] {1,0,0,0,0,1,1}) },
            };



            var generator=new Polynomial(new ulong[] {1,1,0,1});
            BchCode bch=new BchCode(20,7,generator);
            var msg1 = new Polynomial(new ulong[] {1});
            var msg2 = bch.Code(msg1);
            var error= new Polynomial(new ulong[] { 2,2,2,3,4,5,6,1,2,3,4,5,1,2,3,4,5,6,1,2});
            var msg3 = error+msg2;
            msg3.SetMod(7);
            msg3.Reduce();
            var msg4 = bch.Decode(msg3);


            Console.WriteLine("Введите характеристику поля:");
            Console.WriteLine("Введите порождающий полином:\n");



           

            Console.WriteLine("Введите сообщение:" + msg1);
            Console.WriteLine("Закодированное сообщение:" + msg2);

            Console.WriteLine("\nВведите вектор ошибки:" + error);
            Console.WriteLine("Принятое сообщение:" + msg3);
            Console.WriteLine("Исправленное сообщение"+msg4);
            Console.ReadKey();
            
        }

        public static void PseudoPrimeTest()
        {
            var count = 0;
            for (ulong i = 2; i < 100; i++)
            {

                if (IntegerMath.IsStrongPseudoprime(i,50))
                {
                     Console.WriteLine(i);
                    count++;
                }
            }
            Console.WriteLine(count);
        }

        static void PrimitiveRootTest()
        {
            ulong p;
            Console.Write("p= ");
            p = ulong.Parse(Console.ReadLine());

            try
            {
                var result = IntegerMath.FindPrimitiveRoot(p);
                Console.WriteLine("Примитивным корнем по модулю {0} является число {1}", p, result);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
          
            Console.ReadKey();
        }


        static void FftTest()
        {
            Stopwatch timer=new Stopwatch();
            var arr = new ulong[] {0,1,2,3,4,5,6,7};
            var poly = new Polynomial(arr);
            Console.WriteLine();
            Console.WriteLine(poly);
            timer.Start();
            poly.SlowFt();
            timer.Stop();

       
            Console.WriteLine(poly);
            poly.SlowInverseFt();
            Console.WriteLine(poly + " " );
            Console.WriteLine();
            Console.WriteLine(" time: " + timer.ElapsedTicks);
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(poly);
            timer.Reset();
            timer.Start();
            poly.Fft();
            timer.Stop();
            Console.WriteLine(poly);
            poly.InverseFft();
            Console.WriteLine(poly+" ");
            Console.WriteLine(" time: " + timer.ElapsedTicks);


  
     
        }

        static void FftMulTest()
        {

            Stopwatch timer = new Stopwatch();
            var firstPoly = GeneratePoly(1000);


            var secondPoly = GeneratePoly(1000);


            timer.Start();
            var res = Polynomial.FftMul(secondPoly , firstPoly);
            timer.Stop();
            Console.WriteLine(timer.ElapsedTicks + " fft multiply");
            timer.Reset();
            timer.Start();
            var res2 = Polynomial.Mul(secondPoly,firstPoly);
            timer.Stop();
            Console.WriteLine(timer.ElapsedTicks + " school multiply");

            //Console.WriteLine(firstPoly + " deg:" + firstPoly.Deg);
            //Console.WriteLine(secondPoly + " deg:" + secondPoly.Deg);
            //Console.WriteLine(res + " deg:" + res.Deg);
            //Console.WriteLine(res2 + " deg:" + res.Deg);

            //Console.WriteLine(" deg:" + firstPoly.Deg);
            //Console.WriteLine(" deg:" + secondPoly.Deg);
            //Console.WriteLine(" deg:" + res.Deg);
            if(res!=res2)
            {
                Console.WriteLine("ERROR!");
            }



        }
        public static void Do(long n)
        {

            Random rnd = new Random();
            Stopwatch timer = new Stopwatch();

            long i = n;
            long mscount = 0;
            long mycount = 0;
            long myt = 0;
            long mst = 0;
            while (i > 0)
            {
                var a = GenerateNumber(rnd.Next(1,200));
                var b = GenerateNumber(rnd.Next(1, 200));
                Integer c = new Integer(a.ToString());
                Integer d = new Integer(b.ToString());



                timer.Start();


                var res = a *b;

                timer.Stop();
                var ms = timer.Elapsed.Ticks;

                timer.Reset();

                timer.Start();
                var myRes =c*d;
                timer.Stop();
                var my = timer.Elapsed.Ticks;
                myt += my;
                mst += ms;
                if (ms < my)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    mscount++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    mycount++;
                }


                Console.Write("[{" + i + "}]My:" + my);
                Console.WriteLine("|    Microsoft:" + ms);

                timer.Reset();
                Console.ResetColor();
                if (a.ToString() != c.ToString() || b.ToString() != d.ToString())
                {

                    //LongInteger mod = LongInteger.Parse(LongInteger.MaxValue.ToString())+1;
                    //var amod = a%mod;
                    //var bmod = b%mod ;
                    //if ((amod).ToString() != c.ToString())
                    //{
                    //    Console.WriteLine("ERROR!");
                    //}
                    //if ((bmod).ToString() != d.ToString())
                    //{
                    //    Console.WriteLine("ERROR!");
                    //}
                    Console.WriteLine("ERROR!");
                }

                if (myRes.ToString() != res.ToString())
                {

                    Console.WriteLine("ERROR!");


                }

                i--;


            }
            if (mscount > mycount)
            {
                Console.ForegroundColor = ConsoleColor.Red;

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine("Microsoft " + mscount + " " + mst / (double)n);
            Console.WriteLine("My " + mycount + "  " + myt / (double)n);

            Console.ReadKey();

        }
        static BigInteger GenerateNumber(int n)
        {
            RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[n];
            generator.GetBytes(bytes);
            return BigInteger.Abs(new BigInteger(bytes));
        }

        static Polynomial GeneratePoly(int n)
        {
            
            var arr = new ulong[n];
            for(int i=0;i<n;i++)
            {
                arr[i] = (ulong)generator.Next();
            }

            return new Polynomial(arr);

        }
        public static Random generator = new Random();
        static void DivTest(ulong mod)
        {
            var firstPoly = GeneratePoly(4);
            var secondPoly = GeneratePoly(3);

            secondPoly.SetMod(mod);
            firstPoly.SetMod(mod);
            firstPoly.Reduce();
            secondPoly.Reduce();
          
            
            while(secondPoly.Deg==-1)
            {//Генерируем делитель до тех пор, пока не получим мн-н не равный нулю (deg 0 = -1)
                secondPoly = GeneratePoly(3);
                secondPoly.SetMod(mod);
                secondPoly.Reduce();
            }

            var res = firstPoly/secondPoly;
            var rem = firstPoly % secondPoly;

            Console.WriteLine(firstPoly+"=("+secondPoly+")*("+res+")+("+rem+") mod " + mod);


            var check = rem + res*secondPoly;
            check.SetMod(mod);
            check.Reduce();
            if (firstPoly != check)
            {
                Console.Clear();
                Console.WriteLine(check);
                Console.WriteLine(firstPoly);
                throw new Exception();
            }

        }
        static void MulTest(ulong mod)
        {
            var firstPoly = GeneratePoly(2);
            var secondPoly = GeneratePoly(3);

            secondPoly.SetMod(mod);
            firstPoly.SetMod(mod);
            firstPoly.Reduce();
            secondPoly.Reduce();
           

            Polynomial res;
            res = firstPoly * secondPoly;

            res.SetMod(mod);
            res.Reduce();
           
         
            

            Console.WriteLine(res + "=(" + firstPoly + ")*(" + secondPoly +") mod " + mod);

        }

        static void AddTest(ulong mod)
        {
            var firstPoly = GeneratePoly(2);
            var secondPoly = GeneratePoly(3);

            secondPoly.SetMod(mod);
            firstPoly.SetMod(mod);
            firstPoly.Reduce();
            secondPoly.Reduce();


            Polynomial res;
            res = firstPoly + secondPoly;

            res.SetMod(mod);
            res.Reduce();



            Console.WriteLine(res + "=(" + firstPoly + ")+(" + secondPoly + ") mod " + mod);

        }
        static void SubTest(ulong mod)
        {
            var firstPoly = GeneratePoly(2);
            var secondPoly = GeneratePoly(3);

            secondPoly.SetMod(mod);
            firstPoly.SetMod(mod);
            firstPoly.Reduce();
            secondPoly.Reduce();


            Polynomial res;
            res = firstPoly -secondPoly;

            res.SetMod(mod);
            res.Reduce();



            Console.WriteLine(res + "=(" + firstPoly + ")-(" + secondPoly + ") mod "+mod);

        }

        static void GcdTest(ulong mod)
        {
            var firstPoly = GeneratePoly(2);
            var secondPoly = GeneratePoly(3);


            Polynomial res;
            res = PolynomialMath.Gcd(firstPoly,secondPoly,mod);





            Console.WriteLine("GCD(" + firstPoly + ";" + secondPoly + ") = " + res +"  (Z"+mod+")");

        }
      
       
       
    }
}
