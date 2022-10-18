namespace ZgnWebApi.Core.Utilities.Filters
{
    public class Pagination
    {
        public int Limit { get; }
        public int Offset { get; }
        public Pagination()
        {
            Limit = 0;
            Offset = 0;
        }
        public Pagination(int offset, int limit)
        {
            Limit = limit;
            Offset = offset;
        }
        public override string ToString()
        {
            return $"{Limit},{Offset}";
        }
    }
}
