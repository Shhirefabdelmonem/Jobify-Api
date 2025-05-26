using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Jobify.Core.Models
{
    [PrimaryKey(nameof(UserId), nameof(SkillId))]
    public class UserSkill
    {
        public string UserId { get; set; }

        public int SkillId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual JobSeeker User { get; set; }


        [ForeignKey("SkillId")]
        [JsonIgnore]
        public virtual Skill Skill { get; set; }
    }
}
