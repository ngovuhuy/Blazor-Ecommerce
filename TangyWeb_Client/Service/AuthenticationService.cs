using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Tangy_Common;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        private readonly ILocalStorageService _localStorage;

        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvide)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvide;
        }

   

        public async Task<ChangePasswordResponseDTO> ChangePasswordAsync(ChangePasswordDTO changePasswordRequest, string? userId)
        {
            try
            {
                var apiPath = $"api/account/changepassword?userId={userId}";

                var content = JsonConvert.SerializeObject(changePasswordRequest);
                var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

                var uri = new Uri(apiPath, UriKind.RelativeOrAbsolute);
                var response = await _httpClient.PostAsync(uri, bodyContent);

                if (response.IsSuccessStatusCode)
                {
                    var contentTemp = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ChangePasswordResponseDTO>(contentTemp);

                    return new ChangePasswordResponseDTO()
                    {
                        IsPasswordChangedSuccessfully = true
                    };
                }
                else
                {
                    var contentTemp = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ChangePasswordResponseDTO>(contentTemp);

                    return new ChangePasswordResponseDTO()
                    {
                        IsPasswordChangedSuccessfully = false,
                        Errors = result.Errors
                    };
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new ChangePasswordResponseDTO()
                {
                    IsPasswordChangedSuccessfully = false,
                    Errors = new List<string> { "An error occurred while processing the request." }
                };
            }
        }

        public async Task<SignInResponseDTO> Login(SignInRequestDTO signInRequest)
        {
           var content = JsonConvert.SerializeObject(signInRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Account/SignIn", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SignInResponseDTO>(contentTemp);


            if(response.IsSuccessStatusCode)
            {
                await _localStorage.SetItemAsync(SD.Local_Token, result.ToKen);
                await _localStorage.SetItemAsync(SD.Local_UserDetails, result.UserDTO);

                var userId = result.UserDTO.Id;
                await _localStorage.SetItemAsync(SD.Local_UserId, userId);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.ToKen);
                return new SignInResponseDTO()
                {
                    IsAuthSuccessful = true
                };
            }
            else
            {
                return result;
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(SD.Local_Token);
            await _localStorage.RemoveItemAsync(SD.Local_UserDetails);
            _httpClient.DefaultRequestHeaders.Authorization = null;

        }

        public async Task<SignUpResponseDTO> RegisterUser(SignUpRequestDTO signUpRequest)
        {
            var content = JsonConvert.SerializeObject(signUpRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/account/signup", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SignUpResponseDTO>(contentTemp);

            if (response.IsSuccessStatusCode)
            {
                return new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful = true
                };
            }
            else
            {
                return new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful = false, Errors = result.Errors
                };
            }
        }
    }
}
