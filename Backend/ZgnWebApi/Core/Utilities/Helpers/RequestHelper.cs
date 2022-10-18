using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Entities.DTOs;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Filters;
#nullable disable
namespace ZgnWebApi.Core.Utilities.Helpers
{
    public static class RequestHelper
    {
        public static IPageableFilter<T> GetRequestByGridExpression<T>(string? request) where T : class, new()
        {
            if (request is null)
                return new PageableFilter<T>()
                {
                    Key = "",
                    Pagination = new Pagination(0, -1),
                    Filter = null,
                    Sort = new JArray()
                };

            var p = Expression.Parameter(typeof(T), "e");
            dynamic allRequest = JsonConvert.DeserializeObject(request);
            if (!RequestHelper.IsPropertyExist(allRequest, "search"))
            {
                allRequest.search = new JArray();
            }
            if (!RequestHelper.IsPropertyExist(allRequest, "sort"))
            {
                allRequest.sort = new JArray();
            }
            if (!RequestHelper.IsPropertyExist(allRequest, "limit"))
            {
                allRequest.limit = -1;
            }
            if (!RequestHelper.IsPropertyExist(allRequest, "offset"))
            {
                allRequest.offset = 0;
            }
            if (!RequestHelper.IsPropertyExist(allRequest, "searchLogic"))
            {
                allRequest.searchLogic = "AND";
            }
            JArray search = allRequest.search;
            T entity = new();
            string logic = " " + allRequest.searchLogic + " ";
            string exp = "";
            foreach (var data in search)
            {
                exp += RequestHelper.GetExpression(entity.GetType().GetProperties(), data, logic, "e");
            }
            if (exp == "")
            {
                exp = "1==1 and ";
            }
            var lambda = DynamicExpressionParser.ParseLambda(new[] { p }, null, exp[0..^4]);
            return new PageableFilter<T>()
            {
                Key = exp[0..^4],
                Pagination = new Pagination(int.Parse(allRequest.offset.ToString()), int.Parse(allRequest.limit.ToString())),
                Filter = (Expression<Func<T, bool>>)lambda,
                Sort = allRequest.sort
            };
        }
        public static string GetExpression(PropertyInfo[] entityProperties, JToken data, string logic, string key)
        {
            string exp = "";
            foreach (var property in entityProperties)
            {
                try
                {
                    if (property.Name == data["field"].ToString())
                    {
                        string field = data["field"].ToString();
                        var entityProperty = entityProperties.ToList().Where(p => p.Name == field.Replace("View", "")).First();
                        field = field.Replace("View", "");
                        if (entityProperties.ToList().Where(p => p.Name == field + "Id").ToList().Count() > 0)
                        {
                            entityProperty = entityProperties.ToList().Where(p => p.Name == field + "Id").ToList().First();
                            field = field + "Id";
                            data["type"] = entityProperty.PropertyType.Name;
                        }
                        switch (data["operator"].ToString())
                        {
                            case "begins":
                                if (entityProperty.PropertyType == typeof(string))
                                {
                                    exp += "(" + key + "." + field + ".StartsWith(\"" + data["value"] + "\"))" + logic;
                                }
                                else if (entityProperty.PropertyType == typeof(Guid))
                                {
                                    exp += " (" + key + "." + field + ".ToString().StartsWith(\"" + data["value"] + "\"))" + logic;
                                }
                                break;
                            case "ends":
                                //exp += "( " + key + "." + field + " != null AND " + key + "." + field + ".EndsWith(\"" + data["value"] + "\"))" + logic;
                                //exp += " " + key + "." + field + ".EndsWith(\"" + data["value"] + "\")" + logic;
                                if (entityProperty.PropertyType == typeof(string))
                                {
                                    exp += "(" + key + "." + field + ".EndsWith(\"" + data["value"] + "\"))" + logic;
                                }
                                else if (entityProperty.PropertyType == typeof(Guid))
                                {
                                    exp += " " + key + "." + field + ".ToString().EndsWith(\"" + data["value"] + "\"))" + logic;
                                }
                                break;
                            case "contains":
                                if (entityProperty.PropertyType == typeof(string))
                                {
                                    exp += "(" + key + "." + field + ".Contains(\"" + data["value"] + "\"))" + logic;
                                }
                                else if (entityProperty.PropertyType == typeof(Guid))
                                {
                                    exp += " (" + key + "." + field + ".ToString().Contains(\"" + data["value"] + "\"))" + logic;
                                }
                                //exp += "( " + key + "." + field + " != null AND " + key + "." + field + ".Contains(\"" + data["value"] + "\"))" + logic;
                                //exp += " " + key + "." + field + ".Contains(\"" + data["value"] + "\")" + logic;
                                break;
                            case "is":
                                if (data["type"].ToString() == "list")
                                {
                                    if (DoesPropertyExist(data, "text"))
                                    {
                                        exp += " " + key + "." + field + ".id == \"" + JsonConvert.SerializeObject(data["value"]) + "\"" + logic;
                                    }
                                    else
                                    {
                                        exp += " " + key + "." + field + ".text == \"" + data["value"].ToString() + "\"" + logic;
                                    }
                                }
                                else if (
                                    entityProperty.PropertyType == typeof(int) ||
                                    entityProperty.PropertyType == typeof(short) ||
                                    entityProperty.PropertyType == typeof(long) ||
                                    entityProperty.PropertyType == typeof(uint) ||
                                    entityProperty.PropertyType == typeof(ushort) ||
                                    entityProperty.PropertyType == typeof(ulong)
                                    )
                                {
                                    exp += " " + key + "." + field + " == " + data["value"] + logic;
                                }
                                else if (entityProperty.PropertyType == typeof(Guid))
                                {
                                    var guid = data["value"].ToString().Replace("-", "").PadRight(32, '0').ToMaskedString();
                                    exp += " " + key + "." + field + " == \"" + guid + "\"" + logic;
                                }
                                else
                                {
                                    exp += " " + key + "." + field + " == \"" + data["value"] + "\"" + logic;
                                }
                                break;
                            case "between":
                                if (entityProperty.PropertyType == typeof(int))
                                {
                                    exp += " " + key + "." + field + " >= \"" + data["value"][0] + "\"" + logic;
                                    exp += " " + key + "." + field + " <= \"" + data["value"][1] + "\"" + logic;
                                }
                                else if (entityProperty.PropertyType == typeof(DateTime))
                                {
                                    DateTime dateStart = (DateTime)data["value"][0];
                                    DateTime dateEnd = (DateTime)data["value"][1];
                                    exp += "( e." + field + " >= DateTime(" + dateStart.Year + "," + dateStart.Month + "," + dateStart.Day + "," + dateStart.Hour + "," + dateStart.Minute + "," + dateStart.Second + ") AND ";
                                    exp += " " + key + "." + field + " <= DateTime(" + dateEnd.Year + "," + dateEnd.Month + "," + dateEnd.Day + "," + dateEnd.Hour + "," + dateEnd.Minute + "," + dateEnd.Second + "))" + logic;
                                }
                                else if (entityProperty.PropertyType == typeof(float))
                                {
                                    exp += " " + key + "." + field + " >= \"" + (float)data["value"][0] + "\"" + logic;
                                    exp += " " + key + "." + field + " <= \"" + (float)data["value"][1] + "\"" + logic;
                                }
                                break;
                            case "in":
                                if (data["type"].ToString() == "enum")
                                {
                                    if (entityProperty.PropertyType == typeof(Boolean))
                                    {
                                        try
                                        {
                                            var t = " (";
                                            data["svalue"].ToList().ForEach(v =>
                                            {
                                                t += " " + key + "." + field + " ==" +
                                                       JsonConvert.SerializeObject(v.ChangeType<bool>()) +
                                                       " OR ";

                                            });
                                            t += ") ";
                                            exp += " " + t.Replace("OR )", ")") +
                                                   logic;
                                        }
                                        catch (Exception)
                                        { }
                                    }
                                    else if (entityProperty.PropertyType == typeof(List<OptionDataModel>))
                                    {
                                        try
                                        {
                                            var t = " (";
                                            data["svalue"].ToList().ForEach(v =>
                                            {
                                                t += " " + key + "." + field + ".Select(o => o.id).Contains(" +
                                                       JsonConvert.SerializeObject(v.ToString()) +
                                                       ") OR ";

                                            });
                                            t += ") ";
                                            exp += " " + t.Replace("OR )", ")") +
                                                   logic;
                                        }
                                        catch (Exception)
                                        { }
                                    }
                                    //else if (entityProperty.PropertyType == typeof(OptionDataModel))
                                    //{
                                    //    try
                                    //    {
                                    //        exp += " " + key + "." + field + ".id ==\"" + data["value"] + "\"" + logic;
                                    //    }
                                    //    catch (Exception)
                                    //    { }
                                    //}
                                    else if (entityProperty.PropertyType == typeof(string))
                                    {
                                        try
                                        {
                                            var t = " (";
                                            data["svalue"].ToList().ForEach(v =>
                                            {
                                                t += " " + key + "." + field + " ==" +
                                                       JsonConvert.SerializeObject(v.ToString()) +
                                                       " OR ";

                                            });
                                            t += ") ";
                                            exp += " " + t.Replace("OR )", ")") +
                                                   logic;
                                        }
                                        catch (Exception)
                                        { }
                                    }
                                    else if (entityProperty.PropertyType == typeof(int))
                                    {
                                        exp += " " + key + "." + field + " >= \"" + data["value"][0] + "\"" + logic;
                                        exp += " " + key + "." + field + " <= \"" + data["value"][1] + "\"" + logic;
                                    }
                                    else if (entityProperty.PropertyType == typeof(DateTime))
                                    {
                                        DateTime dateStart = (DateTime)data["value"][0];
                                        DateTime dateEnd = (DateTime)data["value"][1];
                                        exp += "( e." + field + " >= DateTime(" + dateStart.Year + "," + dateStart.Month + "," + dateStart.Day + "," + dateStart.Hour + "," + dateStart.Minute + "," + dateStart.Second + ") AND ";
                                        exp += " " + key + "." + field + " <= DateTime(" + dateEnd.Year + "," + dateEnd.Month + "," + dateEnd.Day + "," + dateEnd.Hour + "," + dateEnd.Minute + "," + dateEnd.Second + "))" + logic;
                                    }
                                    else if (entityProperty.PropertyType == typeof(float))
                                    {
                                        exp += " " + key + "." + field + " >= \"" + (float)data["value"][0] + "\"" + logic;
                                        exp += " " + key + "." + field + " <= \"" + (float)data["value"][1] + "\"" + logic;
                                    }
                                    else
                                        exp += " " + key + "." + field + ".Contains(\"" + JsonConvert.SerializeObject(data["value"].ToList()) + "\")" + logic;
                                }

                                break;
                            case "more":
                                if (entityProperty.PropertyType == typeof(int))
                                {
                                    exp += " " + key + "." + field + " >= \"" + (int)data["value"] + "\"" + logic;
                                }
                                else if (entityProperty.PropertyType == typeof(DateTime?) || entityProperty.PropertyType == typeof(DateTime))
                                {
                                    DateTime date = (DateTime)data["value"];
                                    exp += " " + key + "." + field + " >= DateTime(" + date.Year + "," + date.Month + "," + date.Day + "," + date.Hour + "," + date.Minute + "," + date.Second + ")" + logic;
                                }
                                else if (entityProperty.PropertyType == typeof(float))
                                {
                                    exp += " " + key + "." + field + " >= \"" + (float)data["value"] + "\"" + logic;
                                }
                                break;
                            case "less":
                                if (entityProperty.PropertyType == typeof(int))
                                {
                                    exp += " " + key + "." + field + " <= \"" + (float)data["value"] + "\"" + logic;
                                }
                                else if (entityProperty.PropertyType == typeof(DateTime))
                                {
                                    DateTime date = (DateTime)data["value"];
                                    exp += " " + key + "." + field + " <= DateTime(" + date.Year + "," + date.Month + "," + date.Day + "," + date.Hour + "," + date.Minute + "," + date.Second + ")" + logic;
                                }
                                else if (entityProperty.PropertyType == typeof(float))
                                {
                                    exp += " " + key + "." + field + " <= \"" + (float)data["value"] + "\"" + logic;
                                }
                                break;
                            case "<":
                                exp += (data["type"].ToString() == "int" ?
                                        " " + key + "." + field + " < " + (int)data["value"] + "" :
                                        " " + key + "." + field + " < \"" + data["value"] + "\"") + logic;
                                break;
                            case ">":
                                exp += (data["type"].ToString() == "int" ?
                                        " " + key + "." + field + " > " + (int)data["value"] + "" :
                                        " " + key + "." + field + " > \"" + data["value"] + "\"") + logic;
                                break;
                            case "<=":
                                exp += (data["type"].ToString() == "int" ?
                                        " " + key + "." + field + " <= " + (int)data["value"] + "" :
                                        " " + key + "." + field + " <= \"" + data["value"] + "\"") + logic;
                                break;
                            case ">=":
                                exp += (data["type"].ToString() == "int" ?
                                        " " + key + "." + field + " >= " + (int)data["value"] + "" :
                                        " " + key + "." + field + " >= \"" + data["value"] + "\"") + logic;
                                break;
                            default:
                                exp += (data["type"].ToString() == "int" ?
                                        " " + key + "." + field + " == " + (int)data["value"] + "" :
                                        " " + key + "." + field + " == \"" + data["value"] + "\"") + logic;
                                break;

                        }

                    }
                    else if (property.Name == data["field"].ToString().Split('.')[0])
                    {
                        if (typeof(IColumn).IsAssignableFrom(property.PropertyType))
                        {
                            string[] f = data["field"].ToString().Split('.');
                            data["field"] = f[f.Length - 1];
                            string n = key + "." + property.Name;
                            var inExp = RequestHelper.GetExpression(property.PropertyType.GetProperties(), data, logic, n);

                            if (inExp != "" && inExp != null && inExp.Length > 4)
                                exp += " (" + inExp[0..^4] + ")" + logic;

                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"{e.Message} => {e.StackTrace}");
                }
            }
            return exp.Trim() == "(" ? "" : exp;
        }
        public static RequestSaveByFormModel<T> SaveRequestByForm<T>(string request) where T : class, new()
        {
            if (request == null)
                return new RequestSaveByFormModel<T>()
                {
                    Cmd = "error",
                    Recid = 0,
                    Name = "form",
                    Record = null
                };

            dynamic allRequest = JsonConvert.DeserializeObject(request);
            if (!RequestHelper.IsPropertyExist(allRequest, "record"))
            {
                allRequest.record = new JObject();
            }
            if (!RequestHelper.IsPropertyExist(allRequest, "recid"))
            {
                allRequest.recid = 0;
            }
            if (!RequestHelper.IsPropertyExist(allRequest, "name"))
            {
                allRequest.name = "form";
            }
            if (!RequestHelper.IsPropertyExist(allRequest, "cmd"))
            {
                allRequest.cmd = "save";
            }

            JObject jObj = allRequest.record;
            if (jObj["Port"] != null)
                jObj["Port"] = (jObj["Port"].ToString() != "") ? Convert.ToInt16(jObj["Port"].ToString()) : 0;
            while (true)
            {
                try
                {
                    jObj.ToObject<T>();
                    break;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    string type = e.Message.Split("'")[1];
                    string key = e.Message.Split("'")[3].Split(".")[1];
                    if (type == typeof(DateTime).FullName)
                    {
                        jObj.Remove(key);
                    }
                    else
                    {
                        jObj[key] = 0;
                    }
                }
            }
            return new RequestSaveByFormModel<T>()
            {
                Record = jObj.ToObject<T>(),
                Cmd = allRequest.cmd,
                Name = allRequest.name,
                Recid = allRequest.recid
            };
        }
        public static bool IsPropertyExist(dynamic settings, string name)
        {
            return settings[name] != null;
        }
        public static T DeleteRequestByGrid<T>(string request, string key) where T : class, new()
        {
            if (request == null)
                return default;
            dynamic allRequest = JsonConvert.DeserializeObject(request);
            if (!RequestHelper.IsPropertyExist(allRequest, key))
            {
                return default;
            }

            JArray array = allRequest[key];
            return array.ToObject<T>();
        }
        public static RequestSaveByGridModel<T> SaveRequestByGrid<T>(string request) where T : class, new()
        {
            if (request == null)
                return null;
            return JsonConvert.DeserializeObject<RequestSaveByGridModel<T>>(request);
        }
        public static bool DoesPropertyExist(dynamic settings, string name)
        {
            try
            {
                return settings[name] != null;
            }
            catch (Exception)
            {
                return settings.GetType().GetProperty(name) != null;
            }

        }
        public static RequestSaveByExcelModel<T> SaveRequestByExcel<T>(string request) where T : class, new()
        {
            if (request == null)
                return null;

            dynamic allRequest = JsonConvert.DeserializeObject(request);
            IDictionary<string, object> propertyValues = allRequest.record.ToObject<IDictionary<string, object>>();
            List<T> list = new List<T>();
            JArray json = JsonConvert.DeserializeObject(propertyValues["data"].ToString()) as JArray;
            foreach (var property in propertyValues)
            {
                if (property.Key != "overwrite" && property.Key != "data")
                {

                    foreach (JObject item in json)
                    {
                        if (property.Value == null || property.Value.ToString() == "")
                        {
                            item.Remove(property.Key);
                            continue;
                        }
                        dynamic val = property.Value;
                        SetKey(item, property.Key, val.id.ToString());
                    }
                }
            }
            dynamic data = json;
            foreach (var item in data)
            {
                IDictionary<string, object> pValue = item.ToObject<IDictionary<string, object>>();
                IDictionary<string, object> e = new Dictionary<string, object>();
                foreach (var inItem in pValue)
                {
                    if (inItem.Key == "recid")
                    {
                        e.Add("Id", inItem.Value);
                    }
                    else if (!inItem.Key.Contains("."))
                    {
                        e.Add(inItem.Key, inItem.Value);
                    }
                    else
                    {
                        var arr = inItem.Key.Split(".");
                        object check = "";
                        if (!e.TryGetValue(arr[0], out check))
                            e.Add(arr[0], new Dictionary<string, object>());
                        (e[arr[0]] as IDictionary<string, object>).Add(arr[1], inItem.Value);
                    }
                }
                var generatedJson = JsonConvert.SerializeObject(e).Replace("\"NULL\"", "null");
                list.Add(JsonConvert.DeserializeObject<T>(generatedJson));
            }
            var result = new RequestSaveByExcelModel<T>()
            {
                Cmd = allRequest.cmd,
                Name = allRequest.name,
                Recid = allRequest.recid,
                Records = list,
                OverWrite = allRequest.record.overwrite
            };
            return result;
            //return JsonConvert.DeserializeObject<RequestSaveByExcelModel<T>>(request);
        }
        public static void SetKey(JObject parent, string oldKey, string newKey)
        {
            parent[newKey] = parent[oldKey];
            parent.Remove(oldKey);
        }

    }
}
