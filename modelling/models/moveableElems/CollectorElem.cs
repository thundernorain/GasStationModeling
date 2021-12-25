using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GasStationModeling.modelling.pictureView
{
    class CollectorElem : MoveableElem
    {
        Rect CollectorRect { get; set; }

        public CollectorElem(Canvas canvas,
            ImageBrush brush,
            Point location, 
            CollectorView collectorView)
        {
            Tag = collectorView;
            Height = ElementSizeHelper.CELL_WIDTH;
            Width = ElementSizeHelper.CELL_HEIGHT;
            Fill = brush;
            CurrentLocation = location;
            IsGoingToFill = false;
            canvas.Children.Add(this);
        }
    }
}
