using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.core.models
{
    public class Tank
    {
        public ObjectId Id { get; set; }

        private static readonly string image = "";
        public string Image
        {
            get { return image; }
        }
        public string TypeFuel { get; set; }
        public int CurrentVolume { get; set; }
        public int MaxVolume { get; set; }
        public int LimitVolume { get; set; }
    }
}
