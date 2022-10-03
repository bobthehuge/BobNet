using System;
using System.Collections.Generic;
using NumSharp;
using np = NumSharp.np;

namespace NumTests
{
    class Program
    {
        static void Main(string[] args)
        {
            double[][] data_sample = new double[][]
            {
                new double[]{0, 0},
                new double[]{0, 1},
                new double[]{1, 0},
                new double[]{1, 1}
            };

            double[][] data_exp = new double[][]
            {
                new double[] {0},
                new double[] {1},
                new double[] {1},
                new double[] {0}
            };

            var X = new NDArray(data_sample);
            var y = new NDArray(data_exp);

            X = X.T;
            var y2 = y.reshape(1, y.shape[0]);

            var vals = neural_network(X, y2, n1: 2);
            
            Console.WriteLine(predict(X, y2, vals));
        }

        static Dictionary<string, NDArray> init(int n0, int n1, int n2)
        {
            var W1 = np.random.randn(new int[] {n1, n0}) *10;
            var b1 = np.random.randn(new int[] {n1, 1}) *10;
            
            Console.WriteLine(W1.ToString());

            var W2 = np.random.randn(new int[] {n2, n1})*10;
            var b2 = np.random.randn(new int[] {n2, 1})*10;

            return new Dictionary<string, NDArray>()
            {
                {"W1", W1},
                {"b1", b1},
                {"W2", W2},
                {"b2", b2}
            };
        }

        static Dictionary<string, NDArray> for_prop(NDArray X, Dictionary<string, NDArray> param)
        {
            var W1 = param["W1"];
            var b1 = param["b1"];
            var W2 = param["W2"];
            var b2 = param["b2"];

            var Z1 = W1.dot(X) + b1;
            var A1 = 1 / (1 + np.exp(0 - Z1));

            var Z2 = W2.dot(A1) + b2;
            var A2 = 1 / (1 + np.exp(0 - Z2));

            return new Dictionary<string, NDArray>()
            {
                {"A1", A1},
                {"A2", A2}
            };
        }

        static Dictionary<string, NDArray> back_prop(NDArray X, NDArray y, Dictionary<string, NDArray> param,
            Dictionary<string, NDArray> activs)
        {
            var A1 = activs["A1"];
            var A2 = activs["A2"];
            var W2 = param["W2"];

            var m = (double)y.shape[1];

            var dZ2 = A2 - y;
            var dW2 = 1 / m * dZ2.dot(A1.T);
            var db2 = 1 / m * ColSum(dZ2);

            var dZ1 = np.dot(W2.T, dZ2) * A1 * (1 - A1);
            var dW1 = 1 / m * dZ1.dot(X.T);
            var db1 = 1 / m * ColSum(dZ1);

            return new Dictionary<string, NDArray>()
            {
                {"dW1", dW1},
                {"db1", db1},
                {"dW2", dW2},
                {"db2", db2}
            };
        }

        static Dictionary<string, NDArray> update(Dictionary<string, NDArray> grads, Dictionary<string, NDArray> param,
            double rate)
        {
            var W1 = param["W1"];
            var b1 = param["b1"];
            var W2 = param["W2"];
            var b2 = param["b2"];

            var dW1 = grads["dW1"];
            var db1 = grads["db1"];
            var dW2 = grads["dW2"];
            var db2 = grads["db2"];

            W1 -= rate * dW1;
            b1 -= rate * db1;
            W2 -= rate * dW2;
            b2 -= rate * db2;

            return new Dictionary<string, NDArray>()
            {
                {"W1", W1},
                {"b1", b1},
                {"W2", W2},
                {"b2", b2}
            };
        }

        static bool predict(NDArray X, NDArray exp, Dictionary<string, NDArray> param, bool silent = true)
        {
            var activs = for_prop(X, param);
            var A2 = activs["A2"];
            
            if(!silent)
                Console.WriteLine(A2.ToString());

            Console.WriteLine(flatten(A2).ToString());
            Console.WriteLine(exp.ToString());
            
            return DeepVerif(exp, flatten(A2));
        }

        static Dictionary<string, NDArray> neural_network(NDArray X, NDArray y, int n1 = 4, double rate = 0.1,
            int iter = 1000)
        {
            var n0 = X.shape[0];
            var n2 = y.shape[0];

            var param = init(n0, n1, n2);

            for (var i = 0; i < iter; i++)
            {
                var activs = for_prop(X, param);
                var grads = back_prop(X, y, param, activs);
                param = update(grads, param, rate);
            }

            return param;
        }

        static NDArray ColSum(NDArray arr)
        {
            var res = np.zeros(new int[]{arr.shape[0], 1});
                
            for (int i = 0; i < arr.shape[0]; i++)
            for (int j = 0; j < arr.shape[1]; j++)
                res[i][0] += arr[i][j];
            
            return res;
        }

        static NDArray flatten(NDArray arr)
        {
            for (int i = 0; i < arr.shape[0]; i++)
            for (int j = 0; j < arr.shape[1]; j++)
                arr[i][j] = arr[i][j] >= 0.5 ? 1 : 0;

            return arr;
        }

        static bool DeepVerif(NDArray d1, NDArray d2)
        {
            if (d1.shape[0] != d2.shape[0] || d1.shape[1] != d2.shape[1]) return false;

            var A = (double[][])d1.ToJaggedArray<double>();
            var B = (double[][])d2.ToJaggedArray<double>();

            for(var i = 0; i < A.Length; i++)
            for(var j = 0; j < A[0].Length; j++)
                if (Math.Abs(A[i][j] - B[i][j]) > 0.1) return false;

            return true;
        }
        
        static void PrintShape(NDArray arr) => Console.WriteLine($"{arr.shape[0]} : {arr.shape[1]}");
        static double RandomDouble(double min, double max) => new Random().NextDouble() * (max - min) + min;
    }
}
