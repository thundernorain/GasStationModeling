using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.model;
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

        public CarElem(Canvas canvas,ImageBrush brush, Point location, CarView carView)
        {
            Tag = carView;
            Height = ElementSizeHelper.CELL_WIDTH;
            Width = ElementSizeHelper.CELL_HEIGHT;
            Fill = brush;
            CurrentLocation = location;
            IsGoingToFill = false;
            canvas.Children.Add(this);
        }
    }
}
