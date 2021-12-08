using MongoDB.Driver;
using System.Configuration;

namespace GasStationModeling.core.DB
{
    class DbInitializer
    {
        static IMongoDatabase DB { get; set; }

        private static IMongoDatabase InitializeClient(string name)
        {
            string con = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var client =  new MongoClient(con);
            DB = client.GetDatabase(name);
            return DB;
        }

        public static IMongoDatabase getInstance(string name)
        {
            if (DB == null)
                DB = InitializeClient(name);
            return DB;
        }
    }
}
