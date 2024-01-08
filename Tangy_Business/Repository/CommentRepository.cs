using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{


    public class CommentRepository : ICommentRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public CommentRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByProductAsync(int productId)
        {
            var comments = await _db.Comments
                .Where(comment => comment.ProductId == productId)
                .Include(comment => comment.User)
                .ToListAsync();
            return comments;
        }


        public async Task AddCommentAsync(Comment comment)
        {
            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _db.Comments.Update(comment);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _db.Comments.FindAsync(commentId);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            var comment = await _db.Comments
        .Include(c => c.User)    
        .Include(c => c.Product)  
        .FirstOrDefaultAsync(c => c.Id == commentId);

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            var comments = await _db.Comments
             .Include(c => c.User)    
                  .Include(c => c.Product)  
                .ToListAsync();
            return comments;
        }

       
    }
}
