using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Macs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_Models;
using TangyWeb_API.Helper;

namespace TangyWeb_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly APISettings _apiSettings;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, IOptions<APISettings> options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _apiSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO signUpRequestDTO)
        {
            if(signUpRequestDTO == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new ApplicationUser
            {
                UserName = signUpRequestDTO.Email,
                Email = signUpRequestDTO.Email,
                name = signUpRequestDTO.Name,
                PhoneNumber = signUpRequestDTO.PhoneNumber,
                EmailConfirmed = true

            };
            var result = await _userManager.CreateAsync(user,signUpRequestDTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful = false,
                    Errors = result.Errors.Select(u => u.Description)
                });
            }
            var roleResult = await _userManager.AddToRoleAsync(user, SD.Role_Customer);
            if (!roleResult.Succeeded)
            {
                return BadRequest(new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful = false,
                    Errors = result.Errors.Select(u => u.Description)
                });
            }

            return StatusCode(201);
        }


        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO signInRequestDTO)
        {
            if (signInRequestDTO == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(signInRequestDTO.UserName, signInRequestDTO.Password,false,false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(signInRequestDTO.UserName);
                if(user == null)
                {
                    return Unauthorized(new SignInResponseDTO
                    {
                        IsAuthSuccessful = false,
                        ErrorMessage = "Tài khoản hoặc mật khẩu sai vui lòng nhập lại !!!"
                    });
                }
                // everything is valid and we need to login
                var signinCredentials = GetSigningCredentials();
                var claims = await GetClaims(user);
                var tokenOptions = new JwtSecurityToken(
                    issuer: _apiSettings.ValidIssuer,
                    audience: _apiSettings.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: signinCredentials);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(new SignInResponseDTO()
                {
                    IsAuthSuccessful = true,
                    ToKen = token,
                    UserDTO = new UserDTO()
                    {
                        Name = user.name,
                        Id = user.Id,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    }
                });
            }
            else
            {
                return Unauthorized(new SignInResponseDTO
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "InValid Authenication"
                });
            }
          
            return StatusCode(201);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecretKey));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        } 

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id),
            };

            var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(string userId, [FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { ErrorMessage = "Người dùng không tồn tại" });
            }

            var result = await _userManager.CheckPasswordAsync(user, changePasswordDTO.CurrentPassword);

            if (!result)
            {
                return BadRequest(new { ErrorMessage = "Mật khẩu hiện tại không đúng" });
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                // Trả về danh sách lỗi để hiển thị chi tiết lỗi trên giao diện người dùng
                var errors = changePasswordResult.Errors.Select(error => error.Description).ToList();
                return BadRequest(new { Errors = errors });
            }

            return Ok(new { SuccessMessage = "Đổi mật khẩu thành công" });

        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(string userId, [FromBody] EditProfileDTO editProfileDTO)
        {
            // Tìm người dùng trong cơ sở dữ liệu
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { ErrorMessage = "Người dùng không tồn tại" });
            }

            user.Email = editProfileDTO.Gmail;
            user.name = editProfileDTO.Name;
            user.PhoneNumber = editProfileDTO.PhoneNumber;

 
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
               
                var errors = result.Errors.Select(error => error.Description).ToList();
                return BadRequest(new { Errors = errors });
            }

            return Ok(new { SuccessMessage = "Cập nhật hồ sơ thành công" });
        }


        [HttpGet]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            try
            {
                // Tìm người dùng trong cơ sở dữ liệu
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { ErrorMessage = "Người dùng không tồn tại" });
                }

                // Trả về thông tin hồ sơ của người dùng
                var userProfile = new EditProfileDTO
                {
                    Gmail = user.Email,
                    Name = user.name,
                    PhoneNumber = user.PhoneNumber,
                };

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"Lỗi khi lấy thông tin người dùng: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
