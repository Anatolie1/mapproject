using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ProjectEID
{
    public partial class MainWindow : Window
    {
        private List<PointLatLng> _points;
        private GMapControl _gMapControl;

        public MainWindow()
        {
            InitializeComponent();
            _gMapControl = new GMapControl();
            _points = new List<PointLatLng>();
            Loaded += Focus_OnStart;
        }

        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            mapView.MapProvider = OpenStreetMapProvider.Instance;
            mapView.SetPositionByKeywords("Paris, France");                
            mapView.MinZoom = 2;
            mapView.MaxZoom = 20;
            mapView.Zoom = 7;
            mapView.MouseWheelZoomType = MouseWheelZoomType.ViewCenter;
            mapView.DragButton = MouseButton.Left;
            mapView.ShowCenter = true;           
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            RoutingProvider routingProvider = GMapProviders.OpenStreetMap;
            MapRoute route = routingProvider.GetRoute(_points[0], _points[1], false, false, 14);
            GMapRoute mapRoute = new GMapRoute(route.Points);
            mapView.Markers.Add(mapRoute);
            Distance.Content = ((int)route.Distance).ToString() + " km"+"\t"+ route.Duration;
            _points.Add(new PointLatLng(48.3, 4.07));
            var polygon = new GMapPolygon(_points);
            mapView.Markers.Add(polygon);
        }

        private void Focus_OnStart(object sender, EventArgs e)
        {
            Start.Focus();
        }        

        private void Start_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {                
                Destination.Focus();
                var startPoint = ConvertPlaceCoordinate(Start.Text);
                DisplayPoint(startPoint);
            }
        }

        private void Destination_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Load.Focus();
                var destinationPoint = ConvertPlaceCoordinate(Destination.Text);
                DisplayPoint(destinationPoint);
            }
        }

        private PointLatLng ConvertPlaceCoordinate(string name)
        {
            return _gMapControl.GetPositionByKeywords(name);
        }

        private void DisplayPoint(PointLatLng point)
        {

            GMapMarker marker = new GMapMarker(point);
            var image = new Image
            {
                Source = new BitmapImage(new Uri(@"E:\ProjectEID\Markers\2a.png")),
                Width = 50,
                Height = 50,
            };
            marker.Shape = image;
            marker.Offset = new Point(-image.Height / 2, -image.Width);
            mapView.Markers.Add(marker);
            _points.Add(point);
        }

        private void mapView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousePoint = e.GetPosition(mapView);
            var location = mapView.FromLocalToLatLng((int)mousePoint.X, (int)mousePoint.Y);
            Start.Text = location.Lat.ToString();
            Destination.Text = location.Lng.ToString();
            DisplayPoint(location);
            var address = GetAddress(location);
            MessageBox.Show(address);
        }

        private string GetAddress(PointLatLng point)
        {
            string address = "not found";
            List<Placemark> placemarks = null;
            var st = GMapProviders.GoogleMap.GetPlacemarks(point, out placemarks);
            return st == GeoCoderStatusCode.REQUEST_DENIED ? address : "Yes";

            /* var placemark = GMapProviders.GoogleMap.GetPlacemark(point, out var st);
             if (st != GeoCoderStatusCode.OK || placemark == null) return;*/

        }
    }
}
