using Newtonsoft.Json;
using ResourceBibleStudy.Core.Dto;
using ResourceBibleStudy.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace ResourceBibleStudy.Repository.Dao
{
    public class BibleRepository
    {

        public static List<Bible> SessionBible { get; set; }

        public static List<DailyScriptures> SessionDailyScriptures { get; set; }

        static BibleRepository _dailyReadingRepository;

        public static BibleRepository GetInstance()
        {
            return _dailyReadingRepository ?? (_dailyReadingRepository = new BibleRepository());
        }

        public Chapter SearchScriptures(int bookId, int chapterNumber = 1, int verse = 1, int from = 1, int to = 1, BibleConstants.BibleVersions bibleVersion = BibleConstants.BibleVersions.Msg)
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



        public Chapter SearchScriptures(string bookName, int chapterNumber = 1, int verse = 1, int from = 1, int to = 1, BibleConstants.BibleVersions bibleVersion = BibleConstants.BibleVersions.Msg)
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

        public IEnumerable<Chapter> SearchScripturesByChapters(string bookName, int chapterFrom = 1, int chapterTo = 1, BibleConstants.BibleVersions bibleVersion = BibleConstants.BibleVersions.Msg)
        {
            var bible = GetBible();
            var chapterList = new List<Chapter>();

            var dailyBook = new Book();
            foreach (var book in bible.Books.Where(book => book.BookName.ToLower().StartsWith(bookName.Trim().ToLower())))
            {
                dailyBook = book;
                break;
            }

            foreach (var chapter in dailyBook.BookChapter)
            {
                if (chapter.ChapterId == chapterFrom || chapter.ChapterId == chapterTo)
                {
                    chapterList.Add(chapter);
                }
            }

            return chapterList;
        }


        public Bible GetBible(BibleConstants.BibleVersions bibleVersion = BibleConstants.BibleVersions.Msg, string bibleFullName = "The Message Bible")
        {
            var selectedBible = SessionBible?.Find(x => x.ShortName.Equals(bibleVersion.ToString(), StringComparison.CurrentCultureIgnoreCase));
            if (selectedBible != null)
            {
                return selectedBible;
            }

            var bibleFilepath = HostingEnvironment.MapPath($"~/App_Data/Books/{bibleVersion}.json");

            var bibleStreamToString = File.ReadAllText(bibleFilepath);

            selectedBible = BibleParser(bibleStreamToString);

            if (SessionBible != null)
            {
                SessionBible.Add(selectedBible);
            }
            else
            {
                SessionBible = new List<Bible> { selectedBible };
            }

            return selectedBible;
        }

        public static List<DailyScriptures> GetScriptures()
        {

            if (SessionDailyScriptures != null)
            {
                return SessionDailyScriptures;
            }

            var filepath = HostingEnvironment.MapPath("~/App_Data/Books/DailyScriptures.txt");

            var readingPlanTemplate = File.ReadAllText(filepath);

            return SessionDailyScriptures = JsonConvert.DeserializeObject<List<DailyScriptures>>(readingPlanTemplate);
        }

        public Bible BibleParser(string theEntireBibleJsonString)
        {
            //All books after reading from File 
            var bible = JsonConvert.DeserializeObject<Bible>(theEntireBibleJsonString);

            bible.Books = bible.Books.OrderBy(x => x.Id).ToList();

            return bible;
        }

        public DailyScriptures GetDailyScriptures(int dayOfTheYear = 0)
        {
            var getDailyScriptures = GetScriptures();

            if (DateTime.IsLeapYear(DateTime.Now.Year))
            {
                return dayOfTheYear == 0 ? getDailyScriptures[DateTime.Now.DayOfYear + 1] : getDailyScriptures[dayOfTheYear + 1];
            }

            return dayOfTheYear == 0 ? getDailyScriptures[DateTime.Now.DayOfYear] : getDailyScriptures[dayOfTheYear];
        }
    }

}
