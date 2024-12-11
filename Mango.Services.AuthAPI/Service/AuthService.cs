using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Mango_Services.AuthAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, IJwtTokenGenerator jwtTokenGenerator,
            UserManager<ApplicationUser> userManager,
             RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers
                .FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<string> CheckEmailExist(string email)
        {
            var user = await _db.ApplicationUsers
                .FirstAsync(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                return token;
            }
            return null;
        }


        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || !isValid)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            // if user was found, Generate JWT Token

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user,roles);

            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Birthday = user.Birthday,
                IsActive = user.IsActive
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                FirstName = registrationRequestDto.FirstName,
                LastName = registrationRequestDto.LastName,
                PhoneNumber = registrationRequestDto.PhoneNumber,
                Address = registrationRequestDto.Address,
                Birthday = registrationRequestDto.Birthday,
                Gender = registrationRequestDto.Gender,
                IsActive = registrationRequestDto.IsActive,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        FirstName = userToReturn.FirstName,
                        LastName = userToReturn.LastName,
                        PhoneNumber = userToReturn.PhoneNumber,
                        Address = userToReturn.Address,
                        Birthday= userToReturn.Birthday,
                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {

            }
            return "Error Encountered";
        }

        public async Task<string> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.UserName);
            if (user == null)
            {
                return "User not found.";
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            if (result.Succeeded)
            {
                return "";
            }
            else
            {
                return result.Errors.FirstOrDefault()?.Description ?? "Error resetting password.";
            }
        }
        //Reset and return new token
        public async Task<string> GeneratePasswordResetToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<string> UpdateProfile(UpdateProfileRequestDto updateProfileRequestDto)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(updateProfileRequestDto.Email);
            if (user == null)
            {
                return "User not found.";
            }

            // Update user properties
            user.FirstName = updateProfileRequestDto.FirstName;
            user.LastName = updateProfileRequestDto.LastName;
            user.PhoneNumber = updateProfileRequestDto.PhoneNumber;
            user.Address = updateProfileRequestDto.Address;
            user.Birthday = updateProfileRequestDto.Birthday;
            user.Gender = updateProfileRequestDto.Gender;
            user.IsActive = updateProfileRequestDto.IsActive;

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return "";
            }
            else
            {
                return result.Errors.FirstOrDefault()?.Description ?? "Error updating profile.";
            }
        }


    }
}
