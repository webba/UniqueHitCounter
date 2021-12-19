
using System;
using System.Text.RegularExpressions;

namespace BlazorApp.Shared.Entry
{
    public class TryToEntry : LogEntry
    {
        public TryToEntry()
        {
        }

        public TryToEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class TryToEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            string attacker = null;

            string victim = null;

            if (log.cleanedLog.Contains("tries to") || log.cleanedLog.StartsWith("You try to") || log.cleanedLog.Contains("moves in to attack"))
            {
                return new TryToEntry(log);
            }

            return null;
        }
    }
}
