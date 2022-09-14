using System;
using System.Collections;

namespace BNN
{
    public class Matrix
    {
        public double[][] arr;
        
        #region INIT

        public Matrix(int rows, int cols, int rnd_min = 0, int rnd_max = 0)
        {
            this.arr = init_matrix(rows, cols, rnd_min, rnd_max);
        }

        public Matrix(double[][] arr)
        {
            this.arr = (double[][])arr.Clone();
        }

        private static double[] create_line(int length, int max, int min)
        {
            Random rnd = new Random();
            var res = new double[length];

            for (int i = 0; i < length; i++)
                res[i] = RandomDouble(min, max);

            return res;
        }
        
        private static double[][] init_matrix(int rows, int cols, int rnd_min = 0, int rnd_max = 0)
        {
            var newMat = new double[rows][];

            for (var i = 0; i < rows; i++)
                if (rnd_min != rnd_max)
                    newMat[i] = create_line(cols, rnd_max, rnd_min);
                else
                    newMat[i] = new double[cols];

            return newMat;
        }
        #endregion

        #region  Index Handler

        public double[] this[int i]
        {
            get => arr[i];
            set => arr[i] = value;
        }
        public double this[int i, int j]
        {
            get => arr[i][j];
            set => arr[i][j] = value;
        }

        #endregion

        #region Operators

        public static Matrix operator +(Matrix A, Matrix B) => add(A, B);
        public static Matrix operator +(Matrix A, double n) => add(A, n);
        
        public static Matrix operator -(Matrix A, Matrix B) => sub(A, B);
        public static Matrix operator -(Matrix A, double n) => sub(A, n);
        
        public static Matrix operator *(Matrix A, Matrix B) => mult(A, B);
        public static Matrix operator *(Matrix A, double n) => mult(A, n);

        #endregion

        #region Maths

        private static Matrix broadcast(int rows, double[] line)
        {
            var newMat = new Matrix(rows, 1);

            for (var i = 0; i < rows; i++)
                newMat[i] = (double[]) line.Clone();

            return newMat;
        }
        private static Matrix add(Matrix A, Matrix B)
        {
            if (B.rows == 1)
                B = broadcast(A.rows, B[0]);
            
            for (int j = 0; j < A.rows; j++)
            for (int i = 0; i < A[0].Length; i++)
                A[i][j] += B[i][j];

            return A;
        }
        private static Matrix add(Matrix M, double x)
        {
            int n = M.rows;
            int m = M.cols;
            
            for (int j = 0; j < m; j++)
            for (int i = 0; i < n; i++)
                M[i][j] = x + M[i][j];

            return M;
        }
        private static Matrix sub(Matrix A, Matrix B)
        {
            for (int j = 0; j < A.cols; j++)
            for (int i = 0; i < A.rows; i++)
                A[i][j] -= B[i][j];

            return A;
        }
        private static Matrix sub(Matrix M, double x)
        {
            for (int j = 0; j < M.cols; j++)
            for (int i = 0; i < M.rows; i++)
                M[i][j] = x - M[i][j];

            return M;
        }
        private static Matrix mult(Matrix A, Matrix B)
        {
            Matrix res = new Matrix(A.rows, B.cols);
            
            for (int j = 0; j < B.cols; j++)
            for (int i = 0; i < A.rows; i++)
            for (int k = 0; k < A.cols; k++)
                res[i][j] += A[i][k] * B[k][j];

            return res;
        }
        private static Matrix mult(Matrix A, double x)
        { 
            for (int j = 0; j < A.cols; j++)
            for (int i = 0; i < A.rows; i++)
                A[i][j] *= x;

            return A;
        }
        
        public static Matrix Transpose(Matrix M)
        {
            var res = new Matrix(M.cols, M.rows);
            
            for (int j = 0; j < M.cols; j++)
            for (int i = 0; i < M.rows; i++)
                res[j][i] = M[i][j];

            return res;
        }
        
        public static Matrix Sigmoid(Matrix M)
        {
            for (int j = 0; j < M.cols; j++)
            for (int i = 0; i < M.rows; i++)
                M[i][j] = 1d / (1d + Math.Exp(-M[i][j]));

            return M;
        }
        
        public static Matrix Sum(Matrix A)
        {
            var res = new Matrix(1, A.cols);
            
            for (int j = 0; j < A.cols; j++)
            for (int i = 0; i < A.rows; i++)
                res[0][j] += A[i][j];

            return res;
        }

        #endregion

        #region UTILS
        
        public int rows => arr.Length;
        public int cols => arr.Length > 0 ? arr[0].Length : 0;
        
        public static double RandomDouble(int min, int max)
        {
            return new Random().NextDouble() * new Random().Next(min, max);
        }

        public override string ToString()
        {
            return matrix_to_string(this);
        }

        public Matrix Clone()
        {
            return new Matrix(this.arr);
        }
        private static string array_to_string(double[] arr, string end = "\n", string start = "")
        {
            string res = String.Empty;
            res += start + $"[{arr[0]}";

            if (arr.Length == 1)
            {
                res += "]" + end;
                return res;
            }

            for(var i = 1; i < arr.Length - 1; i++)
                res += $", {arr[i]}";
            
            return res += $", {arr[^1]}]" + end;
        }
        private static string matrix_to_string(Matrix mat, string end = "\n", string start = " ")
        {
            string res = String.Empty;
            res += array_to_string(mat[0], mat.rows == 1 ? "]" : "," + end, "[");
            
            if (mat.rows == 1) return res;

            for (var i = 1; i < mat.rows - 1; i++)
                res += array_to_string(mat[i],"," + end, start);
            
            return res += array_to_string(mat[mat.rows - 1],"]" + end, start);
        }

        #endregion

    }
}