using System;
using System.Collections.Generic;

namespace Evercraft
{
    public class Character
    {
        public string name { get; set; }
        public Alignments alignment { get; set; }

        public int armor { get; }

        public int hitPoints { get; set; }

        public int strength { get; }

        public int dexterity { get; }

        public int constitution { get; }

        public int XP { get; private set; }

        public int level
        {
            get
            {
                double levelIncludingPartialLevelMinusOne = XP/1000;
                return (int) Math.Floor(levelIncludingPartialLevelMinusOne) + 1;
            }
        }

        public Character()
        {
            this.armor = 10;
            this.hitPoints = 5;
            this.strength = 10;
            this.dexterity = 10;
        }

        public Character(int strength, int dexterity, int constitution)
        {
            var adjustedHitPoints = 5 + AbilitiesScores.AbilityScore[constitution];
            this.armor = 10 + AbilitiesScores.AbilityScore[dexterity];
            this.hitPoints = adjustedHitPoints <= 0 ? 1 : adjustedHitPoints;
            this.strength = strength;
            this.dexterity = dexterity;
            this.constitution = constitution;
        }

        public bool Attack(IDie die, Character attackedCharacter)
        {
            var rollTotal = die.GetRoll();
            var modifier = AbilitiesScores.AbilityScore[this.strength];

            var didHit = CheckHit(rollTotal, modifier, attackedCharacter);

            if (didHit)
            {
                attackedCharacter.hitPoints -= CalculateDamage(rollTotal, modifier);
                this.XP += 10;
            }
            return didHit;
        }

        public bool IsDead()
        {
            return hitPoints <= 0;
        }

        private bool CheckHit(int rollTotal, int modifier, Character attackedCharacter)
        {
            return rollTotal == 20 || (rollTotal + modifier) >= attackedCharacter.armor;
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