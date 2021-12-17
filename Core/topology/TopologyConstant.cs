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
    }
}
