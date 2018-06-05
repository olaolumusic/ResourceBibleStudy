using System.Collections.Generic;

namespace ResourceBibleStudy.Core.Dto
{

    public class Chapter
    {
        public int BookId { get; set; }
        public int ChapterId { get; set; }
        public List<Verse> ChapterVerses { get; set; }
    }
}
