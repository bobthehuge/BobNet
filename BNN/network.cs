using System;
using System.Reflection;

namespace BNN
{
    public class Network
    {
        public neuron[] Neurons;
        public Matrix DataIn;
        public Matrix Expected;
        public double Rate;

        public Network(Matrix data_in, Matrix expected, int neurons, double rate = 0.1)
        {
            this.Neurons = new neuron[neurons];

            for (int i = 0; i < neurons; i++)
                Neurons[i] = new neuron(data_in);

            this.DataIn = data_in.Clone();
            this.Expected = expected.Clone();
            this.Rate = rate;
        }

        public void train(int n)
        {
            for (var j = 0; j < n; j++)
            for (var i = 0; i < Neurons.Length; i++)
            {
                var model = neuron.model(this.DataIn, Neurons[i].Weights, Neurons[i].Bias);
                var grad = neuron.gradients(model, this.DataIn, this.Expected);
            
                Neurons[i].update(grad.Item1, grad.Item2, 0.1);
            }
        }
    }
}