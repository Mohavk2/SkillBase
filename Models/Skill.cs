using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkillBase.Models
{
    internal class Skill
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "New skill";
        public string? Description { get; set; } = "Description...";
        public string? Notes { get; set; } = "Write your notes here...";
        public string? ImagePath { get; set; } = null;
        public virtual List<ReferenceUrl> References { get; set; } = new();
        public bool IsCompleted { get; set; } = false;
        public int? ParentId { get; set; }
        public Skill? Parent { get; set; }
        public virtual List<Skill>? Children { get; set; }
    }
}
