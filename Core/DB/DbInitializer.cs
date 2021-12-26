using GasStationModeling.core.models;
using GasStationModeling.exceptions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Configuration;

namespace GasStationModeling.core.DB
{
    static class DbInitializer
    {
        static IMongoDatabase DB { get; set; }

        private const string DB_NAME = "GasStation";

        private static IMongoDatabase InitializeClient()
        {
            try
            {
                string con = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
                var client = new MongoClient(con);
                var dbName = client.ListDatabaseNames().ToList()[0];
                DB = client.GetDatabase(dbName);
                return DB;
            }
            catch (Exception)
            {
                throw new DbErrorException(DbErrorMessage.CONNECTION_ERROR);
            }
        }

        public static IMongoDatabase getInstance()
        {
            if (DB == null)
            {
                BsonClassMap.RegisterClassMap<Cashbox>();
                BsonClassMap.RegisterClassMap<FuelDispenser>();
                BsonClassMap.RegisterClassMap<Tank>();
                BsonClassMap.RegisterClassMap<Entrance>();
                BsonClassMap.RegisterClassMap<Exit>();

                DB = InitializeClient();
            }
            
            return DB;
        }
    }
}
