using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GasStationModeling.modelling.helpers
{
    public class BrushHelper
    {

        public static ImageBrush getBrushFor(String resourceName)
        {
            ImageBrush brush = new ImageBrush();
            var roadImage = Application.Current.TryFindResource(resourceName) as BitmapImage;
            brush.ImageSource = roadImage;
            return brush;
        }
    }
}
