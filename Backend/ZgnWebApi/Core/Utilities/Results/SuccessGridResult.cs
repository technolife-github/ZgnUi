namespace ZgnWebApi.Core.Utilities.Results
{
    public class SuccessGridResult<T> : ISuccessGridResult<T>
    {

        public List<T> records { get; }
        public long total { get; }
        public string status { get; }
        public string message { get; }
        public SuccessGridResult(List<T> records, int limit, long totalCount)
        {

            this.records = records;
            this.total = records.Count < limit - 1 ? -1 : totalCount;
            this.status = "success";
            this.message = "Get data successfully";
        }
    }
}
