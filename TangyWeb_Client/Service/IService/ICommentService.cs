using Tangy_DataAccess;
using Tangy_Models;

namespace TangyWeb_Client.Service.IService
{
    public interface ICommentService
    {
        public Task<IEnumerable<Comment>> GetCommentsByProduct(int productId);
        public Task<Comment> CreateComment(Comment comment);

	}
}
