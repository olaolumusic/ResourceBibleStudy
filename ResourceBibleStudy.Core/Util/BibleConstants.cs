namespace ResourceBibleStudy.Core.Util
{
    public class BibleConstants
    {
        #region --- Bible Versions ---

        /*English: King James Version (kjv)
        English: KJV Easy Read (akjv)
        English: American Standard Version (asv)
        English: Basic English Bible (basicenglish)
        English: Darby (darby)
        English: Young's Literal Translation (ylt)
        English: World English Bible (web)
        English: Webster's Bible (wb)
        English: Douay Rheims (douayrheims)
        English: Weymouth NT (weymouth)
        */

        public enum BibleVersions
        {
            Akjv = 1,
            Asv = 2,
            BasicEnglish = 3,
            Darby4,
            Ylt = 5,
            Web = 6,
            Wb = 7,
            Douayrheims = 8,
            Weymouth = 9,
            Tmsg = 10,
        }

        #endregion


        #region -- Templates --
        public const string DailyScriptureTemplate = "DailyReadingTemplate.html";

        public const string MailBody = "{mailbody}";
        public const string Salutation = "{salutation}";
        public const string Title = "{title}";
        #endregion
    }

}
