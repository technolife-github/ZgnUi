using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace ZgnWebApi.Core.Utilities.Filters
{
    public class PageableFilter<T> : IPageableFilter<T>
    {
        public Expression<Func<T, bool>>? Filter { get; set; }
        public Pagination? Pagination { get; set; }
        public JArray? Sort { get; set; }
        public string? Key { get; set; }
        public PageableFilter()
        {
        }
        public PageableFilter(Pagination? pagination)
        {
            Pagination = pagination;
            Filter = null;
            Key = "";
            Sort = new JArray();
        }
        public PageableFilter(Expression<Func<T, bool>>? filter)
        {
            Filter = filter;
            Pagination = new Pagination(0, -1);
            Key = filter?.ToString() ?? "";
            Sort = new JArray();
        }
        public PageableFilter(Expression<Func<T, bool>>? filter, Pagination? pagination)
        {
            Filter = filter;
            Pagination = pagination;
            Key = filter?.ToString() ?? "";
            Sort = new JArray();
        }
    }
}
