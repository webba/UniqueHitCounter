using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorApp.Shared.Entry
{
    public class GlanceEntry : LogEntry
    {
        public GlanceEntry()
        {
        }

        public GlanceEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }

        public string Attacker { get; set; }

        public string Victim { get; set; }

        public string HitLocation { get; set; }
    }
    public class GlanceEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            Match match = Regex.Match(log.cleanedLog, CombatStrings.OtherGlancePattern);

            if (match.Success)
            {
                return new GlanceEntry
                {
                    Log = log.cleanedLog,
                    LogTime = log.datetime,
                    Victim = characterName,
                    HitLocation = match.Groups[1].Value
                };
            }

            match = Regex.Match(log.cleanedLog, CombatStrings.OtherGlancePattern2);

            if (match.Success)
            {
                return new GlanceEntry
                {
                    Log = log.cleanedLog,
                    LogTime = log.datetime,
                    Victim = characterName,
                    HitLocation = match.Groups[1].Value
                };
            }

            match = Regex.Match(log.cleanedLog, CombatStrings.ActorGlancePattern);

            if (match.Success)
            {
                return new GlanceEntry
                {
                    Log = log.cleanedLog,
                    LogTime = log.datetime,
                    Attacker = characterName,
                    Victim = match.Groups[1].Value
                };
            }

            match = Regex.Match(log.cleanedLog, CombatStrings.ActorGlancePattern2);

            if (match.Success)
            {
                return new GlanceEntry
                {
                    Log = log.cleanedLog,
                    LogTime = log.datetime,
                    Attacker = characterName,
                    Victim = match.Groups[1].Value,
                    HitLocation = match.Groups[2].Value
                };
            }

            return null;
        }
    }
}
