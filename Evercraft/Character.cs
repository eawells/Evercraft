using System;

namespace Evercraft
{
    public class Character
    {
        public string name { get; set; }
        public Alignments alignment {get; set;}
    }

    public enum Alignments
    {
        GOOD,
        EVIL,
        NEUTRAL
    }
}