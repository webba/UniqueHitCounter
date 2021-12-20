using BlazorApp.Shared;
using BlazorApp.Shared.Entry;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace UniqueHitCounter.Logic
{
    public class LogParser
    {
        private readonly CombatPost _LogPost;
        private DateTime _CurrentTime;
        private DateTime? _LastTime = null;

        public LogParser(CombatPost post)
        {
            _LogPost = post;
            _CurrentTime = post.StartDate;
        }

        public static int GetDamageInt(string damage)
        {
            return damage switch
            {
                "tickle" => 1,
                "slap" => 2,
                "irritate" => 3,
                "hurt" => 4,
                "harm" => 5,
                "damage" => 6,    
                _ => 0
            };
        }

        public IDictionary<string, IEnumerable<CombatResult>> ProcessResults(IEnumerable<LogEntry> entries)
        {
            var combatEntries = entries.Where(e => e is CombatEntry).Select(e => e as CombatEntry).ToList();
            var stunEntries = entries.Where(e => e is StunEntry).Select(e => e as StunEntry).ToList();
            var uniqueEntries = entries.Where(e => e is UniqueActionEntry).Select(e => e as UniqueActionEntry).ToList();

            var creatures = combatEntries.SelectMany(c => new List<string> { 
                c!.Victim, 
                c!.Attacker 
            }).Distinct().ToList();

            var data = creatures.ToDictionary(c => c, 
                c => (IEnumerable<CombatResult>)creatures.Select(cc => new CombatResult
                    {
                        Player =  cc,
                        Hits = combatEntries.Count(ent => ent!.Attacker == cc && ent!.Victim == c),
                        TimesHit = combatEntries.Count(ent => ent!.Attacker == c && ent!.Victim == cc),
                        KnocksOver = stunEntries.Count(ent => ent!.Attacker == cc 
                            && ent!.Victim == c 
                            && ent.StunType == StunEntry.KnocksSenseless),
                        TimesStunned = stunEntries.Count(ent => ent!.Victim == cc
                            && (ent.StunType == StunEntry.Regains || ent.StunType == StunEntry.BadMove)),
                        TimesThrown = uniqueEntries.Count(ent => ent!.Attacker == c
                            && ent!.Victim == cc
                            && ent.ActionType == UniqueActionEntry.Throws),
                        BiggestHit = combatEntries.Where(ent => ent!.Attacker == cc && ent!.Victim == c)
                            .MaxBy(ent => GetDamageInt(ent!.HitResult)).FirstOrDefault()?.Log ?? "",
                }).ToList()
                );

            return data;
        }

        public IEnumerable<LogEntry> ParseCombatLog()
        {
            IList<LogEntry> result = new List<LogEntry>();

            string[] combatLines = _LogPost.CombatString.Trim().Split('\n');
            for (int i = 0; i < combatLines.Length; i++)
            {
                string line = combatLines[i].Trim();
                Console.WriteLine(line);
                if (line.StartsWith("Logging started"))
                {
                    SetCurrentTimeFromLoggingStarted(line);
                    continue;
                }

                result.Add(ParseCombatLog(line,i, _LogPost.Name));
            }
            return result;
        }

        private void SetCurrentTimeFromLoggingStarted(string line)
        {
            Match match = Regex.Match(line, "[0-9]{4}-[0-9]{2}-[0-9]{2}");
            DateTime.TryParse(match.Value, out DateTime currentTime);
            _CurrentTime = currentTime;
        }

        private LogEntry ParseCombatLog(string v, int lineNumber, string characterName)
        {
            var log = CleanDateAndTimeFromLogLine(v, lineNumber);
            var handlers = new List<ILogEntryHandler>() { 
                new TargetsEntryHandler(),
                new TryToEntryHandler(),
                new CombatEntryHandler(),
                new CastSpellEntryHandler(),
                new GlanceEntryHandler(),
                new FrenzyEntryHandler(),
                new ParryEntryHandler(),
                new StunEntryHandler(),
                new GenericInfoEntryHandler(),
                new MissEntryHandler(),
                new FocusEntryHandler(),
                new ShelterEntryHandler(),
                new UniqueActionEntryHandler(),
                new TauntEntryHandler(),
                new ArmorEffectEntryHandler(),
            };

            foreach (var handler in handlers)
            {
                LogEntry logEntry = handler.Handle(log, characterName);
                if (logEntry != null)
                {
                    return logEntry;
                }
            }

            return new LogEntry
            {
                Log = log.cleanedLog,
                LogTime = log.datetime
            };
        }

        private (string cleanedLog, DateTime datetime) CleanDateAndTimeFromLogLine(string logLine, int lineNumber)
        {
            string[] logSegments = logLine.Split(']');
            switch (logSegments.Length)
            {
                case 1:
                    DateTime dateTime = _CurrentTime == null ? DateTime.UtcNow.AddSeconds(lineNumber) : _CurrentTime.AddSeconds(lineNumber);
                    return (CleanLogLine(logLine), dateTime);
                case 2:
                    string v = logSegments[0].Replace("[", String.Empty);
                    TimeSpan.TryParse(v, out TimeSpan timeSpan);
                    if (_LastTime.HasValue)
                    {
                        if (timeSpan < new TimeSpan(15, 0, 0) && _LastTime.Value.TimeOfDay > new TimeSpan(15, 0, 0))
                        {
                            _CurrentTime = _CurrentTime.AddDays(1);
                        }
                    }
                    DateTime dateTime1 = _CurrentTime + timeSpan;
                    _LastTime = dateTime1;
                    return (CleanLogLine(logSegments[1]), dateTime1);
                case 3:
                    string dateSegment1 = logSegments[0].Replace("[", String.Empty);
                    DateTime.TryParse(dateSegment1, out DateTime dateTime3);
                    string timeSegment = logSegments[1].Replace("[", String.Empty);
                    TimeSpan.TryParse(timeSegment, out TimeSpan timeSpan3);
                    return (CleanLogLine(logSegments[2]), dateTime3 + timeSpan3);
                default:
                    throw new ArgumentOutOfRangeException($"Logline contains to many segments: {logLine}");
            }
        }

        private string CleanLogLine(string logline)
        {
            string v1 = logline.Trim();
            return v1.Remove(v1.Length - 1).Trim();
        }

    }
}
