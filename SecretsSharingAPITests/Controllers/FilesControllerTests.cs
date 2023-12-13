using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretsSharingAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharingAPI.Controllers.Tests
{
    [TestClass()]
    public class FilesControllerTests
    {
        [TestMethod()]
        public void GenerateFileCode_returns9charCode()
        {
            // Arrange
            int expectedLen = 9;
            // Act
            string code = FilesController.GenerateFileCode();
            int actualLen = code.Length;
            //Assert
            Assert.AreEqual(expectedLen, actualLen);
        }
    }
}