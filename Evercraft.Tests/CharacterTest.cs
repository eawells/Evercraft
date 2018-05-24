using NUnit.Framework;
using Evercraft;
using Moq;

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
            character.strength = 12;
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(9);
            Assert.IsTrue(character.Attack(mockedDie.Object, attackedCharacter));
        }

        [Test]
        public void GivenStrengthOf12WhenCharacterAttacksSuccessfullyDamageAdjustsForStrengthModifier()
        {
            character.strength = 12;
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(9);

            character.Attack(mockedDie.Object, attackedCharacter);

            Assert.AreEqual(3, attackedCharacter.hitPoints);
        }

        [Test]
        public void GivenAStrengthOf12AndACriticalHitTotalDamageDealtIsDoubled()
        {
            character.strength = 12;
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(20);

            character.Attack(mockedDie.Object, attackedCharacter);

            Assert.AreEqual(1, attackedCharacter.hitPoints);
        }

        [Test]
        public void GivenAStrengthOf1AndAHitMinimumDamageIs1()
        {
            character.strength = 1;
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(15);

            character.Attack(mockedDie.Object, attackedCharacter);

            Assert.AreEqual(4, attackedCharacter.hitPoints);
        }

        [Test]
        public void GivenAStrengthOf1AndACritialHitMinimumDamageIs1()
        {
            character.strength = 1;
            Character attackedCharacter = new Character();
            var mockedDie = new Mock<IDie>();
            mockedDie.Setup(die => die.GetRoll()).Returns(20);

            character.Attack(mockedDie.Object, attackedCharacter);

            Assert.AreEqual(4, attackedCharacter.hitPoints);
        }
    }
}