using Jobify.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Infrastructure.Configration
{
    public class EntityConfigration : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            // Configure the Gpa property with precision and scale
            builder.Property(e => e.Gpa)
                .HasPrecision(3, 3); // 3 digits in total, 2 after decimal point (e.g., 4.00)
        }
    }
}
