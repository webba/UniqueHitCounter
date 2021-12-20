
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class LogEntry
    {
        public LogEntry()
        {

        }
        public LogEntry((string cleanedLog, DateTime datetime) log)
        {
            Log = log.cleanedLog;
            LogTime = log.datetime;
        }

        public string Log { get; set; }

        public DateTime LogTime { get; set; }
    }

    public class LogEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            return new LogEntry
            {
                Log = log.cleanedLog,
                LogTime = log.datetime
            };
        }


    }
}
