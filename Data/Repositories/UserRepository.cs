using DataSql.EF;
using DataSql.Generic;
using DataSql.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataSql.Repositories
{
    public interface IUserRepository : IGeneric<User>
    {
        IEnumerable<User> GetAll();
    }
    public class UserRepository : BaseGeneric<User>, IUserRepository
    {
        public UserRepository(DataContext db) : base(db) { }

        public IEnumerable<User> GetAll()
        {
            return Query();
        }
    }
}
