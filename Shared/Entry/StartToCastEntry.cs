using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorApp.Shared.Entry
{
    public class StartToCastEntry: LogEntry
    {
        public string Caster { get; set; }
        public string Spell { get; set; }
        public string Target { get; set; }
    }

    public class StartToCastEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log)
        {
            if (log.cleanedLog.Contains("starts to cast"))
            {
                if(log.cleanedLog.Contains(" on "))
                {
                    var match = Regex.Match(log.cleanedLog, @"(.*) starts to cast '?(.*?)'? on (.*)");

                } else
                {
                    var match = Regex.Match(log.cleanedLog, @"(.*) starts to cast '?(.*?)'?$");

                }
            }
            return null;
        }
    }
}
