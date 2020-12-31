using Data.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;

namespace Data.Services
{

    public class MongoQueryService
    {
        private ICourseRepositories _courseRepositories;
        private IStudentRepository _studentRepositories;
        public MongoQueryService(ICourseRepositories courseRepositories, IStudentRepository studentRepositories)
        {
            _studentRepositories = studentRepositories;
            _courseRepositories = courseRepositories;
        }
    }
}
