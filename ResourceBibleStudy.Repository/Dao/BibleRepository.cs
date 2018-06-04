using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Newtonsoft.Json;
using ResourceBibleStudy.Core.Dto;
using ResourceBibleStudy.Core.Util;

namespace ResourceBibleStudy.Repository.Dao
{
    public class BibleRepository
    {

        public static Bible SessionBible { get; set; }

        public static List<DailyScriptures> SessionDailyScriptures { get; set; }

        static BibleRepository _dailyReadingRepository;

        public static BibleRepository GetInstance()
        {
            return _dailyReadingRepository ?? (_dailyReadingRepository = new BibleRepository());
        }

        public Chapter SearchScriptures(int bookId, int chapterNumber = 1, int verse = 1, int from = 1, int to = 1, BibleConstants.BibleVersions bibleVersion = BibleConstants.BibleVersions.Tmsg)
        {
            var bible = GetBible();

            var dailyChapter = new Chapter();

            var dailyBook = new Book();

            foreach (var book in bible.Books.Where(book => book.Id == bookId))
            {
                dailyBook = book;
                break;
            }
            foreach (var chapter in dailyBook.BookChapter.Where(chapter => chapter.ChapterId == chapterNumber))
            {
                dailyChapter = chapter;
                break;
            }


            return dailyChapter;
        }


        public Chapter SearchScriptures(string bookName, int chapterNumber = 1, int verse = 1, int from = 1, int to = 1, BibleConstants.BibleVersions bibleVersion = BibleConstants.BibleVersions.Tmsg)
        {
            var bible = GetBible();

            var dailyBook = new Book();
            foreach (var book in bible.Books.Where(book => book.BookName.ToLower().StartsWith(bookName.Trim().ToLower())))
            {
                dailyBook = book;
                break;
            }
            var dailyChapter = new Chapter();
            foreach (var chapter in dailyBook.BookChapter.Where(chapter => chapter.ChapterId == chapterNumber))
            {
                dailyChapter = chapter;
                break;
            }


            return dailyChapter;
        }


        public Bible GetBible(BibleConstants.BibleVersions bibleVersion = BibleConstants.BibleVersions.Tmsg, string bibleFullName = "The Message Bible")
        {
            if (SessionBible != null)
            {
                return SessionBible;
            }

            var bibleFilepath = HostingEnvironment.MapPath(string.Format("~/App_Data/Books/{0}", "MSG.json.txt")).ToString();

            var bibleStreamToString = File.ReadAllText(bibleFilepath);

            return SessionBible = BibleParser(bibleStreamToString, bibleFullName, null, bibleVersion);

        }

        public static List<DailyScriptures> GetScriptures()
        {

            if (SessionDailyScriptures != null)
            {
                return SessionDailyScriptures;
            }

            var filepath = HostingEnvironment.MapPath(string.Format("~/App_Data/Books/{0}", "DailyScriptures.txt")).ToString();

            var readingPlanTemplate = File.ReadAllText(filepath);

            return SessionDailyScriptures = JsonConvert.DeserializeObject<List<DailyScriptures>>(readingPlanTemplate);
        }

        public Bible BibleParser(string theEntireBibleJsonString, string bibleName = "Unknown", string shortName = "Unknown", BibleConstants.BibleVersions bibleVersion = BibleConstants.BibleVersions.Tmsg)
        {
            var tempBible = new Bible { Books = new List<Book>(), Name = bibleName, ShortName = shortName, Version = bibleVersion.ToString() };

            //All books after reading from File 
            var bibleBooks = JsonConvert.DeserializeObject<Bible>(theEntireBibleJsonString);

            //Each Book in the Bible
            tempBible.Books = bibleBooks.Books.OrderBy(x => x.Id).ToList();

            return tempBible;
        }

        public DailyScriptures GetDailyScriptures(int dayOfTheYear = 0)
        {
            var getDailyScriptures = GetScriptures();

            var bibleJson = JsonConvert.SerializeObject(getDailyScriptures);

            return dayOfTheYear == 0 ? getDailyScriptures[DateTime.Now.DayOfYear] : getDailyScriptures[dayOfTheYear];
        }
    }

}
