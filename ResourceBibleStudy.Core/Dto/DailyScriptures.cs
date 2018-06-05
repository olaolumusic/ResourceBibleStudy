namespace ResourceBibleStudy.Core.Dto
{
    public class DailyScriptures
    {
        public long Id { get; set; }
        public string FirstReading { get; set; }
        public string SecondReading { get; set; }
        public string ThirdReading { get; set; }
        public string BookVersion { get; set; }
        public int DayOfTheYear { get; set; }
    }
}
