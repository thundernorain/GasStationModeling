using GasStationModeling.modelling.helpers;
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
       

        public bool IsGoingToFill { get; set; }
        public bool IsOnStation { get; set; }
        public bool IsFilled { get; set; }
        public bool IsFilling { get; set; }
        public bool IsBypassingObject { get; set; }
        public bool IsGoesHorizontal { get; set; }
        public System.Windows.Point FromLeftBypassingPoint { get; set; }

        private readonly List<System.Windows.Point> destinationPoints;

        public Rect intersectRect { get; set; }

        public Rect DestinationSpot { get; set; }


        public Rect EmptyRect {get;} = new Rect();


        public MoveableElem()
        {
            destinationPoints = new List<System.Windows.Point>();
            DestinationSpot = EmptyRect;
            Canvas.SetZIndex(this, 1);
        }

        public void AddDestinationPoint(System.Windows.Point destPoint)
        {
            destinationPoints.Add(destPoint);
        }

        public System.Windows.Point GetDestinationPoint()
        {
            return destinationPoints.Count == 0 ? new System.Windows.Point(-1, -1) : destinationPoints.Last();
        }

        public bool SpotIsNull
        {
            get
            {
                return DestinationSpot.Equals(EmptyRect);
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                Rect rect = new Rect();
                rect.Width = ElementSizeHelper.CELL_WIDTH;
                rect.Height = ElementSizeHelper.CELL_HEIGHT;
                return new RectangleGeometry(rect);
            }
        }

        public override Geometry RenderedGeometry
        {
            get
            {
                Rect rect = new Rect();
                rect.Width = ElementSizeHelper.CELL_WIDTH;
                rect.Height = ElementSizeHelper.CELL_HEIGHT;
                rect.X = Canvas.GetLeft(this);
                rect.Y = Canvas.GetTop(this);
                return new RectangleGeometry(rect);
            }
        }
        public bool HasDestPoints()
        {
            return destinationPoints.Count > 0;
        }

        public void removeDestinationPoints()
        {
            destinationPoints.Clear();
        }

        public void removeDestinationPoint()
        {
            if (!HasDestPoints()) return;
            destinationPoints.Remove(destinationPoints.Last());

            if(SpotIsNull)
            {
                DestinationSpot = EmptyRect;
            }
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
