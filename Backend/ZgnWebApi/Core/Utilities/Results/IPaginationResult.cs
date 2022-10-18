namespace ZgnWebApi.Core.Utilities.Results
{
    public interface IPaginationResult<T>
    {
        T Data { get; }
        long TotalCount { get; }
    }
}
