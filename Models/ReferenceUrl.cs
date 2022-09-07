using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.Models
{
    internal class ReferenceUrl
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Required]
        public string? Url { get; set; }

    }
}
