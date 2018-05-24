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

        public int dexterity { get; set; }

        public Character()
        {
            this.armor = 10;
            this.hitPoints = 5;
            this.strength = 10;
            this.dexterity = 10;

        }
        public bool Attack(IDie die, Character attackedCharacter)
        {
            var rollTotal = die.GetRoll();
            var modifier = AbilitiesScores.AbilityScore[this.strength];

            var didHit = CheckHit(rollTotal, modifier, attackedCharacter);

            if (didHit)
            {
                attackedCharacter.hitPoints -= CalculateDamage(rollTotal, modifier);
            }
            return didHit;
        }

        public bool IsDead()
        {
            return hitPoints <= 0;
        }

        private bool CheckHit(int rollTotal, int modifier, Character attackedCharacter)
        {
            return rollTotal == 20 || (rollTotal + modifier) >= attackedCharacter.ModifiedArmor();
        }

        public int ModifiedArmor()
        {
            return this.armor + AbilitiesScores.AbilityScore[this.dexterity];
        }

        private int CalculateDamage(int rollTotal, int modifier)
        {
            var multiplier = rollTotal == 20 ? 2 : 1;
            return Math.Max(1,(multiplier * (1 + modifier)));
        }
    }

    public enum Alignments
    {
        GOOD,
        EVIL,
        NEUTRAL
    }
}