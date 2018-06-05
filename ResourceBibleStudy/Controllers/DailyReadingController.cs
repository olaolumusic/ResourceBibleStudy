using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResourceBibleStudy.Core.Dto;
using ResourceBibleStudy.Repository.Dao;

namespace ResourceBibleStudy.Controllers
{ 
    public class DailyReadingController : BaseController
    {
        private Tuple<string, string> ChapterFromAndTo { get; set; }
        static DailyScriptures CurrentScriptures { get; set; }
        static DateTime CurrentDate { get; set; }
        static string ReadingTitle { get; set; }
        static string ReadingContent { get; set; }

        private readonly BibleRepository _dailyReadingRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DailyReadingController()
        {
            _dailyReadingRepository = BibleRepository.GetInstance();
        }
        //
        // GET: /Administrator/ DailyReading/
        public ActionResult Index()
        {
            CurrentDate = DateTime.Now;

            var timeOfTheDay = DateTime.Now.Hour;
            CurrentScriptures = _dailyReadingRepository.GetDailyScriptures();
            if (timeOfTheDay < 12)
            {
                ViewBag.TimeOfTheDay = 1;
                GetReading(CurrentScriptures.FirstReading, 1, CurrentDate);
            }
            else if (timeOfTheDay < 17)
            {
                ViewBag.TimeOfTheDay = 2;
                GetReading(CurrentScriptures.SecondReading, 2, CurrentDate);
            }
            else
            {

                ViewBag.TimeOfTheDay = 3;
                GetReading(CurrentScriptures.ThirdReading, 3, CurrentDate);
            }
            ViewBag.ReadingContent = ReadingContent ?? string.Empty;
            ViewBag.ReadingTitle = ReadingTitle ?? string.Empty;
            return View();
        }

        // GET: /Administrator/Analytics/Summary/Id

        /// <summary>
        /// Get Reading Plan by date
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <returns></returns>
        [ActionName("daily_reading")]
        public JsonResult GetSelectedDailyReading(DateTime selectedDate)
        {
            CurrentScriptures = _dailyReadingRepository.GetDailyScriptures(selectedDate.DayOfYear - 1);
            CurrentDate = selectedDate;
            var readingResult = GetReading(CurrentScriptures.FirstReading, 1, CurrentDate);

            return Json(new { readingTitle = readingResult.Item1, currentDate = selectedDate.ToString("dd MMM yyyy"), readingContent = readingResult.Item2, status = true }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeOfTheDay"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        [ActionName("next_previous")]
        public JsonResult GotoNextPrevious(int timeOfTheDay = 1, string direction = "")
        {
            if (direction == "previous")
            {
                if (timeOfTheDay == 1)
                {
                    timeOfTheDay = 3;
                }
                else
                {
                    timeOfTheDay--;
                }
            }
            else
            {
                if (timeOfTheDay == 3)
                {
                    timeOfTheDay = 1;
                }
                else
                {
                    timeOfTheDay++;
                }
            }

            CurrentScriptures = _dailyReadingRepository.GetDailyScriptures(CurrentDate.DayOfYear - 1);

            Tuple<string, string> readingResult = null;
            switch (timeOfTheDay)
            {
                case 1:
                    readingResult = GetReading(CurrentScriptures.FirstReading, timeOfTheDay, CurrentDate);
                    break;
                case 2:
                    readingResult = GetReading(CurrentScriptures.SecondReading, timeOfTheDay, CurrentDate);
                    break;
                default:

                    readingResult = GetReading(CurrentScriptures.ThirdReading, timeOfTheDay, CurrentDate);
                    break;
            }
            return Json(new { readingTitle = readingResult.Item1, timeOfTheDay, currentDate = CurrentDate.ToString("dd MMM yyyy"), readingContent = readingResult.Item2, status = true }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Get Readong
        /// </summary>
        /// <param name="reading"></param>
        /// <param name="timeOfTheDay"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public Tuple<string, string> GetReading(string reading, int timeOfTheDay = 1, DateTime datetime = new DateTime())
        {
            switch (timeOfTheDay)
            {
                case 1:
                    ReadingTitle = "1st - ";
                    break;
                case 2:
                    ReadingTitle = "2nd - ";
                    break;

                default:
                    ReadingTitle = "3rd - ";
                    break;
            }
            if (!string.IsNullOrEmpty(reading))
            {
                var bookNameAndChapter = reading.Split('.');
                if (bookNameAndChapter[1].Contains("-"))
                {
                    var fromAndTo = bookNameAndChapter[1].Split('-');
                    ChapterFromAndTo = Tuple.Create(fromAndTo[0], fromAndTo[1]);
                }
                else
                {
                    ChapterFromAndTo = Tuple.Create(bookNameAndChapter[1], bookNameAndChapter[1]);
                }
                ReadingTitle += bookNameAndChapter[0];

                ReadingTitle += "\nChapter: " + bookNameAndChapter[1];

                var chapter = _dailyReadingRepository.SearchScriptures(bookNameAndChapter[0], Convert.ToInt16(ChapterFromAndTo.Item1), Convert.ToInt16(ChapterFromAndTo.Item1));
                ReadingContent = "";
                foreach (var verse in chapter.ChapterVerses)
                {
                    ReadingContent += string.Format("<p>{0}: {1} <p/>", verse.Id, verse.VerseText);
                }

            }
            return Tuple.Create(ReadingTitle, ReadingContent);

        }
    }
}