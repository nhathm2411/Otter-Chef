using Microsoft.AspNetCore.Identity;

namespace Mango.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsActive { get; set; }
    }
}
