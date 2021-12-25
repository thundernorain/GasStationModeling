using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.model;
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

        public RefuellerElem(
            Canvas canvas,
            ImageBrush brush,
            Point location,
            RefuellerView refuellerView)
        {
            Tag = refuellerView;
            Height = ElementSizeHelper.CELL_WIDTH;
            Width = ElementSizeHelper.CELL_HEIGHT;
            Fill = brush;
            CurrentLocation = location;
            canvas.Children.Add(this);
        }
    }
}
