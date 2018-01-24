using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNotes.Controllers;
using System.Web.Mvc;
using WebNotesDataBase.Models;

namespace WebNotesTests.Controllers
{
    [TestClass]
    public class UsersControllerTest
    {
        [TestMethod]
        public void Registration()
        {
            // Arrange
            UsersController controller = new UsersController();

            // Act
            ViewResult result = controller.Registration() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Login()
        {
            // Arrange
            UsersController controller = new UsersController();

            // Act
            ViewResult result = controller.Login() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
