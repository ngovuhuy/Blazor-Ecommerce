using Tangy_Models;

namespace TangyWeb_Client.Service.IService
{
    public interface IAuthenticationService
    {
      Task<SignUpResponseDTO>  RegisterUser(SignUpRequestDTO signUpRequestDTO);
      Task <SignInResponseDTO>  Login(SignInRequestDTO signInRequestDTO);
       Task Logout();

        Task<ChangePasswordResponseDTO> ChangePasswordAsync(ChangePasswordDTO changePasswordRequest, string? userId);
        Task<UpdateProfileResponseDTO> UpdateProfile(string userId, EditProfileDTO userProfile);
        Task<EditProfileDTO> GetUserProfile(string userId);
    }
}
