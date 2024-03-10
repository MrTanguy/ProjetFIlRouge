using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetFilRouge.JeuDeLaVie;

namespace ProjetFilRouge.JeuDeLaVie.Tests
{
    [TestClass()]
    public class GameTests
    {
        [TestMethod()]
        public void NewState_CreatesNewState_ReturnsNextGrid()
        {
            // Arrange
            int x = 3;
            int y = 3;
            int ruleAlive = 2;
            Game game = new Game(x, y, ruleAlive);

            // Act
            int[,] nextState = game.NewState();

            // Assert
            Assert.IsNotNull(nextState);
            Assert.AreEqual(x, nextState.GetLength(0));
            Assert.AreEqual(y, nextState.GetLength(1));
        }

        [TestMethod()]
        public void SetRuleAlive_ChangesRuleAlive_RuleAliveUpdated()
        {
            // Arrange
            int x = 3;
            int y = 3;
            int ruleAlive = 2;
            Game game = new Game(x, y, ruleAlive);
            int newRuleAlive = 3;

            // Act
            game.SetRuleAlive(newRuleAlive);

            // Assert
            Assert.AreEqual(newRuleAlive, game.GetRuleAlive());
        }
    }
}
