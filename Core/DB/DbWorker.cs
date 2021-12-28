using GasStationModeling.core.DB.dto;
using GasStationModeling.core.DB.Interfaces;
using GasStationModeling.exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.DB
{
    class DbWorker<T>: IDbWorker<T>
    {
        private static IMongoCollection<T> collection;

        public DbWorker(IMongoDatabase db,string collectionName)
        {
            collection = db.GetCollection<T>(collectionName);
        }

        public List<T> deleteEntry(int id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
            return getCollection();
        }

        public List<T> deleteEntry(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
            return getCollection();
        }

        public List<T> getCollection()
        {
            var filter = new BsonDocument();
            var entries = collection.Find(filter);
            if (entries.CountDocuments() == 0) return new List<T>();
            else return entries.ToList<T>();
        }

        public T getEntry(int id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            var entries = collection.Find(filter);
            if (entries.CountDocuments() == 0)
                throw new EntryNotFoundException(EntryNotFoundErrorMessage.NOT_FOUND);
            else return entries.First();
        }

        public List<T> insertEntry(T entry)
        {
            collection.InsertOne(entry);
            return getCollection();
        }

        public List<T> updateEntry(ObjectId id,UpdateDefinition<T> definition)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            var result = collection.UpdateOne(filter, definition);
            return getCollection();
        }
    }

    class DBWorkerKeys
    {
        public const string FUEL_TYPES_KEY = "fuelTypes";
        public const string FUEL_TANKS_KEY = "fuelTanks";
        public const string FUEL_DISPENSERS_KEY = "fuelDispensers";
        public const string CARS_KEY = "cars";
        public const string TOPOLOGIES_KEY = "topologies";
    }
}
