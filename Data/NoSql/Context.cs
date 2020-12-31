using Data.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Context
    {
        private readonly IMongoDatabase _database = null;

        public Context(IConfiguration configuration)
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
