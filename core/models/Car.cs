using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GasStationModeling.core.models
{
    public class Car
    {
        public ObjectId Id { get; set; }

        [BsonIgnore]
        public string Image { get; }

        public string Model { get; set; }

        public string TypeFuel { get; set; }

        public int CurrentFuelSupply { get; set; }

        public int MaxVolumeTank { get; set; }
    }
}
