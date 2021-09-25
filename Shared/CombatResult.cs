using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class CombatResult
    {
        public string Player { get; set; }
        public int Hits { get; set; }
        public int TimesHit { get; set; }
        public int TimesStunned { get; set; }
        public int TimesThrown { get; set; }
        public int KnocksOver { get; set; }
        public string BiggestHit { get; set; }
    }
}
