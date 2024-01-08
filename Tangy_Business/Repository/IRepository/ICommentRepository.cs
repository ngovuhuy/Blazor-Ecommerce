using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_DataAccess;
using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{
    public interface ICommentRepository
    {
        public Task<IEnumerable<Comment>> GetAll();
        Task<IEnumerable<Comment>> GetCommentsByProductAsync(int productId);
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task AddCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId);
    }
}
