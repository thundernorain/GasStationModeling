using MongoDB.Bson;

namespace GasStationModeling.core.models
{
    public class Fuel : IGasStationElement
    {
        public ObjectId Id { get; set; }

        public double Price { get; set; }

        public string Name { get; set; }
    }
}
