using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.core.topology
{
    public partial class Topology
    {
        public static readonly int MinColsCount = 10;
        public static readonly int MaxColsCount = 20;

        public static readonly int MinRowsCount = 7;
        public static readonly int MaxRowsCount = 15;

        public static readonly double ServiceAreaInShares = 0.25;

        public const int TOPOLOGY_CELL_SIZE = 48;

        public const int CASHBOX_MAX_COUNT = 1;
        public const int FUEL_DISPENSER_MAX_COUNT = 4;
        public const int TANK_MAX_COUNT = 7;
        public const int ENTRANCE_MAX_COUNT = 1;
        public const int EXIT_MAX_COUNT = 1;
    }
}
