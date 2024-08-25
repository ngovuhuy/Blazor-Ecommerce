using System.Net.Http.Json;
using Tangy_DataAccess;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service
{
    public class BlogService : IBlogService
    {
        private readonly HttpClient httpClient;

        public BlogService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<Blog>> GetBlogsAsync()
        {
            var apiGatewayUrl = httpClient.BaseAddress.ToString(); // Lấy địa chỉ cấu hình từ httpClient
            var apiUrl = $"{apiGatewayUrl}api/Blog"; // Sử dụng địa chỉ cấu hình

            var response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var blogs = await response.Content.ReadFromJsonAsync<IEnumerable<Blog>>();
            return blogs;
        }
    }
}
