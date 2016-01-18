using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphUltra.Parts
{
    public class ResidualEdge
    {
        int id;
        int capacity;

        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public ResidualEdge(int id, int capacity)
        {
            this.id = id;
            this.capacity = capacity;
        }

    }
}
