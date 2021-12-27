

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GasStationModeling.core.models
{
    [BsonDiscriminator("Cashbox")]
    class Cashbox : IGasStationElement
    {
        public int LimitCash { get; set; }

        public int CurrentCash { get; set; }
    }
}
