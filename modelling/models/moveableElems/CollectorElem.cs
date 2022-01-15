using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GasStationModeling.modelling.pictureView
{
    public class CollectorElem : MoveableElem
    {
        public bool Start { get; set; }
        public CollectorElem(
            ImageBrush brush,
            Point location, 
            CollectorView collectorView)
        {
            Tag = collectorView;
            Height = ElementSizeHelper.CELL_WIDTH;
            Width = ElementSizeHelper.CELL_HEIGHT;
            Fill = brush;
            IsGoingToFill = false;
            Type = "Collector";
            Start = false;
        }

        public CollectorView View => Tag as CollectorView;
    }
}
