using GasStationModeling.modelling.pictureView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GasStationModeling.modelling.moveableElems
{
    class RefuellerElem : MoveableElem
    {
        Rect CollectorRect { get; set; }

        public RefuellerElem(Canvas canvas, ImageBrush brush, Point location)
        {
            Tag = "Collector";
            Height = 48;
            Width = 48;
            Fill = brush;
            CurrentLocation = location;
            canvas.Children.Add(this);
        }
    }
}
