using Data.EF;
using Data.Models;
using System.Collections.Generic;
using System.Linq;
using Data.Utilities;
using Data.Generic;
using Microsoft.Extensions.Configuration;

namespace Data.Repositories
{
    public interface IProductRepository : IGeneric<Product>
    {
        IEnumerable<Product> GetAll();
        IEnumerable<object> GetAllSelect();
    }
    public class ProductRepository : BaseGeneric<Product>, IProductRepository
    {
        public ProductRepository(DataContext db) : base(db) { }

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
            }, includes: p => p.Category).ToPageList(pageSize: 2, page: 0);
        }
    }
}
