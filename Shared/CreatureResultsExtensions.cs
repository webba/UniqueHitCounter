using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public static class CreatureResultsExtensions
    {
        public static CreatureCombatResult GetCombatResultData(this CombatResults result, string creature)
        {
            if (!result.CombatResultsDict.ContainsKey(creature))
            {
                return null;
            }

            return result.CombatResultsDict[creature];
        }

        public static AttackerVictimCombatResult GetAttackerVictimCombatResult(this CombatResults result, string attacker, string victim)
        {
            var data = result.GetCombatResultData(attacker);
            if (attacker == null)
            {
                return null;
            }

            if (!data.Attacks.ContainsKey(victim))
            {
                return null;
            }

            return data.Attacks[victim];
        }
    }
}
