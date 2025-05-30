﻿using Template.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Template.Services.Shared;

namespace Template.Services
{
    public class TemplateDbContext : DbContext
    {
        public TemplateDbContext()
        {
        }

        public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
        {
            DataGenerator.InitializeUsers(this);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<Rendiconto> Rendiconto { get; set; }
        public DbSet<FeriePermesso> FeriePermesso { get; set; }

    }
}
