using System;
using System.Data;

namespace BNN
{
    public class neuron
    {
        public Matrix Weights;
        public double Bias;

        #region init

        public neuron(Matrix X)
        {
            this.Weights = new Matrix(X.cols, 1, 100);
            this.Bias = Matrix.RandomDouble(-5, 5);
        }

        public neuron(neuron another)
        {
            this.Weights = another.Weights.Clone();
            this.Bias = another.Bias;
        }

        #endregion
        
        public static Matrix model(Matrix X, Matrix W, double b)
        {
            return Matrix.Sigmoid(X * W + b);
        }
        
        public static Tuple<Matrix, Matrix> gradients(Matrix m, Matrix inp, Matrix exp)
        {
            var n = 1d / exp.cols;
            var dW = Matrix.Transpose(inp) * (m - exp) * n;

            var db = Matrix.Sum(m - exp) * n;

            return new Tuple<Matrix, Matrix>(dW, db);
        }

        public void update(Matrix dW, Matrix db, double learning_rate)
        {
            this.Weights -= dW * learning_rate;
            this.Bias -= db[0][0] * learning_rate;
        }

        public void print_data()
        {
            Console.WriteLine("weights : ");
            Console.WriteLine(this.Weights.ToString());
            Console.WriteLine($"bias : {this.Bias}");
        }
    }
}