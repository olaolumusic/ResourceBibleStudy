using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ResourceBibleStudy.Tests.BibleTest
{
    [TestClass]
    public class DailyScripturesTest
    { 

        [TestInitialize]
        public void Init()
        { 
        }

        [TestMethod]
        public void AppLogInitTest()
        {
            //Arrange
            var except = new Exception("Log Test");

            //Act 

            //Assert
            Assert.IsNotNull(except);

        }

        [TestMethod]
        public void GetDeviceIpTest()
        {
            //Arrange

            //Act

            //Assert

        }
    }
}
