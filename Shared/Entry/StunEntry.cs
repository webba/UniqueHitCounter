using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class StunEntry : LogEntry
    {
        public StunEntry()
        {
        }

        public StunEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class StunEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (log.cleanedLog.Contains("make a bad move, making you an easy target") ||
                log.cleanedLog.Contains("makes a bad move and is an easy target") ||
                log.cleanedLog.Contains("regains her bearings") ||
                log.cleanedLog.Contains("regains his bearings") ||
                log.cleanedLog.Contains("brushes away the stunned feeling") ||
                log.cleanedLog.Contains("open yourself to an attack from") ||
                log.cleanedLog.Contains("is knocked senseless from the hit") ||
                log.cleanedLog.Contains("senseless with her hit") ||
                log.cleanedLog.Contains("senseless with his hit") ||
                log.cleanedLog.Contains("over with her shield") ||
                log.cleanedLog.Contains("over with his shield"))
            {
                return new StunEntry(log);
            }

            return null;
        }
    }
}
