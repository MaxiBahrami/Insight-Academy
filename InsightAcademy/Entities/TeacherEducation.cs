using EasyRepository.EFCore.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsightAcademy.Entities
{
    public class TeacherEducation : EasyBaseEntity<int>, IEasyCreateDateEntity, IEasyUpdateDateEntity, IEasySoftDeleteEntity
    {
        [ForeignKey("TeacherProfile")]
        public int ProfileId { get; set; }
        public string EducationTile { get; set; }
        public string DayDate { get; set; }
        public string University { get; set; }

        public string Description { get; set; }

        public TeacherProfile Profile { get; set; }


    }
}
