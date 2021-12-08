using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.core.DB.Interfaces
{
    interface IDbWorker<T>
    {
        List<T> getCollection();

        T getEntry(int Id);

        List<T> deleteEntry(int Id);

        List<T> insertEntry(T entry);
    }
}
