﻿using GasStationModeling.core.DB.Interfaces;
using MongoDB.Bson;
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

        private static string CollectionName;

        public DbWorker(IMongoDatabase db,string collectionName)
        {
            collection = db.GetCollection<T>(collectionName);
        }

        public List<T> deleteEntry(int id)
        {
            var filter = Builders<T>.Filter.Eq("_Id", id);
            collection.DeleteOne(filter);
            return getCollection();
        }

        public List<T> getCollection()
        {
            var filter = new BsonDocument();
            return collection.Find(filter).ToList<T>();
        }

        public T getEntry(int id)
        {
            var filter = Builders<T>.Filter.Eq("_Id", id);
            return collection.Find(filter).First();
        }

        public List<T> insertEntry(T entry)
        {
            collection.InsertOne(entry);
            return getCollection();
        }
    }
}
