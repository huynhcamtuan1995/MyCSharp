using Data.Models;
using System.Collections.Generic;

namespace Data.Interfaces
{
    public interface IProductRepository : IGeneric<Product>
    {
        IEnumerable<Product> GetAll();
        IEnumerable<object> GetAllSelect();
    }
}
