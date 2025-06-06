﻿using Jobify.Core.Models;
using Jobify.Domain.Models;
using Jobify.Infrastructure.Configration;
using Jobify.Services.Commons.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Infrastructure.Presistance.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EntityConfigration());
            modelBuilder.ApplyConfiguration(new JobSeekerConfiguration());
            //modelBuilder.ApplyConfiguration(new RefreshTokenConfigration());

            // Other configurations
        }

        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
