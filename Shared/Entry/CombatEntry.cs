using System;

namespace BlazorApp.Shared.Entry
{
    public class CombatEntry : LogEntry
    {
        public string Attacker { get; set; }

        public string Victim { get; set; }

        public string HitType { get; set; }

        public string HitStrength { get; set; }

        public string HitResult { get; set; }

        public string HitLocation { get; set; }
    }

    public class CombatEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log)
        {
            return new CombatEntry
            {
                Log = log.cleanedLog,
                LogTime = log.datetime,
                HitType = "mauls"
            };
        }
    }
}
