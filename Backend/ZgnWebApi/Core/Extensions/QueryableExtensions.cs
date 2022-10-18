using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ZgnWebApi.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
        {
            foreach (var item in enumerable)
            {
                await action(item);
            }
        }
        public static IEnumerable<T> AddList<T>(this IEnumerable<T> mainList, IEnumerable<T> addList)
        {
            var list = mainList.ToList();
            foreach (var item in addList)
            {
                list.Add(item);
            }
            return list;
        }
        public static IQueryable<T> SortByData<T>(this IQueryable<T> list, JArray sort) where T : class, new()
        {
            T entity = new();
            if (sort == null) return list;
            foreach (var data in sort)
            {
                foreach (var entityProperty in entity.GetType().GetProperties())
                {
                    if (entityProperty.Name.ToLower()[1..] == data["field"].ToString().ToLower()[1..])
                    {
                        if (data["direction"].ToString() == "desc")
                        {
                            try
                            {
                                list = list.OrderByDescending<T>(entityProperty.Name);
                            }
                            catch (Exception)
                            {
                                list = list.OrderByDescending<T>(entityProperty.Name + ".text");
                            }
                        }
                        else
                        {
                            try
                            {
                                list = list.OrderBy<T>(entityProperty.Name);
                            }
                            catch (Exception)
                            {
                                list = list.OrderBy<T>(entityProperty.Name + ".text");
                            }
                        }
                    }
                }

            }

            return list;
        }
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }
        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo info = type.GetProperty(prop);
                expr = Expression.Property(expr, info);
                type = info.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                              && method.IsGenericMethodDefinition
                              && method.GetGenericArguments().Length == 2
                              && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}
