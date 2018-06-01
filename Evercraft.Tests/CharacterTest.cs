using NUnit.Framework;
using Evercraft;
using Moq;
using System.Linq;

namespace Test
{
    public class CharacterTest
    {
        Character character;
        [SetUp]
        public void Setup()
        {
            character = new Character();
        }

        [Test]
        public void CharacterCanBeGivenAName()
        {
            character.name = "Kingdom Death Monster";
            string name = character.name;
            Assert.AreEqual("Kingdom Death Monster", name);
        }

        [Test, Sequential]
        public void CharacterHasAlignment(
            [Values(Alignments.GOOD,Alignments.EVIL,Alignments.NEUTRAL)] Alignments TAlignment)
        {
            character.alignment = TAlignment;
            Alignments actualAlignment = character.alignment;
            Assert.AreEqual(TAlignment, actualAlignment);
        }

        [Test]
        public void CharacterHasArmorClassDefaultedTo10()
        {
            Assert.AreEqual(10, character.armor);
        }

        [Test]
        public void CharacterHasHitPointsDefaultedTo5()
        {
            Assert.AreEqual(5, character.hitPoints);
        }

        [Test]
        public void CharacterCanSuccessfullyAttackAnotherCharacter()
        {
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(11);
            Assert.IsTrue(character.Attack(mockedDie.Object, attackedCharacter));
        }

        [Test]
        public void CharacterAttackFailsIfDieRollIsLessThanOpponentsArmor()
        {
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(5);
            Assert.IsFalse(character.Attack(mockedDie.Object, attackedCharacter));
        }

        [Test]
        public void CharacterAttackSucceedsIfDieRollIsEqualToOpponentsArmor()
        {
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(10);
            Assert.IsTrue(character.Attack(mockedDie.Object, attackedCharacter));
        }

        [Test]
        public void WhenCharacterIsSuccessfullyAttackedHitPointsDecreaseByOne()
        {
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(10);
            character.Attack(mockedDie.Object, attackedCharacter);
            Assert.AreEqual(4,attackedCharacter.hitPoints);
        }

        [Test]
        public void WhenCharacterIsUnsuccessfullyAttackedHitPointsStayTheSame()
        {
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(4);
            character.Attack(mockedDie.Object, attackedCharacter);
            Assert.AreEqual(5, attackedCharacter.hitPoints);
        }

        [Test]
        public void WhenCharacterAttacksWithRollOf20HitPointDamageIsDoubled()
        {
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(20);
            character.Attack(mockedDie.Object, attackedCharacter);
            Assert.AreEqual(3, attackedCharacter.hitPoints);
        }

        [Test]
        public void WhenAttackedCharacterHas0HitPointsTheyAreDead()
        {
            Character attackedCharacter = new Character();
            attackedCharacter.hitPoints = 1;
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(12);
            character.Attack(mockedDie.Object, attackedCharacter);
            Assert.IsTrue(attackedCharacter.IsDead());
        }

        [Test]
        public void WhenCharactersHitPointsGreaterThan0IsDeadReturnsFalse()
        {
            Character attackedCharacter = new Character();
            Assert.IsFalse(attackedCharacter.IsDead());
        }

        [Test]
        public void WhenAttackedCharacterHasNegative1HitPointsTheyAreDead()
        {
            Character attackedCharacter = new Character();
            attackedCharacter.hitPoints = 1;
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(20);
            character.Attack(mockedDie.Object, attackedCharacter);
            Assert.IsTrue(attackedCharacter.IsDead());
        }

        [Test]
        public void GivenStrengthOf12WhenCharacterAttacksAttackIsSuccessfulWithRollOf9()
        {
            Character attacker = new Character(12,10,10);
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(9);
            Assert.IsTrue(attacker.Attack(mockedDie.Object, attackedCharacter));
        }

        [Test]
        public void GivenStrengthOf12WhenCharacterAttacksSuccessfullyDamageAdjustsForStrengthModifier()
        {
            Character attacker = new Character(12,10,10);
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(9);

            attacker.Attack(mockedDie.Object, attackedCharacter);

            Assert.AreEqual(3, attackedCharacter.hitPoints);
        }

