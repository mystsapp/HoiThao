using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

    //public static class EntityToTable
    //{
    //    public static DataTable ToDataTable<T>(this IEnumerable<T> entityList) where T : class
    //    {
    //        var properties = typeof(T).GetProperties();
    //        var table = new DataTable();

    //        foreach (var property in properties)
    //        {
    //            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
    //            table.Columns.Add(property.Name, type);
    //        }
    //        foreach (var entity in entityList)
    //        {
    //            table.Rows.Add(properties.Select(p => p.GetValue(entity, null)).ToArray());
    //        }
    //        return table;
    //    }
    //}
}