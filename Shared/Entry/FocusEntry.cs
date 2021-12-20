using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class FocusEntry : LogEntry
    {
        public FocusEntry()
        {
        }

        public FocusEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class FocusEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (log.cleanedLog.Contains("now seems more focused") || 
                log.cleanedLog.Contains("seems to focus") || 
                log.cleanedLog.Contains("try to focus") ||
                log.cleanedLog.Contains("lose some focus") ||
                log.cleanedLog.Contains("looks disturbed") ||
                log.cleanedLog.Contains("seems a little more brave now") ||
                log.cleanedLog.Contains("are not focused on combat") ||
                log.cleanedLog.Contains("balance your feet and your soul") ||
                log.cleanedLog.Contains("fail to reach a higher degree of focus") ||
                log.cleanedLog.Contains("now focused on the enemy and its every move") ||
                log.cleanedLog.Contains("feel lightning inside, quickening your reflexes") ||
                log.cleanedLog.Contains("consciousness is lifted to a higher level, making you very attentive") ||
                log.cleanedLog.Contains("feel supernatural. Invincible"))
            {
                return new FocusEntry(log);
            }

            return null;
        }
    }
}
