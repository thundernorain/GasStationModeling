using GasStationModeling.core.topology;
using GasStationModeling.modelling.helpers;
using GasStationModeling.settings_screen.model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.mapper
{
    class TopologyMapper
    {      
        public static Canvas mapTopology(Canvas stationCanvas, Topology currentTopology, ModellingSettings settings)
        {
            int width = currentTopology.TopologyColumnCountMain
                + currentTopology.TopologyColumnCountWorker;

            int height = currentTopology.TopologyRowCount;
            stationCanvas.Width = width * ElementSizeHelper.CELL_WIDTH;
            stationCanvas.Height = (height + 3) * ElementSizeHelper.CELL_HEIGHT;

            ImageBrush brush = new ImageBrush();
         
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {               
                    var elemImage = currentTopology.GetCellImage(i, j);
                    brush.ImageSource = elemImage;
                    Rectangle topologyElem = new Rectangle()
                    {
                        Tag = currentTopology.TopologyElements[i, j],
                        Height = 20,
                        Width = 5,
                        Fill = brush
                    };
                    Canvas.SetLeft(topologyElem, ElementSizeHelper.CELL_WIDTH * i);
                    Canvas.SetTop(topologyElem, ElementSizeHelper.CELL_HEIGHT * j);
                    stationCanvas.Children.Add(topologyElem);
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = height; j < stationCanvas.Height; j++)
                {
                    //var roadImage = 
                }
            }

            return stationCanvas;
        }

        public static List<Rectangle> getTopologyElemsWithTypeOf(TopologyElement topologyElementType, Canvas stationCanvas)
        {
            return stationCanvas.Children
                .OfType<Rectangle>()
                .Where(elem => (TopologyElement)elem.Tag  == topologyElementType)
                .ToList();
        }
    }
}
