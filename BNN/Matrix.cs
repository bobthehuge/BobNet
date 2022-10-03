using System;
using System.Collections;
using NumSharp;

namespace BNN
{
    public class Matrix
    {
        public double[][] values;

        #region INIT

        public Matrix(int rows, int cols, int rnd_min = 0, int rnd_max = 0)
        {
            this.values = init_matrix(rows, cols, rnd_min, rnd_max);
        }

        public Matrix(double[][] arr)
        {
            this.values = (double[][]) arr.Clone();
        }
        public Matrix(NDArray arr)
        {
            this.values = init_matrix(arr.shape[0], arr.shape[1]);
            
            for(int i = 0; i < arr.shape[0]; i++)
            for (int j = 0; j < arr.shape[1]; j++)
                this.values[i][j] = arr.GetData(new[] {i, j});
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

        #region Index Handler

        public double[] this[int i]
        {
            get => values[i];
            set => values[i] = value;
        }

        public double this[int i, int j]
        {
            get => values[i][j];
            set => values[i][j] = value;
        }

        #endregion

        #region Operators

        public static Matrix operator +(Matrix A, Matrix B) 
            => new Matrix(A.ToNdArray() + B.ToNdArray());
        public static Matrix operator +(Matrix A, double n) => add(A, n);
        public static Matrix operator +(double n, Matrix A) => add(A, n);

        public static Matrix operator -(Matrix A, Matrix B)
            => new Matrix(A.ToNdArray() - B.ToNdArray());
        public static Matrix operator -(Matrix A, double n) => add(A, -n);
        public static Matrix operator -(double n, Matrix A) => sub(A, n);

        public static Matrix operator *(Matrix A, Matrix B) => mult(A, B);
        public static Matrix operator *(Matrix A, double n) => mult(A, n);
        public static Matrix operator *(double n, Matrix A) => mult(A, n);

        #endregion

        #region Maths

        private static Tuple<Matrix, Matrix> broadcast(Matrix A, Matrix B)
        {
            return null;
        }

        private static Matrix add(Matrix A, Matrix B)
        {
            var tmp = broadcast(A,B);
            A = tmp.Item1;
            B = tmp.Item2;

            for (int i = 0; i < A.rows; i++)
            for (int j = 0; j < A.cols; j++)
                A[i][j] += B[i][j];

            return A;
        }

        private static Matrix add(Matrix M, double x)
        {
            for (int i = 0; i < M.rows; i++)
            for (int j = 0; j < M.cols; j++)
                M[i][j] = x + M[i][j];

            return M;
        }

        private static Matrix sub(Matrix A, Matrix B)
        {
            var tmp = broadcast(A,B);
            A = tmp.Item1;
            B = tmp.Item2;

            for (int i = 0; i < A.rows; i++)
            for (int j = 0; j < A.cols; j++)
                A[i][j] -= B[i][j];


            return A;
        }

        private static Matrix sub(Matrix M, double x)
        {
            for (int i = 0; i < M.rows; i++)
            for (int j = 0; j < M.cols; j++)
                M[i][j] = x - M[i][j];

            return M;
        }

        private static Matrix mult(Matrix A, Matrix B)
        {
            Matrix res = new Matrix(A.rows, B.cols);

            for (int i = 0; i < A.rows; i++)
            for (int j = 0; j < A.cols; j++)
            for (int k = 0; k < A.cols; k++)
                res[i][j] += A[i][k] * B[k][j];

            return res;
        }

        private static Matrix mult(Matrix A, double x)
        {
            for (int i = 0; i < A.rows; i++)
            for (int j = 0; j < A.cols; j++)
                A[i][j] *= x;

            return A;
        }

        public Matrix T()
        {
            var res = new Matrix(this.cols, this.rows);

            for (int i = 0; i < this.rows; i++)
            for (int j = 0; j < this.cols; j++)
                res[j][i] = this[i][j];

            return res;
        }

        public Matrix Sigmoid()
        {
            for (int i = 0; i < this.rows; i++)
            for (int j = 0; j < this.cols; j++)
                this[i][j] = 1d / (1d + Math.Exp(-this[i][j]));

            return this;
        }

        public Matrix Sum(int axis = 0)
        {
            if (axis == 0)
            {
                var res = new Matrix(1, this.cols);

                for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    res[0][j] += this[i][j];

                return res;
            }
            else
            {
                var res = new Matrix(this.rows, 1);

                for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    res[i][0] += this[i][j];

                return res;
            }
        }

        #endregion

        #region UTILS

        public int rows => values.Length;
        public int cols => values.Length > 0 ? values[0].Length : 0;
        public NDArray ToNdArray() => new NDArray(this.values);
        public static double RandomDouble(int min, int max) => new Random().NextDouble() * new Random().Next(min, max);
        public override string ToString() => matrix_to_string(this);
        public Matrix Clone() => new Matrix(this.values);
        private static string array_to_string(double[] arr, string end = "\n", string start = "")
        {
            string res = String.Empty;
            res += start + $"[{arr[0]}";

            if (arr.Length == 1)
            {
                res += "]" + end;
                return res;
            }

            for (var i = 1; i < arr.Length - 1; i++)
                res += $", {arr[i]}";

            return res += $", {arr[^1]}]" + end;
        }
        private static string matrix_to_string(Matrix mat, string end = "\n", string start = " ")
        {
            string res = String.Empty;
            res += array_to_string(mat[0], mat.rows == 1 ? "]" : "," + end, "[");

            if (mat.rows == 1) return res;

            for (var i = 1; i < mat.rows - 1; i++)
                res += array_to_string(mat[i], "," + end, start);

            return res += array_to_string(mat[mat.rows - 1], "]" + end, start);
        }

        #endregion
    }
}