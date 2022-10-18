namespace ZgnWebApi.Core.Utilities.Helpers
{
    public class RequestSaveByGridModel<T>
    {
        public List<T> changes { get; set; }
        public string action { get; set; }
    }
}
