using BaseDataFactory.Generic;
using DataSql.EF;
using DataSql.Models;
using Helper.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSql.Repositories
{
    public interface ICategoryRepository : IGeneric<Category>
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<object>> GetAllSelectAsync();
    }
    public class CategoryRepository : BaseGeneric<Category>, ICategoryRepository
    {
        public CategoryRepository(MyDataContext db) : base(db) { }

        public async Task<IEnumerable<Category>> GetAllAsync() => 
            await Task.Run(() => Query(includes: c => c.Products).ToList());
        public async Task<IEnumerable<object>> GetAllSelectAsync()
        {
            return await Task.Run(() => Query<object>(select: a => new
            {
                ID = a.ID,
                Name = a.Name,
                Products = a.Products.Select(b => b.ID).ToList()
            }).ToPageList());
        }
    }
}
