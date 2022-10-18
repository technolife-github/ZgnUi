using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace ZgnWebApi.Core.Utilities.Helpers
{
    public class ExpressionGetModel<T>
    {
        public Expression<Func<T, bool>> Filter { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public JArray Sort { get; set; }
    }




}
