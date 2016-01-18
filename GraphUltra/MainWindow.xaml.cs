using GraphUltra.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphUltra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Ellipse> vertices = new List<Ellipse>();

        public MainWindow()
        {
            InitializeComponent();
            UltraManager.LogBox = logBox;
        }

        void vertice_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Ellipse vert = (Ellipse)sender;
                if (vert.Stroke == Brushes.Red)
                {
                    // Unmark vertice.
                    vert.Stroke = (SolidColorBrush)vert.Tag;
                }
                else
                {
                    IEnumerable<Ellipse> redVertices = vertices.Where(x => x.Stroke == Brushes.Red);
                    if (redVertices.Any())
                    {
                        if (vert.Name == "source")
                        {

                        }
                        else
                        {
                            Ellipse startVert = redVertices.First();
                            createEdge(startVert, vert, startVert.Name + "_" + vert.Name);
                            startVert.Stroke = (SolidColorBrush)startVert.Tag;
                            vert.Stroke = (SolidColorBrush)vert.Tag;
                        }

                    }
                    else
                    {
                        if (vert.Name == "sink")
                        {

                        }
                        else
                        {
                            vert.Tag = vert.Stroke;
                            vert.Stroke = Brushes.Red;
                        }

                    }
                }
            }
            e.Handled = true;
        }

        private void createEdge(Ellipse startVert, Ellipse endVert, string edgeName)
        {
            
            Vector startVertVec = VisualTreeHelper.GetOffset(startVert);
            Vector endVertVec = VisualTreeHelper.GetOffset(endVert);
            // Vectors are created in the upper left corner of ellipse, so move the vector to center.
            Point startCenterPt = new Point(startVertVec.X + startVert.GetRadius(), startVertVec.Y + startVert.GetRadius());
            Point endCenterPt = new Point(endVertVec.X + endVert.GetRadius(), endVertVec.Y + endVert.GetRadius());
            Path edge = new Path();
            edge.Name= edgeName;

            //make shaft
            LineGeometry shaftGeo = new LineGeometry();
            rescaleLine(shaftGeo, startCenterPt, endCenterPt, startVert.GetRadius(), endVert.GetRadius());
            
            //make arrowhead
            Vector vect = startCenterPt - endCenterPt;
            vect.Normalize();
            vect *= (Math.Sqrt(Math.Pow(startCenterPt.X - endCenterPt.X, 2) + Math.Pow(startCenterPt.Y - endCenterPt.Y, 2)) / 14) + 10;
            Matrix rotationMx = new Matrix();
            LineGeometry arrowHead1 = new LineGeometry();
            arrowHead1.StartPoint = shaftGeo.EndPoint;
            rotationMx.Rotate(30);
            arrowHead1.EndPoint = shaftGeo.EndPoint + vect * rotationMx;
            LineGeometry arrowHead2 = new LineGeometry();
            arrowHead2.StartPoint = shaftGeo.EndPoint;
            rotationMx.Rotate(-60);
            arrowHead2.EndPoint = shaftGeo.EndPoint + vect * rotationMx;
            
            // group shaft + arrowhead
            GeometryGroup edgeGeo = new GeometryGroup();
            edgeGeo.Children.Add(shaftGeo);
            edgeGeo.Children.Add(arrowHead1);
            edgeGeo.Children.Add(arrowHead2);
            edge.Data = edgeGeo;
            edge.Stroke = Brushes.Blue;
            edge.StrokeThickness = 2;

            TextBox capacityBox = new TextBox();
            capacityBox.Width = 30;
            capacityBox.Text = "";
            capacityBox.Tag = edge;
            capacityBox.Name = edgeName + "capacity";
            //capacityBox.LostKeyboardFocus += capacityBox_LostKeyboardFocus;
            capacityBox.KeyUp += capacityBox_KeyUp;
            Canvas.SetLeft(capacityBox, (startCenterPt.X + endCenterPt.X) / 2);
            Canvas.SetTop(capacityBox, (startCenterPt.Y + endCenterPt.Y) / 2);
            canvas.Children.Add(capacityBox);
            canvas.Children.Add(edge);
            vertices.ForEach(x => x.MouseDown -= vertice_MouseDown);
            capacityBox.Tag = edge;
            Keyboard.Focus(capacityBox);
        }

        void capacityBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                finalizeEdgeCreation((TextBox)sender);  
            }
        }

        void finalizeEdgeCreation(TextBox sender)
        {
            vertices.ForEach(x => x.MouseDown += vertice_MouseDown);
            Path renderEdge = (Path)sender.Tag;
            string edgeName = renderEdge.Name;
            string[] splitEdgeName = edgeName.Split('_');
            int startVertId;
            int endVertId;

            if (edgeName.Contains("source"))
            {
                startVertId = UltraManager.SourceId;
            }
            else
            {
                startVertId = Convert.ToInt32(Regex.Replace(splitEdgeName[0], "[^0-9]", ""));
            }

            if (edgeName.Contains("sink"))
            {
                endVertId = UltraManager.SinkId;
            }
            else
            {
                endVertId = Convert.ToInt32(Regex.Replace(splitEdgeName[1], "[^0-9]", ""));
            }
            
            int capacity = (sender.Text == "" ? 0 : Convert.ToInt32(sender.Text));
            
            Label flowLbl = new Label();
            flowLbl.Name = sender.Name.Replace("capacity", "") + "flow";
            flowLbl.Content = capacity;
            UltraManager.addEdge(startVertId, endVertId, capacity, renderEdge, flowLbl);
            Canvas.SetLeft(flowLbl, (double)sender.GetValue(Canvas.LeftProperty));
            Canvas.SetTop(flowLbl, (double)sender.GetValue(Canvas.TopProperty));
            canvas.Children.Add(flowLbl);
            canvas.Children.Remove(sender);
        }

        /// <summary>
        /// Creates Vertice on canvas at specified left and top location.
        /// </summary>
        void createVertice(double leftLoc, double topLoc)
        {
            Ellipse vertice = new Ellipse
            {
                Width = 15,
                Height = 15

            };
            vertice.Stroke = Brushes.Black;
            vertice.Name = "v" + UltraManager.NextVerticeNr;
            vertice.Tag = vertice.Stroke; //save color of vertice for changing back
            vertice.StrokeThickness = 10;
            vertice.MouseDown += vertice_MouseDown;
            Canvas.SetLeft(vertice, leftLoc);
            Canvas.SetTop(vertice, topLoc);
            canvas.Children.Add(vertice);
            vertices.Add(vertice);
            Label vertLabel = new Label();
            vertLabel.FontSize = 8;
            vertLabel.Content = vertice.Name;
            Canvas.SetLeft(vertLabel, leftLoc);
            Canvas.SetTop(vertLabel, topLoc - 15);
            UltraManager.addVertice(vertice, vertLabel);
            canvas.Children.Add(vertLabel);

        }

        /// <summary>
        /// Creates source vertice.
        /// </summary>
        void createSource(double leftLoc, double topLoc)
        {
            Ellipse vertice = new Ellipse
            {
                Width = 30,
                Height = 30

            };
            vertice.Stroke = Brushes.DarkBlue;
            vertice.Tag = vertice.Stroke;
            vertice.Name = "source";
            vertice.StrokeThickness = 15;
            vertice.MouseDown += vertice_MouseDown;
            Canvas.SetLeft(vertice, leftLoc);
            Canvas.SetTop(vertice, topLoc);
            canvas.Children.Add(vertice);
            vertices.Add(vertice);
            Label vertLabel = new Label();
            vertLabel.Content = vertice.Name;
            vertLabel.FontSize = 14;
            UltraManager.addSource(vertice, vertLabel);
            Canvas.SetLeft(vertLabel, leftLoc);
            Canvas.SetTop(vertLabel, topLoc - 30);
            canvas.Children.Add(vertLabel);
        }

        /// <summary>
        /// Creates sink vertice.
        /// </summary>
        void createSink(double leftLoc, double topLoc)
        {
            Ellipse vertice = new Ellipse
            {
                Width = 30,
                Height = 30

            };
            vertice.Stroke = Brushes.LightBlue;
            vertice.Tag = vertice.Stroke;
            vertice.Name = "sink";
            vertice.StrokeThickness = 15;
            vertice.MouseDown += vertice_MouseDown;
            Canvas.SetLeft(vertice, leftLoc);
            Canvas.SetTop(vertice, topLoc);
            canvas.Children.Add(vertice);
            vertices.Add(vertice);
            Label vertLabel = new Label();
            vertLabel.Content = vertice.Name;
            vertLabel.FontSize = 14;
            Canvas.SetLeft(vertLabel, leftLoc);
            Canvas.SetTop(vertLabel, topLoc - 30);
            UltraManager.addSink(vertice, vertLabel);
            canvas.Children.Add(vertLabel);
        }

        /// <summary>
        /// Rescales the line so that it touches the circles edges, instead of centers.
        /// </summary>
        /// <param name="edge">The line that is an graphs edge</param>
        /// <param name="startPt">Vector of starting point</param>
        /// <param name="endPt">Vector of ending point</param>
        void rescaleLine(LineGeometry edge, Point startPt, Point endPt, double radius1, double radius2)
        {
            double yb = endPt.Y;
            double xb = endPt.X;
            double ya = startPt.Y;
            double xa = startPt.X;

            // a value of the line given by y = ax + b
            double a = (ya - yb) / (xa - xb);
            // Angle alpha of a pythagorean triangle described by circle center, 
            // intersection of line with circle and point that lies on right angle.
            double alpha = Math.Atan(a);
            Vector radiusVec = startPt - endPt;
            radiusVec.Normalize();
            Matrix rotationMx = new Matrix();
            rotationMx.Rotate(alpha + 180);
            radiusVec *= radius1;
            edge.StartPoint = startPt + radiusVec * rotationMx;
            rotationMx.Rotate(180);
            radiusVec.Normalize();
            radiusVec *= radius2;
            edge.EndPoint = endPt + radiusVec * rotationMx;
        }

        private void createGraphBtn_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            UltraManager.ClearAll();

            createSource(50, canvas.ActualHeight / 2);
            createSink(canvas.ActualWidth - 50, canvas.ActualHeight / 2);

            int verticeCount = Convert.ToInt32(NrOfVertTxBox.Text);

            if (verticeCount == 1)
            {
                createVertice(canvas.ActualWidth / 2, canvas.ActualHeight / 2);
            }
            else if (verticeCount > 1)
            {
                int nrOfColumns = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(verticeCount) / 2.0));
                int columnWidth = Convert.ToInt32(canvas.ActualWidth - 100) / (nrOfColumns + 1);

                for (int i = 0; i < verticeCount; i++)
                {
                    if (i % 2 == 0)
                    {
                        int columnNr = (i / 2) + 1;
                        int leftOffset = (columnWidth * columnNr) + 50;
                        createVertice(leftOffset, canvas.ActualHeight / 3);
                    }
                    else
                    {
                        int columnNr = (i / 2) + 1;
                        int leftOffset = (columnWidth * columnNr) + 50;
                        createVertice(leftOffset, (canvas.ActualHeight / 3) * 2);
                    }
                }
            }      
        }

        private void startBut_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Ellipse> ellipses = canvas.Children.OfType<Ellipse>();
            ellipses.ToList().ForEach(x => x.MouseDown -= vertice_MouseDown);
            createGraphBtn.Click -= createGraphBtn_Click;

            residualCanvas.Children.Clear();
            foreach (var ui in canvas.Children)
            {
                var xaml = System.Windows.Markup.XamlWriter.Save(ui);
                var deepCopy = System.Windows.Markup.XamlReader.Parse(xaml) as UIElement;
                residualCanvas.Children.Add(deepCopy);
            }
        }




    }
}
