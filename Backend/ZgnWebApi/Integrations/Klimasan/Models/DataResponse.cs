namespace ZgnWebApi.Integrations.Klimasan.Models
{
    public class DataResponse<T>
    {
        public T? Data { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
    }
}
