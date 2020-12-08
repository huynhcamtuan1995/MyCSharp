using APISample.Models;
using APISample.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISample.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private ICategoryService _categoryService;
        private IProductService _productService;

        public HomeController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public IEnumerable<Category> GetCategories() => _categoryService.GetAll();
        public IEnumerable<object> GetSelectCategories() => _categoryService.GetAllSelect();
        public IEnumerable<Product> GetProducts() => _productService.GetAll();
        public IEnumerable<object> GetSelectProducts() => _productService.GetAllSelect();
    }
}
