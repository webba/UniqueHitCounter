using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class CreatureCombatResult
    {
        public int Hits { get; set; }
        public int TimesHit { get; set; }
        public int TimesStunned { get; set; }
        public int TimesThrown { get; set; }
        public int KnocksOver { get; set; }
        public int BiggestHitDamage { get; set; }
        public string BiggestHit { get; set; }
        public IDictionary<string, AttackerVictimCombatResult> Attacks { get; set; } = new Dictionary<string, AttackerVictimCombatResult>();
    }
}
