namespace ZgnWebApi.Core.Utilities.Results
{
    public interface IErrorGridResult
    {
        string status { get; }
        string message { get; }
    }
}
