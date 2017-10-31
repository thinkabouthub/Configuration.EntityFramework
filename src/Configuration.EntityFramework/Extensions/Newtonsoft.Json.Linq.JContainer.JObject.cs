using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Linq
{
    public static class JObjectExtensions
    {
        public static IDictionary<string, object> ToDictionary(this JObject @object)
        {
            var result = @object.ToObject<Dictionary<string, object>>();

            var jObjectKeys = (from r in result
                               let key = r.Key
                               let value = r.Value
                               where value.GetType() == typeof(JObject)
                               select key).ToList();

            var jArrayKeys = (from r in result
                              let key = r.Key
                              let value = r.Value
                              where value.GetType() == typeof(JArray)
                              select key).ToList();

            jArrayKeys.ForEach(key => result[key] = ((JArray)result[key]).Values().Select(x => ((JValue)x).Value).ToArray());
            jObjectKeys.ForEach(key => result[key] = ToDictionary(result[key] as JObject));

            return result;
        }
    }
}
