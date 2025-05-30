using Jobify.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobify.Infrastructure.Configration
{
    public class JobSeekerConfiguration : IEntityTypeConfiguration<JobSeeker>
    {
        public void Configure(EntityTypeBuilder<JobSeeker> builder)
        {
            // Configure TPT inheritance - this creates a separate table for JobSeeker
            builder.ToTable("JobSeekers");


        }
    }
}