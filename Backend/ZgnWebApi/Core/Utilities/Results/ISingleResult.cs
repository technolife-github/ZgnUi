namespace ZgnWebApi.Core.Utilities.Results
{
    public interface ISingleResult
    {
        object Id { get; }
        bool Success { get; }
        string Message { get; set; }

    }
}
