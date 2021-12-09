using MongoDB.Driver;
using System.Configuration;

namespace GasStationModeling.core.DB
{
    static class DbInitializer
    {
        static IMongoDatabase DB { get; set; }

        private const string DB_NAME = "GasStation";

        private static IMongoDatabase InitializeClient()
        {
            string con = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var client =  new MongoClient(con);
            var dbName = client.ListDatabaseNames().ToList()[0];
            DB = client.GetDatabase(dbName);
            //DB = client.GetDatabase(DB_NAME);
            return DB;
        }

        public static IMongoDatabase getInstance()
        {
            if (DB == null)
                DB = InitializeClient();
            return DB;
        }
    }
}
