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

        public IEnumerable<Product> GetAll() => Query(includes: p => p.Category).ToList();

        public IEnumerable<object> GetAllSelect()
        {
            return QuerySelect<object>(select: a => new
            {
                ID = a.ID,
                Name = a.Name,
                Quantity = a.Quantity,
                Category = new
                {
                    ID = a.Category.ID,
                    Name = a.Category.Name,
                    ProductIds = a.Category.Products.Select(b => b.ID).ToList()
                }
            }, includes: p => p.Category).ToList();
        }
    }
}
