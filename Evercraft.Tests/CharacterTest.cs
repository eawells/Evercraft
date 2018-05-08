using NUnit.Framework;
using Evercraft;

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
    }
}