namespace ZgnWebApi.Core.Utilities.Results
{
    public class ErrorGridResult : IErrorGridResult
    {

        public string status { get; }
        public string message { get; }
        public ErrorGridResult(string status, string message)
        {
            this.status = status;
            this.message = message;
        }

        public ErrorGridResult(string message)
        {
            status = "error";
            this.message = message;
        }

    }
}
