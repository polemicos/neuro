using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuro
{
    public class Topology
    {
        public int inputs { get; }
        public int outputs { get; }
        public List<int> hidden { get; }

        public Topology(int inputs, int outputs, params int[] layers) {
            this.inputs = inputs;
            this.outputs = outputs;
            hidden = new List<int>();
            hidden.AddRange(layers);
        }
    }
}
