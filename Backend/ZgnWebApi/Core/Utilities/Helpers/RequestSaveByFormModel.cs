namespace ZgnWebApi.Core.Utilities.Helpers
{
    public class RequestSaveByFormModel<T>
    {
        public T Record { get; set; }
        public string Cmd { get; set; }
        public string Name { get; set; }
        public object Recid { get; set; }
    }




}
