using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace China_AreaCode_data
{
    /// <summary>
    /// 快捷类型转换
    /// </summary>
    public static class ConvertHelperExtend
    {
        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="formatting">格式</param>
        /// <param name="settings">配置</param>
        /// <returns></returns>
        public static string PackJson(this object obj, Formatting formatting = Formatting.None, JsonSerializerSettings settings = (JsonSerializerSettings)null)
        {
            return JsonConvert.SerializeObject(obj, formatting, settings);
        }

        /// <summary>
        /// 反序列化Json字符串
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="jsonStr">Json字符串</param>
        /// <param name="settings">配置</param>
        /// <returns></returns>
        public static T PackJsonObject<T>(this string jsonStr, JsonSerializerSettings settings = (JsonSerializerSettings)null)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr, settings);
        }
        /// <summary>
        /// 转成数值
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="defaultValue">默认值0</param>
        /// <returns>数值</returns>
        public static int PackInt(this object value, int defaultValue = 0)
        {
            int num;
            if ((value == null) || (value == DBNull.Value))
            {
                return defaultValue;
            }
            if (value is int)
            {
                return (int)value;
            }
            if (!int.TryParse(value.ToString(), out num))
            {
                return defaultValue;
            }
            return num;
        }
        /// <summary>
        /// 转成小数
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="num">保留位数 *默认两位</param>
        /// <param name="defaultValue">默认值0m</param>
        /// <returns>小数</returns>
        public static decimal PackDecimal(this object value, int num = 2, decimal defaultValue = 0m)
        {
            decimal number;
            if ((value == null) || (value == DBNull.Value))
            {
                return defaultValue;
            }
            if (value is decimal)
            {
                return Math.Round((decimal)value, num);
            }
            if (!decimal.TryParse(value.ToString(), out number))
            {
                return defaultValue;
            }
            return Math.Round(number, num);
        }
        /// <summary>
        /// 转成浮点数
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns>小数</returns>
        public static float PackFloat(this object value, float defaultVal = float.NaN)
        {
            float num;
            if ((value == null) || (value == DBNull.Value))
            {
                return defaultVal;
            }
            if (value is float)
            {
                return (float)value;
            }
            if (!float.TryParse(value.ToString(), out num))
            {
                return defaultVal;
            }
            return num;
        }
        /// <summary>
        /// 转成字符
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>字符</returns>
        public static string PackString(this object value)
        {
            if ((value == null) || (value == DBNull.Value))
            {
                return string.Empty;
            }
            if (value.GetType() == typeof(byte[]))
            {
                return Encoding.ASCII.GetString((byte[])value, 0, ((byte[])value).Length);
            }
            return value.ToString();
        }
        /// <summary>
        /// 转成日期型
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>日期</returns>
        public static DateTime PackDateTime(this object value)
        {
            DateTime time;
            if ((value == null) || (value == DBNull.Value))
            {
                return new DateTime(2000, 1, 1);
            }
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            if (!DateTime.TryParse(value.ToString(), out time))
            {
                return new DateTime(2000, 1, 1);
            }
            return time;
        }
        /// <summary>
        /// 转成日期型
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>日期</returns>
        public static DateTimeOffset PackDateTimeOffset(this object value)
        {
            DateTimeOffset time;
            if ((value == null) || (value == DBNull.Value))
            {
                return new DateTimeOffset(new DateTime(2000, 1, 1));
            }
            if (value is DateTimeOffset)
            {
                return (DateTimeOffset)value;
            }
            if (!DateTimeOffset.TryParse(value.ToString(), out time))
            {
                return new DateTimeOffset(new DateTime(2000, 1, 1));
            }
            return time;
        }
        /// <summary>
        /// 转成长整型
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns>长整型</returns>
        public static long PackLong(this object value, long defaultVal = 0)
        {
            long num;
            if ((value == null) || (value == DBNull.Value))
            {
                return defaultVal;
            }
            if (value is long)
            {
                return (long)value;
            }
            if (!long.TryParse(value.ToString(), out num))
            {
                return defaultVal;
            }
            return num;
        }
        /// <summary>
        /// 转成字符
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>长整型</returns>
        public static long PackChar(this object value)
        {
            char result = char.MinValue;
            if (value == null || value == DBNull.Value) return result;
            if (typeof(Char) == value.GetType()) return (char)value;
            string val = PackString(value);
            if (char.TryParse(val, out result)) return result;
            if (string.IsNullOrEmpty(val)) return char.MinValue;
            return val.ToCharArray()[0];
        }
        /// <summary>
        /// 转成比特
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static byte PackByte(this object value, byte defaultVal = Byte.MinValue)
        {
            byte num;
            if ((value == null) || (value == DBNull.Value))
            {
                return defaultVal;
            }
            if (value is byte)
            {
                return (byte)value;
            }
            if (!byte.TryParse(value.ToString(), out num))
            {
                return defaultVal;
            }
            return num;
        }
        /// <summary>
        /// 转成Guid
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns></returns>
        public static Guid PackGuid(this object value)
        {
            Guid result = Guid.Empty;
            if (value == null || value == DBNull.Value) return result;
            if (typeof(Guid) == value.GetType()) return (Guid)value;
            string val = PackString(value);
            if (Guid.TryParse(val, out result)) return result;
            return Guid.Empty;
        }
        /// <summary>
        /// 转成TimeSpan
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static TimeSpan PackTimeSpan(this object value)
        {
            TimeSpan result = TimeSpan.Zero;
            if (value == null || value == DBNull.Value) return result;
            if (typeof(TimeSpan) == value.GetType()) return (TimeSpan)value;
            string val = PackString(value);
            if (TimeSpan.TryParse(val, out result)) return result;
            return TimeSpan.Zero;
        }
        /// <summary>
        /// 转成单精度浮点数
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static Single PackSingle(this object value, byte defaultVal = Byte.MinValue)
        {
            Single result = defaultVal;
            if (value == null || value == DBNull.Value) return defaultVal;
            if (typeof(Single) == value.GetType()) return (Single)value;
            string val = PackString(value);
            if (!Single.TryParse(val, out result)) return defaultVal;
            return result;
        }
        /// <summary>
        /// 新开一行
        /// </summary>
        /// <param name="strBur">被封装的字符串</param>
        public static void PackTRStart(this StringBuilder strBur)
        {
            strBur.Append("<tr>");
        }
        /// <summary>
        /// 行结束
        /// </summary>
        /// <param name="strBur">被封装的字符串</param>
        public static void PackTREnd(this StringBuilder strBur)
        {
            strBur.Append("</tr>");
        }
        /// <summary>
        /// 获取封装后的TD列HTML
        /// </summary>
        /// <param name="strBur">被封装的字符串</param>
        /// <param name="values">封装值列</param>
        public static void PackAppendTD(this StringBuilder strBur, params object[] values)
        {
            foreach (var obj in values)
            {
                strBur.AppendFormat("<td>{0}</td>", obj.PackString());
            }
        }
        /// <summary>
        /// 获取封装后的一整行HTML
        /// </summary>
        /// <param name="strBur">被封装的字符串</param>
        /// <param name="values">封装值列</param>
        public static void PackAppendRow(this StringBuilder strBur, params object[] values)
        {
            strBur.Append("<tr>");
            foreach (var obj in values)
            {
                strBur.AppendFormat("<td>{0}</td>", obj.PackString());
            }
            strBur.Append("</tr>");
        }
        /// <summary>
        /// 获取封装后的a链接HTML
        /// </summary>
        /// <param name="value">链接名</param>
        /// <param name="href">链接地址</param>
        /// <param name="onclick">链接点击事件</param>
        /// <param name="target">链接目标</param>
        /// <param name="cssClass">链接样式</param>
        /// <returns></returns>
        public static string PackLink(this object value, string href, string onclick = "", string target = "", string cssClass = "")
        {
            return PackLink(value.PackString(), href, onclick, target, cssClass);
        }
        /// <summary>
        /// 获取封装后的a链接HTML
        /// </summary>
        /// <param name="value">链接名</param>
        /// <param name="href">链接地址</param>
        /// <param name="onclick">链接点击事件</param>
        /// <param name="target">链接目标</param>
        /// <param name="cssClass">链接样式</param>
        /// <returns></returns>
        public static string PackLink(string value, string href, string onclick = "", string target = "", string cssClass = "")
        {
            if (!string.IsNullOrEmpty(onclick) && string.IsNullOrEmpty(href)) href = "javascript:void(0);";
            return string.Format(" <a href='{0}' {2} {3} {4} >{1}</a> ", href.PackHtml(), value
                 , string.IsNullOrEmpty(onclick) ? "" : string.Format("onclick='{0}'", onclick.PackHtml())
                 , string.IsNullOrEmpty(target) ? "" : string.Format("target='{0}'", target.PackHtml())
                 , string.IsNullOrEmpty(cssClass) ? "" : string.Format("class='{0}'", cssClass.PackHtml()));
        }
        /// <summary>
        /// 获取新增按钮HTML
        /// </summary>
        /// <param name="cssClass">按钮样式</param>
        /// <returns></returns>
        public static string PackAddButton(string cssClass = "form_bto_only")
        {
            return PackButton("新增", "ShowEditPage(0)", cssClass);
        }
        /// <summary>
        /// 获取编辑按钮HTML
        /// </summary>
        /// <param name="id">编辑编号</param>
        /// <param name="cssClass">按钮样式</param>
        /// <returns></returns>
        public static string PackEditButton(this object id, string cssClass = "form_bto_only")
        {
            return PackButton("编辑", string.Format("ShowEditPage({0})", id.PackInt()), cssClass);
        }
        /// <summary>
        /// 获取删除按钮HTML
        /// </summary>
        /// <param name="id">删除编号</param>
        /// <param name="cssClass">按钮样式</param>
        /// <returns></returns>
        public static string PackDelButton(this object id, string cssClass = "form_bto_only")
        {
            return PackButton("删除", string.Format("DelData({0})", id.PackInt()), cssClass);
        }
        /// <summary>
        /// 获取无效按钮HTML
        /// </summary>
        /// <param name="onclick">按钮点击事件</param>
        /// <param name="cssClass">按钮样式</param>
        /// <returns></returns>
        public static string PackInvalidButton(string onclick, string cssClass = "form_bto_only")
        {
            return PackButton("无效", onclick, cssClass);
        }
        /// <summary>
        /// 获取封装后的按钮HTML
        /// </summary>
        /// <param name="value">按钮名</param>
        /// <param name="onclick">按钮点击事件</param>
        /// <param name="cssClass">按钮样式</param>
        /// <returns></returns>
        public static string PackButton(this object value, string onclick, string cssClass = "form_bto_only")
        {
            return PackButton(value.PackString(), onclick, cssClass);
        }
        /// <summary>
        /// 获取封装后的按钮HTML
        /// </summary>
        /// <param name="value">按钮名</param>
        /// <param name="onclick">按钮点击事件</param>
        /// <param name="cssClass">按钮样式</param>
        /// <returns></returns>
        public static string PackButton(string value, string onclick, string cssClass = "form_bto_only")
        {
            return string.Format(" <input type='button' class='{0}' onclick='{1}' value='{2}' /> ", cssClass.PackHtml(), onclick.PackHtml(), value);
        }
        /// <summary>
        /// 获取对应值的枚举描述
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="enumType">枚举</param>
        /// <returns></returns>
        public static string PackEnumValue(this object value, Type enumType)
        {
            if (value == null || value == DBNull.Value || enumType == null) return string.Empty;
            string enumname = string.Empty;
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            //检索所有字段
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum == true)
                {
                    //枚举英文
                    string name = field.Name;
                    if ((int)Enum.Parse(enumType, name, true) == PackInt(value))
                    {
                        DescriptionAttribute da = null;
                        object[] arrobj = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (arrobj.Length > 0)
                        {
                            da = arrobj[0] as DescriptionAttribute;
                        }
                        if (da != null)
                        {
                            //枚举中文描述
                            enumname = da.Description;
                        }
                    }
                }
            }
            return enumname;
        }

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value">枚举</param>
        /// <returns></returns>
        public static string PackEnumDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                DescriptionAttribute[] customAttributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (customAttributes.Length > 0)
                {
                    return customAttributes[0].Description;
                }
            }
            return value.ToString();
        }

        /// <summary>
        /// 将字符转成安全的HTML
        /// </summary>
        /// <param name="value">字符</param>
        /// <returns>HTML转义符</returns>
        public static string PackHtml(this string value)
        {
            return value.Replace("&", "&amp;")
                        .Replace(">", "&gt;")
                        .Replace("<", "&lt;")
                        .Replace("\"", "&quot;")
                        .Replace("\'", "&apos;");
        }
        /// <summary>
        /// 将字符转成安全的HTML(带换行)
        /// </summary>
        /// <param name="value">字符</param>
        /// <returns>HTML转义符</returns>
        public static string PackHtmlWihtBR(this string value)
        {
            return value.Replace("&", "&amp;")
                        .Replace(">", "&gt;")
                        .Replace("<", "&lt;")
                        .Replace("\"", "&quot;")
                        .Replace("\'", "&apos;")
                        .Replace("\n", "<br />");
        }
        /// <summary>
        /// 转义SQL查询字符串中的特殊字符
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>转义后字符串</returns>
        public static string PackSqlSearchStr(this string value)
        {
            return value.Replace("[", "[[]")
                        .Replace("%", "[%]")
                        .Replace("_", "[_]");
        }
        /// <summary>
        /// 左截取字符串
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="len">截取长度</param>
        /// <returns></returns>
        public static string PackLeft(this string s, int len)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (s.Length <= len) return s;
            return s.Substring(0, len);
        }
        /// <summary>
        /// 右截取字符串
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="len">截取长度</param>
        /// <returns></returns>
        public static string PackRight(this string s, int len)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Substring(len > s.Length ? 0 : s.Length - len);
        }
        /// <summary>
        /// 截断字符串超出结尾加省略号
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="len">截取长度</param>
        /// <returns></returns>
        public static string PackEllipsis(this string s, int len)
        {
            if (s == null || s.Length == 0 || len < 0) return string.Empty;
            int bytesCount = Encoding.GetEncoding("gb2312").GetByteCount(s);
            if (bytesCount > len)
            {
                int readyLength = 0;
                int byteLength;
                for (int i = 0; i < s.Length; i++)
                {
                    byteLength = Encoding.GetEncoding("gb2312").GetByteCount(new char[] { s[i] });
                    readyLength += byteLength;
                    if (readyLength == len)
                    {
                        s = s.Substring(0, i + 1) + "...";
                        break;
                    }
                    else if (readyLength > len)
                    {
                        s = s.Substring(0, i) + "....";
                        break;
                    }
                }
            }
            return s;
        }

        #region XML 类型转换
        public static T XmlToObject<T>(this string xml)
        {
            using (var rdr = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(rdr);
            }
        }

        public static string ToXml<T>(this object o)
        {
            var xsSubmit = new XmlSerializer(typeof(T));
            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, o);
                    return XElement.Parse(sww.ToString()).ToString();
                }
            }
        }
        #endregion
    }
}
