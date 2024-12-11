using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/AssignRole",
            });
        }

        public async Task<ResponseDto?> CheckEmailExistAsync(string email)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = email,
                Url = SD.AuthAPIBase + "/api/auth/CheckEmailExist",
            }, withBearer: false);
        }

        public async Task<ResponseDto?> GetAllAdminUser()
        {
            ResponseDto? response = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/auth/role/ADMIN",
            });
            return response;
        }

        public async Task<ResponseDto?> GetAllCustomerUser()
        {
            ResponseDto? response = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/auth/role/CUSTOMER",
            });
            return response;
        }

        public async Task<ResponseDto?> GetUserById(string id)
        {
            ResponseDto? response = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/auth/"+ id,
            });
            return response;
        }
        public async Task<ResponseDto?> GetUserByEmail(string email)
        {
            ResponseDto? response = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/auth/GetUserByEmail/" + email,
            });
            return response;
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/login",
            }, withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/register",
            }, withBearer: false);
        } 
        public async Task<ResponseDto?> UpdateProfileAsync(UpdateProfileRequestDto updateProfileRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = updateProfileRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/updateProfile",
            });
        }

        public async Task<ResponseDto?> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = resetPasswordDto,
                Url = SD.AuthAPIBase + "/api/auth/ResetPassword",
            });
        }

    }
}
