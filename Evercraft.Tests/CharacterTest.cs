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
    }
}