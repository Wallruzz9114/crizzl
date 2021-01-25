namespace Crizzl.Application.Helpers
{
    public class UserFilters
    {
        private const int MaxPageSize = 50;
        private int maxItems = 10;
        public int PageNumber { get; set; } = 1;
        public int ItemsCount
        {
            get => maxItems;
            set { maxItems = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public string Gender { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public string OrderBy { get; set; }
    }
}