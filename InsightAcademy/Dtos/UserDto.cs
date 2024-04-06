using InsightAcademy.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsightAcademy.Dtos
{
    public class UserDto
    {
        public IFormFile ProfileImage { get; set; }
        public byte[] ProfileImagepath { get; set; }
        public string? Username { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public eRole Role { get; set; }     
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public SelectList cites { get; set; }
        public SelectList countrylist { get; set; }
        public string? Country { get; set; }
    }

}
