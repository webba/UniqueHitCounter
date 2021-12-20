using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class TargetsEntry : LogEntry
    {
        public TargetsEntry()
        {
        }

        public TargetsEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class TargetsEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            string attacker = null;

            string victim = null;

            if (log.cleanedLog.Contains("tries to move into position to target") || 
                log.cleanedLog.Contains(" targets your ") ||
                log.cleanedLog.Contains("move into position for") ||
                log.cleanedLog.Contains("fails to move into position") ||
                log.cleanedLog.Contains("fail to move into position"))
            {
                return new TargetsEntry(log);
            }

            return null;
        }
    }
}
