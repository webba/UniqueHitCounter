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

                result.Add(ParseCombatLog(line,i));
            }
            return result;
        }

        private void SetCurrentTimeFromLoggingStarted(string line)
        {
            Match match = Regex.Match(line, "[0-9]{4}-[0-9]{2}-[0-9]{2}");
            DateTime.TryParse(match.Value, out DateTime currentTime);
            _CurrentTime = currentTime;
        }

        private LogEntry ParseCombatLog(string v, int lineNumber)
        {
            var log = CleanDateAndTimeFromLogLine(v, lineNumber);
            var handlers = new List<ILogEntryHandler>() { 
                new TryToEntryHandler(),
                new CombatEntryHandler(),
            };

            foreach (var handler in handlers)
            {
                LogEntry logEntry = handler.Handle(log);
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
                    return (logLine, dateTime);
                case 2:
                    string v = logSegments[0].Replace("[", String.Empty);
                    TimeSpan.TryParse(v, out TimeSpan timeSpan);
                    return (logSegments[1].Trim(), _CurrentTime + timeSpan);
                case 3:
                    string dateSegment = logSegments[0].Replace("[", String.Empty);
                    DateTime.TryParse(dateSegment, out DateTime dateTime3);
                    string timeSegment = logSegments[1].Replace("[", String.Empty);
                    TimeSpan.TryParse(timeSegment, out TimeSpan timeSpan3);
                    return (logSegments[2].Trim(), dateTime3 + timeSpan3);
                default:
                    throw new ArgumentOutOfRangeException($"Logline contains to many segments: {logLine}");
            }
        }
    }
}
