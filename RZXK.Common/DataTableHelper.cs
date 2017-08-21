using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace RZXK.Common
{

    public static class AttachedProperty
    {
        private static readonly ConditionalWeakTable<object, Dictionary<string, object>> _table =
            new ConditionalWeakTable<object, Dictionary<string, object>>();

        public static T GetAttachedPropertyValue<T>(this object owner, string propertyName)
        {
            Dictionary<string, object> values;
            if (_table.TryGetValue(owner, out values))
            {
                object temp;
                if (values.TryGetValue(propertyName, out temp))
                {
                    return (T)temp;
                }
            }

            return default(T);
        }

        public static void SetAttachedPropertyValue<T>(this object owner, string propertyName, T value)
        {
            Dictionary<string, object> values;
            if (!_table.TryGetValue(owner, out values))
            {
                values = new Dictionary<string, object>();
                _table.Add(owner, values);
            }

            values[propertyName] = value;
        }
    }


    public class DataTableHelper
    {
        /// <summary>
        /// 转化一个DataTable
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="list">
        /// </param>
        /// <returns>
        /// </returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> list)
        {
            var pList = new List<PropertyInfo>();
            Type type = typeof(T);
            var dt = new DataTable();
            Array.ForEach(
                type.GetProperties(),
                p =>
                {
                    pList.Add(p);
                    dt.Columns.Add(p.Name, p.PropertyType);
                });
            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">
        /// 类型
        /// </typeparam>
        /// <param name="dt">
        /// DataTable
        /// </param>
        /// <returns>
        /// </returns>
        public static IList<T> ToList<T>(DataTable dt) where T : class, new()
        {
            var prlist = new List<PropertyInfo>();
            Type type = typeof(T);
            Array.ForEach(
                type.GetProperties(),
                p =>
                {
                    if (dt.Columns.IndexOf(p.Name) != -1)
                    {
                        prlist.Add(p);
                    }
                });
            var oblist = new List<T>();

            // System.Data.SqlTypes.
            foreach (DataRow row in dt.Rows)
            {
                var ob = new T();
                prlist.ForEach(
                    p =>
                    {
                        var values = row[p.Name];

                        string pType = p.PropertyType.ToString();
                        if (row[p.Name] is DBNull)
                        {
                            if (pType == "System.Int32")
                            {
                                p.SetValue(ob, 0, null);
                            }
                            else if (pType == "System.Double")
                            {
                                p.SetValue(ob, Convert.ToDouble(0), null);
                            }
                            else
                            {
                                p.SetValue(ob, "", null);
                            }


                        }
                        else if (row[p.Name] is Int64)
                        {
                            try
                            {
                                p.SetValue(ob, Convert.ToInt32(row[p.Name]), null);
                            }
                            catch (Exception)
                            {
                                p.SetValue(ob, row[p.Name].ToString(), null);
                            }

                        }
                        else if (row[p.Name] is Int32)
                        {
                            try
                            {
                                p.SetValue(ob, Convert.ToInt32(row[p.Name]), null);
                            }
                            catch (Exception)
                            {
                                p.SetValue(ob, row[p.Name].ToString(), null);
                            }

                        }
                        else if (row[p.Name] is Double)
                        {
                            p.SetValue(ob, Convert.ToDouble(row[p.Name]), null);
                        }
                        else if (pType == "System.Double")
                        {
                            p.SetValue(ob, Convert.ToDouble(row[p.Name]), null);
                        }
                        else if (pType == "System.Int32")
                        {
                            p.SetValue(ob, Convert.ToInt32(row[p.Name]), null);
                        }
                        else if (row[p.Name] is System.Double)
                        {
                            p.SetValue(ob, Convert.ToDouble(row[p.Name]), null);
                        }
                        else if (row[p.Name] is UInt32)
                        {
                            p.SetValue(ob, Convert.ToInt32(row[p.Name]), null);
                        }
                        else
                        {
                            p.SetValue(ob, row[p.Name].ToString(), null);
                        }
                    });
                oblist.Add(ob);
            }

            return oblist;
        }

        public static List<T> ToList3<T>(DataTable dt) where T : class, new()
        {
            var prlist = new List<PropertyInfo>();
            Type type = typeof(T);
            Array.ForEach(
                type.GetProperties(),
                p =>
                {
                    if (dt.Columns.IndexOf(p.Name) != -1)
                    {
                        prlist.Add(p);
                    }
                });
            var oblist = new List<T>();

            // System.Data.SqlTypes.
            foreach (DataRow row in dt.Rows)
            {
                var ob = new T();
                prlist.ForEach(
                    p =>
                    {
                        if (row[p.Name] != DBNull.Value)
                        {
                            p.SetValue(ob, row[p.Name], null);
                        }
                    });
                oblist.Add(ob);
            }

            return oblist;
        }

        public static IList<T> ToListByField<T>(DataTable dt) where T : class, new()
        {
            var prlist = new List<FieldInfo>();
            Type type = typeof(T);
            Array.ForEach(
                type.GetFields(System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.FlattenHierarchy),
                p =>
                {
                    if (dt.Columns.IndexOf(p.Name) != -1)
                    {
                        prlist.Add(p);
                    }
                });
            var oblist = new List<T>();

            // System.Data.SqlTypes.
            foreach (DataRow row in dt.Rows)
            {
                var ob = new T();
                prlist.ForEach(
                    p =>
                    {
                        if (row[p.Name] != DBNull.Value)
                        {
                            p.SetValue(ob, row[p.Name]);
                        }
                    });
                oblist.Add(ob);
            }

            return oblist;
        }

        private List<T> CreateAnonymousList<T>(T obj)
        {
            return new List<T>();
        }

        private static T AnonCast<T>(object obj, T type)
        {
            return (T)obj;
        }

        public static IEnumerable ToList2<T>(DataTable dt, T anType) where T : class
        {
            var prlist = new List<PropertyInfo>();
            Type type = typeof(T);
            Array.ForEach(
                type.GetProperties(),
                p =>
                {
                    if (dt.Columns.IndexOf(p.Name) != -1)
                    {
                        prlist.Add(p);
                    }
                });
            var oblist = new List<object>();

            ////System.Data.SqlTypes.
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                var ob = Activator.CreateInstance<T>();

                // T ob = DeepCloen<T>(anType);
                prlist.ForEach(
                    p =>
                    {
                        if (row[p.Name] != DBNull.Value)
                        {
                            ob.SetAttachedPropertyValue(p.Name, row[p.Name]);

                            // p.SetValue(ob, row[p.Name], null);
                        }
                    });
                oblist.Add(ob);
            }

            return oblist;
        }

        // private static T DeepCloen<T>(T t) where T : class

        // Json
        // using (Stream objectStream = new MemoryStream())
        // {
        // IFormatter formatter = new BinaryFormatter();
        // formatter.Serialize(objectStream, t);
        // objectStream.Seek(0, SeekOrigin.Begin);
        // return formatter.Deserialize(objectStream) as T;
        // }
        // }
        public static IList<dynamic> ToList(DataTable dt)
        {
            IList<dynamic> list = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                var obj = new DynamicDictionary();
                foreach (var column in dt.Columns)
                {
                    ((IDictionary<string, object>)obj)[column.ToString()] = row[column.ToString()];
                }

                list.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">
        /// 集合
        /// </param>
        /// <returns>
        /// </returns>
        public static DataTable ToDataTableTow(IList list)
        {
            var result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (var pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    var tempList = new ArrayList();
                    foreach (var pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }

                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }

            return result;
        }


        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">
        /// 集合项类型
        /// </typeparam>
        /// <param name="list">
        /// 集合
        /// </param>
        /// <returns>
        /// 数据集(表)
        /// </returns>
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            return ToDataTable(list, null);
        }


        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">
        /// 集合项类型
        /// </typeparam>
        /// <param name="list">
        /// 集合
        /// </param>
        /// <param name="propertyName">
        /// 需要返回的列的列名
        /// </param>
        /// <returns>
        /// 数据集(表)
        /// </returns>
        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            var propertyNameList = new List<string>();
            if (propertyName != null)
            {
                propertyNameList.AddRange(propertyName);
            }

            var result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (var pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                        {
                            result.Columns.Add(pi.Name, pi.PropertyType);
                        }
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    var tempList = new ArrayList();
                    foreach (var pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }

                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }

            return result;
        }
    }
}
