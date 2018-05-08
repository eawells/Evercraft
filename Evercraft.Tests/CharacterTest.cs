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
        public void CharacterCanBeGivenAName(){
            Character character = new Character();
            character.name = "Kingdom Death Monster";
            string name = character.name;
            Assert.AreEqual("Kingdom Death Monster", name);
        }
    }
}