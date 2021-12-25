using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GasStationModeling.core.models
{
    public class Car : IGasStationElement
    {
        public ObjectId Id { get; set; }

        public string Image { get; set; }

        public string Model { get; set; }

        public string TypeFuel { get; set; }

        public int CurrentFuelSupply { get; set; }

        public int MaxVolumeTank { get; set; }
    }
}
