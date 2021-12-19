using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class UniqueActionEntry : LogEntry
    {
        public UniqueActionEntry()
        {
        }

        public UniqueActionEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class UniqueActionEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (log.cleanedLog.Contains("picks up and throws") ||
                log.cleanedLog.Contains("makes a circular powerful sweep") ||
                log.cleanedLog.Contains("dragon breathes fire") ||
                log.cleanedLog.Contains("dragon burns you") ||
                log.cleanedLog.Contains("shakes the earth") ||
                log.cleanedLog.Contains("forest giant stomps") ||
                log.cleanedLog.Contains("swings out in front of himself and slashes everyone") ||
                log.cleanedLog.Contains("stomps the ground and knocks everyone around"))
            {
                return new UniqueActionEntry(log);
            }

            return null;
        }
    }
}
