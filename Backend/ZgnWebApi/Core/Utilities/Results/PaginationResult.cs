namespace ZgnWebApi.Core.Utilities.Results
{
    public class PaginationResult<T> : IPaginationResult<T>
    {
        public PaginationResult(T data, long totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }
        public T Data { get; }
        public long TotalCount { get; }
    }
}
