using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GaloisFieldLib.Polynomial_arithmetic;

namespace GaloisFieldLib
{
    public class Matrix
    {
        Polynomial[,] _matrix;
        private FactorRing _field;

        public Matrix(Polynomial[,] matrix,FactorRing field)
        {
            _field = field;
            _matrix = matrix;
        }


        public Polynomial Det()
        {
            var matrix=new Polynomial[_matrix.GetLength(0), _matrix.GetLength(0)];
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int j = 0; j < _matrix.GetLength(0); j++)
                {
                    matrix[i, j] =_matrix[i, j];
                }
            }

            for (int i = 0; i < matrix.GetLength(0)-1; i++)
            {
                var t = i;
                while (matrix[t, i]==Polynomial.Zero)
                {
                    t++;
                    if (t == matrix.GetLength(0))
                    {
                        return Polynomial.Zero;
                    }
                }
                if (t != i)
                {
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        matrix[i, j] = _field.Add(matrix[i, j], matrix[t, j]);
                    }
                }
                var inverse = _field.Inverse(matrix[i, i]);
             
                for (int k = 1; k+i < matrix.GetLength(0); k++)
                {
                    var coeff = matrix[i +k, i];
                    for (int j = i; j < matrix.GetLength(0); j++)
                    {
                        matrix[i + k, j] = _field.Sub(matrix[i + k, j], _field.Mul(_field.Mul(matrix[i, j], inverse), coeff));
                    }
                }
               
               
            }
            var det=matrix[0,0];
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                det = _field.Mul(det, matrix[i, i]);
            }
            return det;
        }

        public List<Polynomial> Solve(List<Polynomial> b)
        {
            var matrix = new Polynomial[_matrix.GetLength(0), _matrix.GetLength(0)];
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int j = 0; j < _matrix.GetLength(0); j++)
                {
                    matrix[i, j] = _matrix[i, j];
                }
            }
            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                var t = i;
                while (matrix[t, i] == Polynomial.Zero)
                {
                    t++;
                    if (t > matrix.GetLength(0))
                    {
                       throw new InvalidOperationException();
                    }
                }
                if (t != i)
                {
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        matrix[i, j] = _field.Add(matrix[i, j], matrix[t, j]);
                        
                    }
                    b[i] = _field.Add(b[i], b[t]);
                }
                var inverse = _field.Inverse(matrix[i, i]);

                for (int k = 1; k + i < matrix.GetLength(0); k++)
                {
                    var coeff = matrix[i + k, i];
                    for (int j = i; j < matrix.GetLength(0); j++)
                    {
                        matrix[i + k, j] = _field.Sub(matrix[i + k, j],
                            _field.Mul(_field.Mul(matrix[i, j], inverse), coeff));
                       
                    }
                    b[i + k] = _field.Sub(b[i + k], _field.Mul(_field.Mul(b[i], inverse), coeff));
                }
            }

            for (int i = matrix.GetLength(0)-1; i > 0; i--)
            {
                var inverse = _field.Inverse(matrix[i, i]);
                for (int k = 1; i-k >=0; k++)
                {
                    var coeff = matrix[i - k, i];
                    for (int j = i; j > 0; j--)
                    {
                        matrix[i - k, j] = _field.Sub(matrix[i - k, j], _field.Mul(_field.Mul(matrix[i, j], inverse), coeff));
                      
                    }
                    b[i - k] = _field.Sub(b[i - k], _field.Mul(_field.Mul(b[i], inverse), coeff));
                }
            }
            var result=new List<Polynomial>();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var inverse = _field.Inverse(matrix[i, i]);
                result.Add(_field.Mul(b[i],inverse));
            }
            return result;

        }
    }
}
