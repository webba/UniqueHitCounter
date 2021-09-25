using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorApp.Shared.Entry
{
    public class CastSpellEntry : LogEntry
    {
        public string Caster { get; set; }
        public string Spell { get; set; }
        public string Target { get; set; }
        public bool IsStart { get; set; }
        public string SpellResult { get; set; }

        public CastSpellEntry()
        {

        }

        public CastSpellEntry((string cleanedLog, DateTime datetime) log, string caster, string spell, string target, bool isStart, string spellResult) : base(log)
        {
            Caster = caster;
            Spell = spell;
            Target = target;
            IsStart = isStart;
            SpellResult = spellResult;
        }
    }

    public class CastSpellEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            string spell = null;
            string caster = null;
            string target = null;
            bool isStart = false;
            string spellResult = null;

            if (log.cleanedLog.Contains("starts to cast"))
            {
                isStart = true;
                if (log.cleanedLog.Contains(" on "))
                {
                    Match match = Regex.Match(log.cleanedLog, @"(.*) starts to cast '?(.*?)'? on (.*)");
                    caster = match.Groups[1].Value;
                    spell = match.Groups[2].Value;
                    target = match.Groups[3].Value;

                }
                else
                {
                    Match match = Regex.Match(log.cleanedLog, @"(.*) starts to cast '?(.*?)'?$");
                    caster = match.Groups[1].Value;
                    spell = match.Groups[2].Value;

                }
            } else if (log.cleanedLog.Contains(" casts "))
            {
                if(log.cleanedLog.Contains("casts a "))
                {
                    Match match = Regex.Match(log.cleanedLog, @"(.*) casts a '?(.*?)'? at (.*?)( but glances off the armour, dealing no damage)?$");
                    caster = match.Groups[1].Value;
                    spell = match.Groups[2].Value;
                    target = match.Groups[3].Value;
                    spellResult = match.Groups[4].Value;
                }
                else if (log.cleanedLog.Contains(" on "))
                {
                    Match match = Regex.Match(log.cleanedLog, @"(.*) casts '?(.*?)'? on (.*)");
                    caster = match.Groups[1].Value;
                    spell = match.Groups[2].Value;
                    target = match.Groups[3].Value;

                }
                else
                {
                    Match match = Regex.Match(log.cleanedLog, @"(.*) casts '?(.*?)'?$");
                    caster = match.Groups[1].Value;
                    spell = match.Groups[2].Value;

                }
            }

            if (caster != null && spell != null)
            {
                if (caster.Equals("you", StringComparison.OrdinalIgnoreCase))
                {
                    caster = characterName;
                }

                if (target != null)
                {
                    if (target.Equals("you", StringComparison.OrdinalIgnoreCase))
                    {
                        target = characterName;
                    }
                    else if (target.Equals("himself", StringComparison.OrdinalIgnoreCase)|| target.Equals("herself", StringComparison.OrdinalIgnoreCase))
                    {
                        target = caster;
                    }
                }
                return new CastSpellEntry(log, caster, spell, target, isStart, spellResult);

            }

            return null;
        }
    }
}
