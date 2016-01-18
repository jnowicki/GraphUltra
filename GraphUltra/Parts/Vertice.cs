using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GraphUltra.Parts
{
    public class Vertice
    {
        Mark mark;
        bool isSource;
        bool isSink;
        string label;
        int id;
        List<Vertice> neighbours = new List<Vertice>();
        List<Edge> outEdges = new List<Edge>();
        List<Edge> inEdges = new List<Edge>();
        Ellipse renderObj;
        Label txtCtrl;

        public Label TxtCtrl
        {
            get { return txtCtrl; }
            set { txtCtrl = value; }
        }

        public Vertice(int id, string label, Ellipse renderObj, Label txtCtrl)
        {
            this.renderObj = renderObj;
            this.txtCtrl = txtCtrl;
            this.id = id;
            this.label = label;
        }

        public string Label
        {
            get { return label; }
        }

        public Mark Mark
        {
            get { return mark; }
            set { mark = value; }
        }

        public Ellipse RenderObj
        {
            get { return renderObj; }
            set { renderObj = value; }
        }

        public List<Edge> InEdges
        {
            get { return inEdges; }
        }

        public List<Edge> OutEdges
        {
            get { return outEdges; }
        }
        public List<Vertice> Neighbours
        {
            get { return neighbours; }
        }

        public int Id
        {
            get { return id; }
        }

        public void designateAsSource()
        {
            isSource = true;
            isSink = false;
        }

        public void designateAsSink()
        {
            isSink = true;
            isSource = false;
        }

        public bool IsSource
        {
            get { return isSource; }
            set { isSource = value; }
        }

        public bool IsSink
        {
            get { return isSink; }
            set { isSink = value; }
        }
    }
}
