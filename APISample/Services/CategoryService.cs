using APISample.Models;
using APISample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APISample.Services
{
    public class CategoryService : BaseGenericRepository<Category>, ICategoryService
    {
        public CategoryService(DataContext db) : base(db) {}

        public IEnumerable<Category> GetAll(params Expression<Func<Category, object>>[] includes) => Query(includes: includes).ToList();
    }
}
