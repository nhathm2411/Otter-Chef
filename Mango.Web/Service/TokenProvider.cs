using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);

        }

        public string? GetToken()
        {
            string? token = null;

            bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie, out token);

            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookie, token);
        }

        public string? GetEmailFromToken()
        {
            string? token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Giả sử email được lưu trữ trong claim có tên là "email"
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email");

            return emailClaim?.Value;
        }
    }
}
