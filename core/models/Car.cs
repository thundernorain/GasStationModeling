using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.core.models
{
    public class Car
    {
        public ObjectId Id { get; set; }

        private static readonly string image = "";
        public string Image
        {
            get { return image; }
        }

        public string Model { get; set; }
        public string TypeFuel { get; set; }
        public int CurrentFuelSupply { get; set; }
        public int MaxVolumeTank { get; set; }

    }
}
