using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuro
{
    public class Layer
    {
        public List<Neuron> neurons { get; }
        public int neuronsCount => neurons?.Count ?? 0;
        public neuronType neuronType;

        public Layer(List<Neuron> neurons, neuronType neuronType = neuronType.hidden)
        {
            this.neurons = neurons;
            this.neuronType = neuronType;
        }

        public List<double> getSignals()
        {
            var res = new List<double>();
            foreach(var neuron in neurons)
            {
                res.Add(neuron.output);
            }
            return res;
        }


        public override string ToString()
        {
            return neuronType.ToString();
        }

    }
}
