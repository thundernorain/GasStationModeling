using GasStationModeling.modelling.model;
using GasStationModeling.modelling.pictureView;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.helpers
{
    class IntersectInspector
    {
        private Canvas stationCanvas;
        private DestinationPointHelper dpHelper;

        public IntersectInspector(Canvas stationCanvas,DestinationPointHelper dpHelper)
        {
            this.stationCanvas = stationCanvas;
            this.dpHelper = dpHelper;
        }

        public Point PreventIntersection(ref MoveableElem activeVehicle, Directions direction)
        {
            var destPoint = activeVehicle.GetDestinationPoint();

            Rect activeVehicleRect = new Rect(
                left(activeVehicle) + 4,
                top(activeVehicle) + 4,
                activeVehicle.Width - 8,
                activeVehicle.Height - 8);

            foreach (var elem in stationCanvas.Children)
            {
                if (elem == activeVehicle)
                {
                    continue;
                }

                var moveableElem = elem as UIElement;

                Rect moveableElemRect = new Rect(
                    Canvas.GetLeft(moveableElem) + 1,
                    Canvas.GetTop(moveableElem) + 1,
                    ElementSizeHelper.CELL_WIDTH - 2,
                    ElementSizeHelper.CELL_HEIGHT - 2);

                if (!activeVehicleRect.IntersectsWith(moveableElemRect))
                {
                    continue;
                }

                if (elem is MoveableElem anotherVehicle)
                {

                    switch (direction)
                    {
                        case Directions.Up:
                            {                     
                                if (activeVehicle.Tag is CarView
                                    && (destPoint.Equals(dpHelper.EntrancePoint) 
                                        || dpHelper.FuelDispensersDestPoints.ContainsValue(destPoint)))
                                {
                                    Rect anotherVehicleRect = anotherVehicle.createIntersectRect();

                                    var destSpot = new Rect(
                                                            destPoint.X,
                                                            destPoint.Y,
                                                                     7,
                                                                     7);

                                    if (anotherVehicleRect.IntersectsWith(destSpot))
                                    {
                                        activeVehicle.removeDestinationPoints();
                                        activeVehicle.AddDestinationPoint(dpHelper.LeavePointFilled);
                                    }
                                }
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
                                if (activeVehicle.Tag is CarView
                                    && (destPoint.Equals(dpHelper.EntrancePoint) 
                                        || dpHelper.FuelDispensersDestPoints.ContainsValue(destPoint)))
                                {
                                    Rect anotherVehicleRect = anotherVehicle.createIntersectRect();

                                    var destSpot = new Rect(
                                                            destPoint.X,
                                                            destPoint.Y,
                                                                     7,
                                                                     7);

                                    if (anotherVehicleRect.IntersectsWith(destSpot))
                                    {
                                        activeVehicle.removeDestinationPoints();
                                        activeVehicle.AddDestinationPoint(dpHelper.LeavePointFilled);
                                    }
                                }

                                Canvas.SetLeft(activeVehicle, right(anotherVehicle));
                                break;
                            }
                    }
                }
                else if(elem is Rectangle)
                {
                    // Fuel Dispenser
                    if ((elem as Rectangle).Tag is DispenserView || (elem as Rectangle).Tag is CashBoxView)
                    {
                        var fuelDispenser = elem as Rectangle;

                        var initialDestinationPoint = activeVehicle.GetDestinationPoint();

                        double newDestX = -1;
                        double newDestY = -1;

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
                                    if (left(activeVehicle) >= right(fuelDispenser) ||
                                        right(activeVehicle) <= left(fuelDispenser) &&
                                        top(activeVehicle) < bottom(fuelDispenser)) break;

                                    Canvas.SetTop(activeVehicle, bottom(fuelDispenser));

                                    if (!activeVehicle.IsBypassingObject)
                                    {
                                        activeVehicle.IsBypassingObject = true;


                                        //left
                                        if (destPoint.X < left(activeVehicle))
                                        {
                                            newDestX = left(fuelDispenser) - (activeVehicle.Width + 1);
                                            bypassFromLeft = true;
                                        }
                                        //right
                                        else if (destPoint.X >= left(activeVehicle))
                                        {
                                            newDestX = right(fuelDispenser);
                                            bypassFromRight = true;
                                        }

                                        newDestY = bottom(fuelDispenser) + 1;


                                        newDestinationPoint1 = new Point(newDestX,
                                            newDestY);

                                        newDestY = top(fuelDispenser) - activeVehicle.Height;
                                        newDestinationPoint2 = new Point(newDestX,
                                            newDestY);
                                           
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
                                    if (left(activeVehicle) >= right(fuelDispenser) ||
                                       right(activeVehicle) <= left(fuelDispenser) &&
                                       bottom(activeVehicle) > top(fuelDispenser)) break;

                                    Canvas.SetTop(activeVehicle, top(fuelDispenser) - activeVehicle.Height);
                                    if (!activeVehicle.IsBypassingObject)
                                    {
                                        activeVehicle.IsBypassingObject = true;

                                        //left
                                        if (destPoint.X < left(activeVehicle))
                                        {
                                            newDestX = left(fuelDispenser) - (activeVehicle.Width + 1);
                                            bypassFromLeft = true;
                                        }
                                        //right
                                        else if (destPoint.X >= left(activeVehicle))
                                        {
                                            newDestX = right(fuelDispenser);
                                            bypassFromRight = true;
                                        }

                                        newDestY = bottom(fuelDispenser);

                                        newDestinationPoint1 = new Point(newDestX,
                                            newDestY);

                                        //activeVehicle.removeDestinationPoint();
                                        activeVehicle.AddDestinationPoint(newDestinationPoint1);
                                    }

                                    break;
                                }

                            case Directions.Left:
                                {
                                    if (top(activeVehicle) >= bottom(fuelDispenser) ||
                                       bottom(activeVehicle) <= top(fuelDispenser) &&
                                       left(activeVehicle) < right(fuelDispenser)) break;
                                    else
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

                                            //activeVehicle.removeDestinationPoint();
                                            activeVehicle.AddDestinationPoint(newDestinationPoint2);
                                            activeVehicle.AddDestinationPoint(newDestinationPoint1);
                                        }
                                        break;
                                    }
                                }
                        }
                    }
                }          
            }
            return activeVehicle.GetDestinationPoint();
        }

        public double left(UIElement element)
        {
            return Canvas.GetLeft(element);
        }

        public double right(UIElement element)
        {
            return Canvas.GetLeft(element) + ElementSizeHelper.CELL_WIDTH;
        }

        public double top(UIElement element)
        {
            return Canvas.GetTop(element);
        }

        public double bottom(UIElement element)
        {
            return Canvas.GetTop(element) + ElementSizeHelper.CELL_HEIGHT;
        }
    }   
}
