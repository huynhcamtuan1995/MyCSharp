using APISample.Models;
using APISample.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APISample.Repositories
{
    public interface IProductService : IGeneric<Product>
    {
        IEnumerable<Product> GetAll();
        IEnumerable<object> GetAllSelect();
    }
}
