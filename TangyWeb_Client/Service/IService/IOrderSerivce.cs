using Tangy_DataAccess;
using Tangy_Models;

namespace TangyWeb_Client.Service.IService
{
    public interface IOrderSerivce
    {
        public Task<IEnumerable<OrderDTO>> GetAll(string? userId);
        public Task<OrderHeader> GetAllOrder();

        
        public Task<OrderDTO> Get(int orderId);
        //public Task<List<OrderHeader>>GetOrderByUser(string userId);
        public Task<List<OrderDTO>> GetUserOrder(string? userId);
        public Task<OrderDTO> Create(StripePaymentDTO paymentDTO);


        public Task<OrderHeaderDTO> MarkPaymentSuccessful(OrderHeaderDTO orderHeader);
    }
}
