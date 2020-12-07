using APISample.Models;
using APISample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APISample.Services
{
    public interface IProductService : IGenericRepository<Product>
    {
        IEnumerable<Product> GetAll();
        IEnumerable<object> GetAllSelect();
    }
}
