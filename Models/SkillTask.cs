using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.Models
{
    internal class SkillTask
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "New Task";
        public string? Description { get; set; } = "Description...";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual List<Link> Links { get; set; } = new();
        public bool IsCompleted { get; set; } = false;
        [Required]
        public int SkillId { get; set; }
        public Skill? Skill { get; set; }
    }
}
