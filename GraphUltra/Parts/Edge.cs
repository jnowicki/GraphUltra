using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphUltra.Parts
{
    public class Edge
    {
        int id;
        int capacity;
        int startVertId;
        int endVertId;
        Path renderObj;

        public Path RenderObj
        {
            get { return renderObj; }
            set { renderObj = value; }
        }
        Label txtCtrl;

        public Label TxtCtrl
        {
            get { return txtCtrl; }
            set { txtCtrl = value; }
        }

        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }
        int flow = 0;

        public Edge(int id, int capacity, int startVertId, int endVertId)
        {
            this.id = id;
            this.capacity = capacity;
            this.startVertId = startVertId;
            this.endVertId = endVertId;
        }

        public int Flow
        {
            get { return flow; }
            set { flow = value; }
        }

    }
}
