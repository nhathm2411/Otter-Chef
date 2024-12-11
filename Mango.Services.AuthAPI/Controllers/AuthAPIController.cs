using Mango.MessageBus;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Mango_Services.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        protected ResponseDto _response;

        public AuthAPIController(IAuthService authService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _authService = authService;
            _response = new();
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = $"User with ID {id} does not exist.";
                return NotFound(_response);
            }

            _response.IsSuccess = true;
            _response.Result = user;
            return Ok(_response);
        }



        [HttpGet("role/{roleName}")]
        public async Task<ActionResult<ResponseDto?>> GetUsersByRole(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return NotFound($"Role {roleName} does not exist.");
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            _response.Result = usersInRole;

            int userCount = usersInRole.Count;
            return Ok(_response);
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }

            return Ok(_response);
        }
        [HttpPost("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequestDto model)
        {
            var errorMessage = await _authService.UpdateProfile(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            _response.Message = "Update Profile Successfully!";
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }

            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";
                return BadRequest(_response);
            }

            return Ok(_response);
        }

        [HttpPost("CheckEmailExist")]
        public async Task<IActionResult> CheckEmailExist([FromBody] string email)
        {
            var token = await _authService.CheckEmailExist(email); //true
            if (token == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Email does not exist";
                return BadRequest(_response);
            }
            _response.Result = token;
            return Ok(_response);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {

            var errorMessage = await _authService.ResetPassword(resetPasswordDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            _response.Message = "Reset password successfully!";
            return Ok(_response);
        }

        [HttpGet("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = $"User with email: {email} was not found!";
                return BadRequest(_response);
            }
            _response.Result = user;
            _response.Message = "Get user by email successfully!";
            return Ok(_response);
        }
    }
}
