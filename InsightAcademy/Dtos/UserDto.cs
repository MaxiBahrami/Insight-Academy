using InsightAcademy.Entities;

namespace InsightAcademy.Dtos
{
    public class UserDto
    {
        public IFormFile ProfileImage { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public eRole Role { get; set; }     
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
