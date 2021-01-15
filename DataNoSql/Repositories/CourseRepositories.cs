using DataNoSql.Context;
using DataNoSql.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataNoSql.Repositories
{
    public interface ICourseRepositories
    {
        Task<List<Course>> GetAllAsync();
        Task<Course> GetByIdAsync(string id);
        Task<Course> CreateAsync(Course Course);
        Task UpdateAsync(string id, Course Course);
        Task DeleteAsync(string id);
    }
    public class CourseRepositories : ICourseRepositories
    {
        private readonly MongoContext _db;
        public CourseRepositories(MongoContext db)
        {
            _db = db;
        }
        public async Task<List<Course>> GetAllAsync()
        {
            return await _db.Courses.Find(s => true).ToListAsync();
        }
        public async Task<Course> GetByIdAsync(string id)
        {
            return await _db.Courses.Find<Course>(s => s.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Course> CreateAsync(Course Course)
        {
            await _db.Courses.InsertOneAsync(Course);
            return Course;
        }
        public async Task UpdateAsync(string id, Course Course)
        {
            await _db.Courses.ReplaceOneAsync(s => s.Id == id, Course);
        }
        public async Task DeleteAsync(string id)
        {
            await _db.Courses.DeleteOneAsync(s => s.Id == id);
        }
    }
}
