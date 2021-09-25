using BlazorApp.Shared;
using BlazorApp.Shared.Entry;

using System;
using System.Collections.Generic;
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
                new TryToEntryHandler(),
                new CombatEntryHandler(),
                new CastSpellEntryHandler(),
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
