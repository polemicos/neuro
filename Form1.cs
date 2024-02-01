
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace neuro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<List<Tuple<double, double[]>>> datasets;

        private Topology topology;
        private Network network;
        private double difference;
        

        private void Form1_Load(object sender, EventArgs e)
        {

            datasets = new List<List<Tuple<double, double[]>>>
            {
                new List<Tuple<double, double[]>>{
                    new Tuple<double, double[]>(0, new double[]{5, 4}),
                    new Tuple<double, double[]>(0, new double[]{7, 6}),
                    new Tuple<double, double[]>(0, new double[]{6, 10}),
                    new Tuple<double, double[]>(0, new double[]{4, 5}),
                    new Tuple<double, double[]>(0, new double[]{10, 10}),
                    new Tuple<double, double[]>(1, new double[]{1, 1}),
                    new Tuple<double, double[]>(1, new double[]{1, 3}),
                    new Tuple<double, double[]>(1, new double[]{2, 2}),
                    new Tuple<double, double[]>(1, new double[]{2, 3}),
                    new Tuple<double, double[]>(1, new double[]{3, 3})
                },
                new List<Tuple<double, double[]>>{
                    new Tuple<double, double[]>(0, new double[]{6, 4}),
                    new Tuple<double, double[]>(0, new double[]{9.5, 7}),
                    new Tuple<double, double[]>(0, new double[]{7, 3}),
                    new Tuple<double, double[]>(0, new double[]{8, 3.7}),
                    new Tuple<double, double[]>(0, new double[]{10, 4}),
                    new Tuple<double, double[]>(1, new double[]{1, 7}),
                    new Tuple<double, double[]>(1, new double[]{4, 3}),
                    new Tuple<double, double[]>(1, new double[]{2, 2}),
                    new Tuple<double, double[]>(1, new double[]{1, 5.8}),
                    new Tuple<double, double[]>(1, new double[]{3, 3})
                } 
            };

            

            topology = new Topology(2, 1, 0.8, 2);
            network = new Network(topology);
            foreach(var dataset in datasets)
            {
                difference = network.Learn(dataset, 1000);
                label1.Text = "difference: " + difference.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            difference = network.Learn(datasets[1], 100);
            label1.Text = "difference: " + difference.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Plot.GeneratePoints(network, datasets[0]);
            for (int i = 0; i < 3; i++)
            {
                this.chart.Series[i].Points.Clear();
            }


            var points = Plot.points;
            var boundary = Plot.boundary;
            //classes
            foreach (var point in points)
            {
                PointF pointValue = point.Item2;

                switch (point.Item1)
                {
                    case 1:
                        chart.Series[1].Points.AddXY(pointValue.X, pointValue.Y);
                        break;

                    case 0:
                        chart.Series[2].Points.AddXY(pointValue.X, pointValue.Y);
                        break;

                    // Add more cases if needed

                    default:
                        // Handle other cases if necessary
                        break;
                }
            }


            //boundary
            foreach (var point in boundary)
            {
                chart.Series[0].Points.AddXY(point.X, point.Y+10);
            }
        }
    }
}
