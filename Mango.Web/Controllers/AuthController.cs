using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IEmailSender _emailSender;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider, IEmailSender emailSender)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
            _emailSender = emailSender;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(SD.RoleAdmin))
                {
                    return RedirectToAction("AdminIndex", "Admin");
                }
                else if (User.IsInRole(SD.RoleCustomer))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            return View(loginRequestDto);
        }
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(SD.RoleAdmin))
                {
                    return RedirectToAction("AdminIndex", "Admin");
                }
                else if (User.IsInRole(SD.RoleCustomer))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer}
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto responseDto = await _authService.LoginAsync(obj);
            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto =
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View(obj);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            if (obj.IsActive == null)
            {
                obj.IsActive = true;
            }
            ResponseDto responseDto = await _authService.CheckEmailExistAsync(obj.Email);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["error"] = "Email already existed!";
                return View();
            }
            Random random = new Random();

            var codeOTP = random.Next(100000, 999999).ToString();
            var receiver = obj.Email;
            var subject = "Otter Chef Registration Verification";
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "otterchef-txtlogo.png");

            var message = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            color: #333;
                            line-height: 1.5;
                        }}
                        .verification-box {{
                            text-align: center;
                            padding: 20px;
                            background-color: #f5f5f5;
                            border-radius: 8px;
                        }}
                        .verification-box img {{
                            width: 300px;
                            height: 90px;
                        }}
                        .otp-code {{
                            font-size: 30px;
                            font-weight: bold;
                            color: #a3745e;
                            margin-top: 20px;
                        }}
                        .resend-button {{
                            display: inline-block;
                            margin-top: 20px;
                            padding: 10px 20px;
                            background-color: #007bff;
                            color: white;
                            border: none;
                            border-radius: 5px;
                            text-decoration: none;
                            font-size: 16px;
                        }}
                        .resend-button:hover {{
                            background-color: #0056b3;
                        }}
                    </style>
                </head>
                <body>
                    <div class='verification-box'>
                        <h2>We warmly welcome you to Otter Chef</h2>
                        <p>A verification email has been sent to your email <strong>{receiver}</strong>.</p>
                        <p>Please check your email and verify the OTP code to complete your account registration.</p>
                        <p class='otp-code'>OTP: {codeOTP}</p>
                        <img src='cid:OtterChefLogo' alt='Otter Chef Logo'>
                    </div>
                </body>
                </html>";

            // Gửi email với hình ảnh nhúng
            await _emailSender.SendEmailAsync(receiver, subject, message, logoPath, codeOTP);


            TempData["Otp"] = codeOTP;
            TempData["RegistrationData"] = JsonConvert.SerializeObject(obj);
            TempData["success"] = "You're on the final stretch! After your next meal, enter your OTP to complete your registration!";
            return RedirectToAction(nameof(VerifyOtp));
        }

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(OtpVerificationDto otpDto)
        {
            string storedOtp = TempData["Otp"]?.ToString();
            var registrationDataJson = TempData["RegistrationData"]?.ToString();
            var registrationData = registrationDataJson != null ? JsonConvert.DeserializeObject<RegistrationRequestDto>(registrationDataJson) : null;

            if (storedOtp != otpDto.Otp)
            {
                TempData["Otp"] = storedOtp;
                TempData["RegistrationData"] = registrationDataJson;
                TempData["error"] = "Invalid OTP. Please try again.";
                return View();
            }

            ResponseDto result = await _authService.RegisterAsync(registrationData);
            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationData.Role))
                {
                    registrationData.Role = SD.RoleCustomer;
                }

                ResponseDto assignRole = await _authService.AssignRoleAsync(registrationData);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    var receiver = registrationData.Email;
                    var subject = "Welcome to Otter Chef";
                    var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "otter-welcome-email.png");

                    var message = $@"
            <html>
            <head>
                <style>
                    .verification-box {{
                        text-align: center; 
                    }}
                    .verification-box img {{
                        width: 940px;
                        height: 880px;
                        margin: auto;
                        display: block;
                    }}
                </style>
            </head>
            <body>
                <div class='verification-box'>
                   <img src='cid:OtterChefLogo' alt='Otter Chef Logo' />
                </div>
            </body>
            </html>";

                    await _emailSender.SendEmailAsync(receiver, subject, message, logoPath);
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = result?.Message ?? "Registration failed.";
            }

            return View(otpDto);
        }



        [HttpGet]
        public async Task<IActionResult> ResendOtp()
        {
            var registrationDataJson = TempData["RegistrationData"]?.ToString();
            var registrationData = registrationDataJson != null ? JsonConvert.DeserializeObject<RegistrationRequestDto>(registrationDataJson) : null;

            Random random = new Random();

            var codeOTP = random.Next(100000, 999999).ToString();
            var receiver = registrationData.Email;
            var subject = "Otter Chef Registration Verification";
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "otterchef-txtlogo.png");

            // Tạo nội dung email với mã OTP
            var message = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            color: #333;
                            line-height: 1.5;
                        }}
                        .verification-box {{
                            text-align: center;
                            padding: 20px;
                            background-color: #f5f5f5;
                            border-radius: 8px;
                        }}
                        .verification-box img {{
                            width: 300px;
                            height: 90px;
                        }}
                        .otp-code {{
                            font-size: 30px;
                            font-weight: bold;
                            color: #a3745e;
                            margin-top: 20px;
                        }}
                        .resend-button {{
                            display: inline-block;
                            margin-top: 20px;
                            padding: 10px 20px;
                            background-color: #007bff;
                            color: white;
                            border: none;
                            border-radius: 5px;
                            text-decoration: none;
                            font-size: 16px;
                        }}
                        .resend-button:hover {{
                            background-color: #0056b3;
                        }}
                    </style>
                </head>
                <body>
                    <div class='verification-box'>
                        <h2>We warmly welcome you to Otter Chef</h2>
                        <p>A verification email has been sent to your email <strong>{receiver}</strong>.</p>
                        <p>Please check your email and verify the OTP code to complete your account registration.</p>
                        <p class='otp-code'>OTP: {codeOTP}</p>
                        <img src='cid:OtterChefLogo' alt='Otter Chef Logo'>
                    </div>
                </body>
                </html>";

            await _emailSender.SendEmailAsync(receiver, subject, message, logoPath, codeOTP);

            TempData["Otp"] = codeOTP;
            TempData["RegistrationData"] = JsonConvert.SerializeObject(registrationData);

            return RedirectToAction(nameof(VerifyOtp));
        }

        [HttpGet]
        public async Task<IActionResult> ResendOtpForgotPassword()
        {
            var email = TempData["email"]?.ToString();

            if (string.IsNullOrEmpty(email))
            {
                TempData["error"] = "Email not found. Please try again.";
                return RedirectToAction("ForgotPassword");
            }

            Random random = new Random();
            var codeOTP = random.Next(100000, 999999).ToString();
            var receiver = email;
            var subject = "Otter Chef Recover Password Verification";
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "otterchef-txtlogo.png");

            var message = $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                color: #333;
                line-height: 1.5;
            }}
            .verification-box {{
                text-align: center;
                padding: 20px;
                background-color: #f5f5f5;
                border-radius: 8px;
            }}
            .verification-box img {{
                width: 300px;
                height: 90px;
            }}
            .otp-code {{
                font-size: 30px;
                font-weight: bold;
                color: #a3745e;
                margin-top: 20px;
            }}
        </style>
    </head>
    <body>
        <div class='verification-box'>
            <h2>RESET PASSWORD OTP VERIFICATION</h2>
            <p>A verification email has been sent to your email <strong>{receiver}</strong>.</p>
            <p>Please check your email and verify the OTP code to recover your password.</p>
            <p class='otp-code'>OTP: {codeOTP}</p>
            <img src='cid:OtterChefLogo' alt='Otter Chef Logo'>
        </div>
    </body>
    </html>";

            await _emailSender.SendEmailAsync(receiver, subject, message, logoPath, codeOTP);

            TempData["Otp"] = codeOTP; // Cập nhật mã OTP mới
            TempData["email"] = email;

            TempData["success"] = $"The OTP was sent to {receiver}!";
            return RedirectToAction("ResetPasswordVerifyOtp", "Auth");
        }





        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
              jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
             jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role,
             jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            ResponseDto responseDto = await _authService.CheckEmailExistAsync(email);
            if (responseDto != null && responseDto.IsSuccess)
            {

                Random random = new Random();

                var codeOTP = random.Next(100000, 999999).ToString();
                var receiver = email;
                var subject = "Otter Chef Recover Password Verification";
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "otterchef-txtlogo.png");

                var message = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            color: #333;
                            line-height: 1.5;
                        }}
                        .verification-box {{
                            text-align: center;
                            padding: 20px;
                            background-color: #f5f5f5;
                            border-radius: 8px;
                        }}
                        .verification-box img {{
                            width: 300px;
                            height: 90px;
                        }}
                        .otp-code {{
                            font-size: 30px;
                            font-weight: bold;
                            color: #a3745e;
                            margin-top: 20px;
                        }}
                        .resend-button {{
                            display: inline-block;
                            margin-top: 20px;
                            padding: 10px 20px;
                            background-color: #007bff;
                            color: white;
                            border: none;
                            border-radius: 5px;
                            text-decoration: none;
                            font-size: 16px;
                        }}
                        .resend-button:hover {{
                            background-color: #0056b3;
                        }}
                    </style>
                </head>
                <body>
                    <div class='verification-box'>
                        <h2>RESET PASSWORD OTP VERIFICATION</h2>
                        <p>A verification email has been sent to your email <strong>{receiver}</strong>.</p>
                        <p>Please check your email and verify the OTP code to recover your password.</p>
                        <p class='otp-code'>OTP: {codeOTP}</p>
                        <img src='cid:OtterChefLogo' alt='Otter Chef Logo'>
                    </div>
                </body>
                </html>";

                await _emailSender.SendEmailAsync(receiver, subject, message, logoPath, codeOTP);
                TempData["Otp"] = codeOTP;
                TempData["email"] = email;
                _tokenProvider.SetToken(responseDto.Result.ToString());
                TempData["success"] = $"The OTP was sent to {receiver}!";
                return RedirectToAction("ResetPasswordVerifyOtp", "Auth");
            }
            else
            {
                TempData["error"] = "The Email does not exist. Please try again.";
                return View();
            }
        }


        [HttpGet]
        public IActionResult ResetPasswordVerifyOtp()
        {
            TempData.Keep("email");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordVerifyOtp(OtpVerificationDto otpDto)
        {
            string storedOtp = TempData["Otp"]?.ToString();
            var resetPasswordDataJson = TempData["ResetPasswordData"]?.ToString();

            if (storedOtp == otpDto.Otp)
            {
                TempData["success"] = "Verification Successfully";
                TempData.Keep("Otp");
                TempData.Keep("email");
                TempData.Keep("ResetPasswordData");
                return RedirectToAction("ResetPassword", "Auth");
            }
            else
            {
                TempData["Otp"] = storedOtp;
                TempData["ResetPasswordData"] = resetPasswordDataJson;
                TempData["error"] = "Invalid OTP. Please try again.";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword()
        {
            TempData.Keep("email");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string newPassword, string confirmPassword)
        {
            if (newPassword == confirmPassword)
            {
                ResetPasswordDto resetPasswordDto = new ResetPasswordDto()
                {
                    UserName = TempData["email"]?.ToString(),
                    NewPassword = newPassword,
                    Token = _tokenProvider.GetToken()
                };
                ResponseDto responseDto = await _authService.ResetPassword(resetPasswordDto);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = responseDto.Message;
                    return RedirectToAction("Login", "Auth");
                }
                TempData["error"] = responseDto.Message;
                return View();
            }
            else
            {
                TempData["error"] = "New Password and Confirm Password do not match!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var email = _tokenProvider.GetEmailFromToken();
            ResponseDto checkEmailResponse = await _authService.CheckEmailExistAsync(email);
            if (checkEmailResponse != null && checkEmailResponse.IsSuccess)
            {

                ResponseDto loginResponse = await _authService.LoginAsync(new LoginRequestDto
                {
                    UserName = email,
                    Password = currentPassword,
                });

                if (loginResponse != null && loginResponse.IsSuccess)
                {
                    ResetPasswordDto resetPasswordDto = new ResetPasswordDto()
                    {
                        UserName = email.ToString(),
                        NewPassword = newPassword,
                        Token = checkEmailResponse.Result.ToString()
                    };
                    ResponseDto resetResponse = await _authService.ResetPassword(resetPasswordDto);
                    if (resetResponse != null && resetResponse.IsSuccess)
                    {
                        TempData["success"] = "Change Password successfully!";
                        return RedirectToAction("Index", "Home");
                    }
                    TempData["error"] = resetResponse.Message;
                    return View();
                }
                TempData["error"] = "Current password is not correct!";
                return View();
            }
            TempData["error"] = checkEmailResponse.Message;
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {

            // Retrieve the user ID from the claims
            var userId = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                ResponseDto responseDto = await _authService.GetUserById(userId);
                if (responseDto.IsSuccess && responseDto.Result != null)
                {
                    var userData = JsonConvert.DeserializeObject<RegistrationRequestDto>(responseDto.Result.ToString());
                    var updateRequest = new RegistrationRequestDto()
                    {
                        Email = userData.Email,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        Gender = userData.Gender,
                        PhoneNumber = userData.PhoneNumber,
                        Address = userData.Address,
                        Birthday = userData.Birthday,
                        IsActive = userData.IsActive
                    };

                    return View(updateRequest);
                }
                ViewData["error"] = responseDto.Message;
                return RedirectToAction("Index", "Home");
            }
            ViewData["error"] = "Token is invalid";
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequestDto updateProfileDto)
        {

            ResponseDto responseDto = await _authService.UpdateProfileAsync(updateProfileDto);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = responseDto.Message;
                return RedirectToAction(nameof(UpdateProfile));
            }
            TempData["error"] = responseDto.Message;
            return View();
        }

        [HttpGet]
        public IActionResult GetAllCustomer()
        {
            IEnumerable<ApplicationUser> list;
            ResponseDto response = _authService.GetAllCustomerUser().GetAwaiter().GetResult();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ApplicationUser>>(Convert.ToString(response.Result));
            }
            else
            {
                list = new List<ApplicationUser>();
            }
            return Json(new { data = list });
        }
        [HttpGet]
        public async Task<IActionResult> ManageCustomers()
        {
            return View();
        }

        public async Task<IActionResult> BanAccount(string email)
        {
            ApplicationUser? user = null;
            ResponseDto response = await _authService.GetUserByEmail(email);
            if (response != null && response.IsSuccess)
            {
                user = JsonConvert.DeserializeObject<ApplicationUser>(Convert.ToString(response.Result));
            }
            user.IsActive = false;
            UpdateProfileRequestDto updateProfileRequestDto = new UpdateProfileRequestDto()
            {
                Email = email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                Address = user.Address,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
            };
            ResponseDto responseDto = await _authService.UpdateProfileAsync(updateProfileRequestDto);
            if (responseDto.IsSuccess && responseDto != null)
            {
                TempData["success"] = $"The user with email: {email} was banned!";
            }
            else
            {
                TempData["error"] = $"Fail to ban the User with email: {email}!";
            }
            return RedirectToAction(nameof(ManageCustomers));
        }
        public async Task<IActionResult> UnBanAccount(string email)
        {
            ApplicationUser? user = null;
            ResponseDto response = await _authService.GetUserByEmail(email);
            if (response != null && response.IsSuccess)
            {
                user = JsonConvert.DeserializeObject<ApplicationUser>(Convert.ToString(response.Result));
            }
            user.IsActive = true;
            UpdateProfileRequestDto updateProfileRequestDto = new UpdateProfileRequestDto()
            {
                Email = email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                Address = user.Address,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
            };
            ResponseDto responseDto = await _authService.UpdateProfileAsync(updateProfileRequestDto);
            if (responseDto.IsSuccess && responseDto != null)
            {
                TempData["success"] = $"The user with email: {email} was unbanned!";
            }
            else
            {
                TempData["error"] = $"Fail to unban the User with email: {email}!";
            }
            return RedirectToAction(nameof(ManageCustomers));
        }
    }
}



