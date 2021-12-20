using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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

        public const string Regains = "regains";
        public const string BadMove = "bad move";
        public const string KnocksSenseless = "knocks senseless";

        public string StunType { get; set; }

        public string Attacker { get; set; }

        public string Victim { get; set; }
    }

    public class StunEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (log.cleanedLog.Contains("You make a bad move, making you an easy target"))
            {
                return new StunEntry
                {
                    Log = log.cleanedLog,
                    LogTime = log.datetime,
                    Victim = TextUtil.WordToUpper(characterName),
                    StunType = StunEntry.BadMove
                };

            }
            else if (log.cleanedLog.Contains("open yourself to an attack from"))
            {
                Match match = Regex.Match(log.cleanedLog, @"You open yourself to an attack from (.*?)");

                if (match.Success)
                {
                    return new StunEntry
                    {
                        Log = log.cleanedLog,
                        LogTime = log.datetime,
                        Attacker = TextUtil.WordToUpper(match.Groups[1].Value),
                        Victim = TextUtil.WordToUpper(characterName),
                        StunType = StunEntry.BadMove
                    };
                }
                return new StunEntry(log);

            }
            else if (log.cleanedLog.Contains("makes a bad move and is an easy target"))
            {
                Match match = Regex.Match(log.cleanedLog, @"(.*?) makes a bad move and is an easy target");

                if (match.Success)
                {
                    return new StunEntry
                    {
                        Log = log.cleanedLog,
                        LogTime = log.datetime,
                        Victim = TextUtil.WordToUpper(match.Groups[1].Value),
                        StunType = StunEntry.BadMove
                    };
                }
                return new StunEntry(log);

            }
            else if (log.cleanedLog.Contains("regains her bearings") ||
                log.cleanedLog.Contains("regains his bearings"))
            {
                Match match = Regex.Match(log.cleanedLog, @"(.*?) regains his bearings");

                if (match.Success)
                {
                    return new StunEntry
                    {
                        Log = log.cleanedLog,
                        LogTime = log.datetime,
                        Victim = TextUtil.WordToUpper(match.Groups[1].Value),
                        StunType = StunEntry.Regains
                    };
                }
                return new StunEntry(log);

            }
            else if (log.cleanedLog.Contains("regains her bearings"))
            {
                Match match = Regex.Match(log.cleanedLog, @"(.*?) regains her bearings");

                if (match.Success)
                {
                    return new StunEntry
                    {
                        Log = log.cleanedLog,
                        LogTime = log.datetime,
                        Victim = TextUtil.WordToUpper(match.Groups[1].Value),
                        StunType = StunEntry.Regains
                    };
                }
                return new StunEntry(log);

            }
            else if (log.cleanedLog.Contains("is knocked senseless from the hit"))
            {
                Match match = Regex.Match(log.cleanedLog, @"(.*?) is knocked senseless from the hit");

                if (match.Success)
                {
                    return new StunEntry
                    {
                        Log = log.cleanedLog,
                        LogTime = log.datetime,
                        Attacker = TextUtil.WordToUpper(characterName),
                        Victim = TextUtil.WordToUpper(match.Groups[1].Value),
                        StunType = StunEntry.KnocksSenseless
                    };
                }
                return new StunEntry(log);
            }
            else if (log.cleanedLog.Contains(" knocks ") && log.cleanedLog.Contains(" senseless with "))
            {
                Match match = Regex.Match(log.cleanedLog, @"(.*?) knocks (.*?) senseless with (his|her) hit");

                if (match.Success)
                {
                    return new StunEntry
                    {
                        Log = log.cleanedLog,
                        LogTime = log.datetime,
                        Attacker = TextUtil.WordToUpper(match.Groups[1].Value),
                        Victim = TextUtil.WordToUpper(match.Groups[2].Value),
                        StunType = StunEntry.KnocksSenseless
                    };
                }
                return new StunEntry(log);
            }
            else if (log.cleanedLog.Contains("brushes away the stunned feeling") ||
                log.cleanedLog.Contains("open yourself to an attack from") ||
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
