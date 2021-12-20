using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class GenericInfoEntry : LogEntry
    {
        public GenericInfoEntry()
        {
        }

        public GenericInfoEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class GenericInfoEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (log.cleanedLog.Contains("You are not attacking anyone") || 
                log.cleanedLog.Contains("is targetting") ||
                log.cleanedLog.Contains("You need to get into the fight more first") ||
                log.cleanedLog.Contains("heals some of your wounds") ||
                log.cleanedLog.Contains("heal some of your wounds") ||
                log.cleanedLog.Contains("You partially dispel the") ||
                log.cleanedLog.Contains("You must turn towards"))
            {
                return new GenericInfoEntry(log);
            }

            return null;
        }
    }
}
