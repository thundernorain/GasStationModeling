﻿using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.model;
using GasStationModeling.modelling.pictureView;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GasStationModeling.modelling.moveableElems
{
    public class RefuellerElem : MoveableElem
    {
        public RefuellerElem(
            ImageBrush brush,
            Point location,
            RefuellerView refuellerView)
        {
            Tag = refuellerView;
            Height = ElementSizeHelper.CELL_WIDTH;
            Width = ElementSizeHelper.CELL_HEIGHT;
            Fill = brush;
        }
    }
}
