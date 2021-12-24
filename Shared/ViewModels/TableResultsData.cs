using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorApp.Shared.ViewModels
{
    public class TableResultsData
    {
        [Display(Name = "Name")]
        public string CreatureName { get; set; }

        [Display(Name = "Hits")]
        public int Hits { get; set; }

        [Display(Name = "Times Hit")]
        public int TimesHit { get; set; }

        [Display(Name = "Times Stunned")]
        public int TimesStunned { get; set; }

        [Display(Name = "Times Thrown")]
        public int TimesThrown { get; set; }

        [Display(Name = "Knocks")]
        public int KnocksSenseless { get; set; }

        [Display(Name = "Biggest Hit")]
        public string BiggestHit { get; set; }
    }
}