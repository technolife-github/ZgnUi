namespace ZgnWebApi.Core.Utilities.Results
{
    public class SuccessFormResult<T> : ISuccessFormResult<T>
    {

        public T record { get; }
        public string message { get; }
        public SuccessFormResult(T record, string message)
        {
            this.record = record;
            this.message = message;
        }
    }
}
