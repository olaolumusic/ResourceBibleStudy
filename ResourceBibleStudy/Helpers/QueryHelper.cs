namespace ResourceBibleStudy.Helpers
{
    internal class QueryHelper
    {

        public static PagingInfo GetPagingRowNumber(int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
                pageIndex = 1;

            pageIndex = (pageIndex - 1) * pageSize;
            var page = pageIndex + pageSize;

            return new PagingInfo(pageIndex, page);
        }
    }

    sealed class PagingInfo
    {
        public PagingInfo(int rowStart, int rowEnd)
        {
            RowStart = rowStart;
            RowEnd = rowEnd;
        }

        public int RowEnd { get; private set; }
        public int RowStart { get; private set; }
    }
}
