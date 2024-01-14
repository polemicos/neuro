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
        public double output { get; private set; }
        

        public Neuron(int inCount, neuronType neuronType = neuronType.normal)
        {
            this.neuronType = neuronType;
            weights = new List<double>();

            for(int i = 0; i < inCount; i++)
            {
                weights.Add(1);
            }

        }

        public double feedForward(List<double> inputs) 
        {
            var sum = 0.0;
            for(int i=0; i< inputs.Count; i++)
            {
                sum += inputs[i] * weights[i];
            }

            output = sigmoid(sum);
            return output;
        }

        private double sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Pow(Math.E, -x));
        }


        //потом удалить
        public void setWeights(params double[] weights)
        {
            for(int i = 0; i < weights.Length; i++)
            {
                this.weights[i] = weights[i];
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
        normal = 1,
        output = 2
    }
}
