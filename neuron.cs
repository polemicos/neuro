using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuro
{
    public class Neuron
    {
        public List<double> weights { get; }
        public neuronType neuronType { get; }
        public List<double> inputs { get; }
        public double output { get; private set; }
        public double delta { get; private set; }

        

        public Neuron(int inCount, neuronType neuronType = neuronType.hidden)
        {
            this.neuronType = neuronType;
            weights = new List<double>();
            inputs = new List<double>();
            randomWeights(inCount);

        }

        private void randomWeights(int inCount)
        {

            var rnd = new Random();
            for (int i = 0; i < inCount; i++)
            {
                if(neuronType == neuronType.input)
                {
                    weights.Add(1);
                }else weights.Add(rnd.NextDouble());
                inputs.Add(0);
            }
        }

        public double feedForward(List<double> inputs) 
        {
            for(int i=0; i < inputs.Count; i++)
            {
                this.inputs[i] = inputs[i];
            }

            var sum = 0.0;
            for(int i=0; i< inputs.Count; i++)
            {
                sum += inputs[i] * weights[i];
            }
            if (neuronType != neuronType.input)
            {
                output = sigmoid(sum);
            }
            else output = sum;
            return output;
        }

        private double sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Pow(Math.E, -x));
        }

        private double sigmoidDx(double x)
        {
            return sigmoid(x) / (1 - sigmoid(x));
        }


        public void balance(double error, double rate)
        {
            if (neuronType == neuronType.input) return;

            delta = error * sigmoidDx(output);

            for(int i=0; i < weights.Count; i++)
            {
                weights[i] = weights[i] - inputs[i] * delta * rate;

            }
        }

        public override string ToString()
        {
            return output.ToString();
        }

    }


    public enum neuronType
    {
        input = 0,
        hidden = 1,
        output = 2
    }
}
