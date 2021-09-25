using BlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UniqueHitCounter.Logic
{
    public class LogParser
    {
        private const string _TimePattern = @"\[[0-9]{2}:[0-9]{2}:[0-9]{2}]";
        private readonly CombatPost _LogPost;
        private string? _LogLineWithoutDate = null;
        private DateTime _CurrentTime;

        public LogParser(CombatPost post)
        {
            _LogPost = post;
            _CurrentTime = post.StartDate;
        }

        public IEnumerable<CombatEntry> ParseCombatLog()
        {
            IList<CombatEntry> result = new List<CombatEntry>();

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

        private CombatEntry ParseCombatLog(string v, int lineNumber)
        {
            (string cleanedLog, DateTime datetime) = GetDateTime(v, lineNumber);
            return new CombatEntry
            {
                Log = cleanedLog,
                LogTime = datetime,
            };

        }

        private (string cleanedLog, DateTime datetime) GetDateTime(string logLine, int lineNumber)
        {
            string[] logSegments = logLine.Split(']');

            if(logSegments.Length == 1)
            {
                DateTime dateTime = _CurrentTime == null ? DateTime.UtcNow.AddSeconds(lineNumber) : _CurrentTime.AddSeconds(lineNumber);
                return (logLine, dateTime);
            }
            else if (logSegments.Length == 2)
            {
                string v = logSegments[0].Replace("[", String.Empty);
                TimeSpan.TryParse(v, out TimeSpan timeSpan);
                return (logSegments[1].Trim(), _CurrentTime + timeSpan);
            }
            else
            {
                string dateSegment = logSegments[0].Replace("[", String.Empty);
                DateTime.TryParse(dateSegment, out DateTime dateTime);
                string timeSegment = logSegments[1].Replace("[", String.Empty);
                TimeSpan.TryParse(timeSegment, out TimeSpan timeSpan);
                return (logSegments[2].Trim(), dateTime + timeSpan);
            }
        }
    }
}
