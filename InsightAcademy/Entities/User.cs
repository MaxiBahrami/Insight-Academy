using EasyRepository.EFCore.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsightAcademy.Entities
{
    public class User : EasyBaseEntity<int>, IEasyCreateDateEntity, IEasyUpdateDateEntity, IEasySoftDeleteEntity
    {
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public eRole Role { get; set; }
        public byte[] ProfileImage { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

    }
    public enum eRole
    {
        Student=10,
        Teacher=20,
        Admin=30
    }
}
