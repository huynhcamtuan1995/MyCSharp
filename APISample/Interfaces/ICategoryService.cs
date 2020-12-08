using APISample.Models;
using APISample.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APISample.Repositories
{
    public interface ICategoryService : IGeneric<Category>
    {
        IEnumerable<Category> GetAll();
        IEnumerable<object> GetAllSelect();
    }
}
