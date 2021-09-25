using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class LogEntry
    {
        public string Log { get; set; }

        public DateTime LogTime { get; set; }
    }

    public class CombatEntry : LogEntry
    {
        public string Attacker { get; set; }

        public string Victim { get; set; }

        public string HitType { get; set; }

        public string HitStrength { get; set; }

        public string HitResult { get; set; }

        public string HitLocation { get; set; }
    }

    public class TryToEntry : LogEntry
    {
        public string Attacker { get; set; }

        public string Victim { get; set; }

        public string HitType { get; set; }
    }
}
