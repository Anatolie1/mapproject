using GMap.NET.WindowsPresentation;
using System.Windows;

namespace ProjectEID
{
    internal class UserMarker : UIElement
    {
        private MainWindow mainWindow;
        private GMapMarker mapMarker;

        public UserMarker(MainWindow mainWindow, GMapMarker mapMarker)
        {
            this.mainWindow = mainWindow;
            this.mapMarker = mapMarker;
        }
    }
}