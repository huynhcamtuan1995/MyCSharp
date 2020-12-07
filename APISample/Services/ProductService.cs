using APISample.Models;
using APISample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APISample.Services
{
    public class ProductService : BaseGenericRepository<Product>, IProductService
    {
        public ProductService(DataContext db) : base(db) { }

        public IEnumerable<Product> GetAll(params Expression<Func<Product, object>>[] includes) => Query(includes: includes).ToList();
    }
}
