using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class ParryEntry : LogEntry
    {
        public ParryEntry()
        {
        }

        public ParryEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class ParryEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            string attacker = null;

            string victim = null;

            if (log.cleanedLog.Contains("easily parry with") ||
                log.cleanedLog.Contains("skillfully parry with") ||
                log.cleanedLog.Contains("safely parry with") ||
                log.cleanedLog.Contains("barely parry with") ||
                log.cleanedLog.Contains("easily parries with") ||
                log.cleanedLog.Contains("skillfully parries with") ||
                log.cleanedLog.Contains("safely parries with") ||
                log.cleanedLog.Contains("barely parries with") ||
                log.cleanedLog.Contains("easily evade the blow") ||
                log.cleanedLog.Contains("skillfully evade the blow") ||
                log.cleanedLog.Contains("safely evade the blow") ||
                log.cleanedLog.Contains("barely evade the blow") ||
                log.cleanedLog.Contains("easily evades the blow") ||
                log.cleanedLog.Contains("skillfully evades the blow") ||
                log.cleanedLog.Contains("safely evades the blow") ||
                log.cleanedLog.Contains("barely evades the blow"))
            {
                return new ParryEntry(log);
            }

            return null;
        }
    }
}
