﻿using GasStationModeling.modelling.model;
using GasStationModeling.modelling.pictureView;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GasStationModeling.modelling.helpers
{
    class IntersectInspector
    {
        private Canvas stationCanvas;

        public IntersectInspector(Canvas stationCanvas)
        {
            this.stationCanvas = stationCanvas;
        }

        public Point PreventIntersection(ref MoveableElem activeVehicle, Directions direction)
        {
            var destPoint = activeVehicle.GetDestinationPoint();

            Rect activeVehicleRect = new Rect(
                Canvas.GetLeft(activeVehicle),
                Canvas.GetTop(activeVehicle),
                activeVehicle.Width,
                activeVehicle.Height);

            foreach (var elem in stationCanvas.Children.OfType<MoveableElem>())
            {
                if (elem == activeVehicle)
                {
                    continue;
                }

                var moveableElem = elem;

                Rect moveableElemRect = new Rect(
                    Canvas.GetLeft(moveableElem),
                    Canvas.GetTop(moveableElem),
                    moveableElem.Width,
                    moveableElem.Height);

                if (!activeVehicleRect.IntersectsWith(moveableElemRect))
                {
                    continue;
                }

                /*var anotherVehicle = moveableElem;

                switch (direction)
                {
                    case Directions.Up:
                        {
                            Canvas.SetTop(activeVehicle, bottom(anotherVehicle));

                            break;
                        }

                    case Directions.Right:
                        {
                            Canvas.SetLeft(activeVehicle, left(anotherVehicle) - activeVehicle.Width);
                            break;
                        }

                    case Directions.Down:
                        {
                            Canvas.SetTop(activeVehicle, top(anotherVehicle) - activeVehicle.Height);
                            break;
                        }

                    case Directions.Left:
                        {
                            Canvas.SetLeft(activeVehicle, right(anotherVehicle));
                            break;
                        }
                }

                // Fuel Dispenser
                if (moveableElem.Tag is DispenserView || moveableElem.Tag is CashBoxView)
                {
                    var fuelDispenser = moveableElem;

                    var initialDestinationPoint = activeVehicle.GetDestinationPoint();

                    double newDestX;
                    double newDestY;

                    bool bypassFromLeft = false;
                    bool bypassFromRight = false;
                    bool bypassFromBottom = false;
                    bool bypassFromTop = false;

                    Point newDestinationPoint1;
                    Point newDestinationPoint2;
                    Point newDestinationPoint3;

                    switch (direction)
                    {
                        case Directions.Up:
                            {
                                Canvas.SetTop(activeVehicle, bottom(fuelDispenser));

                                if (!activeVehicle.IsBypassingObject)
                                {
                                    activeVehicle.IsBypassingObject = true;

                                    if (destPoint.X < left(activeVehicle))
                                    {
                                        newDestX = left(fuelDispenser) - (activeVehicle.Width + 5);
                                        bypassFromLeft = true;
                                    }
                                    else
                                    {
                                        newDestX = right(fuelDispenser) + (activeVehicle.Width + 5);
                                        bypassFromRight = true;
                                    }

                                    newDestY = bottom(fuelDispenser) + 10;


                                    newDestinationPoint1 = new Point(newDestX,
                                        newDestY);

                                    newDestY = top(fuelDispenser) + activeVehicle.Height + 10;
                                    newDestinationPoint2 = new Point(newDestX,
                                        newDestY);

                                    activeVehicle.removeDestinationPoint();
                                    activeVehicle.AddDestinationPoint(newDestinationPoint2);
                                    activeVehicle.AddDestinationPoint(newDestinationPoint1);
                                }
                                break;
                            }

                        case Directions.Right:
                            {
                                break;
                            }

                        case Directions.Down:
                            {
                                Canvas.SetTop(activeVehicle, top(fuelDispenser) - activeVehicle.Height);   
                                if (!activeVehicle.IsBypassingObject)
                                {
                                    activeVehicle.IsBypassingObject = true;


                                    newDestX = left(fuelDispenser) - (activeVehicle.Width + 5);

                                    newDestY = top(fuelDispenser) - 10;

                                    newDestinationPoint1 = new Point(newDestX,
                                        newDestY);

                                    activeVehicle.removeDestinationPoint();
                                    activeVehicle.AddDestinationPoint(newDestinationPoint1);
                                }

                                break;
                            }

                        case Directions.Left:
                            {
                                Canvas.SetLeft(activeVehicle, right(fuelDispenser));
                                if (!activeVehicle.IsBypassingObject)
                                {
                                    activeVehicle.IsBypassingObject = true;


                                    newDestX = right(fuelDispenser) + 10;
                                    newDestY = top(fuelDispenser) - (ElementSizeHelper.CELL_HEIGHT + 5);

                                    newDestinationPoint1 = new Point(newDestX,
                                        newDestY);

                                    if (activeVehicle.IsFilled)
                                    {
                                        newDestX = left(fuelDispenser) - ElementSizeHelper.CELL_WIDTH - 5;
                                    }
                                    else
                                    {
                                        activeVehicle.IsGoesHorizontal = true;
                                        newDestX = (int)initialDestinationPoint.X + ElementSizeHelper.CELL_WIDTH / 2;
                                    }

                                    newDestinationPoint2 = new Point(newDestX, newDestY);

                                    activeVehicle.FromLeftBypassingPoint = newDestinationPoint2;

                                    newDestinationPoint3 = new Point(left(fuelDispenser) - 20,
                                        destPoint.Y - 20);

                                    activeVehicle.removeDestinationPoint();
                                    activeVehicle.AddDestinationPoint(newDestinationPoint2);
                                    activeVehicle.AddDestinationPoint(newDestinationPoint1);
                                }

                                break;
                            }
                    }
                }*/
            }

            return activeVehicle.GetDestinationPoint();
        }

        public double left(UIElement element)
        {
            return Canvas.GetLeft(element);
        }

        public double right(UIElement element)
        {
            return stationCanvas.Width - (Canvas.GetLeft(element) + ElementSizeHelper.CELL_WIDTH);
        }

        public double top(UIElement element)
        {
            return Canvas.GetTop(element);
        }

        public double bottom(UIElement element)
        {
            return stationCanvas.Height - (Canvas.GetTop(element) + ElementSizeHelper.CELL_WIDTH);
        }
    }

    
}
