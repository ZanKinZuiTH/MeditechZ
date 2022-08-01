using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ShareLibrary
{
    public static class Extension
    {
        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }
        public class GetDateMonth
        {
            public DateTime firstDate { get; set; }
            public DateTime lastDate { get; set; }
        }


        public static DateTime? todateTH(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return DateTime.ParseExact(value, "dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
            else
                return null;
        }

        public static GetDateMonth Dateofmonth(this DateTime date)
        {
            GetDateMonth obj = new GetDateMonth();
            obj.firstDate = new DateTime(date.Year, date.Month, 1);
            obj.lastDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            return obj;
        }
        public static string ToThaiFormate(this DateTime? date)
        {
            return (date != null) ? date.Value.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH")) : "";
        }
        public static string ToThaiFormate(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
        }

        public static string ToThaiFormate2(this DateTime? date)
        {
            return (date != null) ? date.Value.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("th-TH")) : "";
        }
        public static string ToThaiFormate2(this DateTime date)
        {
            return date.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("th-TH"));
        }
        public static string ToThaiFormate3(this DateTime? date)
        {
            return (date != null) ? date.Value.ToString("dd/MM/yy", new System.Globalization.CultureInfo("th-TH")) : "";
        }
        public static string ToThaiFormate3(this DateTime date)
        {
            return date.ToString("dd/MM/yy", new System.Globalization.CultureInfo("th-TH"));
        }
        public static string ToEnFormate(this DateTime? date)
        {
            return (date != null) ? date.Value.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US")) : "";
        }
        public static string ToEnFormate(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
        }


        public static string[] Splittxt(this string Val)
        {
            return Val.Split(',');
        }
        public static int toGetSlot<T, TKey>(this List<T> _obj, Func<T, TKey> keySelector)
        {
            int i = 1;
            if (_obj.Count == 0)
            {
                i = 1;
            }
            else
            {
                var obj = _obj.OrderByDescending(keySelector).FirstOrDefault();
                i = (int)(obj.GetType().GetProperty("SortOrder").GetValue(obj, null));
                i += 1;
            }
            return i;
        }
        public static int Toint(this string val)
        {
            return int.Parse(val);
        }
        public static int Toint(this int? val)
        {
            return (val != null) ? val.ToString().Toint() : 0;
        }
        public static int ToPagging(this string val)
        {
            return (!string.IsNullOrEmpty(val)) ? int.Parse(val) : 1;
        }
        public static T Tomaplist<T>(this Object r) where T : new()
        {
            T item = new T();
            if (r != null)
            {
                IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(System.DayOfWeek))
                    {
                        DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), r.GetType().GetProperty(property.Name).GetValue(r, null).ToString());
                        property.SetValue(item, day, null);
                    }
                    else
                    {
                        try
                        {
                            object value = r.GetType().GetProperty(property.Name).GetValue(r, null);
                            if (value == DBNull.Value)
                                value = null;
                            property.SetValue(item, value, null);
                        }
                        catch (Exception) { }
                    }
                }
            }
            return item;
        }
        public static List<T> ToList_list<T>(this IEnumerable<object> obj) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (object r in obj)
            {
                T item = new T();
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(System.DayOfWeek))
                    {
                        DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), r.GetType().GetProperty(property.Name).GetValue(r, null).ToString());
                        property.SetValue(item, day, null);
                    }
                    else
                    {
                        if (r.GetType().GetProperty(property.Name) != null)
                        {
                            object value = r.GetType().GetProperty(property.Name).GetValue(r, null);
                            if (value == DBNull.Value)
                                value = null;
                            property.SetValue(item, value, null);
                        }
                    }
                }
                result.Add(item);
            }
            return result;
        }
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }
        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    if (row.Table.Columns.Contains(property.Name))
                    {
                        object value = row[property.Name];
                        if (value == DBNull.Value)
                            value = null;
                        if (value != null)
                        {
                            if (value.GetType() == typeof(System.TimeSpan))
                            {
                                property.SetValue(item, Convert.ToString(value), null);
                            }
                            else
                            {
                                property.SetValue(item, value, null);
                            }
                        }
                        else
                        {
                            property.SetValue(item, value, null);
                        }
                    }
                }
            }
            return item;
        }
        public static DataTable ToDataTable<T>(this IList<T> data, string tableName)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            table.TableName = tableName;
            return table;
        }

        public static object CloneObject(this object objSource)
        {
            //ดึง Type ของ Object ออกมา และสร้าง Object ใหม่ โดยใช้ Activator.CreateInstance
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);

            //List Property ของ Object ต้นทางมาให้หมด
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //Loop ยัดค่าลงไป
            foreach (PropertyInfo property in propertyInfo)
            {
                //ตรวจสอบก่อนว่า Property สามารถเขียนได้ หรือไม่
                if (property.CanWrite)
                {
                    //ตรวจสอบ Type ถ้าเป็น Enum หรือ String ก็ยัดข้อมูลงไปได้
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                    {
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }
                    //ถ้าไม่ใช้เงื่อนไขข้างต้น ต้อง Recursive เพราะเป็น Property ที่มีเงื่อนไขซับซ้อน
                    else
                    {
                        object objPropertyValue = property.GetValue(objSource, null);
                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            property.SetValue(objTarget, objPropertyValue.CloneObject(), null);
                        }
                    }
                }
            }
            return objTarget;
        }

    }
}
