using System.ComponentModel.DataAnnotations;

namespace SkillBase.Models
{
    internal class Link
    {
        public int Id { get; set; }
        public string? Name { get; set; } = "Google";
        [Required]
        public string? Url { get; set; } = "https://google.com";
        [Required]
        public int DayTaskId { get; set; }
    }
}
