using System.Linq.Expressions;

namespace ZgnWebApi.Core.Utilities.Filters
{
    public interface IPageableFilter<T> : IPageableFilter
    {
        public Expression<Func<T, bool>>? Filter { get; set; }
    }
}
