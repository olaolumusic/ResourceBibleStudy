using System.Collections.Generic;

namespace ResourceBibleStudy.Core.Dto
{
    public class Book
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public List<Chapter> BookChapter { get; set; }
    }

}
