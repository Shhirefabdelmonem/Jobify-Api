using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Jobify.Core.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string SchoolName { get; set; }

        [Required]
        [StringLength(100)]
        public string Major { get; set; }

        [Required]
        [StringLength(50)]
        public string DegreeType { get; set; }

        [Range(0, 4.0)]
        public decimal? Gpa { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        // Foreign key
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore] // Prevents circular reference in JSON serialization
        public virtual AppUser User { get; set; }
    }
}
