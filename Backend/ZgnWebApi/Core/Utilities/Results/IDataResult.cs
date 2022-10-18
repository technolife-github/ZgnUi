namespace ZgnWebApi.Core.Utilities.Results
{
    public interface IDataResult<T> : ISingleResult
    {
        T Data { get; }
    }
}
