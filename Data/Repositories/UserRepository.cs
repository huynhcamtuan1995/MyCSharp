using DataSql.EF;
using DataSql.Generic;
using DataSql.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataSql.Repositories
{
    public interface IUserRepository : IGeneric<User>
    {
        Task<IEnumerable<User>> GetAllAsync();
    }
    public class UserRepository : BaseGeneric<User>, IUserRepository
    {
        public UserRepository(DataContext db) : base(db) { }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await Task.Run(() => Query());
        }
    }
}
