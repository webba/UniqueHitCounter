using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class ShelterEntry : LogEntry
    {
        public ShelterEntry()
        {
        }

        public ShelterEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class ShelterEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (log.cleanedLog.Contains("seems to shelter") ||
                log.cleanedLog.Contains("prepare to shelter") ||
                log.cleanedLog.Contains("You shelter the"))
            {
                return new ShelterEntry(log);
            }

            return null;
        }
    }
}
