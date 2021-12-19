using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class FrenzyEntry : LogEntry
    {
        public FrenzyEntry()
        {
        }

        public FrenzyEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class FrenzyEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (log.cleanedLog.Contains("suddenly goes into a frenzy"))
            {
                return new FrenzyEntry(log);
            }

            return null;
        }
    }
}
