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
        private DataContext _db;
        public HomeController(ICategoryRepository categoryRepository, IProductRepository productRepository, DataContext db)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _db = db;
        }

        [HttpGet]
        public ActionResult RunRawSqlQuery(string query = "select ID,Name from Categories", params object[] parameters)
        {
            var result =_db.Categories.FromSqlRaw(query, parameters).ToList();
            return Json(result);
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
