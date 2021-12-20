using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class MissEntry : LogEntry
    {
        public MissEntry()
        {
        }

        public MissEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class MissEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (log.cleanedLog.Contains("miss with the"))
            {
                return new MissEntry(log);
            }

            return null;
        }
    }
}
