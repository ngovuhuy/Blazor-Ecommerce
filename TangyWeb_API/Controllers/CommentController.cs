using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace TangyWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
     
        private readonly ApplicationDbContext _context;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        public CommentController(ApplicationDbContext context, ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _context = context;
            _mapper = mapper;
        }
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByProduct(int productId)
        {
            var comments = await _context.Comments
                .Where(comment => comment.ProductId == productId)
                .ToListAsync();

            if (comments == null || !comments.Any())
            {
                return NotFound("No comments found for the specified product.");
            }

            var commentDTOs = comments.Select(comment => CommentToDTO(comment)).ToList();
            return commentDTOs;
        }

        private static Comment CommentToDTO(Comment comment) =>
      new Comment
      {
          Id = comment.Id,
          Text = comment.Text,
          CreatedAt = comment.CreatedAt,
          UserId = comment.UserId,
          ProductId = comment.ProductId,
          Name = comment.Name,
          
      };

        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment(Comment commentDTO)
        {
            if (string.IsNullOrWhiteSpace(commentDTO.Text))
            {
                return BadRequest("Comment text cannot be empty or contain only whitespace.");
            }

            var comment = new Comment
            {
                Text = commentDTO.Text,
                CreatedAt = DateTime.Now,
                UserId = commentDTO.UserId,
                ProductId = commentDTO.ProductId,
                Name = commentDTO.Name,
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, CommentToDTO(comment));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return CommentToDTO(comment);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            var comments = await _context.Comments.ToListAsync();
            var commentDTOs = comments.Select(comment => CommentToDTO(comment)).ToList();
            return commentDTOs;
        }
    }
}

