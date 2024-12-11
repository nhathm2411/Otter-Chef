namespace Mango.Web.Models.Dto
{
    public class UpdateProfileRequestDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
