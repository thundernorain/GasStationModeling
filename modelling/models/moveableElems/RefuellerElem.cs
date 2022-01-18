using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.model;
using GasStationModeling.modelling.pictureView;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GasStationModeling.modelling.moveableElems
{
    public class RefuellerElem : MoveableElem
    {
        public bool IsWaiting { get; set; }
        public bool IsGoingToWait { get; set; }

        public RefuellerElem(
            ImageBrush brush,
            Point location,
            RefuellerView refuellerView)
        {
            Tag = refuellerView;
            Height = ElementSizeHelper.CELL_WIDTH;
            Width = ElementSizeHelper.CELL_HEIGHT;
            Fill = brush;
            Type = "Collector";
            IsFilled= false;
            IsOnStation = false;
            IsWaiting = false;
            IsGoingToWait = false;
        }

        public RefuellerView View => Tag as RefuellerView;
    }
}
