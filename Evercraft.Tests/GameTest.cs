using NUnit.Framework;
using Evercraft;

namespace Test
{
    public class GameTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GameHasName()
        {
            Game game = new Game();
            string name = game.getName();
            Assert.AreEqual("Kingdom Death Monster",name);
        }
    }
}