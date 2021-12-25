using MongoDB.Bson;

namespace GasStationModeling.core.models
{
    public class Car : IGasStationElement
    {
        public ObjectId Id { get; set; }

        public string Image { get; set; }

        public string Model { get; set; }

        public string TypeFuel { get; set; }

        public double CurrentFuelSupply { get; set; }

        public double MaxVolumeTank { get; set; }
    }
}
