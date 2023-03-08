using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApi.Core.Authentication.Repository.Model
{
    public class Session
    {
        public string Key { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string UserLang { get; set; }

        public static Session Create(SqlDataReader r)
        {
            Session s = new Session();
            s.Key = r["ses_key"].ToString();
            s.UserId = Convert.ToInt32(r["usr_id"].ToString());
            s.CompanyId = Convert.ToInt32(r["usr_com_id"].ToString());
            s.UserLang = r["usr_lang"].ToString();
            return s;
        }
    }
}