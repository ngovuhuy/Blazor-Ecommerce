using Tangy_Models;

namespace TangyWeb_Client.Service.IService
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDTO>> GetAll();
        public Task<IEnumerable<ProductDTO>> Search(string searchQuery);
        public Task<ProductDTO> Get(int productId);



    }
}
