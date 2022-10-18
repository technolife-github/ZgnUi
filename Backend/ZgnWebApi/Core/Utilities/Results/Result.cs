namespace ZgnWebApi.Core.Utilities.Results
{
    public class Result : ISingleResult
    {

        public Result(object id, bool success, string message) : this(success, message)
        {
            Id = id;
        }
        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }

        public Result(bool success)
        {
            Success = success;
        }

        public object Id { get; }
        public bool Success { get; }
        public string Message { get; set; }
    }
}
