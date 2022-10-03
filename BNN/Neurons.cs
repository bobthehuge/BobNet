using System;
using System.Data;

namespace BNN
{
    public class Neuron
    {
        public Matrix Weights;
        public double Bias;

        #region init

        public Neuron(Matrix X)
        {
            this.Weights = new Matrix(X.cols, X.rows , -10, 10);
            this.Bias = Matrix.RandomDouble(-5, 5);
        }
        
        public Neuron(int rows, int cols)
        {
            this.Weights = new Matrix(rows, cols , -10, 10);
            this.Bias = Matrix.RandomDouble(-5, 5);
        }

        public Neuron(Neuron another)
        {
            this.Weights = another.Weights.Clone();
            this.Bias = another.Bias;
        }

        #endregion

        public void print_data()
        {
            Console.WriteLine("weights : ");
            Console.WriteLine(this.Weights.ToString());
            Console.WriteLine($"bias : {this.Bias}");
        }
    }
}