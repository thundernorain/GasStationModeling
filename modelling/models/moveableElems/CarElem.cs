using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GasStationModeling.modelling.pictureView
{
    public class CarElem : MoveableElem
    {
        
        public CarElem(ImageBrush brush, Point location, CarView carView)
        {
            Tag = carView;
            Height = ElementSizeHelper.CELL_WIDTH;
            Width = ElementSizeHelper.CELL_HEIGHT;
            Fill = brush;
            IsGoingToFill = false;
        }
    }
}
