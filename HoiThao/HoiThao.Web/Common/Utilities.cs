using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;


namespace HoiThao.Web.Common
{
    
    public class Utilities
    {
        public static string NextID(string lastID, string prefixID)
        {
            int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
            int lengthNumberID = lastID.Length - prefixID.Length;
            string zeroNumber = "";
            for (int i = 1; i <= lengthNumberID; i++)
            {
                if (nextID < Math.Pow(10, i))
                {
                    for (int j = 1; j <= lengthNumberID - i; i++)
                    {
                        zeroNumber += "0";
                    }
                    return prefixID + zeroNumber + nextID.ToString();
                }
            }
            return prefixID + nextID;
        }
    }

    public class NullToEmptyStringResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties()
                    .Select(p => {
                        var jp = base.CreateProperty(p, memberSerialization);
                        jp.ValueProvider = new NullToEmptyStringValueProvider(p);
                        return jp;
                    }).ToList();
        }
    }

    public class NullToEmptyStringValueProvider : IValueProvider
    {
        PropertyInfo _MemberInfo;
        public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
        {
            _MemberInfo = memberInfo;
        }

        public object GetValue(object target)
        {
            object result = _MemberInfo.GetValue(target);
            if (_MemberInfo.PropertyType == typeof(string) && result == null) result = "";
            return result;

        }

        public void SetValue(object target, object value)
        {
            _MemberInfo.SetValue(target, value);
        }
    }
}