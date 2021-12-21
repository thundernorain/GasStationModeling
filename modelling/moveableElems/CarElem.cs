using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GasStationModeling.modelling.pictureView
{
    class CarElem : MoveableElem
    {
        Rect CarRect { get; set; }

        public CarElem(Canvas canvas,ImageBrush brush, Point location)
        {
            Tag = "car";
            Height = 48;
            Width = 48;
            Fill = brush;
            CurrentLocation = location;
            IsGoingToFill = false;
            canvas.Children.Add(this);
        }
    }
}
