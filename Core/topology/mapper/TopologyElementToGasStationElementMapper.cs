using GasStationModeling.core.models;
using GasStationModeling.core.topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.core.mapper
{
    class TopologyElementToGasStationElementMapper
    {
        public IGasStationElement[,] MapArray(TopologyElement[,] topologyElements)
        {
            var result = new IGasStationElement[topologyElements.GetLength(0), topologyElements.GetLength(1)];

            for(int i = 0; i < topologyElements.GetLength(0); i++)
            {
                for (int j = 0; j < topologyElements.GetLength(1); j++)
                {
                    result[i,j] = Map(topologyElements[i, j]);
                }
            }

            return result;
        }

        public IGasStationElement Map(TopologyElement topologyElement)
        {
            switch (topologyElement)
            {
                case TopologyElement.Tank:
                    return new Tank();
                    break;

                case TopologyElement.CashBox:
                    return new Cashbox();
                    break;

                case TopologyElement.FuelDispenser:
                    return new FuelDispenser();
                    break;

                case TopologyElement.Entrance:
                    return new Entrance();
                    break;

                case TopologyElement.Exit:
                    return new Exit();
                    break;
            }

            return null;
        }
    }
}
