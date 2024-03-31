﻿using InsightAcademy.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsightAcademy.Context
{
    public class InsightAcademy_DBContext:DbContext
    {
        public InsightAcademy_DBContext(DbContextOptions<InsightAcademy_DBContext>options):base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
