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
    class CollectorElem : MoveableElem
    {
        Rect CollectorRect { get; set; }

        public CollectorElem(Canvas canvas, ImageBrush brush, Point location)
        {
            Tag = "Collector";
            Height = 48;
            Width = 48;
            Fill = brush;
            CurrentLocation = location;
            IsGoingToFill = false;
            canvas.Children.Add(this);
        }
    }
}
