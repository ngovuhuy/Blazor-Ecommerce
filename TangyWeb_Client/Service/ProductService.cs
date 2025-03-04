﻿using Newtonsoft.Json;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private string BaseServerUrl;
        private string BaseUrl;

        public ProductService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient =  httpClient;
            _configuration = configuration;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
            BaseUrl = _configuration.GetValue<string>("GlobalConfiguration:BaseUrl");
        }
        public async Task<ProductDTO> Get(int productId)
        {
            var response = await _httpClient.GetAsync($"/api/Product/{productId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var product = JsonConvert.DeserializeObject<ProductDTO>(content);
                product.ImageUrl = BaseServerUrl + product.ImageUrl;
                return product;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }

        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            //var response = await _httpClient.GetAsync("/api/Product");
            var response = await _httpClient.GetAsync(BaseUrl + "/api/Product");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(content);
                foreach(var prod in products)
                {
                    prod.ImageUrl = BaseServerUrl + prod.ImageUrl;
                }
                return products;
            }
            return new List<ProductDTO>();  
        }

        public async Task<IEnumerable<ProductDTO>> Search(string searchQuery)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product/search?query={Uri.EscapeDataString(searchQuery)}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(content);

                    foreach (var prod in products)
                    {
                        prod.ImageUrl = BaseServerUrl + prod.ImageUrl;
                    }

                    return products;
                }
                else
                {
                    return new List<ProductDTO>();
                }
            }
            catch (Exception ex)
            {

                return new List<ProductDTO>();
            }
        }
    }
}
