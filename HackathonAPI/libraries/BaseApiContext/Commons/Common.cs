using WebApi.Core.Authentication;
using WebApi.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;

namespace WebApi.Core.Commons
{
    public class Common
    {
        private static string DATE_FORMAT2 { get { return "yyyy-MM-dd HH:mm:ss"; } }

        /// <summary>
        /// Writes SQL command to pretty executable string.
        /// </summary>
        /// <param name="CMD">Sql command</param>
        /// <returns></returns>
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

        public static string toDBDate(DateTime date)
        {
            return date.ToString(DATE_FORMAT2);
        }

        public static string getMimeType(string fileType)
        {
            if (fileType.ToLower() == "pdf") return "application/pdf";
            if (fileType.ToLower() == "doc") return "application/msword";
            if (fileType.ToLower() == "docx") return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            if (fileType.ToLower() == "xls") return "application/vnd.ms-excel";
            if (fileType.ToLower() == "xlsx") return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (fileType.ToLower() == "ppt") return "application/vnd.ms-powerpoint";
            if (fileType.ToLower() == "pptx") return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            return "application/octet-stream";
        }
    }
}