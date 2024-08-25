using Tangy_DataAccess;

namespace TangyWeb_Client.Service.IService
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetBlogsAsync();
    }
}
