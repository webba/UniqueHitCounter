using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorApp.Shared.Entry
{
    public class StartToCastEntry : LogEntry
    {
        public string Caster { get; set; }
        public string Spell { get; set; }
        public string Target { get; set; }

        public StartToCastEntry()
        {

        }

        public StartToCastEntry((string cleanedLog, DateTime datetime) log, string caster, string spell, string target) : base(log)
        {
            Caster = caster;
            Spell = spell;
            Target = target;
        }
    }

    public class StartToCastEntryHandler : ILogEntryHandler
    {
        public LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName)
        {
            if (log.cleanedLog.Contains("starts to cast"))
            {
                string caster = null;
                string spell = null;
                string target = null;
                if (log.cleanedLog.Contains(" on "))
                {
                    Match match = Regex.Match(log.cleanedLog, @"(.*) starts to cast '?(.*?)'? on (.*)");
                    caster = match.Groups[0].Value;
                    spell = match.Groups[1].Value;
                    target = match.Groups[1].Value;

                }
                else
                {
                    Match match = Regex.Match(log.cleanedLog, @"(.*) starts to cast '?(.*?)'?$");
                    caster = match.Groups[0].Value;
                    spell = match.Groups[1].Value;

                }
                if (caster != null && spell != null)
                {
                    if(caster.ToLower() == "you")
                    {
                        caster = characterName;
                    }

                    if (target != null)
                    {
                        if (target.ToLower() == "you")
                        {
                            target = characterName;
                        }
                        else if (target == "himself" || target == "herself")
                        {
                            target = caster;
                        }
                    }

                    return new StartToCastEntry(log, caster, spell, target);
                }
            }
            return null;
        }
    }
}
