namespace BlazorApp.Shared
{
    public static class CombatStrings
    {
        public static string[] OtherActions =
        {
          "ducks low and hits",
          "knocks",
          "strongly pierces",
          "stabs",
          "punches holes in",
          "engraves",
          "fools and cuts",
          "assaults",
          "doubly slashes",
          "pounds",
          "squishes",
          "bonks",
          "devastates",
          "whips",
          "stomps",
          "restructures",
          "stings",
          "graces",
          "cuts open",
          "mauls",
          "gong-gongs",
          "dissolves",
          "strings",
          "punctures",
          "penetrates",
          "dissects",
          "paints",
          "cuts",
          "redecorates",
          "jams",
          "hurts",
          "bowls",
          "lunges and cuts",
          "doubly grinds",
          "spruces",
          "impacts",
          "numbs",
          "opens up",
          "pierces",
          "hits",
          "bites",
          "breathes",
          "burns",
          "poisons",
          "claws",
          "headbutts",
          "kicks",
          "squeezes",
          "tailwhips",
          "wingbuffs"
        };

        public static string[] ActorActions =
        {
            "duck low and hit",
            "knock",
            "strongly pierce",
            "stab",
            "punch holes in",
            "engrave",
            "fool and cut",
            "assault",
            "doubly slash",
            "pound",
            "squish",
            "bonk",
            "devastate",
            "whip",
            "stomp",
            "restructure",
            "sting",
            "grace",
            "cut open",
            "maul",
            "gong-gong",
            "dissolve",
            "string",
            "puncture",
            "penetrate",
            "dissect",
            "paint",
            "cut",
            "redecorate",
            "jam",
            "hurt",
            "bowl",
            "lunge and cut",
            "doubly grind",
            "spruce",
            "impact",
            "numb",
            "open up",
            "pierce",
            "hit",
            "bite",
            "breathe",
            "burn",
            "poison",
            "claw",
            "headbutt",
            "kick",
            "squeeze",
            "tailwhip",
            "wingbuff"
        };

        public static string[] Strength =
        {
            "unnoticeably",
            "very lightly",
            "lightly",
            "pretty hard",
            "hard",
            "very hard",
            "extremely hard",
            "deadly hard"
        };

        public static string[] WoundLocations =
        {
            "body",
            "head",
            "torso",
            "left arm",
            "right arm",
            "left upper arm",
            "right upper arm",
            "left thigh",
            "right thigh",
            "left underarm",
            "right underarm",
            "left calf",
            "right calf",
            "left hand",
            "right hand",
            "left foot",
            "right foot",
            "neck",
            "left eye",
            "right eye",
            "center eye",
            "chest",
            "top of the back",
            "stomach",
            "lower back",
            "crotch",
            "left shoulder",
            "right shoulder",
            "second head",
            "face",
            "left leg",
            "right leg",
            "hip",
            "baseOfNose",
            "legs",
            "mandibles",
            "eyes",
            "left foreleg",
            "right foreleg",
            "right foreleg ",
            "left hindleg",
            "right hindleg",
            "left leg",
            "right leg",
            "upper body",
            "center body",
            "lower body",
            "tail",
            "nostrils",
            "thigh of the left foreleg",
            "thigh of the right foreleg",
            "thigh of the left hindleg",
            "thigh of the right hindleg",
            "calf of the left foreleg",
            "calf of the right foreleg",
            "calf of the left hindleg",
            "calf of the right hindleg",
            "left hoof",
            "right hoof",
            "left head",
            "right head",
            "left foreclaw",
            "right foreclaw",
            "left hindclaw",
            "right hindclaw",
            "left wing",
            "right wing",
            "left paw",
            "right paw",
            "left wingtip",
            "right wingtip",
            "left claw",
            "right claw",
            "claws",
            "beak"
        };
        
        public static string[] Damage =
        {
            "tickle",
            "slap",
            "irritate",
            "hurt",
            "harm",
            "damage"
        };

        public static string OtherPattern = $"(.*?) ({string.Join("|", OtherActions)}) (.*?) " +
            $"({string.Join("|", Strength)}) " +
            $"in the ({string.Join("|", WoundLocations)}) " +
            $"and ({string.Join("|", Damage)})s it";

        public static string ActorPattern = $"(You) ({string.Join("|", ActorActions)}) (.*?) " +
            $"({string.Join("|", Strength)}) " +
            $"in the ({string.Join("|", WoundLocations)}) " +
            $"and ({string.Join("|", Damage)}) it";

        public static string ActorGlancePattern = $"Your attack glances off (.*?)'s armour";

        public static string ActorGlancePattern2 = $"(.*?) takes no real damage from the hit to the ({string.Join("|", WoundLocations)})";

        public static string OtherGlancePattern = $"The attack to the ({string.Join("|", WoundLocations)}) glances off your armour";

        public static string OtherGlancePattern2 = $"You take no real damage from the blow to the ({string.Join("|", WoundLocations)})";
    }
}
