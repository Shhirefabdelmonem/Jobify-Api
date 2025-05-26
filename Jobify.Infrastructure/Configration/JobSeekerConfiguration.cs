using Jobify.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobify.Infrastructure.Configration
{
    public class JobSeekerConfiguration : IEntityTypeConfiguration<JobSeeker>
    {
        public void Configure(EntityTypeBuilder<JobSeeker> builder)
        {
            // Configure TPT inheritance
            builder.ToTable("JobSeekers");
            
            // Configure properties
            builder.Property(js => js.FirstName)
                .HasMaxLength(50);
                
            builder.Property(js => js.LastName)
                .HasMaxLength(50);
                
            builder.Property(js => js.Phone)
                .HasMaxLength(20);
                
            builder.Property(js => js.LinkedIn)
                .HasMaxLength(255);
                
            builder.Property(js => js.GitHub)
                .HasMaxLength(255);
                
            builder.Property(js => js.Portfolio)
                .HasMaxLength(255);
                
            // Configure relationships with NO ACTION instead of CASCADE
            // Use HasOne-WithMany pattern instead to avoid constraint naming issues
            builder.HasOne<AppUser>().WithOne().HasForeignKey<JobSeeker>(js => js.Id);
            
            builder.HasMany(js => js.Educations)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
                
            builder.HasMany(js => js.Experiences)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
                
            builder.HasMany(js => js.Skills)
                .WithOne()
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}