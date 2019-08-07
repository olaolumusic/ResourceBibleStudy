using System.ComponentModel;

namespace ResourceBibleStudy.Core.Util
{
    public class BibleConstants
    {
        #region --- Bible Versions ---

        /*English: King James Version (kjv)
        English: KJV Easy Read (akjv)
        English: American Standard Version (asv)
        English: Basic English Bible (basic english)
        English: Darby (darby)
        English: Young's Literal Translation (ylt)
        English: World English Bible (web)
        English: Webster's Bible (wb)
        English: Douay Rheims (douayrheims)
        English: Weymouth NT (weymouth)
        */

        public enum BibleVersions
        {

            [Description("The Message Bible")]
            Msg,

            [Description("New King James Translation")]
            Nkjv,

            [Description("New Living Translation")]
            Nlt,

            [Description("New International Version")]
            Niv,

            [Description("American Standard Version")]
            Asv,

            [Description("Basic English Bible")]
            BasicEnglish,

            [Description("Young's Literal Translation")]
            Ylt,

            [Description("Webster's Bible")]
            Web
        }

        #endregion
        public enum BibleCategory
        {
            [Description("Bible")]
            Bible = 0,

            [Description("Chapter")]
            Chapter
        }

        #region -- Templates --
        public const string DailyScriptureTemplate = "DailyReadingTemplate.html";

        public const string MailBody = "{mailbody}";
        public const string Salutation = "{salutation}";
        public const string Title = "{title}";
        #endregion
    }

}
