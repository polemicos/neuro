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
        private List<Tuple<double, double[]>> dataset;

        private Graphics graphics;
        private Topology topology;
        private Network network;
        private double difference;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            graphics = CreateGraphics();
            graphics.DrawLine(Pens.Black, new Point(540, 0), new Point(540, 720));
            graphics.DrawLine(Pens.Black, new Point(0, 360), new Point(1080, 360));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //TODO: переделать подачу данных чтоб в одном тапле было сразу несколько точек
            dataset = new List<Tuple<double, double[]>>
            {
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
            };

            topology = new Topology(2, 1, 0.2, 2);
            network = new Network(topology);
            difference = network.Learn(dataset, 100);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = difference.ToString();
        }
    }
}
