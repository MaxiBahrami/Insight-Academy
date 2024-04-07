using EasyRepository.EFCore.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace InsightAcademy.Entities
{
    public class TeacherProfile : EasyBaseEntity<int>, IEasyCreateDateEntity, IEasyUpdateDateEntity, IEasySoftDeleteEntity
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Languages { get; set; }
        public string Rate { get; set; }
        public string Introduction { get; set; }
        public string TagLine { get; set; }
        public int Zipcode { get; set; }
        public bool MyHome { get; set; }
        public bool student {  get; set; }
        public bool online { get; set; }     
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string Whatsapp { get; set;}
        public string WebSite { get; set;}
        public User User { get; set; }
    }
}
