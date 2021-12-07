using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.core.models
{
    public class Fuel
    {
        public ObjectId Id { get; set; }

        private static readonly string image = "";
        public string Image
        {
            get { return image; }
        }
        public int Price { get; set; }

        public string Name  { get; set; }
    }
}