        [Test]
        public void GivenAStrengthOf12AndACriticalHitTotalDamageDealtIsDoubled()
        {
            Character attacker = new Character(12,10,10);
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(20);

            attacker.Attack(mockedDie.Object, attackedCharacter);

            Assert.AreEqual(1, attackedCharacter.hitPoints);
        }

        [Test]
        public void GivenAStrengthOf1AndAHitMinimumDamageIs1()
        {
            Character attacker = new Character(1,10,10);
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(15);

            attacker.Attack(mockedDie.Object, attackedCharacter);

            Assert.AreEqual(4, attackedCharacter.hitPoints);
        }

        [Test]
        public void GivenAStrengthOf1AndACritialHitMinimumDamageIs1()
        {
            Character attacker = new Character(1,10,10);
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(20);

            attacker.Attack(mockedDie.Object, attackedCharacter);

            Assert.AreEqual(4, attackedCharacter.hitPoints);
        }

        [Test]
        public void GivenAttackedCharacterWithDexterity12AndRollOf10AttackFails()
        {
            Character attackedCharacter = new Character(10,12,10);
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(10);

            Assert.IsFalse(character.Attack(mockedDie.Object, attackedCharacter));
        }

        [Test]
        public void GivenAttackedCharacterWithDexterity1AndRollOf5AttackSucceeds()
        {
            Character attackedCharacter = new Character(10,1,10);
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(5);

            Assert.IsTrue(character.Attack(mockedDie.Object, attackedCharacter));
        }

        [Test]
        public void WhenCharacterIsCreatedWithConstitutionScore15HitPointsAdjustedTo7()
        {
            Character customCharacter = new Character(10, 10, 15);
            var expectedHitPoints = 7;
            Assert.AreEqual(expectedHitPoints, customCharacter.hitPoints);
        }

        [Test]
        public void WhenCharacterIsCreatedWithConstitutionScore1HitPointsIs1()
        {
            Character customCharacter = new Character(10, 10, 1);
            var expectedHitPoints = 1;
            Assert.AreEqual(expectedHitPoints, customCharacter.hitPoints);
        }

        [Test]
        public void WhenCharacterIsCreatedWithDexterityScore1HitPointsAdjustedTo5()
        {
            Character customCharacter = new Character(10, 1, 10);
            var expectedArmor = 5;
            Assert.AreEqual(expectedArmor, customCharacter.armor);
        }

        [Test]
        public void GivenAttackedCustomCharacterWithDexterity12AndRollOf11AttackSucceeds()
        {
            Character attackedCharacter = new Character(10, 12, 10);
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(11);

            Assert.IsTrue(character.Attack(mockedDie.Object, attackedCharacter));
        }

        [Test]
        public void WhenACharacterAttacksSuccessfullyTheyGain10XP()
        {
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(12);

            character.Attack(mockedDie.Object, attackedCharacter);
            var expectedXP = 10;
            Assert.AreEqual(expectedXP, character.XP);
        }

        [Test]
        public void WhenACharacterIsCreatedTheyAreLevelOne()
        {
            int expectedLevel = 1;
            Assert.AreEqual(expectedLevel,character.level);
        }

        [Test]
        public void WhenACharacterAttacksSuccessfully100TimesTheirLevelIs2()
        {
            AttackNTimesSuccessfully(100, character);

            int expectedLevel = 2;
            Assert.AreEqual(expectedLevel,character.level);
        }

        [Test]
        public void WhenACharacterAttacksSuccessfully99TimesTheirLevelIsStill1()
        {
            AttackNTimesSuccessfully(99, character);
            int expectedLevel = 1;
            Assert.AreEqual(expectedLevel,character.level);

        }

        public void AttackNTimesSuccessfully(int n, Character attacker)
        {
            var attackedCharacter = new Character(1, 1, 1);

            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(18);

            foreach (int i in Enumerable.Range(1, n))
            {
                attacker.Attack(mockedDie.Object, attackedCharacter);
            }
        }
    }
}