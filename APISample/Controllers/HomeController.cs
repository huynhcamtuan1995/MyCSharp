using Data.EF;
using Data.Repositories;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

        [HttpGet]
        public ActionResult RunRawSqlQuery(string query = "select ID,Name from Categories", params object[] parameters)
        {
            return Json(_categoryRepository.GetWithRawSql<object>(query));
        }

        [HttpGet]
        public ActionResult GetCategories() => Json(_categoryRepository.GetAll());
        [HttpGet]
        public ActionResult GetSelectCategories() => Json(_categoryRepository.GetAllSelect());
        [HttpGet]
        public ActionResult GetProducts() => Json(_productRepository.GetAll());
        [HttpGet]
        public ActionResult GetSelectProducts() => Json(_productRepository.GetAllSelect());


    }
}
