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
    public class Experience
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; }

        [Required]
        [StringLength(100)]
        public string Company { get; set; }

        [Required]
        [StringLength(50)]
        public string JobType { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(500)]
        public string? Summary { get; set; }

        // Foreign key
        public string UserId { get; set; }



        [ForeignKey("UserId")]
        [JsonIgnore] // Prevents circular reference in JSON serialization
        public virtual AppUser User { get; set; }
    }
}
