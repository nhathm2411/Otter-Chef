using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class RegistrationRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Role { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        public bool? IsActive { get; set; }
    }

}
