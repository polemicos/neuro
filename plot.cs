using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuro
{
    public class Plot
    {

        public static List<Tuple<double, PointF>> points;
        public static List<PointF> boundary;

        public static void GeneratePoints(Network network, List<Tuple<double, double[]>> dataset)
        {
            points = new List<Tuple<double, PointF>>();
            for (int i = 0; i < dataset.Count; i++)
            {
                float x = (float)dataset[i].Item2[0];
                float y = (float)dataset[i].Item2[1];

                points.Add(new Tuple<double, PointF>(dataset[i].Item1, new PointF(x, y)));
            }
            boundary = new List<PointF>();
            for (double x1 = 0; x1 <= 10; x1 += 0.1)
            {
                double x2 = FindX2(x1, network);
                boundary.Add(new PointF((float)x1, (float)x2));
            }
        }

        static double FindX2(double x1, Network network)
        {
            int numHiddenLayers = network.topology.hidden.Count();

            // hidden
            List<double>[] hiddenNeurons = new List<double>[numHiddenLayers + 1];
            for (int layer = 0; layer <= numHiddenLayers; layer++)
            {
                int numHiddenNeurons = network.layers[layer].neuronsCount;
                hiddenNeurons[layer] = new List<double>();
                var bias = network.layers[layer].bias;

                for (int neuron = 0; neuron < numHiddenNeurons; neuron++)
                {
                    Neuron hiddenNeuron = network.layers[layer].neurons[neuron];

                    for (int weight = 0; weight < hiddenNeuron.weights.Count; weight++)
                    {
                        double weightedSum = hiddenNeuron.weights[weight] * x1 + bias;
                        if (layer > 0)
                        {
                            for (int prevNeuron = 0; prevNeuron < numHiddenNeurons; prevNeuron++)
                            {
                                weightedSum += hiddenNeuron.weights[weight] * Neuron.sigmoid(hiddenNeurons[layer - 1][prevNeuron]);
                            }
                        }
                        hiddenNeurons[layer].Add(Neuron.sigmoid(weightedSum));
                    }
                }
            }

            // output
            double output = network.layers.Last().neurons[0].weights[0] * x1 + network.layers.Last().bias;
            for (int i = 1; i < network.layers.Last().neurons[0].weights.Count; i++)
            {
                output += network.layers.Last().neurons[0].weights[i] * Neuron.sigmoid(hiddenNeurons[numHiddenLayers][i - 1]);
            }

            // x2
            double predictedClass = 1.0 / (1.0 + Math.Exp(-output));

            // x2
            double x2 = -network.layers[1].neurons[0].weights[0] / network.layers[1].neurons[0].weights[1] * x1 +
                         Math.Log(predictedClass / (1.0 - predictedClass)) / network.layers[1].neurons[0].weights[1];
            return x2;
        }

    }
}
