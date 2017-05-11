using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GaloisFieldLib.Integer_arithmetic;
using GaloisFieldLib.Polynomial_arithmetic;

namespace GaloisFieldLib.Сyclic_codes
{
    public class BchCode
    {
        private ulong _fieldChar;
        private int _errorsNumber;
        private ulong _codeLength;
        private FactorRing _field;
        private Polynomial _primitiveRoot;
        private Dictionary< Polynomial,int> _codeZeros;
        private Dictionary<int,Polynomial> _logTable;
        private Dictionary< Polynomial,int> _invertLogTable;
        Polynomial _generator;
        public ulong CodeLength{ get { return _codeLength; } }
        public ulong NumberOfInformationSymbols { get { return _codeLength -(ulong) _generator.Deg; } }
        public int ErrorNumber { get { return _errorsNumber; } }

        public BchCode(int errorsNumber, ulong fieldChar,Polynomial fieldGenerator)
        {
            _fieldChar = fieldChar;
            _codeLength = IntegerMath.Pow(fieldChar,fieldGenerator.Deg)-1;
            _errorsNumber = errorsNumber;
            _field=new FactorRing(fieldGenerator,fieldChar);
            _primitiveRoot = _field.FindPrimitiveRoot();
            _logTable=new Dictionary<int,Polynomial>();
            _invertLogTable=new Dictionary<Polynomial, int>();
            _logTable.Add(0,Polynomial.One);
            _invertLogTable.Add( Polynomial.One,0);
            for (int i = 1; (ulong)i < _field.Card-1; i++)
            {
                    var t = _field.Pow(_primitiveRoot, (ulong) i);
                    _logTable.Add(i, _field.Pow(_primitiveRoot, (ulong)i));
                    _invertLogTable.Add(_logTable[i], i);
            }
            _codeZeros = new Dictionary< Polynomial,int>();
            for (int i=1;i<2*errorsNumber+1;i++)
            {
                _codeZeros.Add(_logTable[i],i);
            }
            List<Polynomial> minimalPolynomials=new List<Polynomial>();
            _generator=new Polynomial(new ulong[] {1});
            foreach (var polynomial in _codeZeros)
            {
                var minPoly=_field.FindMinimalPolynomial(polynomial.Key);
                if (!minimalPolynomials.Contains(minPoly))
                {
                    minimalPolynomials.Add(minPoly);
                    _generator = _generator*minPoly;
                    _generator.SetMod(_fieldChar);
                    _generator.Reduce();
                }
            }

        }
        public Polynomial Code(Polynomial message)
        {
            if (message.Deg > (int)NumberOfInformationSymbols - 1)
            {
                throw new InvalidOperationException("Недопустимая длинна!");
            }
            var codeWord = message*_generator;
            codeWord.SetMod(_fieldChar);
            codeWord.Reduce();
            return codeWord;
        }
        public Polynomial Decode(Polynomial message)
        {
            Polynomial result=message;

            var syndroms = new List<Polynomial>();
            var nonZeroSyndromsCount = 0;
            foreach (var polynomial in _codeZeros)
            {
                var s = Syndrome(message, polynomial.Key);
                syndroms.Add(Syndrome(message, polynomial.Key));
                if (s != Polynomial.Zero)
                {
                    nonZeroSyndromsCount++;
                }
            }
            if (nonZeroSyndromsCount != 0)
            {
                var errorNumber = _errorsNumber;
                var matrix = BuildMatrix(syndroms, errorNumber);
                while (matrix.Det() == Polynomial.Zero)
                {
                    errorNumber--;
                    matrix = BuildMatrix(syndroms, errorNumber);
                }
                var b = new List<Polynomial>();
                for (int i = errorNumber; i < 2 * errorNumber; i++)
                {
                    b.Add(_field.Sub(Polynomial.Zero, syndroms[i]));
                }
                var coefficients = matrix.Solve(b);
                coefficients.Insert(0, Polynomial.One);


                var roots_1 = FindRoots(coefficients);
                var roots = new List<Polynomial>();

                foreach (var root in roots_1)
                {
                    roots.Add(_field.Inverse(root));
                }

                b.Clear();
                for (int i = 0; i < errorNumber; i++)
                {
                    b.Add(syndroms[i]);
                }

                matrix = BuildMatrixForFindErrorValues(roots);
                var errorValues = matrix.Solve(b);
                var errorVector = new ulong[_codeLength];
                for (int i = 0; i < roots.Count; i++)
                {
                    var index = _invertLogTable[roots[i]];
                    errorVector[index] = errorValues[i][0];
                }
                var error = new Polynomial(errorVector);
                error.SetMod(_fieldChar);
                result = message - error;
                result.SetMod(_fieldChar);
                result.Reduce();
            }


            return result/_generator;
        }
        private List<Polynomial> FindRoots(List<Polynomial> coefficients)
        {
            var poly=new PolynomialOverFiniteField(coefficients.ToArray(),_field);
            var points = _field.GetAllElements();
            var result =new List<Polynomial>();
            var i = 0;
            while (result.Count!=poly.Deg)
            {

                if (poly.Value(points[i]) == Polynomial.Zero)
                {
                    result.Add(points[i]);
                }
                i++;
            }
            return result;
        }
        public Polynomial Syndrome(Polynomial message,Polynomial point)
        {
            var word=new PolynomialOverFiniteField(message,_field);
            return word.Value(point);
        }
        Matrix BuildMatrix(List<Polynomial> syndroms,int errorNumber)
        {
            var syndromsMatrix = new Polynomial[errorNumber, errorNumber];
            for (int i = 0; i < errorNumber; i++)
            {
                for (int j = 0; j < errorNumber; j++)
                {
                    syndromsMatrix[i, j] = syndroms[errorNumber - 1 + i - j];
                }
            }
            return  new Matrix(syndromsMatrix, _field);
        }
        Matrix BuildMatrixForFindErrorValues(List<Polynomial> roots)
        {
            var matrix = new Polynomial[roots.Count, roots.Count];
            for (int i = 0; i < roots.Count; i++)
            {
                for (int j = 0; j < roots.Count; j++)
                {
                    matrix[i, j] = _field.Pow(roots[j],(ulong)i+1);
                }
            }
            return new Matrix(matrix, _field);
        }
    }
}
