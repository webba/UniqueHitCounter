using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class CombatResults
    {
        public IDictionary<string, CreatureCombatResult> CombatResultsDict { get; set; }
        public string Creator { get; set; }
        public DateTime Created { get; set; }
        public DateTime FightStart { get; set; }
        public DateTime FightEnd { get; set; }
    }
}
