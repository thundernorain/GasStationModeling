using GasStationModeling.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.core.topology.mapper
{
    public class GasStationElementToTopologyElement
    {
        public TopologyElement[,] MapArray(IGasStationElement[,] array)
        {
            var result = new TopologyElement[array.GetLength(0), array.GetLength(1)];

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    result[i, j] = Map(array[i, j]);
                }
            }

            return result;
        }

        public TopologyElement Map(IGasStationElement element)
        {
            if(element is Cashbox)
            {
                return TopologyElement.CashBox;
            }
            else if(element is FuelDispenser)
            {
                return TopologyElement.FuelDispenser;
            }
            else if(element is Tank)
            {
                return TopologyElement.Tank;
            }
            else if(element is Entrance)
            {
                return TopologyElement.Entrance;
            }
            else if(element is Exit)
            {
                return TopologyElement.Exit;
            }

            return TopologyElement.Nothing;
        }
    }
}
