namespace Mango.Services.AuthAPI.Models.Dto
{
    public class ResetPasswordDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
