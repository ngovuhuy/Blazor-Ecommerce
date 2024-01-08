using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_Common;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace TangyWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;

        public ProductController(IProductRepository productRepository,ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context;

        }
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepository.GetAll());
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string query)
        {
            try
            {
                var products = await _productRepository.SearchProducts(query);
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về lỗi 500
                return StatusCode(500, "Internal Server Error");
            }

            //try
            //{
            //    var results = _context.Products
            //        .Where(p => p.Name.ToLower().Contains(query.ToLower()) || p.Description.ToLower().Contains(query.ToLower()))
            //        .Include(p => p.Category)
            //        .ToList();

            //    return Ok(results);
            //}
            //catch (Exception ex)
            //{

            //    return StatusCode(500, "Internal Server Error");
            //}
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(int? productId)
        {
            if (productId == null || productId == 0)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            var product = await _productRepository.Get(productId.Value);
            if(product == null)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(product);
        }
    }
}
