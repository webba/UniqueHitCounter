using BlazorApp.Shared;
using System;
using System.Collections.Generic;

namespace UniqueHitCounter.Logic
{
    public class LogParser
    {
        public static IEnumerable<CombatEntry> ParseCombatLog(CombatPost post)
        {
            IList<CombatEntry> result = new List<CombatEntry>();

            foreach (var line in post.CombatString.Trim().Split('\n'))
            {
                Console.WriteLine(line);    
                result.Add(new CombatEntry()
                {
                    Log = line.Trim()
                });
            }


            return result;
        }
    }
}
