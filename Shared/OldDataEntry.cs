using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BlazorApp.Shared
{
    public class OldDataEntry
    {
        public string Name { get; set; }

        public int Hits { get; set; }
        
        public int Hits_Taken { get; set; }

        public int Stuns { get; set; }

        public int Throws { get; set; }

        public string Log { get; set; }

        public int Damage { get; set; }

        public int Knocks { get; set; }
    }
}
