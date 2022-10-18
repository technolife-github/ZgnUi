using System.Security.Claims;

namespace ZgnWebApi.Core.Utilities.Helpers
{
    public class RequestSaveByExcelModel<T>
    {
        public ClaimsPrincipal User { get; set; }
        public List<T> Records { get; set; }
        public string Cmd { get; set; }
        public string Name { get; set; }
        public object Recid { get; set; }
        public bool OverWrite { get; set; }
    }




}
