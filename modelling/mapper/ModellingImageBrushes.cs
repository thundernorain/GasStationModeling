using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GasStationModeling.modelling.mapper
{
    class ModellingImageBrushes
    {

        public static ImageBrush getImageBrushFor(String resourceName)
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = Application.Current.TryFindResource(resourceName) as BitmapImage;
            return brush;
        }
    }
}
