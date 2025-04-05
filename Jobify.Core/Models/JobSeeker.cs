using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Core.Models
{
    public class JobSeeker:AppUser
    {
        // Personal Information
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [Url]
        [StringLength(255)]
        public string? LinkedIn { get; set; }

        [Url]
        [StringLength(255)]
        public string? GitHub { get; set; }

        [Url]
        [StringLength(255)]
        public string? Portfolio { get; set; }

        // Navigation properties for related entities
        public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
        public virtual ICollection<Experience> Experiences { get; set; } = new List<Experience>();
        public virtual ICollection<UserSkill> Skills { get; set; } = new List<UserSkill>();

        // Computed property for full name
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
