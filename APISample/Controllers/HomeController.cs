using APISample.Models;
using APISample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISample.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private ICategoryService _categoryService;
        private IProductService _productService;

        public HomeController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [Route("[action]")]
        public IEnumerable<Category> GetCategories() => _categoryService.GetAll(x=>x.Products);

        [Route("[action]")]
        public IEnumerable<object> GetProducts() => _productService.GetAll(x=>x.Category).Select(x => new { 
            x.ID, x.Name, x.Quantity,
            Category = x.Category.Products.Select(y=>y.ID).ToList()
        }).ToList();
    }
}
