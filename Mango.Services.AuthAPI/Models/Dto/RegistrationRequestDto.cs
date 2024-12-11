namespace Mango.Services.AuthAPI.Models.Dto
{
    public class RegistrationRequestDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }

        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
