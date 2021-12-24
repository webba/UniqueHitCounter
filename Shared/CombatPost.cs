using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Shared
{
    public class CombatPost
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string CombatString { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
