using Mango.Web.Models;
using Mango.Web.Models.Dto;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto); 
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto); 
        Task<ResponseDto?> UpdateProfileAsync(UpdateProfileRequestDto updateProfileRequestDto); 
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> CheckEmailExistAsync(string email);
        Task<ResponseDto?> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<ResponseDto?> GetAllCustomerUser();
        Task<ResponseDto?> GetAllAdminUser();
        Task<ResponseDto?> GetUserById(string id);
        Task<ResponseDto?> GetUserByEmail(string email);

    }
}
