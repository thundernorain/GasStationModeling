using GasStationModeling.core.topology;
using GasStationModeling.modelling.model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.helpers
{
    public class CanvasParser
    {
        public Canvas StationCanvas {get;set;}
        //TODO : просто в маппере сюда все закидывать будем, а потом брать

        #region CanvasGetters

        public Rectangle CashBox { get; set; }

        public List<Rectangle> Dispensers { get; set; }

        public List<Rectangle> Tanks { get; set; }

        public Rectangle Entrance { get; set; }

        public Rectangle Exit { get; set; }

        #endregion

        public CanvasParser()
        {
            Dispensers = new List<Rectangle>();
            Tanks = new List<Rectangle>();
        }
    }
}
