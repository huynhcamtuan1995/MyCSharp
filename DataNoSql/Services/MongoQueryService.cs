using DataNoSql.Repositories;

namespace DataNoSql.Services
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
