namespace ZgnWebApi.Core.Utilities.Results
{
    public interface ISuccessGridResult<T>
    {
        List<T> records { get; }
        long total { get; }
        string status { get; }
        string message { get; }
    }
}
