using System;

namespace Evercraft
{
    public class Character
    {
        public string name { get; set; }
        public Alignments alignment { get; set; }

        public int armor { get; set; }
        public int hitPoints { get; set; }
        public Character()
        {
            this.armor = 10;
            this.hitPoints = 5;
        }
        public bool Attack(IDie die, Character character)
        {
            if(die.GetRoll() > character.armor)
            {
               return true;
            }
            return false;
        }
    }

    public enum Alignments
    {
        GOOD,
        EVIL,
        NEUTRAL
    }
}