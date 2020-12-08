using APISample.Models;
using APISample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using APISample.Utilities;

namespace APISample.Services
{
    public class CategoryService : BaseGenericRepository<Category>, ICategoryService
    {
        public CategoryService(DataContext db) : base(db) { }

        public IEnumerable<Category> GetAll() => Query(includes: c => c.Products).ToList();
        public IEnumerable<object> GetAllSelect()
        {
            return Query<object>(select: a => new
            {
                ID = a.ID,
                Name = a.Name,
                Products = a.Products.Select(b => b.ID).ToList()
            }, includes: c => c.Products).ToPageList();
        }
    }
}
