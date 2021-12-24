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
        private DateTime? _CurrentTime = null;
        private DateTime? _LastTime = null;
        private bool _HasFirstDate = false;

        public LogParser(CombatPost post)
        {
            _LogPost = post;
            if(post.StartDate != null)
            {
                _CurrentTime = post.StartDate;
                _HasFirstDate = true;
            }
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

        public IDictionary<string, CreatureCombatResult> ProcessResults(IEnumerable<LogEntry> entries)
        {
            var creatureResults = new Dictionary<string, CreatureCombatResult>();

            foreach (var entry in entries)
            {
                if(entry is CombatEntry cEntry)
                {
                    if(cEntry.Attacker != null && cEntry.Victim != null)
                    {
                        // Handle Attacker
                        if (!creatureResults.ContainsKey(cEntry.Attacker))
                        {
                            creatureResults.Add(cEntry.Attacker, new CreatureCombatResult());
                        }

                        // Poll attacker First/LastTime
                        if(creatureResults[cEntry.Attacker].FirstTime == null || entry.LogTime < creatureResults[cEntry.Attacker].FirstTime)
                        {
                            creatureResults[cEntry.Attacker].FirstTime = entry.LogTime;
                        }
                        if (creatureResults[cEntry.Attacker].LastTime == null || entry.LogTime > creatureResults[cEntry.Attacker].LastTime)
                        {
                            creatureResults[cEntry.Attacker].LastTime = entry.LogTime;
                        }

                        // Increment Hits
                        creatureResults[cEntry.Attacker].Hits++;

                        // Handle damage
                        int damage = GetDamageInt(cEntry.HitResult);
                        if(damage > creatureResults[cEntry.Attacker].BiggestHitDamage)
                        {
                            creatureResults[cEntry.Attacker].BiggestHitDamage = damage;
                            creatureResults[cEntry.Attacker].BiggestHit = cEntry.Log;
                        }

                        // Handle Hit on victim
                        if (!creatureResults[cEntry.Attacker].Attacks.ContainsKey(cEntry.Victim))
                        {
                            creatureResults[cEntry.Attacker].Attacks.Add(cEntry.Victim,new AttackerVictimCombatResult());
                        }

                        // Increment Hits to victim
                        creatureResults[cEntry.Attacker].Attacks[cEntry.Victim].Hits++;

                        // Handle damage to victim
                        int damageVictim = GetDamageInt(cEntry.HitResult);
                        if (damageVictim > creatureResults[cEntry.Attacker].Attacks[cEntry.Victim].BiggestHitDamage)
                        {
                            creatureResults[cEntry.Attacker].Attacks[cEntry.Victim].BiggestHitDamage = damageVictim;
                            creatureResults[cEntry.Attacker].Attacks[cEntry.Victim].BiggestHit = cEntry.Log;
                        }

                        //Handle Victim
                        if (!creatureResults.ContainsKey(cEntry.Victim))
                        {
                            creatureResults.Add(cEntry.Victim, new CreatureCombatResult());
                        }

                        // Poll victim First/LastTime
                        if (creatureResults[cEntry.Victim].FirstTime == null || entry.LogTime < creatureResults[cEntry.Victim].FirstTime)
                        {
                            creatureResults[cEntry.Victim].FirstTime = entry.LogTime;
                        }
                        if (creatureResults[cEntry.Victim].LastTime == null || entry.LogTime > creatureResults[cEntry.Victim].LastTime)
                        {
                            creatureResults[cEntry.Victim].LastTime = entry.LogTime;
                        }

                        // Increment Hits Recieved
                        creatureResults[cEntry.Victim].TimesHit++;
                    }
                }
                else if(entry is StunEntry stunEntry)
                {
                    // Handle Bad Move/Regains 
                    if ((stunEntry.StunType == StunEntry.Regains || stunEntry.StunType == StunEntry.BadMove) && stunEntry.Victim != null)
                    {
                        // Handle Victim
                        if (!creatureResults.ContainsKey(stunEntry.Victim))
                        {
                            creatureResults.Add(stunEntry.Victim, new CreatureCombatResult());
                        }

                        // Poll victim First/LastTime
                        if (creatureResults[stunEntry.Victim].FirstTime == null || entry.LogTime < creatureResults[stunEntry.Victim].FirstTime)
                        {
                            creatureResults[stunEntry.Victim].FirstTime = entry.LogTime;
                        }
                        if (creatureResults[stunEntry.Victim].LastTime == null || entry.LogTime > creatureResults[stunEntry.Victim].LastTime)
                        {
                            creatureResults[stunEntry.Victim].LastTime = entry.LogTime;
                        }

                        // Increment stuns
                        creatureResults[stunEntry.Victim].TimesStunned++;
                    }
                    // Handle Knocks
                    else if(stunEntry.StunType == StunEntry.KnocksSenseless && stunEntry.Attacker != null && stunEntry.Victim != null)
                    {
                        // Handle Attacker
                        if (!creatureResults.ContainsKey(stunEntry.Attacker))
                        {
                            creatureResults.Add(stunEntry.Attacker, new CreatureCombatResult());
                        }

                        // Poll attacker First/LastTime
                        if (creatureResults[stunEntry.Attacker].FirstTime == null || entry.LogTime < creatureResults[stunEntry.Attacker].FirstTime)
                        {
                            creatureResults[stunEntry.Attacker].FirstTime = entry.LogTime;
                        }
                        if (creatureResults[stunEntry.Attacker].LastTime == null || entry.LogTime > creatureResults[stunEntry.Attacker].LastTime)
                        {
                            creatureResults[stunEntry.Attacker].LastTime = entry.LogTime;
                        }

                        // Increment Knocks over
                        creatureResults[stunEntry.Attacker].KnocksOver++;

                        // Handle Knocked on victim
                        if (!creatureResults[stunEntry.Attacker].Attacks.ContainsKey(stunEntry.Victim))
                        {
                            creatureResults[stunEntry.Attacker].Attacks.Add(stunEntry.Victim, new AttackerVictimCombatResult());
                        }

                        // Poll victim First/LastTime
                        if (creatureResults[stunEntry.Victim].FirstTime == null || entry.LogTime < creatureResults[stunEntry.Victim].FirstTime)
                        {
                            creatureResults[stunEntry.Victim].FirstTime = entry.LogTime;
                        }
                        if (creatureResults[stunEntry.Victim].LastTime == null || entry.LogTime > creatureResults[stunEntry.Victim].LastTime)
                        {
                            creatureResults[stunEntry.Victim].LastTime = entry.LogTime;
                        }

                        // Increment knocks
                        creatureResults[stunEntry.Attacker].Attacks[stunEntry.Victim].KnocksOver++;
                    }
                }
                else if (entry is UniqueActionEntry uEntry)
                {
                    // Handle throws
                    if(uEntry.ActionType == UniqueActionEntry.Throws && uEntry.Attacker != null && uEntry.Victim != null)
                    {
                        // Handle Attacker
                        if (!creatureResults.ContainsKey(uEntry.Attacker))
                        {
                            creatureResults.Add(uEntry.Attacker, new CreatureCombatResult());
                        }

                        // Handle victim
                        if (!creatureResults[uEntry.Attacker].Attacks.ContainsKey(uEntry.Victim))
                        {
                            creatureResults[uEntry.Attacker].Attacks.Add(uEntry.Victim, new AttackerVictimCombatResult());
                        }

                        // Poll attacker First/LastTime
                        if (creatureResults[uEntry.Attacker].FirstTime == null || entry.LogTime < creatureResults[uEntry.Attacker].FirstTime)
                        {
                            creatureResults[uEntry.Attacker].FirstTime = entry.LogTime;
                        }
                        if (creatureResults[uEntry.Attacker].LastTime == null || entry.LogTime > creatureResults[uEntry.Attacker].LastTime)
                        {
                            creatureResults[uEntry.Attacker].LastTime = entry.LogTime;
                        }

                        // Increment Throws
                        creatureResults[uEntry.Attacker].Attacks[uEntry.Victim].Throws++;

                        // Handle Victim
                        if (!creatureResults.ContainsKey(uEntry.Victim))
                        {
                            creatureResults.Add(uEntry.Victim, new CreatureCombatResult());
                        }

                        // Poll victim First/LastTime
                        if (creatureResults[uEntry.Victim].FirstTime == null || entry.LogTime < creatureResults[uEntry.Victim].FirstTime)
                        {
                            creatureResults[uEntry.Victim].FirstTime = entry.LogTime;
                        }
                        if (creatureResults[uEntry.Victim].LastTime == null || entry.LogTime > creatureResults[uEntry.Victim].LastTime)
                        {
                            creatureResults[uEntry.Victim].LastTime = entry.LogTime;
                        }

                        // Increment Times Thrown
                        creatureResults[uEntry.Victim].TimesThrown++;
                    }
                }
            }
            return creatureResults;
        }

        public IEnumerable<LogEntry> ParseCombatLog()
        {
            IList<LogEntry> result = new List<LogEntry>();

            string[] combatLines = _LogPost.CombatString.Trim().Split('\n');
            for (int i = 0; i < combatLines.Length; i++)
            {
                string line = combatLines[i].Trim();
                if (line.StartsWith("Logging started"))
                {
                    SetCurrentTimeFromLoggingStarted(line);
                    if (!_HasFirstDate)
                    {
                        foreach(LogEntry logEntry in result.Reverse())
                        {
                            int backDays = 0;
                            TimeSpan lastTime = new TimeSpan(0, 0, 0);
                            if(logEntry.LogTime.TimeOfDay > lastTime)
                            {
                                backDays++;
                            }

                            lastTime = logEntry.LogTime.TimeOfDay;
                            logEntry.LogTime = _CurrentTime?.AddDays(-1 * backDays).Add(logEntry.LogTime.TimeOfDay) ?? logEntry.LogTime;
                        }
                        _HasFirstDate = true;
                    }
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
                    DateTime dateTime = _CurrentTime ?? DateTime.UtcNow.AddSeconds(lineNumber);
                    return (CleanLogLine(logLine), dateTime);
                case 2:
                    string v = logSegments[0].Replace("[", String.Empty);
                    TimeSpan.TryParse(v, out TimeSpan timeSpan);
                    if (_LastTime.HasValue)
                    {
                        if (timeSpan < new TimeSpan(15, 0, 0) && _LastTime.Value.TimeOfDay > new TimeSpan(15, 0, 0))
                        {
                            _CurrentTime = _CurrentTime?.AddDays(1);
                        }
                    }
                    DateTime dateTime1 = (_CurrentTime ?? DateTime.Today) + timeSpan;
                    _LastTime = dateTime1;
                    return (CleanLogLine(logSegments[1]), dateTime1);
                case 3:
                    string dateSegment1 = logSegments[0].Replace("[", String.Empty);
                    DateTime.TryParse(dateSegment1, out DateTime dateTime3);
                    string timeSegment = logSegments[1].Replace("[", String.Empty);
                    TimeSpan.TryParse(timeSegment, out TimeSpan timeSpan3);
                    _HasFirstDate = true;
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
