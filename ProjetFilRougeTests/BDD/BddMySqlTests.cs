using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetFilRouge.BDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFilRouge.BDD.Tests
{
    [TestClass()]
    public class BddMySqlTests
    {
        private readonly BddMySql bddMySql;

        public BddMySqlTests()
        {
            // Initialise une nouvelle instance de BddMySql pour chaque test
            bddMySql = new BddMySql();
        }

        [TestMethod]
        public void Login_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            string username = "testUser";
            string password = "testPassword";

            // Act
            bool result = bddMySql.Login(username, password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Register_NewUser_ReturnsTrue()
        {
            // Arrange
            string username = "newUser";
            string password = "newPassword";

            // Act
            bool result = bddMySql.Register(username, password);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
