using System.Net;
using System.Net.Http.Json;
using Tangy_DataAccess;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service
{
    public class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private string BaseServerUrl;
        public CommentService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
        }

        public async Task<Comment> CreateComment(Comment comment)
        {
            var response = await _httpClient.PostAsJsonAsync<Comment>("api/comment", comment);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Comment>();
        }
        public async Task<IEnumerable<Comment>> GetCommentsByProduct(int productId)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<Comment[]>($"api/comment/product/{productId}");
				return response ?? Enumerable.Empty<Comment>();
			}
			catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
			{
				// Xử lý trường hợp không tìm thấy (lỗi 404) bằng cách trả về danh sách trống
				return Enumerable.Empty<Comment>();
			}
		}

	}
}
