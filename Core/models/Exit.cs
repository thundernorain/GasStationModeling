﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.core.models
{
    [BsonDiscriminator("Exit")]
    class Exit : IGasStationElement { }
}
