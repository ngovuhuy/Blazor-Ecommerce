using Newtonsoft.Json;
using System.Text;
using Tangy_DataAccess;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service
{
    public class OrderService : IOrderSerivce
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private string BaseServerUrl;
        public OrderService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
        }

        public async Task<OrderDTO> Create(StripePaymentDTO paymentDTO)
        {
          var content = JsonConvert.SerializeObject(paymentDTO);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Order/Create", bodyContent);

            string responseResult = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<OrderDTO>(responseResult);
                return result;
            }
            return new OrderDTO();
        }

        public async Task<OrderDTO> Get(int orderHeaderId)
        {
            var response = await _httpClient.GetAsync($"/api/Order/{orderHeaderId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var order = JsonConvert.DeserializeObject<OrderDTO>(content);
                return order; 
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }

        }

        //public async Task<List<OrderHeader>> GetOrderByUser(string? userId)
        //{
        //    var response = await _httpClient.GetAsync($"/api/Order/GetOrderbyUser/{userId}");
        //    var content = await response.Content.ReadAsStringAsync();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var order = JsonConvert.DeserializeObject<List<OrderHeader>>(content);
        //        return order;
        //    }
        //    else
        //    {
        //        var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
        //        throw new Exception(errorModel.ErrorMessage);
        //    }

        //}

        public async Task<List<OrderDTO>> GetUserOrder(string userId)
        {
            var response = await _httpClient.GetAsync($"/api/Order/GetUserOrders?userId={userId}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var order = JsonConvert.DeserializeObject<List<OrderDTO>>(content);
                return order;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                Console.WriteLine($"HTTP Status Code: {response.StatusCode}");
                Console.WriteLine($"Error Content: {content}");
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetAll(string? userId = null)
        {
            var response = await _httpClient.GetAsync("/api/Order");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(content);
             
                return orders;
            }
            return new List<OrderDTO>(); 
        }

        public async Task<OrderHeaderDTO> MarkPaymentSuccessful(OrderHeaderDTO orderHeader)
		{
			var content = JsonConvert.SerializeObject(orderHeader);
			var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync("api/Order/PaymentSuccessful", bodyContent);
			string responseResult = response.Content.ReadAsStringAsync().Result;
			if (response.IsSuccessStatusCode)
			{
				var result = JsonConvert.DeserializeObject<OrderHeaderDTO>(responseResult);
				return result;
			}
			var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(responseResult);
			throw new Exception(errorModel.ErrorMessage);
		}

        public async Task<OrderHeader> GetAllOrder()
        {
            var response = await _httpClient.GetAsync("/api/Order");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<OrderHeader>(content);

                return orders;
            }
            return new OrderHeader();
        }

    }
}
