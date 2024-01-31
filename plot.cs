﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuro
{
    public class Plot
    {

        private static List<Tuple<double, PointF>> points;
        private static List<PointF> boundary;

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
            for (int layer = 1; layer <= numHiddenLayers; layer++)
            {
                int numHiddenNeurons = network.layers[layer].neuronsCount;
                hiddenNeurons[layer] = new List<double>(); // Инициализация списка

                for (int neuron = 0; neuron < numHiddenNeurons; neuron++)
                {
                    Neuron hiddenNeuron = network.layers[layer].neurons[neuron];

                    for (int weight = 0; weight < hiddenNeuron.weights.Count; weight++)
                    {
                        double weightedSum = hiddenNeuron.weights[weight] * x1;
                        if (layer > 1)
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
            double output = network.layers.Last().neurons[0].weights[0] * x1;
            for (int i = 1; i < network.layers.Last().neurons[0].weights.Count; i++)
            {
                output += network.layers.Last().neurons[0].weights[i] * Neuron.sigmoid(hiddenNeurons[numHiddenLayers][i - 1]);
            }

            // x2
            double x2 = (Math.Log((1.0 / output) - 1.0) - network.layers[1].neurons[0].weights[0] * x1) / network.layers[1].neurons[0].weights[1];
            return x2;
        }

        public static Bitmap DrawDecisionBoundary()
        {
            // Определяем минимальные и максимальные значения по осям
            float minX = (float)points.Min(p => p.Item2.X);
            float maxX = (float)points.Max(p => p.Item2.X);
            float minY = (float)points.Min(p => p.Item2.Y);
            float maxY = (float)points.Max(p => p.Item2.Y);

            // Определяем размер изображения
            int pointSize = 50;
            int width = (int)((maxX - minX) * pointSize) + pointSize;
            int height = (int)((maxY - minY) * pointSize) + pointSize;

            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                foreach (var point in points)
                {
                    float scaledX = (float)((point.Item2.X - minX) / (maxX - minX) * (width - pointSize));
                    float scaledY = (float)((point.Item2.Y - minY) / (maxY - minY) * (height - pointSize));

                    RectangleF rect = new RectangleF(scaledX, height - scaledY - pointSize, pointSize, pointSize);

                    if (point.Item1 == 0)
                    {
                        g.FillRectangle(Brushes.Red, rect);
                    }
                    else if (point.Item1 == 1)
                    {
                        g.FillRectangle(Brushes.Blue, rect);
                    }
                }

                // Рисуем линию границы решения
                PointF[] linePoints = new PointF[] { new PointF(0, height / 2), new PointF(width, height / 2) };
                g.DrawLines(Pens.Green, linePoints);
            }
            return bmp;
        }
    }
}