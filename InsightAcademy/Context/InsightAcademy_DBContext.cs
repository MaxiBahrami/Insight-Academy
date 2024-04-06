using InsightAcademy.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsightAcademy.Context
{
    public class InsightAcademy_DBContext:DbContext
    {
        public InsightAcademy_DBContext(DbContextOptions<InsightAcademy_DBContext>options):base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TeacherProfile> TeacherProfile { get; set; }
        public DbSet<TeacherEducation> TeacherEducation { get; set; }
    }
}
