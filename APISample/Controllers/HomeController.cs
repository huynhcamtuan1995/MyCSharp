using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APISample.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;

        public HomeController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public IEnumerable<Category> GetCategories() => _categoryRepository.GetAll();
        public IEnumerable<object> GetSelectCategories() => _categoryRepository.GetAllSelect();
        public IEnumerable<Product> GetProducts() => _productRepository.GetAll();
        public IEnumerable<object> GetSelectProducts() => _productRepository.GetAllSelect();
    }
}
