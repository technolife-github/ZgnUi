using Newtonsoft.Json.Linq;

namespace ZgnWebApi.Core.Utilities.Filters
{
    public interface IPageableFilter
    {
        public Pagination? Pagination { get; set; }
        public JArray? Sort { get; set; }
        public string? Key { get; set; }
    }
}
