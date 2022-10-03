using System;
using System.Linq;
using System.Numerics;
using NumSharp;

namespace BNN
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Matrix data_in = new Matrix(4, 2);
            data_in.values = new double[][]
            {
                new double[]{0,0},
                new double[]{0,1},
                new double[]{1,0},
                new double[]{1,1}
            };

            Matrix data_exp = new Matrix(4, 1);
            data_exp.values = new double[][]
            {
                new double[]{0},
                new double[]{1},
                new double[]{1},
                new double[]{0}
            };

            Neuron in1 = new Neuron(data_in);
            Neuron in2 = new Neuron(data_in);

            Neuron hid1 = new Neuron(2, 2);
            Neuron hid2 = new Neuron(1, 2);

            Neuron out1 = new Neuron(data_in);

            Neuron[] neurons = new Neuron[] {hid1, hid2};

            for (int i = 0; i < 4; i++)
            {
                var act = for_prop(data_in, neurons);
                var grads = back_prop(data_in, data_exp, act, neurons);
                neurons = update(grads, neurons);
            }
            
            Console.WriteLine(for_prop(data_in, neurons)[1][0][0] >= 0.5);
        }

        public static Matrix[] for_prop(Matrix X, Neuron[] neurons)
        {
            var W1 = neurons[0].Weights;
            var b1 = neurons[0].Bias;
            
            var W2 = neurons[1].Weights;
            var b2 = neurons[1].Bias;

            var Z1 = W1 * X + b1;
            var A1 = Z1.Sigmoid();

            var Z2 = W2 * A1 + b2;
            var A2 = Z2.Sigmoid();

            return new Matrix[] {A1, A2};
        }

        public static Tuple<Matrix[], double[]> back_prop(Matrix X, Matrix y, Matrix[] act, Neuron[] neurons)
        {
            var A1 = act[0];
            var A2 = act[1];
            
            var W1 = neurons[0].Weights;
            var b1 = neurons[0].Bias;
            
            var W2 = neurons[1].Weights;
            var b2 = neurons[1].Bias;

            Console.WriteLine($"{A2.ToNdArray().shape[0]} : {A2.ToNdArray().shape[1]}");
            Console.WriteLine($"{y.ToNdArray().shape[0]} : {y.ToNdArray().shape[1]}");
            
            var dZ2 = A2 - y;
            var dW2 = (double) 1 / y.cols * (dZ2 * A1.T());
            var db2 = (double) 1 / y.cols * dZ2.Sum(1);
            
            var dZ1 = W2.T() * dZ2 * A1 * (1 - A1);
            var dW1 = (double) 1 / y.cols * (dZ1 * X.T());
            var db1 = (double) 1 / y.cols * dZ1.Sum(1);

            return new Tuple<Matrix[], double[]>(new Matrix[] {dW1, dW2}, new double[] {db1[0][0], db2[0][0]});
        }

        public static Neuron[] update(Tuple<Matrix[], double[]> grads, Neuron[] neurons, double rate = 0.1)
        {
            neurons[0].Weights -= rate * grads.Item1[0];
            neurons[0].Bias -= rate * grads.Item2[0];

            neurons[1].Weights -= rate * grads.Item1[1];
            neurons[1].Bias -= rate * grads.Item2[1];

            return neurons;
        }
    }
}