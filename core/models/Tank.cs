using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GasStationModeling.core.models
{
    public class Tank
    {
        public ObjectId Id { get; set; }

        [BsonIgnore]
        public string Image { get; }

        [BsonIgnore]
        public string TypeFuel { get; set; }

        [BsonIgnore]
        public double CurrentVolume { get; set; }

        public double MaxVolume { get; set; }

        public string Name { get; set; }

        [BsonIgnore]
        public double LimitVolume { get; set; }
    }
}
