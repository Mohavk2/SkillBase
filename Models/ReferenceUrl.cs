using System.ComponentModel.DataAnnotations;

namespace SkillBase.Models
{
    internal class ReferenceUrl
    {
        public int Id { get; set; }
        public string? Name { get; set; } = "Google";
        [Required]
        public string? Url { get; set; } = "https://google.com";
        [Required]
        public int SkillId { get; set; }
    }
}
