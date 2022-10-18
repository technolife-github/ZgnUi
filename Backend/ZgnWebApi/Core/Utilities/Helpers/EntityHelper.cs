using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Entities.DTOs;

namespace ZgnWebApi.Core.Utilities.Helpers
{
    public static class EntityHelper
    {
        public static IColumn Copy(IColumn source, IColumn target)
        {
            if (target == null)
            {
                return source;
            }
            var sourceProperties = source.GetType().GetProperties();
            var targetProperties = target.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var targetProperty = targetProperties.SingleOrDefault(x =>
                    sourceProperty.Name.ToLower() == x.Name.ToLower() && sourceProperty.PropertyType == x.PropertyType);
                if (targetProperty == null)
                    continue;
                var value = sourceProperty.GetValue(source);
                if (sourceProperty.PropertyType == typeof(List<OptionDataModel>))
                {
                    targetProperty.SetValue(target, value);
                    continue;
                }
                if (value == null)
                    continue;
                if (sourceProperty.Name == "Id")
                    continue;
                if (typeof(IColumn).IsAssignableFrom(sourceProperty.PropertyType))
                {
                    value = EntityHelper.Copy((IColumn)sourceProperty.GetValue(source), (IColumn)targetProperty.GetValue(target));
                }
                targetProperty.SetValue(target, value);

            }

            return target;
        }
    }




}
