using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;

namespace TangY_Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;   
        }




        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _blogRepository.GetAll());
        }
     

        [HttpPost]
        public IActionResult CreateBlog([FromBody] Blog blog)
        {
            if (blog == null)
            {
                return BadRequest();
            }

            _blogRepository.Create(blog);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] Blog blog)
        {
            if (blog == null || id != blog.Id)
            {
                return BadRequest();
            }

            var updatedBlog = await _blogRepository.Update(blog);

            if (updatedBlog == null)
            {
                return NotFound();
            }

            return Ok(updatedBlog);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var result = await _blogRepository.Delete(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
