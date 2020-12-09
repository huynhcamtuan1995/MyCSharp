using Data.Models;
using System.Collections.Generic;

namespace Data.Interfaces
{
    public interface ICategoryRepository : IGeneric<Category>
    {
        IEnumerable<Category> GetAll();
        IEnumerable<object> GetAllSelect();
    }
}
