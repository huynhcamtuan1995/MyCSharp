using APISample.EF;
using APISample.Models;
using APISample.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APISample.Repositories
{
    public class ProductService : BaseGeneric<Product>, IProductService
    {
        public ProductService(DataContext db) : base(db) { }

        public IEnumerable<Product> GetAll() => Query(includes: p => p.Category).ToList();

        public IEnumerable<object> GetAllSelect()
        {
            return Query<object>(select: a => new
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
