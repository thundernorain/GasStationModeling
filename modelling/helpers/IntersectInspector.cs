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

        private const int collisionMaskCorrector = 6;

        public IntersectInspector(Canvas stationCanvas,DestinationPointHelper dpHelper)
        {
            this.stationCanvas = stationCanvas;
            this.dpHelper = dpHelper;
        }

        public Point PreventIntersection(ref MoveableElem activeVehicle, Directions direction)
        {
            var destPoint = activeVehicle.GetDestinationPoint();

            Rect activeVehicleRect = new Rect(
                left(activeVehicle) + collisionMaskCorrector,
                top(activeVehicle) + collisionMaskCorrector,
                activeVehicle.Width - 2 * collisionMaskCorrector,
                activeVehicle.Height - 2 * collisionMaskCorrector);

            foreach (var elem in stationCanvas.Children)
            {
                if (elem == activeVehicle)
                {
                    continue;
                }

                var moveableElem = elem as UIElement;

                Rect moveableElemRect = new Rect(
                    Canvas.GetLeft(moveableElem),
                    Canvas.GetTop(moveableElem),
                    ElementSizeHelper.CELL_WIDTH,
                    ElementSizeHelper.CELL_HEIGHT);

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
                                                                     10,
                                                                     10);

                                    if (anotherVehicleRect.IntersectsWith(destSpot))
                                    {
                                        activeVehicle.removeDestinationPoints();
                                        activeVehicle.AddDestinationPoint(dpHelper.LeavePointFilled);
                                        break;
                                    }
                                }
                                Canvas.SetTop(activeVehicle, bottom(anotherVehicle) - collisionMaskCorrector / 2);

                                break;
                            }

                        case Directions.Right:
                            {
                                Canvas.SetLeft(activeVehicle, left(anotherVehicle) - (activeVehicle.Width - collisionMaskCorrector / 2));
                                break;
                            }

                        case Directions.Down:
                            {
                                Canvas.SetTop(activeVehicle, top(anotherVehicle) - (activeVehicle.Height - collisionMaskCorrector / 2));
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
                                                                     10,
                                                                     10);

                                    if (anotherVehicleRect.IntersectsWith(destSpot))
                                    {
                                        activeVehicle.removeDestinationPoints();
                                        activeVehicle.AddDestinationPoint(dpHelper.LeavePointFilled);
                                    }
                                }

                                Canvas.SetLeft(activeVehicle, right(anotherVehicle) - collisionMaskCorrector / 2);
                                break;
                            }
                    }
                }
                else if(elem is Rectangle)
                {
                    // Fuel Dispenser
                    if ((elem as Rectangle).Tag is DispenserView || (elem as Rectangle).Tag is CashBoxView)
                    {
                        var stationItem = elem as Rectangle;

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
                                    if (left(activeVehicle) >= right(stationItem) ||
                                        right(activeVehicle) <= left(stationItem) &&
                                        top(activeVehicle) < bottom(stationItem)) break;

                                    Canvas.SetTop(activeVehicle, bottom(stationItem) - collisionMaskCorrector / 2);

                                    if (!activeVehicle.IsBypassingObject)
                                    {
                                        activeVehicle.IsBypassingObject = true;


                                        //left
                                        if (destPoint.X < left(activeVehicle))
                                        {
                                            newDestX = left(stationItem) - (activeVehicle.Width + 1);
                                            bypassFromLeft = true;
                                        }
                                        //right
                                        else if (destPoint.X > left(activeVehicle))
                                        {
                                            newDestX = right(stationItem) + 1;
                                            bypassFromRight = true;
                                        }                    

                                        newDestY = bottom(stationItem) + 1;


                                        newDestinationPoint1 = new Point(newDestX,
                                            newDestY);

                                        newDestY = top(stationItem) - activeVehicle.Height;
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
                                    if (left(activeVehicle) >= right(stationItem) ||
                                       right(activeVehicle) <= left(stationItem) &&
                                       bottom(activeVehicle) > top(stationItem)) break;

                                    Canvas.SetTop(activeVehicle, top(stationItem) - activeVehicle.Height + collisionMaskCorrector / 2);
                                    if (!activeVehicle.IsBypassingObject)
                                    {
                                        activeVehicle.IsBypassingObject = true;

                                        //left
                                        if (destPoint.X < left(activeVehicle))
                                        {
                                            newDestX = left(stationItem) - (activeVehicle.Width + 1);
                                            bypassFromLeft = true;
                                        }
                                        //right
                                        else if (destPoint.X > left(activeVehicle))
                                        {
                                            newDestX = right(stationItem) + ElementSizeHelper.CELL_WIDTH + 1;
                                            bypassFromRight = true;
                                        }
                                        else
                                        {
                                            newDestinationPoint2 = new Point();
                                        }
                                        newDestY = bottom(stationItem) + ElementSizeHelper.CELL_WIDTH + 1;

                                        newDestinationPoint1 = new Point(newDestX,
                                            newDestY);

                                        activeVehicle.AddDestinationPoint(newDestinationPoint1);
                                    }

                                    break;
                                }

                            case Directions.Left:
                                {
                                    if (top(activeVehicle) >= bottom(stationItem) ||
                                       bottom(activeVehicle) <= top(stationItem) &&
                                       left(activeVehicle) < right(stationItem)) break;
                                    else
                                    {
                                        Canvas.SetLeft(activeVehicle, right(stationItem) - collisionMaskCorrector / 2);
                                        if (!activeVehicle.IsBypassingObject)
                                        {
                                            activeVehicle.IsBypassingObject = true;


                                            newDestX = right(stationItem);
                                            newDestY = bottom(stationItem) - 5;

                                            newDestinationPoint1 = new Point(newDestX,
                                                newDestY);

                                            //upper
                                            if (destPoint.Y < top(activeVehicle))
                                            {
                                                newDestX = left(stationItem) - ElementSizeHelper.CELL_WIDTH - 5;
                                                newDestY = top(stationItem) - ElementSizeHelper.CELL_HEIGHT;
                                            }
                                            else
                                            {
                                                activeVehicle.IsGoesHorizontal = true;
                                                newDestX = (int)initialDestinationPoint.X + ElementSizeHelper.CELL_WIDTH;
                                            }

                                            newDestinationPoint2 = new Point(newDestX, newDestY);
                                             
                                            activeVehicle.FromLeftBypassingPoint = newDestinationPoint2;

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
