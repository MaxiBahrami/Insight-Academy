using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsightAcademy.Dtos
{
    public class TeacherProfileDto:UserDto
    {
        public int id { get; set; }
         public int UserId { get; set; }     
        public string Languages { get; set; }
        public string HourlyRate { get; set; }
        public int ZipCode { get; set; }
        public string Introduction { get; set; }
        public string TagLine { get; set; }
        public bool MyHome { get; set; }
        public bool student {  get; set; }
        public bool online { get; set; }     
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string Whatsapp { get; set;}      
        public int formnumber { get; set; }
        public SelectList Languageslist { get; set; }

    }
}
