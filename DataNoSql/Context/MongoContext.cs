using DataNoSql.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DataNoSql.Context
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("ConnectionStrings").GetSection("MongoDB")["Connection"]);
            if (client != null)
                _database = client.GetDatabase(configuration.GetSection("ConnectionStrings").GetSection("MongoDB")["Database"]);
        }

        public IMongoCollection<Student> Students
        {
            get
            {
                return _database.GetCollection<Student>("Students");
            }
        }

        public IMongoCollection<Course> Courses
        {
            get
            {
                return _database.GetCollection<Course>("Students");
            }
        }
    }
}
