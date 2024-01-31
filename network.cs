using System;
using System.Collections.Generic;
using System.Linq;

namespace neuro
{
    public class Network
    {
        public List<Layer> layers { get; }
        public Topology topology { get; }
        

        public Network(Topology topology)
        {
            this.topology = topology;
            layers = new List<Layer>();

            mkInput();
            mkHiddens();
            mkOutput();
        }

        public Neuron feedForward(params double[] inputSignals)
        {
            feedInput(inputSignals);
            goThrough();

            if(topology.outputs == 1)
            {
                return layers.Last().neurons[0];
            }
            else
            {
                return layers.Last().neurons.OrderByDescending(n => n.output).First();
            }

        }

        private double backpropagation(double exp, params double[] inputs)
        {
            var actual = feedForward(inputs).output;
            var diff = actual - exp;
            foreach(var n in layers.Last().neurons)
            {
                n.balance(diff, topology.learningRate);
            }

            for(int i = layers.Count - 2; i >= 0; i--)
            {
                var l = layers[i];
                var prevL = layers[i + 1];
                for(int j = 0; j < l.neuronsCount; j++)
                {
                    var neuron = l.neurons[j];
                    for(int k=0; k<prevL.neuronsCount; k++)
                    {
                        var prevN = prevL.neurons[k];
                        var err = prevN.weights[j] * prevN.delta;
                        neuron.balance(err, topology.learningRate);
                    }
                }
            }
            return diff * diff;
        }

        public double Learn(List<Tuple<double, double[]>> dataset, int epoch)
        {
            var err = 0.0;
            for(int i=0; i < epoch; i++)
            {
                foreach(var data in dataset)
                {
                    err += backpropagation(data.Item1, data.Item2);
                }
            }
            return err / epoch;
        }

        private void goThrough()
        {
            for (int i = 1; i < layers.Count; i++)
            {
                var layer = layers[i];
                var prevLayerSignals = layers[i - 1].getSignals();

                foreach (var neuron in layer.neurons)
                {
                    neuron.feedForward(prevLayerSignals);
                }
            }
        }

        private void feedInput(params double[] inputSignals)
        {
            for (int i = 0; i < inputSignals.Length; i++)
            {
                var signal = new List<double> { inputSignals[i] };
                var neuron = layers[0].neurons[i];

                neuron.feedForward(signal);
            }
        }

        private void mkHiddens()
        {
            for(int hiddens = 0; hiddens < topology.hidden.Count; hiddens++)
            {
                var hiddenNeurons = new List<Neuron>();
                var prevLayer = layers.Last();
                for (int i = 0; i < topology.hidden[hiddens]; i++)
                {
                    var neuron = new Neuron(prevLayer.neuronsCount, neuronType.output);
                    hiddenNeurons.Add(neuron);
                }
                var hiddenLayer = new Layer(hiddenNeurons, neuronType.output);
                layers.Add(hiddenLayer);
            }
        }

        private void mkOutput()
        {
            var outputNeurons = new List<Neuron>();
            var prevLayer = layers.Last();
            for (int i = 0; i < topology.outputs; i++)
            {
                var neuron = new Neuron(prevLayer.neuronsCount, neuronType.output);
                outputNeurons.Add(neuron);
            }
            var outputLayer = new Layer(outputNeurons, neuronType.output);
            layers.Add(outputLayer);
        }

        private void mkInput()
        {
            var inputNeurons = new List<Neuron>();
            for(int i = 0; i < topology.inputs; i++)
            {
                var neuron = new Neuron(1, neuronType.input);
                inputNeurons.Add(neuron);
            }
            var inputLayer = new Layer(inputNeurons, neuronType.input);
            layers.Add(inputLayer);
        }


    }
}
