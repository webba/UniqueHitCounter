using System;
using System.Text.RegularExpressions;

namespace BlazorApp.Shared.Entry
{
    public class CombatEntry : LogEntry
    {
        public CombatEntry()
        {
        }

        public CombatEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }

        public string Attacker { get; set; }

        public string Victim { get; set; }

        public string HitType { get; set; }

        public string HitStrength { get; set; }

        public string HitResult { get; set; }

        public string HitLocation { get; set; }
    }

    public class CombatEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            Match match = Regex.Match(log.cleanedLog, CombatStrings.OtherPattern);

            if (match.Success)
            {
                return new CombatEntry
                {
                    Log = log.cleanedLog,
                    LogTime = log.datetime,
                    Attacker = TextUtil.WordToUpper(match.Groups[1].Value),
                    HitType = match.Groups[2].Value,
                    Victim = TextUtil.WordToUpper(match.Groups[3].Value.ToLower() == "you" ? characterName : match.Groups[3].Value),
                    HitStrength = match.Groups[4].Value,
                    HitLocation = match.Groups[5].Value,
                    HitResult = match.Groups[6].Value
                };
            }

            match = Regex.Match(log.cleanedLog, CombatStrings.ActorPattern);

            if (match.Success)
            {
                return new CombatEntry
                {
                    Log = log.cleanedLog,
                    LogTime = log.datetime,
                    Attacker = TextUtil.WordToUpper(characterName),
                    HitType = match.Groups[2].Value,
                    Victim = TextUtil.WordToUpper(match.Groups[3].Value),
                    HitStrength = match.Groups[4].Value,
                    HitLocation = match.Groups[5].Value,
                    HitResult = match.Groups[6].Value
                };
            }

            return null;
        }
    }
}
