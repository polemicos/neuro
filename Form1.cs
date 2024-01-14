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
        Graphics graphics;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            graphics = CreateGraphics();
            graphics.DrawLine(Pens.Black, new Point(540, 0), new Point(540, 720));
            graphics.DrawLine(Pens.Black, new Point(0, 360), new Point(1080, 360));
        }
    }
}
