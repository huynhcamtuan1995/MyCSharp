using APISample.Models;
using APISample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APISample.Services
{
    public interface ICategoryService : IGenericRepository<Category>
    {
        IEnumerable<Category> GetAll();
        IEnumerable<object> GetAllSelect();
    }
}
