using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResourceBibleStudy.Core.Dto;
using ResourceBibleStudy.Repository.Dao;

namespace ResourceBibleStudy.Tests.BibleTest
{
    [TestClass]
    public class BibleStudyTest
    {
        Bible _bible;
        BibleRepository _dailyReadingRepository;

        [TestInitialize]
        public void Init()
        {
            _dailyReadingRepository = new BibleRepository();
            _bible = GetBible();
        }

        [TestMethod]
        public void GetBibleStudyTest()
        {
            //Arrange 

            //Act
            var totalScriptures = _bible.Books.FirstOrDefault();

            //Assert
            Assert.IsNotNull(_bible);
        }

        [TestMethod]
        public void GetDeviceIpTest()
        {
            //Arrange

            //Act

            //Assert

        }

        public Bible GetBible(string bibleVersion = "TMSG")
        {
            if (_bible != null)
            {
                return _bible;
            }
            var bibleFilepath = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\MSG.json";
            var bibleStreamToString = File.ReadAllText(bibleFilepath); 

            return _bible = _dailyReadingRepository.BibleParser(bibleStreamToString);

        }

    }
}
