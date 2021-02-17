using DataSql.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<ActionResult> RunRawSqlQuery(string query = "select ID, Name from Categories", params object[] parameters)
        {
            return await Task.Run(() =>
                Json(_categoryRepository.GetWithRawSql<object>(query)));
        }

        [HttpGet]
        public async Task<ActionResult> GetCategories() =>
            Json(await _categoryRepository.GetAllAsync());
        [HttpGet]
        public async Task<ActionResult> GetSelectCategories() =>
            Json(await _categoryRepository.GetAllSelectAsync());
        [HttpGet]
        public async Task<ActionResult> GetProducts() =>
            Json(await _productRepository.GetAllAsync());
        [HttpGet]
        public async Task<ActionResult> GetSelectProducts() =>
            Json(await _productRepository.GetAllSelectAsync());


    }
}
