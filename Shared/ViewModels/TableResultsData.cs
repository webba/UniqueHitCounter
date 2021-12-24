using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorApp.Shared.ViewModels
{
    public class TableResultsData
    {
        public const string DisplayCreatureName = "Name";
        public const string DisplayHits = "Hits";
        public const string DisplayTimesHit = "Times Hit";
        public const string DisplayTimesStunned = "Times Stunned";
        public const string DisplayTimesThrown = "Times Thrown";
        public const string DisplayKnocksSenseless = "Knocks";
        public const string DisplayBiggestHit = "Biggest Hit";


        [Display(Name = DisplayCreatureName)]
        public string CreatureName { get; set; }

        [Display(Name = DisplayHits)]
        public int Hits { get; set; }

        [Display(Name = DisplayTimesHit)]
        public int TimesHit { get; set; }

        [Display(Name = DisplayTimesStunned)]
        public int TimesStunned { get; set; }

        [Display(Name = DisplayTimesThrown)]
        public int TimesThrown { get; set; }

        [Display(Name = DisplayKnocksSenseless)]
        public int KnocksSenseless { get; set; }

        [Display(Name = DisplayBiggestHit)]
        public string BiggestHit { get; set; }
    }
}