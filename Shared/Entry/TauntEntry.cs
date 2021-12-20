using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class TauntEntry : LogEntry
    {
        public TauntEntry()
        {
        }

        public TauntEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class TauntEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (log.cleanedLog.Contains(" starts to annoy ") ||
                log.cleanedLog.Contains("snarls aggressively and taunts") ||
                (log.cleanedLog.Contains(" ignores ") && log.cleanedLog.Contains(" antics")))
            {
                return new TauntEntry(log);
            }

            return null;
        }
    }
}
