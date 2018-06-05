using System.Collections.Generic;

namespace ResourceBibleStudy.Core.Dto
{
    public class Bible
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Version { get; set; }
        public List<Book> Books { get; set; }
    } 
}
