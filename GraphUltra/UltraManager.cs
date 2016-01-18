using GraphUltra.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GraphUltra
{
    public static class UltraManager
    {
        public const int SourceId = 254;
        public const int SinkId = 255;

        static TextBox logBox;

        public static TextBox LogBox
        {
            get { return logBox; }
            set { logBox = value; }
        }

        static List<Vertice> allVertices = new List<Vertice>();

        public static List<Vertice> AllVertices
        {
            get { return UltraManager.allVertices; }
            set { UltraManager.allVertices = value; }
        }

        static int verticeIdCnt = 0;

        public static int NextVerticeNr { get { return verticeIdCnt; } }

        static List<Edge> allEdges = new List<Edge>();

        public static List<Edge> AllEdges
        {
            get { return UltraManager.allEdges; }
            set { UltraManager.allEdges = value; }
        }
        static int edgeIdCnt = 0;

        static List<ResidualEdge> allResEdges;

        public static List<ResidualEdge> AllResEdges
        {
            get { return UltraManager.allResEdges; }
            set { UltraManager.allResEdges = value; }
        }

        public static void ClearAll()
        {
            allVertices.Clear();
            allEdges.Clear();
            logBox.Clear();
            verticeIdCnt = 0;
            edgeIdCnt = 0;
        }

        public static void addSource(Ellipse renderObj, Label textCtrl)
        {
            if (allVertices.Exists(x => x.Id == SourceId))
            {
                logBox.AppendText("Illegal try to add source vertex.\n");
            }
            else
            {
                Vertice source = new Vertice(SourceId, "source" , renderObj, textCtrl);
                source.designateAsSource();
                allVertices.Add(source);
                logBox.AppendText("Adding source vertex.\n");
            }
            
        }

        public static void addSink(Ellipse renderObj, Label textCtrl)
        {
            if (allVertices.Exists(x => x.Id == SinkId))
            {
                logBox.AppendText("Illegal try to add sink vertex.\n");
            }
            else
            {
                Vertice sink = new Vertice(SinkId, "sink", renderObj, textCtrl);
                sink.designateAsSink();
                allVertices.Add(sink);
                logBox.AppendText("Adding sink vertex.\n");
            }
        }

        public static void addVertice(Ellipse renderObj, Label textCtrl)
        {
            Vertice vert = new Vertice(verticeIdCnt, "v" + verticeIdCnt, renderObj, textCtrl);
            allVertices.Add(vert);
            logBox.AppendText("Adding vertex " + verticeIdCnt + ".\n");
            verticeIdCnt++;

        }

        public static void addEdge(int startVertId, int endVertId, int capacity, Path renderObj, Label textCtrl)
        {
            Edge edge = new Edge(edgeIdCnt++, capacity, startVertId, endVertId);
            edge.RenderObj = renderObj;
            edge.TxtCtrl = textCtrl;
            allEdges.Add(edge);
            Vertice startVert = allVertices.Find(x => x.Id == startVertId);
            startVert.OutEdges.Add(edge);
            Vertice endVert = allVertices.Find(x => x.Id == endVertId);
            endVert.InEdges.Add(edge);
            startVert.Neighbours.Add(endVert);
            endVert.Neighbours.Add(startVert);
            logBox.AppendText("Adding edge " + startVert.Label + "->" + endVert.Label + " with capacity " + capacity + "\n");

        }

        

        

    }
}
