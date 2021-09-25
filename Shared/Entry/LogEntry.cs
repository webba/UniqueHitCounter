
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class LogEntry
    {
        public string Log { get; set; }

        public DateTime LogTime { get; set; }
    }

    public class LogEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log)
        {
            return new LogEntry
            {
                Log = log.cleanedLog,
                LogTime = log.datetime
            };
        }


    }
}
