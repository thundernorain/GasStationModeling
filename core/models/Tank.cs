using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GasStationModeling.core.models
{
    [BsonDiscriminator("Tank")]
    public class Tank : IGasStationElement
    {
        public ObjectId Id { get; set; }

        [BsonIgnore]
        public string Image { get; }

        [BsonIgnore]
        public Fuel TypeFuel { get; set; }

        [BsonIgnore]
        public double CurrentVolume { get; set; }

        [BsonIgnore]
        public double LimitVolume {
            get
            {
                return MaxVolume * 0.95;
            }
        }

        public double MaxVolume { get; set; }

        public string Name { get; set; }

    }
}
