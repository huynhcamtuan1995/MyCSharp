using DataSql.EF;
using DataSql.Generic;
using DataSql.Models;
using DataSql.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSql.Repositories
{
    public interface IProductRepository : IGeneric<Product>
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<object>> GetAllSelectAsync();
    }
    public class ProductRepository : BaseGeneric<Product>, IProductRepository
    {
        public ProductRepository(DataContext db) : base(db) { }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await Task.Run(() => Query(includes: p => p.Category).ToList());

        public async Task<IEnumerable<object>> GetAllSelectAsync()
        {
            return await Task.Run(() => Query<object>(select: a => new
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
            }, includes: p => p.Category).ToPageList(pageSize: 2, page: 0));
        }
    }
}
