using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorApp.Shared.Entry
{
    public class UniqueActionEntry : LogEntry
    {
        public UniqueActionEntry()
        {
        }

        public UniqueActionEntry((string cleanedLog, DateTime datetime) log) : base(log)
        {
        }

        public const string Throws = "throws";

        public string ActionType { get; set; }

        public string Attacker { get; set; }

        public string Victim { get; set; }
    }

    public class UniqueActionEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (log.cleanedLog.Contains("You make a bad move, making you an easy target"))
            {
                Match match = Regex.Match(log.cleanedLog, @"(.*?) picks up and throws (.*?)");

                if (match.Success)
                {
                    return new UniqueActionEntry
                    {
                        Log = log.cleanedLog,
                        LogTime = log.datetime,
                        Attacker = TextUtil.WordToUpper(match.Groups[1].Value),
                        Victim = TextUtil.WordToUpper(match.Groups[2].Value),
                        ActionType = UniqueActionEntry.Throws
                    };
                }
                return new UniqueActionEntry(log);

            }
            else if (log.cleanedLog.Contains("picks up and throws") ||
                log.cleanedLog.Contains("makes a circular powerful sweep") ||
                log.cleanedLog.Contains("dragon breathes fire") ||
                log.cleanedLog.Contains("dragon burns you") ||
                log.cleanedLog.Contains("shakes the earth") ||
                log.cleanedLog.Contains("forest giant stomps") ||
                log.cleanedLog.Contains("swings out in front of himself and slashes everyone") ||
                log.cleanedLog.Contains("stomps the ground and knocks everyone around"))
            {
                return new UniqueActionEntry(log);
            }

            return null;
        }
    }
}
