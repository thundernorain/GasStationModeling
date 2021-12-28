using GasStationModeling.core.models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.core.DB.dto
{
    public class TopologyDTO
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public String Name { get; set; }
        public IGasStationElement[,] Topology { get; set; }
        public int ServiceAreaWidth { get; set; }
    }
}
