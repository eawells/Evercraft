using System;
using System.Collections.Generic;

namespace Evercraft
{
    public class Character
    {
        public string name { get; set; }
        public Alignments alignment { get; set; }

        public int armor { get; set; }
        public int hitPoints { get; set; }

        public int strength { get; set; }

        public Character()
        {
            this.armor = 10;
            this.hitPoints = 5;
            this.strength = 10;

        }
        public bool Attack(IDie die, Character attackedCharacter)
        {
            var rollTotal = die.GetRoll();
            var modifier = AbilitiesScores.AbilityScore[this.strength];

            if(rollTotal == 20)
            {
                attackedCharacter.hitPoints -= Math.Max(1,(2 + modifier * 2));
                return true;
            }

            rollTotal += modifier;

            if(rollTotal >= attackedCharacter.armor)
            { //die roll //adjust per abilities //attack
                attackedCharacter.hitPoints -= Math.Max(1,(1 + modifier));
                return true;
            }
            return false;
        }

        public bool IsDead()
        {
            return hitPoints <= 0;
        }
    }

    public enum Alignments
    {
        GOOD,
        EVIL,
        NEUTRAL
    }
}