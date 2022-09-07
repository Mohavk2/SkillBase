using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.Models
{
    internal class Skill
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "Undefined";
        public string? Description { get; set; } = string.Empty;
        public string? Notes { get; set; } = string.Empty;
        public string? ImagePath { get; set; } = null;
        public virtual List<ReferenceUrl>? References { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int? ParentId { get; set; }
        public Skill? Parent { get; set; }
        public virtual List<Skill>? Children { get; set; }
    }
}
