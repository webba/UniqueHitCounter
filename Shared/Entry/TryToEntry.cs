
using System;

namespace BlazorApp.Shared.Entry
{
    public class TryToEntry : LogEntry
    {
        public string Attacker { get; set; }

        public string Victim { get; set; }

        public string HitType { get; set; }
    }

    public class TryToEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log)
        {
            if (log.cleanedLog.StartsWith("You try to"))
            {

            }
            return new TryToEntry
            {
                Log = log.cleanedLog,
                LogTime = log.datetime,
                Victim = "You"
            };
        }

    }
}
