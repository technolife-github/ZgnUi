namespace ZgnWebApi.Core.Utilities.Results
{
    public class SuccessResult : Result
    {
        public SuccessResult(object id, string message) : base(id, true, message)
        {

        }
        public SuccessResult(string message) : base(true, message)
        {

        }

        public SuccessResult() : base(true)
        {

        }
    }
}
