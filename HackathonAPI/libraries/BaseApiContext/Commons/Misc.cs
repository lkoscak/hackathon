using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace WebApi.Core.Commons
{
    public class Misc
    {
        public static string DATE_FORMAT { get { return "yyyyMMddHHmmss"; } }
        public static string DATE_FORMAT2 { get { return "yyyy-MM-dd HH:mm:ss"; } }

        public static void WriteQueryToXML(string SQL, string RowName, XmlWriter Writer, SqlConnection SQLConnection)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SQL, SQLConnection);
                using (SqlDataReader Data = cmd.ExecuteReader())
                {
                    while (Data.Read())
                    {
                        try
                        {
                            Writer.WriteStartElement(RowName);
                            for (int i = 0; i < Data.FieldCount; i++)
                            {
                                string s = "";
                                if (Data[i] != DBNull.Value) s = Convert.ToString(Data[i]);
                                Writer.WriteAttributeString(Data.GetName(i), s);
                            }
                            Writer.WriteEndElement();
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        public static double DecodeCoordinate(double c)
        {
            int D = (int)c / 100;
            double S = c - (double)D * 100;
            S = (S / 60.0) * 100.0;
            return ((double)D + S / 100.0);
        }
        public static double EncodeCoordinate(double c)
        {
            int D = (int)c;
            double S = c - (double)D;
            S *= 60;
            return (double)D * 100 + S;
        }
        public static float DecodeCoordinate(float c)
        {
            int D = (int)c / 100;
            float S = c - (float)D * 100;
            S = (S / 60.0f) * 100.0f;
            return ((float)D + S / 100.0f);
        }
        public static float EncodeCoordinate(float c)
        {
            int D = (int)c;
            float S = c - (float)D;
            S *= 60;
            return (float)D * 100 + S;
        }

        public static double GPSDistance(double lat1, double lon1, double lat2, double lon2)
        {
            if (lat1 == lat2 && lon1 == lon2) return 0.0;
            double d = 6378.7 * Math.Acos(Math.Sin(lat1 / 57.2958) * Math.Sin(lat2 / 57.2958) + Math.Cos(lat1 / 57.2958) * Math.Cos(lat2 / 57.2958) * Math.Cos(lon2 / 57.2958 - lon1 / 57.2958));
            return d;
        }
        public static DateTime ParseDate(string dt)
        {
            DateTime dtDate = DateTime.MinValue;
            if (dt == null) return dtDate;
            dt = dt.Trim();
            if (dt.Length != 14) return DateTime.MinValue;
            try
            {
                dtDate = new DateTime(int.Parse(dt.Substring(0, 4)),
                                      int.Parse(dt.Substring(4, 2)),
                                      int.Parse(dt.Substring(6, 2)),
                                      int.Parse(dt.Substring(8, 2)),
                                      int.Parse(dt.Substring(10, 2)),
                                      int.Parse(dt.Substring(12, 2)));
            }
            catch { }
            return dtDate;
        }
        public static string ServerURL(HttpRequest Request)
        {
            int P = Request.Url.AbsoluteUri.LastIndexOf("/");
            if (P < 0) return "";
            return Request.Url.AbsoluteUri.Substring(0, P);
        }
        public static string FormatTimespan(TimeSpan TS)
        {
            string S = "";
            bool Write = false;
            if (TS.Days > 0 || Write) { S += TS.Days.ToString() + "d "; Write = true; }
            if (TS.Hours > 0 || Write) { S += TS.Hours.ToString() + "h "; Write = true; }
            if (TS.Minutes > 0 || Write) { S += TS.Minutes.ToString() + "min "; Write = true; }
            if (TS.Seconds > 0 || Write) { S += TS.Seconds.ToString() + "s "; Write = true; }
            return S;
        }
        public static float ParseSingle(string s)
        {
            if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",") s = s.Replace(".", ",");
            else if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".") s = s.Replace(",", ".");
            try
            {
                float r = float.Parse(s); ;
                return r;
            }
            catch { }
            return 0.0f;
        }
        public static double ParseDouble(string s)
        {
            if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",") s = s.Replace(".", ",");
            else if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".") s = s.Replace(",", ".");
            try
            {
                double r = double.Parse(s);
                return r;
            }
            catch { }
            return 0.0;
        }
        public static string FilterString(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '|') { s = s.Remove(i, 1); s = s.Insert(i, ";"); }
                if (s[i] == '\r') { s = s.Remove(i, 1); s = s.Insert(i, " "); }
                if (s[i] == '\n') { s = s.Remove(i, 1); s = s.Insert(i, " "); }
            }
            return s;
        }
        public static bool IsInt(string T)
        {
            if (T == null) return false;
            if (T.Trim() == "") return false;
            try
            {
                Convert.ToInt32(T);
                return true;
            }
            catch { return false; }
        }
        public static bool IsFloat(string T)
        {
            if (T == null) return false;
            if (T.Trim() == "") return false;
            try
            {
                Convert.ToSingle(T);
                return true;
            }
            catch { return false; }
        }
        public static string ToString(SqlCommand CMD)
        {
            if (CMD.CommandType != CommandType.StoredProcedure)
            {
                string s = CMD.CommandText;
                for (int i = 0; i < CMD.Parameters.Count; i++) s = s.Replace(CMD.Parameters[i].ParameterName, Convert.ToString(CMD.Parameters[i].Value));
                return s;
            }
            else
            {
                string s = "EXEC " + CMD.CommandText + " ";
                for (int i = 0; i < CMD.Parameters.Count; i++) s += CMD.Parameters[i].ParameterName + "='" + Convert.ToString(CMD.Parameters[i].Value) + "',";
                return s;
            }
        }
        public static string MD5Encode(string Text)
        {
            byte[] bytePS = System.Text.Encoding.UTF8.GetBytes(Text);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] MD5bytes = md5.ComputeHash(bytePS);
            string MD5PS = "";
            for (int i = 0; i < MD5bytes.Length; i++) MD5PS += MD5bytes[i].ToString("x").PadLeft(2, '0');
            return MD5PS;
        }

        public static string ParseDate2(string data)
        {
            // 20150101000000 => 01.01.2015
            if (data.Length < 8) return "";
            return data.Substring(6, 2) + "." + data.Substring(4, 2) + "." + data.Substring(0, 4);
        }

    }
}