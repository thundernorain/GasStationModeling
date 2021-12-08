using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GasStationModeling.core.models
{
    public class FuelDispenser
    {
        public ObjectId Id { get; set; }

        [BsonIgnore]
        public string Image { get; }

        public string Name { get; set; }

        public double SpeedRefueling { get; set; }
    }
}
