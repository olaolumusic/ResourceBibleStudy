using ResourceBibleStudy.Core.Dto;
using ResourceBibleStudy.Core.Util;
using ResourceBibleStudy.Helpers;
using ResourceBibleStudy.Repository.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ResourceBibleStudy.Controllers
{
    public class HomeController : BaseController
    {
        private readonly BibleRepository _bibleRepository;
        static Bible BibleContent { get; set; }

        public HomeController()
        {
            _bibleRepository = new BibleRepository();
        }

        /// <summary>
        /// Bible Index
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        // GET: /Administrator/ BibleReading/
        public ActionResult Index(BibleConstants.BibleVersions translation = BibleConstants.BibleVersions.Msg)
        {
            BibleContent = _bibleRepository.GetBible(translation);

            ViewBag.BibleContent = BibleContent;

            return View();
        }


        /// <summary>
        /// Content
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="bookId"></param>
        /// <param name="bookChapter"></param>
        /// <returns></returns>
        [ActionName("content")]
        public string GetSelectedBible(int pageNumber = 1, int bookId = 1, int chapterId = 1)
        {
            var readingResult = new StringBuilder();

            var model = _bibleRepository.GetBible();

            if (pageNumber >= 5 && pageNumber <= 10 && bookId == 1 && chapterId == 1)
            {
                pageNumber -= 4;
                var queryHelper = QueryHelper.GetPagingRowNumber(pageNumber, 11);
                var portion = model.Books.Where(x => x.Id >= queryHelper.RowStart && x.Id <= queryHelper.RowEnd);
                readingResult.Append(RenderPartialViewToString("_BookTableOfContent", portion));
            }
            else if (pageNumber > 10)
            {
                pageNumber -= 10;
                var book = model.Books.FirstOrDefault(x => x.Id == bookId);

                if (book != null)
                {
                    var chapter = book.BookChapter
                        .FirstOrDefault(x => x.ChapterId == chapterId);

                    var queryHelper = QueryHelper.GetPagingRowNumber(pageNumber, 5);

                    if (chapter != null)
                    {
                        var verses = chapter.ChapterVerses
                            .Where(x => x.Id <= queryHelper.RowEnd && x.Id >= queryHelper.RowStart)
                            .ToList();
                        if (verses == null || verses.Count == 0)
                        {
                            chapterId += 1;
                            queryHelper = QueryHelper.GetPagingRowNumber(1, 5);
                            chapter = book.BookChapter
                               .FirstOrDefault(x => x.ChapterId == chapterId);
                            verses = chapter.ChapterVerses
                          .Where(x => x.Id <= queryHelper.RowEnd && x.Id >= queryHelper.RowStart)
                          .ToList();
                        }
                        ViewBag.BookName = book.BookName;
                        ViewBag.Chapter = chapterId;
                        readingResult.Append(RenderPartialViewToString("_BookContent", verses));
                    }
                }
            } 
            return readingResult.ToString();

        }

        /// <summary>
        /// Select Bible
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="bookChapter"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [ActionName("search_book")]
        public string GetSelectedBook(int bookId = 1, int bookChapter = 0, int pageNumber = 1)
        {
            var readingResult = new StringBuilder();
            var firstTwelve = new List<Verse>();

            var paginInfo = QueryHelper.GetPagingRowNumber(pageNumber, 7);

            var selectedBook = _bibleRepository.GetBible().Books.FirstOrDefault(x => x.Id == bookId);
            ViewBag.Chapter = bookChapter;

            if (pageNumber == 1 && bookChapter == 0)
            {
                ViewBag.BookName = selectedBook?.BookName;

                readingResult.Append(RenderPartialViewToString("_ChapterTableOfContent", selectedBook));
            }
            else
            {
                ViewBag.BookName = selectedBook?.BookName;

                var chapter = selectedBook?.BookChapter.FirstOrDefault(x => x.ChapterId == bookChapter);
                firstTwelve.AddRange(chapter?.ChapterVerses.Where(x => x.Id >= paginInfo.RowStart && x.Id < paginInfo.RowEnd));
                readingResult.Append(RenderPartialViewToString("_BookContent", firstTwelve));
            }

            return readingResult.ToString();
        }

        /// <summary>
        /// Book By Id
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public ActionResult Book(int bookId = 1)
        {
            ViewBag.Book = _bibleRepository.GetBible().Books.FirstOrDefault(x => x.Id == bookId);

            return View();
        }

        /// <summary>
        /// Select Translation
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="bookId"></param> 
        /// <param name="bibleVersion"></param>
        /// <returns></returns>
        [ActionName("select_translation")]
        public JsonResult SelectTranslation(int chapterId = 1, int bookId = 1, BibleConstants.BibleVersions bibleVersion = BibleConstants.BibleVersions.Msg)
        {

            var bible = _bibleRepository.GetBible(bibleVersion);
            var book = bible.Books.Find(x => x.Id == bookId);

            var chapter = book?.BookChapter.FirstOrDefault(x => x.ChapterId == chapterId);

            var readingContent = chapter?.ChapterVerses.Aggregate("", (current, verse) =>
              current + $"<p>{verse.Id}: {verse.VerseText} <p/>");

            var readingTitle = book?.BookName + " \nChapter: " + chapter?.ChapterId;

            return Json(new { readingTitle, chapterId, bookId, translation = bible.Name, shortName = bible.ShortName, readingContent, status = true },
                JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Goto Next Previous
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="bookId"></param>
        /// <param name="direction"></param>
        /// <param name="bibleVersion"></param>
        /// <returns></returns>
        [ActionName("next_previous")]
        public JsonResult GotoNextPrevious(int chapterId = 1, int bookId = 1, string direction = "", BibleConstants.BibleVersions bibleVersion = BibleConstants.BibleVersions.Msg)
        {
            var bible = _bibleRepository.GetBible(bibleVersion);
            if (direction == "previous")
            {
                if (chapterId == 1 && bookId != 1)
                {
                    bookId--;
                    var gotoLastChapter = bible.Books.Last(x => x.Id == bookId);
                    chapterId = gotoLastChapter.BookChapter.Last().ChapterId;
                }
                else
                {
                    if (bookId == 1)
                    {
                        bookId = 66;
                        var gotoLastChapter = bible.Books.Last(x => x.Id == bookId);
                        chapterId = gotoLastChapter.BookChapter.Last().ChapterId;
                    }
                    else
                    {
                        chapterId--;
                    }
                }
            }
            else
            {

                var checkLastChapterReached = bible.Books.FirstOrDefault(x => x.Id == bookId);

                if (checkLastChapterReached != null && chapterId == checkLastChapterReached.BookChapter.Last().ChapterId)
                {
                    if (bookId == 66)
                    {
                        bookId = 1;
                    }
                    else
                    {
                        bookId++;
                    }
                    chapterId = 1;
                }
                else
                {
                    chapterId++;
                }
            }

            var book = bible.Books.FirstOrDefault(x => x.Id == bookId);

            var chapter = book?.BookChapter.FirstOrDefault(x => x.ChapterId == chapterId);

            var readingContent = chapter?.ChapterVerses.Aggregate("", (current, verse) =>
              current + $"<p>{verse.Id}: {verse.VerseText} <p/>");

            var readingTitle = book?.BookName + " \nChapter: " + chapter?.ChapterId;

            return Json(new { readingTitle, chapterId, bookId, readingContent, status = true }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public ActionResult Chapter(int chapterId = 1, int bookId = 1)
        {
            var bible = _bibleRepository.GetBible();
            var book = bible.Books.FirstOrDefault(x => x.Id == bookId);

            var chapter = book?.BookChapter.FirstOrDefault(x => x.ChapterId == chapterId);

            ViewBag.ReadingContent = chapter?.ChapterVerses.Aggregate("", (current, verse) =>
              current + $"<p>{verse.Id}: {verse.VerseText} <p/>");

            ViewBag.ReadingTitle = book?.BookName + " \nChapter: " + chapter?.ChapterId;

            ViewBag.ChapterId = chapterId;
            ViewBag.BookId = bookId;
            ViewBag.Translation = new Translation
            {
                Abbreviation = bible.ShortName,
                Name = bible.Name
            };

            //Translations
            ViewBag.Translations = GetTranslations();

            return View();
        }

        /// <summary>
        /// Select Daily Reading
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [ActionName("bible_portion")]
        public JsonResult GetSelectedDailyReading(int pageNumber = 1)
        {
            string readingResult;

            var model = _bibleRepository.GetBible();
            switch (pageNumber)
            {
                case 4:
                    readingResult = RenderPartialViewToString("_BookTableOfContent", model);
                    break;

                default:
                    readingResult = RenderPartialViewToString("_BookContent", model);
                    break;
            }

            return Json(new { BookContent = readingResult, status = true });

        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="search"></param>
        /// <param name="category"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [ActionName("search")]
        [HttpPost]
        public JsonResult Search(string search, string category, int? bookId)
        {
            IEnumerable<Book> bookSearchResult = null;
            var bible = _bibleRepository.GetBible();

            if (category == BibleConstants.BibleCategory.Bible.ToString())
            {
                try
                {
                    bookSearchResult = string.IsNullOrEmpty(search) ?
                        bible.Books : bible.Books.Where(x => x.BookName.InvariantStartsWith(search));


                }
                catch (Exception exception)
                {
                    //_appLogRepository.Log(exception);
                }
                return Json(new
                {
                    searchResult = RenderPartialViewToString("_SearchBibleAjax", bookSearchResult),
                    status = true
                });
            }

            var book = bible.Books.FirstOrDefault(x => x.Id == bookId);

            var chapterSearchResult = !string.IsNullOrEmpty(search) ?
                book?.BookChapter?.Where(x => x.ChapterId == Convert.ToInt16(search)) :
                book?.BookChapter;

            ViewBag.BookName = book?.BookName;
            ViewBag.BookId = book?.Id;

            return Json(new
            {
                searchResult = RenderPartialViewToString("_SearchChapterAjax", chapterSearchResult),
                status = true
            });


        }
        public ActionResult BibleAnimated() {
            return View();
        }
    }
}
