using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.pictureView
{
    public class MoveableElem : Shape
    {

        public Rect intersectRect { get; set; }

        protected override Geometry DefiningGeometry { get; }
        public override Geometry RenderedGeometry { get; }

        public bool IsGoingToFill { get; set; }
        public bool IsOnStation { get; set; }
        public bool IsFilled { get; set; }
        public bool IsFilling { get; set; }
        public bool IsBypassingObject { get; set; }
        public bool IsGoesHorizontal { get; set; }
        public System.Windows.Point FromLeftBypassingPoint { get; set; }

        private readonly List<System.Windows.Point> destinationPoints;

        public System.Windows.Point DestinationSpot { get; set; }

        public MoveableElem()
        {
            destinationPoints = new List<System.Windows.Point>();
        }

        public void AddDestinationPoint(System.Windows.Point destPoint)
        {
            destinationPoints.Add(destPoint);
        }

        public System.Windows.Point GetDestinationPoint()
        {
            return destinationPoints.Count == 0 ? new System.Windows.Point(-1, -1) : destinationPoints.Last();
        }

        public bool HasDestPoints()
        {
            return destinationPoints.Count > 0;
        }

        public void removeDestinationPoints()
        {
            destinationPoints.Clear();
        }

        public void removeDestinationPoint(Point destpoint)
        {
            destinationPoints.Remove(destpoint);
        }

        public Rect createIntersectRect()
        {
            intersectRect = new Rect(
                Canvas.GetLeft(this),
                Canvas.GetTop(this),
                this.Width, 
                this.Height);
            return intersectRect;
        }
    }
}
