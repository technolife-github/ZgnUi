using System.Text.RegularExpressions;
using ZgnWebApi.Core.Entities.DTOs;
using ZgnWebApi.Core.Utilities.IoC;
using static ZgnWebApi.Integrations.BlueBotics.Models.BlueBotics.Responses;

namespace ZgnWebApi.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool ContainsAnyOrEmpty(this int[] pages, int[] arg)
        {
            if (pages is null)
            {
                return true;
            }
            if (arg is null)
            {
                return true;
            }
            if (pages.Length == 0 || arg.Length == 0)
            {
                return true;
            }
            return pages.Any(x => arg.Contains(x));
        }
        public static bool IsAssignableTo(this Type type, Type assignableType)
        {
            return assignableType.IsAssignableFrom(type);
        }

        public static bool IsAssignableTo<TAssignable>(this Type type)
        {
            return IsAssignableTo(type, typeof(TAssignable));
        }
        public static T ChangeType<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }
        public static string ToMaskedString(this string value)
        {
            var pattern = "^(/d{8})(/d{4})(/d{4})(/d{4})(/d{12})$";
            var regExp = new Regex(pattern);
            return regExp.Replace(value, "$1-$2-$3-$4-$5");
        }
        public static string ToSqlType(this string value)
        {
            return value
                .Replace("Int32", "int")
                .Replace("Int64", "bigint")
                .Replace("Int16", "smallint")
                .Replace("Int", "int")
                .Replace("DateTime", "datetime")
                .Replace("Boolean", "bit")
                .Replace("String", "nvarchar(max)");
        }
        public static string ToSqlRow(this string value)
        {
            return value
                .Replace(" ", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace(".", "")
                .Replace("-", "")
                .Replace("_", "")
                .Replace("/", "")
                .Replace("*", "")
                .Replace("Ç", "C")
                .Replace("Ş", "S")
                .Replace("ş", "s")
                .Replace("İ", "I")
                .Replace("ı", "i")
                .Replace("ü", "u")
                .Replace("Ü", "U")
                .Replace("Ö", "O")
                .Replace("ö", "o")
                .Replace("ğ", "g")
                .Replace("Ğ", "G");
        }
        public static string ToPascalCase(this string original)
        {
            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }
        public static List<KeyValueResult> InitializeMission(this List<KeyValueResult> keyValues, Mission mission)
        {
            keyValues.Add(new KeyValueResult("Id",mission.Missionid));
            keyValues.Add(new KeyValueResult("State",mission.State));
            keyValues.Add(new KeyValueResult("Navigationstate",mission.Navigationstate));
            keyValues.Add(new KeyValueResult("Transportstate",mission.Transportstate));
            keyValues.Add(new KeyValueResult("Fromnode",mission.Fromnode));
            keyValues.Add(new KeyValueResult("Tonode",mission.Tonode));
            keyValues.Add(new KeyValueResult("Isloaded",mission.Isloaded));
            keyValues.Add(new KeyValueResult("Payload",mission.Payload));
            keyValues.Add(new KeyValueResult("Priority",mission.Priority));
            keyValues.Add(new KeyValueResult("Assignedto",mission.Assignedto));
            keyValues.Add(new KeyValueResult("Deadline",mission.Deadline));
            keyValues.Add(new KeyValueResult("Missiontype",mission.Missiontype));
            keyValues.Add(new KeyValueResult("Groupid",mission.Groupid));
            keyValues.Add(new KeyValueResult("Istoday",mission.Istoday));
            keyValues.Add(new KeyValueResult("Schedulerstate",mission.Schedulerstate));
            keyValues.Add(new KeyValueResult("Askedforcancellation",mission.Askedforcancellation));
            return keyValues;
        }
    }
}
