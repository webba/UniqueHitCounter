using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class AttackerVictimCombatResult
    {
        public int Hits { get; set; }
        public int Throws { get; set; }
        public int KnocksOver { get; set; }
        public int BiggestHitDamage { get; set; }
        public string BiggestHit { get; set; }
    }
}
