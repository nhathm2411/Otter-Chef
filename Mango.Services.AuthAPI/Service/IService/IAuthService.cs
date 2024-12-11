using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDTO);
        Task<string> UpdateProfile(UpdateProfileRequestDto updateProfileRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO);
        Task<bool> AssignRole(string email, string roleName);
        Task<string> CheckEmailExist(string email);
        Task<string> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<string> GeneratePasswordResetToken(string email);

        //Task<bool> ResetPassword(RegistrationRequestDto registrationRequestDTO);
    }
}
