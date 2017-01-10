using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Graph_algorithms
{
    public partial class MainWindow : Window
    {
        #region Initialization

        public MainWindow()
        {
            InitializeComponent();
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            appFolder = System.Reflection.Assembly.GetEntryAssembly().Location;
            appFolder = appFolder.Substring(0, appFolder.LastIndexOf('\\') + 1);
            this.Title = "Graph algorithms - New graph";
        }


        private void StartupWindow_Activated(object sender, EventArgs e)
        {
            Resize(LayoutRoot.ActualWidth, LayoutRoot.ActualHeight);
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            Resize(LayoutRoot.ActualWidth, LayoutRoot.ActualHeight);
        }


        private void savedGraphListBox_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSavedGraphs();
            try
            {
                LoadGraph((ListBoxItem)savedGraphListBox.Items[0]);
            }
            catch (Exception)
            {

            }
        }

        private void UpdateSavedGraphs()
        {
            savedGraphListBox.Items.Clear();
            if (!System.IO.Directory.Exists(appFolder + "saved"))
            {
                System.IO.Directory.CreateDirectory(appFolder + "saved");
            }
            var savedGraph = System.IO.Directory.EnumerateFiles(appFolder + "saved");
            for (int i = 0; i < savedGraph.Count(); i++)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Name = "savedGraph" + i;
                listBoxItem.Tag = savedGraph.ElementAt(i);
                listBoxItem.Content = savedGraph.ElementAt(i).Substring(savedGraph.ElementAt(i).LastIndexOf('\\'), savedGraph.ElementAt(i).Length - savedGraph.ElementAt(i).LastIndexOf('\\'));
                listBoxItem.Selected += ListBoxItem_Selected;
                savedGraphListBox.Items.Add(listBoxItem);
            }
        }
        #endregion

        #region Error handling

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.ToString());
            EnableControls();
        }

        #endregion

        #region Resize handling

        private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Resize(e.NewSize.Width, e.NewSize.Height);
        }

        private void Resize(double width, double height)
        {
            try
            {
                editorCanvas.Width = settingsCanvas.Width = width - 20;
                editorCanvas.Height = height - 100;
                Canvas.SetTop(settingsCanvas, Canvas.GetTop(editorCanvas) + 20 + editorCanvas.ActualHeight);
                settingsCanvas.Height = height - 10 - Canvas.GetTop(settingsCanvas);
                graphScrollViewer.Height = editorCanvas.ActualHeight - 20;
                graphScrollViewer.Width = editorCanvas.ActualWidth - 200;
                Canvas.SetLeft(toolsTextBlock, 20 + graphScrollViewer.ActualWidth + 10);
                Canvas.SetLeft(vertexButton, 20 + graphScrollViewer.ActualWidth);
                Canvas.SetTop(twoWayConnectionButton, Canvas.GetTop(vertexButton) + vertexButton.ActualHeight + 10);
                Canvas.SetLeft(twoWayConnectionButton, Canvas.GetLeft(vertexButton));
                Canvas.SetTop(connectionWeightTextBox, Canvas.GetTop(twoWayConnectionButton) + twoWayConnectionButton.ActualHeight + 10);
                Canvas.SetLeft(connectionWeightTextBox, Canvas.GetLeft(twoWayConnectionButton));
                Canvas.SetTop(distanceTextBlock, Canvas.GetTop(connectionWeightTextBox));
                Canvas.SetLeft(distanceTextBlock, Canvas.GetLeft(connectionWeightTextBox) + connectionWeightTextBox.ActualWidth + 10);
                debugScrollViewer.Height = settingsCanvas.ActualHeight;
                Canvas.SetLeft(clearButton, Canvas.GetLeft(connectionWeightTextBox));
                Canvas.SetTop(clearButton, Canvas.GetTop(connectionWeightTextBox) + connectionWeightTextBox.ActualHeight + 10);
                Canvas.SetTop(startingVertexTextBox, Canvas.GetTop(clearButton) + clearButton.ActualHeight + 10);
                Canvas.SetLeft(startingVertexTextBox, Canvas.GetLeft(clearButton));
                Canvas.SetTop(startingVertexTextBlock, Canvas.GetTop(startingVertexTextBox));
                Canvas.SetLeft(startingVertexTextBlock, Canvas.GetLeft(startingVertexTextBox) + startingVertexTextBox.ActualWidth + 10);
                Canvas.SetTop(endingVertexTextBox, Canvas.GetTop(startingVertexTextBox) + startingVertexTextBox.ActualHeight + 10);
                Canvas.SetLeft(endingVertexTextBox, Canvas.GetLeft(startingVertexTextBox));
                Canvas.SetTop(endingVertexTextBlock, Canvas.GetTop(endingVertexTextBox));
                Canvas.SetLeft(endingVertexTextBlock, Canvas.GetLeft(endingVertexTextBox) + endingVertexTextBox.ActualWidth + 10);
                Canvas.SetTop(saveButton, Canvas.GetTop(endingVertexTextBox) + endingVertexTextBox.ActualHeight + 10);
                Canvas.SetLeft(saveButton, Canvas.GetLeft(endingVertexTextBox));
                Canvas.SetTop(savedGraphsTextBlock, Canvas.GetTop(saveButton) + saveButton.ActualHeight + 10);
                Canvas.SetLeft(savedGraphsTextBlock, Canvas.GetLeft(saveButton));
                Canvas.SetTop(savedGraphListBox, Canvas.GetTop(savedGraphsTextBlock) + savedGraphsTextBlock.ActualHeight + 10);
                Canvas.SetLeft(savedGraphListBox, Canvas.GetLeft(savedGraphsTextBlock));
                Canvas.SetTop(logCheckBox, Canvas.GetTop(savedGraphListBox) + savedGraphListBox.ActualHeight + 10);
                Canvas.SetLeft(logCheckBox, Canvas.GetLeft(savedGraphListBox));
                
            }
            catch (Exception)
            {
                
            }
        }

        #endregion

        #region Variable declaration

        string appFolder = string.Empty;

        private class Graph
        {
            public Graph()
            {
                connections = new List<Connection>();
            }

            public List<Connection> connections { get; private set; }

            public void AddConnection(Connection c)
            {
                List<Connection> ReversedConnections = new List<Connection>();
                for (int i = 0; i < connections.Count; i++)
                    ReversedConnections[i] = new Connection(connections[i].End, connections[i].Start, connections[i].Len);
                if (connections.Contains(c)) return;
                if (ReversedConnections.Contains(c)) return;
                connections.Add(c);
            }
        }

        private class Connection
        {
            public Connection(int start, int end, int len)
            {
                Start = start;
                End = end;
                Len = len;
            }
            public int Start { get; set; }
            public int End { get; set; }
            public int Len { get; set; }
        }

        private class Vector2
        {
            public Vector2(double x, double y)
            {
                X = x;
                Y = y;
            }
            public double X { get; set; }
            public double Y { get; set; }
        }

        private class SimpleConnection
        {
            public SimpleConnection(int start, int end)
            {
                Start = start;
                End = end;
            }
            public int Start { get; set; }
            public int End { get; set; }
        }

        List<Connection> connections = new List<Connection>();
        List<int> Vertices = new List<int>();

        private class VertexLabel
        {
            public VertexLabel(int p, int d)
            {
                P = p;
                D = d;
            }

            public VertexLabel()
            {

            }

            public int P { get; set; }
            public int D { get; set; }
        }
        #endregion

        #region Editor

        #region Editor variables

        bool userIsPlacingVertex = false;
        bool userIsPlacingTwoWayConnection = false;
        byte twoWayConnectionStep = 0;
        const double vertexSize = 50;
        int initialIndex = 0;

        List<TextBlock> ConnectionLength;
        List<Canvas> VertexCanvas;
        List<Line> Connections;
        int index = 0;
        #endregion

        #region Input handling


        private void graphCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Vector2 mousePosition = new Vector2(e.GetPosition(graphCanvas).X, e.GetPosition(graphCanvas).Y);

            double[] distances = new double[VertexCanvas.Count];

            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = GetDistanceBetween(mousePosition, new Graph_algorithms.MainWindow.Vector2(Canvas.GetLeft(VertexCanvas[i]), Canvas.GetTop(VertexCanvas[i])));
            }

            for (int i = 0; i < distances.Length; i++)
            {
                double min = distances[i];
                for (int j = 0; j < distances.Length; j++)
                {
                    if (min > distances[j])
                    {
                        min = distances[j];
                        index = j;
                    }
                }
            }
            Solve(index);
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            LoadGraph(sender as ListBoxItem);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(appFolder + "saved\\" + DateTime.Now.Ticks + ".graph");
            streamWriter.Write(VertexCanvas.Count + ";");
            for (int i = 0; i < VertexCanvas.Count; i++)
            {
                streamWriter.Write(string.Format(",{0},{1},{2};", Canvas.GetTop(VertexCanvas[i]), Canvas.GetLeft(VertexCanvas[i]), VertexCanvas[i].ActualHeight));
            }
            streamWriter.Write(" ~ " + Connections.Count + ";");
            for (int i = 0; i < Connections.Count; i++)
            {
                streamWriter.Write(",{0},{1},{2},{3},{4},{5},{6},{7},{8};"
                    , Canvas.GetTop(Connections[i]), Canvas.GetLeft(Connections[i]),
                    Connections[i].X1, Connections[i].Y1, Connections[i].X2, Connections[i].Y2, connections[i].Len
                    , connections[i].Start, connections[i].End);
            }
            streamWriter.Close();
            UpdateSavedGraphs();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearWindow();

        }
        private void StartupWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V)
            {
                AddNewVertex();
            }
            else if (e.Key == Key.C)
            {
                if (userIsPlacingTwoWayConnection) return;
                AddNewConnection();
            }
        }

        private void textBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            graphCanvas.Width = graphCanvas.ActualWidth;
            graphCanvas.Width += 100;
            graphCanvas.Height = graphCanvas.ActualHeight;
            graphCanvas.Height += 100;
        }

        private void twoWayConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewConnection();
        }

        private Vector2 GetLineOptimalStartingPosition(Canvas fromVertex)
        {
            return new Vector2(Canvas.GetTop(fromVertex) + fromVertex.ActualHeight / 2, Canvas.GetLeft(fromVertex) + fromVertex.ActualWidth / 2);
        }

        private double GetDistanceBetween(Vector2 v1, Vector2 v2)
        {
            return Math.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y));
        }

        private void vertexButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewVertex();
        }

        private void graphScrollViewer_MouseEnter(object sender, MouseEventArgs e)
        {
            if (userIsPlacingVertex)
            {
                Canvas.SetTop(VertexCanvas[VertexCanvas.Count - 1], e.GetPosition(graphScrollViewer).Y - vertexSize / 2 + graphScrollViewer.VerticalOffset);
                Canvas.SetLeft(VertexCanvas[VertexCanvas.Count - 1], e.GetPosition(graphScrollViewer).X - vertexSize / 2 + graphScrollViewer.HorizontalOffset);
            }
            else if (userIsPlacingTwoWayConnection)
            {
                
            }
        }

        private void graphScrollViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            if (userIsPlacingVertex)
            {

            }
            else if (userIsPlacingTwoWayConnection)
            {

            }
        }

        private void graphScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {

                if (userIsPlacingVertex)
                {
                    Canvas.SetTop(VertexCanvas[VertexCanvas.Count - 1], e.GetPosition(graphScrollViewer).Y - vertexSize / 2 + graphScrollViewer.VerticalOffset);
                    Canvas.SetLeft(VertexCanvas[VertexCanvas.Count - 1], e.GetPosition(graphScrollViewer).X - vertexSize / 2 + graphScrollViewer.HorizontalOffset);
                }
                else if (userIsPlacingTwoWayConnection)
                {
                    Vector2 mousePosition = new Vector2(e.GetPosition(graphCanvas).X, e.GetPosition(graphCanvas).Y);

                    double[] distances = new double[VertexCanvas.Count];

                    for (int i = 0; i < distances.Length; i++)
                    {
                        distances[i] = GetDistanceBetween(mousePosition, new Vector2(Canvas.GetLeft(VertexCanvas[i]), Canvas.GetTop(VertexCanvas[i])));
                    }

                    for (int i = 0; i < distances.Length; i++)
                    {
                        double min = distances[i];
                        for (int j = 0; j < distances.Length; j++)
                        {
                            if (min > distances[j])
                            {
                                min = distances[j];
                                index = j;
                            }
                        }
                    }
                    switch (twoWayConnectionStep)
                    {
                        case 0:
                            double x = Canvas.GetLeft(VertexCanvas[index]) + vertexSize / 2;
                            double y = Canvas.GetTop(VertexCanvas[index]) + vertexSize / 2;
                            Canvas.SetLeft(Connections[Connections.Count - 1], x);
                            Canvas.SetTop(Connections[Connections.Count - 1], y);
                            Connections[Connections.Count - 1].X1 = 0;
                            Connections[Connections.Count - 1].Y1 = 0;
                            Connections[Connections.Count - 1].X2 = mousePosition.X - x;
                            Connections[Connections.Count - 1].Y2 = mousePosition.Y - y;
                            initialIndex = index;

                            Canvas.SetTop(ConnectionLength[ConnectionLength.Count - 1], y + e.GetPosition(Connections[Connections.Count - 1]).Y / 2);
                            Canvas.SetLeft(ConnectionLength[ConnectionLength.Count - 1], x + e.GetPosition(Connections[Connections.Count - 1]).X / 2);
                            break;
                        case 1:
                            double x1 = Canvas.GetLeft(VertexCanvas[initialIndex]) + vertexSize / 2;
                            double y1 = Canvas.GetTop(VertexCanvas[initialIndex]) + vertexSize / 2;
                            Connections[Connections.Count - 1].X2 = Canvas.GetLeft(VertexCanvas[index]) + vertexSize / 2 - x1;
                            Connections[Connections.Count - 1].Y2 = Canvas.GetTop(VertexCanvas[index]) + vertexSize / 2 - y1;

                            Canvas.SetTop(ConnectionLength[ConnectionLength.Count - 1], y1 + e.GetPosition(Connections[Connections.Count - 1]).Y / 2);
                            Canvas.SetLeft(ConnectionLength[ConnectionLength.Count - 1], x1 + e.GetPosition(Connections[Connections.Count - 1]).X / 2);
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception)
            {
                
            }
        }

        private void graphCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (userIsPlacingVertex)
            {
                userIsPlacingVertex = false;
                EnableControls();
            }
            else if (userIsPlacingTwoWayConnection)
            {
                twoWayConnectionStep++;
                switch (twoWayConnectionStep)
                {
                    case 1:

                        break;
                    default:
                        userIsPlacingTwoWayConnection = false;
                        EnableControls();
                        twoWayConnectionStep = 0;
                        connections.Add(new Connection(initialIndex, index, Convert.ToInt32(ConnectionLength[ConnectionLength.Count - 1].Text)));
                        connectionWeightTextBox.Text = (new Random()).Next(1, 20).ToString();
                        Connections[Connections.Count - 1].Tag = new Vector2(initialIndex, index);
                        endingVertexTextBox.Text = (VertexCanvas.Count - 1).ToString();
                        break;
                }
            }
        }


        #endregion

        #region Useful Methods

        private void LoadGraph(ListBoxItem item)
        {
            Title = "Graph algorithms - " + item.Content.ToString().Remove(0, 1);
            string location = (item.Tag as string);
            ClearWindow();
            try
            {
                textBlock1.Text = string.Empty;
                var reader = new System.IO.StreamReader(item.Tag as string);
                string file = reader.ReadToEnd();
                reader.Close();
                var verticesInfo = file.Split('~', ';');
                for (int k = 0; k < verticesInfo.Length; k++)
                    textBlock1.Text += "|" + verticesInfo[k] + "|";
                int n1 = Convert.ToInt32(verticesInfo[0]);
                int c = 0;
                for (int i = 1; i <= n1; i++, c = i)
                {
                    var verticesdata = verticesInfo[i].Split(',');
                    for (int j = 1; j < verticesdata.Length; j += 3)
                    {
                        if (verticesdata[j] != string.Empty)
                        {
                            AddNewVertex(Convert.ToDouble(verticesdata[j]), Convert.ToDouble(verticesdata[j + 1]), Convert.ToDouble(verticesdata[j + 2]));
                        }
                    }
                }
                c++;
                int n2 = Convert.ToInt32(verticesInfo[c]);
                for (int i = c + 1; i < c + n2; i++)
                {
                    var verticesdata = verticesInfo[i].Split(',');
                    for (int k = 0; k < verticesdata.Length; k++)
                         textBlock1.Text += "/" + verticesdata[k] + "/";
                    for (int j = 1; j < verticesdata.Length; j += 9)
                    {
                        if (verticesdata[j] != string.Empty)
                        {
                            AddNewConnection(Convert.ToDouble(verticesdata[j]),
                                Convert.ToDouble(verticesdata[j + 1]),
                                Convert.ToDouble(verticesdata[j + 2]),
                                Convert.ToDouble(verticesdata[j + 3]),
                                Convert.ToDouble(verticesdata[j + 4]),
                                Convert.ToDouble(verticesdata[j + 5]),
                                Convert.ToInt32(verticesdata[j + 6]),
                                Convert.ToInt32(verticesdata[j + 7]),
                                Convert.ToInt32(verticesdata[j + 8]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                textBlock1.Text += "\nsomething went wrong\n" + ex.ToString();
                EnableControls();
            }
            EnableControls();
        }

        private void ClearWindow()
        {
            graphCanvas.Children.Clear();
            ConnectionLength = null;
            VertexCanvas = null;
            Connections = null;
            Vertices = new List<int>();
            connections = new List<Connection>();
            userIsPlacingTwoWayConnection = userIsPlacingVertex = false;
        }

        private void AddNewVertex()
        {
            DisableControls();
            userIsPlacingVertex = true;
            if (VertexCanvas == null) VertexCanvas = new List<Canvas>();
            Canvas newCanvas = new Canvas();
            newCanvas.Width = newCanvas.Height = vertexSize;
            newCanvas.Name = "VertexCanvas" + VertexCanvas.Count;
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(App.Current.StartupUri.ToString().Substring(0, App.Current.StartupUri.ToString().LastIndexOf("/")) + "/Resources/circle.png", UriKind.Absolute));
            image.Width = image.Height = vertexSize;
            newCanvas.Children.Add(image);
            TextBlock label = new TextBlock();
            label.Text = VertexCanvas.Count.ToString();
            label.FontSize = 20;
            label.FontWeight = FontWeights.Bold;
            newCanvas.Children.Add(label);
            Canvas.SetTop(label, vertexSize / 2 - 15);
            Canvas.SetLeft(label, vertexSize / 2 - 7.5);
            VertexCanvas.Add(newCanvas);
            graphCanvas.Children.Add(VertexCanvas[VertexCanvas.Count - 1]);
            Canvas.SetTop(VertexCanvas[VertexCanvas.Count - 1], -100);
            Vertices.Add(VertexCanvas.Count - 1);
            userIsPlacingVertex = true;
        }

        private void AddNewVertex(double top, double left, double size)
        {
            DisableControls();
            if (VertexCanvas == null)
                VertexCanvas = new List<Canvas>();
            Canvas newCanvas = new Canvas();
            newCanvas.Width = newCanvas.Height = vertexSize;
            newCanvas.Name = "VertexCanvas" + VertexCanvas.Count;
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(Application.Current.StartupUri.ToString().Substring(0, App.Current.StartupUri.ToString().LastIndexOf("/")) + "/Resources/circle.png", UriKind.Absolute));
            image.Width = image.Height = vertexSize;
            newCanvas.Children.Add(image);
            TextBlock label = new TextBlock();
            label.Text = VertexCanvas.Count.ToString();
            label.FontSize = 20;
            label.FontWeight = FontWeights.Bold;
            newCanvas.Children.Add(label);
            Canvas.SetTop(label, vertexSize / 2 - 15);
            Canvas.SetLeft(label, vertexSize / 2 - 7.5);
            VertexCanvas.Add(newCanvas);
            graphCanvas.Children.Add(VertexCanvas[VertexCanvas.Count - 1]);
            Canvas.SetTop(VertexCanvas[VertexCanvas.Count - 1], top);
            Canvas.SetLeft(VertexCanvas[VertexCanvas.Count - 1], left);
            Vertices.Add(VertexCanvas.Count - 1);
        }

        private void AddNewConnection()
        {
            DisableControls();
            userIsPlacingTwoWayConnection = true;
            if (Connections == null) Connections = new List<Line>();
            if (ConnectionLength == null) ConnectionLength = new List<TextBlock>();
            TextBlock len = new TextBlock();
            len.Text = connectionWeightTextBox.Text;
            len.Foreground = new SolidColorBrush(Colors.Black);
            len.FontSize = 14;
            len.Name = "ConnectionLength" + ConnectionLength.Count;
            len.FontWeight = FontWeights.Bold;
            ConnectionLength.Add(len);
            graphCanvas.Children.Add(len);
            Canvas.SetTop(len, -100);
            Line line = new Line();
            line.Name = "line" + Connections.Count;
            line.Stroke = Brushes.Black;
            Connections.Add(line);
            graphCanvas.Children.Add(line);
            Canvas.SetTop(line, -100);
            Canvas.SetLeft(line, 0);
        }

        private void AddNewConnection(double top, double left, double X1, double Y1, double X2, double Y2, int length, int start, int end)
        {
            DisableControls();
            if (Connections == null) Connections = new List<Line>();
            if (ConnectionLength == null) ConnectionLength = new List<TextBlock>();
            TextBlock len = new TextBlock();
            len.Text = length.ToString();
            len.Foreground = new SolidColorBrush(Colors.Black);
            len.FontSize = 14;
            len.Name = "ConnectionLength" + ConnectionLength.Count;
            len.FontWeight = FontWeights.Bold;
            ConnectionLength.Add(len);
            graphCanvas.Children.Add(len);
            Canvas.SetTop(len, (2*top + Y2) / 2);
            Canvas.SetLeft(len, (2*left + X2) / 2);
            Line line = new Line();
            line.Name = "line" + Connections.Count;
            line.Stroke = Brushes.Black;
            Connections.Add(line);
            graphCanvas.Children.Add(line);
            Canvas.SetTop(line, top);
            Canvas.SetLeft(line, left);
            line.X1 = X1;
            line.Y1 = Y1;
            line.X2 = X2;
            line.Y2 = Y2;
            connections.Add(new Connection(start, end, length));
            Connections[Connections.Count - 1].Tag = new Vector2(start, end);
        }
        #endregion

        #endregion

        #region Actual algorithm

        private void dijkstraButton_Click(object sender, RoutedEventArgs e)
        {
            Solve();
        }

        private void Solve()
        {
            Solve(Convert.ToInt32(endingVertexTextBox.Text));
        }
        async private void Solve(int destination)
        {
            DisableControls();
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].Stroke == Brushes.Red)
                {
                    Connections[i].StrokeThickness = 1;
                    Connections[i].Stroke = Brushes.Black;
                }
            }
            LogWindow log = new LogWindow((bool)logCheckBox.IsChecked);
            if ((bool)logCheckBox.IsChecked) log.Show();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            dijkstraButton.IsEnabled = false;
            await Task.Delay(1);
            string s = string.Empty;
            await log.Add("start");
            //START
            int startingVertex = Convert.ToInt32(startingVertexTextBox.Text);
            int endingVertex = destination;
            endingVertexTextBox.Text = destination.ToString();
            await log.Add("the starting vertex is " + startingVertex);
            List<int> PermanentList = new List<int>();
            PermanentList.Add(startingVertex);
            await log.Add(string.Format("P={" + "{0}" + "}", startingVertex));
            List<int> TemporaryList = GetAllVerticesConnectedToVertex(startingVertex);
            await log.Add(GetTemporaryListAsString(TemporaryList));
            VertexLabel[] label = new VertexLabel[Vertices.Count];
            label[startingVertex] = new VertexLabel(0, 0);
            for (int i = 0; i < label.Length; i++)
            {
                if (i == startingVertex)
                {
                    // await log.Add(string.Format("label[{0}] ({1}, {2}) D={3} Pred={4} ", i, startingVertex, i, label[i].D, label[i].P));
                    continue;
                }
                label[i] = new VertexLabel(-1, int.MaxValue);
                if (TemporaryList.Contains(i))
                    label[i] = new VertexLabel(startingVertex, GetDistance(startingVertex, i));
                //await log.Add(string.Format("label[{0}] ({1}, {2}) D={3} Pred={4} ", i, label[i].P, i, label[i].D, label[i].P));
            }
            while (TemporaryList.Count != 0) //correct but not fast: TemporaryList.Count != 0
            {
                //handle infinite loops
                if (!(bool)logCheckBox.IsChecked)
                    if (!(bool)coloredCheckBox.IsChecked)
                        if (sw.Elapsed.TotalMilliseconds > 5000)
                        {
                            EnableControls();
                            textBlock1.Text = "exited infinite loop";
                            log.Close();
                            return;
                        }
                for (int i = 0; i < PermanentList.Count; i++)
                {
                    for (int j = 0; j < TemporaryList.Count; j++)
                    {
                        int dist = label[PermanentList[i]].D + GetDistance(PermanentList[i], TemporaryList[j]);
                        //await log.Add(string.Format("dist({0}, {1})={2}", PermanentList[i], TemporaryList[j], dist));
                        if (dist < label[TemporaryList[j]].D)
                            if (GetDistance(PermanentList[i], TemporaryList[j]) != -1)
                            {
                                label[TemporaryList[j]].D = dist;
                                label[TemporaryList[j]].P = PermanentList[i];
                            }
                    }
                }

                int indexOfMin = 0;
                int min = int.MaxValue;
                for (int i = 0; i < label.Length; i++)
                {
                    if (i == startingVertex) continue;
                    if (PermanentList.Contains(i)) continue;
                    if (min > label[i].D)
                    {
                        min = label[i].D;
                        indexOfMin = i;
                    }
                }
                //await log.Add("minimum found! " + min + ", between " + indexOfMin + " and " + label[indexOfMin].P);
                if (!PermanentList.Contains(indexOfMin))
                {
                    PermanentList.Add(indexOfMin);
                }
                TemporaryList = GetAllVerticesConnectedToVertices(PermanentList);
                //filter
                for (int i = 0; i < PermanentList.Count; i++)
                {
                    for (int j = 0; j < TemporaryList.Count; j++)
                    {
                        if (PermanentList[i] == TemporaryList[j])
                        {
                            TemporaryList.RemoveAt(j);
                            i = 0;
                            break;
                        }
                    }
                }
                //remove multiple occurences

                for (int i = 0; i < TemporaryList.Count; i++)
                {
                    for (int j = 0; j < TemporaryList.Count; j++)
                    {
                        if (i == j) continue;
                        if (TemporaryList[i] == TemporaryList[j])
                        {
                            TemporaryList.RemoveAt(j);
                            i = 0;
                            break;
                        }
                    }
                }
                if ((TemporaryList.Count == 1) && (TemporaryList[0] == 0)) TemporaryList = new List<int>();

                if ((bool)coloredCheckBox.IsChecked)
                {
                    for (int i = 0; i < VertexCanvas.Count; i++)
                    {
                        if ((VertexCanvas[i]).Background != new SolidColorBrush(Colors.Transparent)) VertexCanvas[i].Background = new SolidColorBrush(Colors.Transparent);
                    }
                    for (int i = 0; i < PermanentList.Count; i++)
                    {
                        (VertexCanvas[PermanentList[i]]).Background = new SolidColorBrush(Colors.Green);
                    }
                    for (int i = 0; i < TemporaryList.Count; i++)
                    {
                        (VertexCanvas[TemporaryList[i]]).Background = new SolidColorBrush(Colors.Blue);
                    }
                    await Task.Delay(100);
                }
                await log.Add(GetPermanentListAsString(PermanentList) + "\n" + GetTemporaryListAsString(TemporaryList));
            }
            textBlock1.Text = GetPath(endingVertex, startingVertex, label);
            await log.Add("stop");
            if (!(bool)logCheckBox.IsChecked) log.Close();
            sw.Stop();
            textBlock1.Text += "\n" + (sw.Elapsed.TotalMilliseconds + "ms");
            EnableControls();
        }
    
        private string GetPath(int endingVertex, int stopAtVertex, VertexLabel[] list)
        {
            string s = endingVertex + ", ";
            for (int i = 0; i < Connections.Count; i++)
            {
                Vector2 v = Connections[i].Tag as Vector2;
                if (((v.X == endingVertex) && (v.Y == list[endingVertex].P)) || ((v.X == list[endingVertex].P) && (v.Y == endingVertex)))
                {
                    Connections[i].StrokeThickness = 3;
                    Connections[i].Stroke = Brushes.Red;
                }
            }
            if (endingVertex == stopAtVertex) return s + "distance: " + list[endingVertex].D;
            return s + GetPath(list[endingVertex].P, stopAtVertex, list);
        }
        private List<int> GetAllVerticesConnectedToVertex(int n)
        {
            List<int> something = new List<int>();
            foreach(var x in connections)
            {
                if (x.Start == n) something.Add(x.End);
                else if (x.End == n) something.Add(x.Start);
            }
            return something;
        }

        private List<int> GetAllVerticesConnectedToVertices(List<int> n)
        {
            //if (n.Count == connections.Count) return new List<int>();
            List<int> something = new List<int>();
            for (int i = 0; i < n.Count; i++)
            {
                foreach (var x in connections)
                {
                    if (x.Start == n[i]) /*if (!something.Contains(x.End))*/ something.Add(x.End);
                    else if (x.End == n[i]) /*if (!something.Contains(x.Start))*/ something.Add(x.Start);
                }
            }
            return something;
        }

        private int GetDistance(int v1, int v2)
        {
            for (int i = 0; i < connections.Count; i++)
                if (((v1 == connections[i].Start) && (v2 == connections[i].End)) || ((v1 == connections[i].End) && (v2 == connections[i].Start)))
                    return connections[i].Len;
            return -1;
        }

        private string GetTemporaryListAsString(List<int> TemporaryList)
        {
            string s = "T={";
            for (int i = 0; i < TemporaryList.Count; i++)
            {
                s += (i != TemporaryList.Count - 1) ? TemporaryList[i] + ", " : TemporaryList[i].ToString();
            }
            s += "}";
            return s;
        }

        private string GetPermanentListAsString(List<int> PermanentList)
        {
            string s = "P={";
            for (int i = 0; i < PermanentList.Count; i++)
            {
                s += (i != PermanentList.Count - 1) ? PermanentList[i] + ", " : PermanentList[i].ToString();
            }
            s += "}";
            return s;
        }






        #endregion

        #region Input handling

        #endregion

        #region Useful methods

        private void EnableControls()
        {
            logCheckBox.IsEnabled = dijkstraButton.IsEnabled = vertexButton.IsEnabled = connectionWeightTextBox.IsEnabled = clearButton.IsEnabled = startingVertexTextBox.IsEnabled = endingVertexTextBox.IsEnabled = saveButton.IsEnabled = true;
            twoWayConnectionButton.IsEnabled = (VertexCanvas != null) ? (VertexCanvas.Count >= 2) ? true : false : false;
        }

        private void DisableControls()
        {
            logCheckBox.IsEnabled = dijkstraButton.IsEnabled = twoWayConnectionButton.IsEnabled = vertexButton.IsEnabled = connectionWeightTextBox.IsEnabled = clearButton.IsEnabled = startingVertexTextBox.IsEnabled = endingVertexTextBox.IsEnabled = saveButton.IsEnabled = false;
        }

        #endregion

    }
}
