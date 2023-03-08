using FleetWebApi.BusinessLogic.EmailNS.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApi.Core.Context;
using WebApi.Core.DB;

namespace FleetWebApi.BusinessLogic.EmailNS
{
    public class EmailManager
    {
        IContextManager contextManager;
        public EmailManager(IContextManager contextManager)
        {
            this.contextManager = contextManager;
        }
        public async Task<int> SendMailWithAttachment(Email email, SqlConnection Connection)
        {

            int id = -1;

            using (SqlCommand cmd = new SqlCommand("SendEMailWithAttachment", Connection))//, Transaction))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@subject", System.Data.SqlDbType.VarChar).Value = email.subject;
                cmd.Parameters.Add("@mailto", System.Data.SqlDbType.VarChar).Value = email.mailTo;
                cmd.Parameters.Add("@text", System.Data.SqlDbType.Text).Value = email.text;
                cmd.Parameters.Add("@isHtml", System.Data.SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@emailfrom", System.Data.SqlDbType.VarChar).Value = email.mailFrom;
                cmd.Parameters.Add("@mat_name", System.Data.SqlDbType.VarChar).Value = email.attachmentName;
                cmd.Parameters.Add("@mat_contenttype", System.Data.SqlDbType.VarChar).Value = email.contentType;
                cmd.Parameters.Add("@mat_content", System.Data.SqlDbType.Image).Value = email.Content;
                if (email.attachmentName2 != "")
                {
                    cmd.Parameters.Add("@mat_name2", System.Data.SqlDbType.VarChar).Value = email.attachmentName2;
                    cmd.Parameters.Add("@mat_contenttype2", System.Data.SqlDbType.VarChar).Value = email.contentType2;
                    cmd.Parameters.Add("@mat_content2", System.Data.SqlDbType.Image).Value = email.Content2;
                }

                contextManager.loggerManager.info("sendMailWithAttachment CMD: " + DBManager.ToString(cmd));
                using (SqlDataReader R = await cmd.ExecuteReaderAsync())
                {
                    if (R.Read())
                    {
                        if (R["id"] != DBNull.Value) id = Convert.ToInt32((R["id"].ToString()));
                        contextManager.loggerManager.info("sendMailWithAttachment mailId: " + R["id"].ToString());

                    }
                }
            }
            return id;
        }
        public async Task<int> SendMailWithAttachment(Email email, SqlConnection Connection, SqlTransaction Transaction)
        {

            int id = -1;

            using (SqlCommand cmd = new SqlCommand("SendEMailWithAttachment", Connection, Transaction))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@subject", System.Data.SqlDbType.VarChar).Value = email.subject;
                cmd.Parameters.Add("@mailto", System.Data.SqlDbType.VarChar).Value = email.mailTo;
                cmd.Parameters.Add("@text", System.Data.SqlDbType.Text).Value = email.text;
                cmd.Parameters.Add("@isHtml", System.Data.SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@emailfrom", System.Data.SqlDbType.VarChar).Value = email.mailFrom;
                cmd.Parameters.Add("@mat_name", System.Data.SqlDbType.VarChar).Value = email.attachmentName;
                cmd.Parameters.Add("@mat_contenttype", System.Data.SqlDbType.VarChar).Value = email.contentType;
                cmd.Parameters.Add("@mat_content", System.Data.SqlDbType.Image).Value = email.Content;
                if (email.attachmentName2 != "")
                {
                    cmd.Parameters.Add("@mat_name2", System.Data.SqlDbType.VarChar).Value = email.attachmentName2;
                    cmd.Parameters.Add("@mat_contenttype2", System.Data.SqlDbType.VarChar).Value = email.contentType2;
                    cmd.Parameters.Add("@mat_content2", System.Data.SqlDbType.Image).Value = email.Content2;
                }

                contextManager.loggerManager.info("sendMailWithAttachment CMD: " + DBManager.ToString(cmd));
                using (SqlDataReader R = await cmd.ExecuteReaderAsync())
                {
                    if (R.Read())
                    {
                        if (R["id"] != DBNull.Value) id = Convert.ToInt32((R["id"].ToString()));
                        contextManager.loggerManager.info("sendMailWithAttachment mailId: " + R["id"].ToString());

                    }
                }
            }
            return id;
        }
    }
   
   
}
