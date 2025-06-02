using Jobify.Core.Models;
using Jobify.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Services.Commons.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
