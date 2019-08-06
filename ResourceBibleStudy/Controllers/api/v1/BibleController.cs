using System;
using System.ComponentModel;
using System.Web.Http;
using ResourceBibleStudy.Core.Util;
using ResourceBibleStudy.Filters;
using ResourceBibleStudy.Repository.Dao;

namespace ResourceBibleStudy.Controllers.api.v1
{    /// <summary>
    /// Resource Custom Bible
    /// </summary>
    [ApiExceptionFilter]
    [RoutePrefix("api/v1/bible")]
    public class BibleController : ApiController
    {
        private readonly BibleRepository _bibleRepository;
        /// <summary>
        /// Dictionary Constructor
        /// </summary>
        public BibleController()
        {
            _bibleRepository = new BibleRepository(); 
        }


        /// <exception cref="InvalidOperationException"></exception>
        [Description(description: "Get Bible Passages using search criteria eg. book name, chapter and verse")]
        [Route("")]
        public IHttpActionResult Get(int bookId, int chapterNumber = 1, int verse = 1, int from = 1, int to = 1, BibleConstants.BibleVersions bibleVersion = BibleConstants.BibleVersions.Msg)
        {
            try
            {
                var result = _bibleRepository.SearchScriptures(bookId, chapterNumber, verse, from, to, bibleVersion);
                return Ok(result);
            }
            catch (Exception exception)
            { 
                throw new InvalidOperationException("Unable to fetch bible passage", exception);
            }
        }

        /// <exception cref="InvalidOperationException"></exception>
        [Description(description: "Get Daily Reading Plan using day of the year")]
        [Route("dailyreading")]
        public IHttpActionResult GetDailyReading(int dayOfTheYear)
        {
            try
            {
                var result = _bibleRepository.GetDailyScriptures(dayOfTheYear);
                return Ok(result);
            }
            catch (Exception exception)
            { 
                throw new InvalidOperationException("Unable to fetch bible passage", exception);
            }
        }

    }
}