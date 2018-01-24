using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNotes.Controllers;
using System.Web;
using System.Web.Mvc;

namespace WebNotesTests.Controllers
{
    [TestClass]
    public class NotesControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            NotesController controller = new NotesController();

            // Act
            RedirectToRouteResult result = controller.Index() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
