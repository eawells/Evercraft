using NUnit.Framework;
using Evercraft;

namespace Test
{
    public class CharacterTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CharacterHasName()
        {
            Character character = new Character();
            string name = character.getName();
            Assert.AreEqual("Kingdom Death Monster",name);
        }
    }
}