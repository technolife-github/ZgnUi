namespace ZgnWebApi.Core.Utilities.Results
{
    public interface ISuccessFormResult<T>
    {
        T record { get; }
        string message { get; }
    }
}
