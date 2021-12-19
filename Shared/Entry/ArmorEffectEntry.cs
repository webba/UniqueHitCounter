using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Entry
{
    public class ArmorEffectEntry : LogEntry
    {
        public ArmorEffectEntry()
        {
        }

        public ArmorEffectEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }
    }

    public class ArmorEffectEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (
                (
                    log.cleanedLog.Contains("A sudden pain hits") &&
                    log.cleanedLog.Contains("in the same location that")
                )
                ||
                (
                    log.cleanedLog.Contains("Dark stripes spread along") &&
                    log.cleanedLog.Contains("drained")
                )
                )
            {
                return new ArmorEffectEntry(log);
            }

            return null;
        }
    }
}
