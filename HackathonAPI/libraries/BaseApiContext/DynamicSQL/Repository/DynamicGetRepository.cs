using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Core.Commons;
using WebApi.Core.Repository;
using WebApi.Core.Logging;
using WebApi.Core.Config;
using WebApi.Core.Context;
using System.Diagnostics;
using System.Web;

namespace WebApi.Core.DynamicSQL.Repository
{
    public class DynamicGetRepository : BaseRepository, IDynamicGetRepository
    {
        private DynamicQuery _dynamicQuery;

        public DynamicGetRepository(IContextManager contextManager) : base(contextManager)
        {
            _dynamicQuery = new DynamicQuery(ContextManager.loggerManager);
        }

        //public async Task<List<Dictionary<string, object>>> GetAsync(DynamicGetRequest param)
        //{
        //    var details = new List<Dictionary<string, object>>();

        //    using (SqlConnection c = new SqlConnection(_configManager.getConnstionString()))
        //    {
        //        await c.OpenAsync();
        //        using (SqlDataReader Data = await DynamicQuery.GetDataAsync(c, Common.SessionKey, param.cmd, param.param == null ? new object[] { } : param.param.ToArray<object>()))
        //        {
        //            if (Data.HasRows)
        //            {
        //                while (await Data.ReadAsync())
        //                {
        //                    var dict = new Dictionary<string, object>();

        //                    for (int i = 0; i < Data.FieldCount; i++)
        //                    {
        //                        dict.Add(Data.GetName(i), Data.IsDBNull(i) ? null : Data.GetValue(i));
        //                    }

        //                    details.Add(dict);
        //                }
        //            }
        //        }
        //    }

        //    return details;
        //}


        public async Task<DynamicGetGroupResponse> GetAsync(List<DynamicGetRequest> CMDs, string sessionKey)
        {
            if (string.IsNullOrWhiteSpace(sessionKey))
            {
                var e = new Exception("SessionKey is empty !!!");
                ContextManager.loggerManager.error(e);
                throw e;
            }
            DynamicGetGroupResponse dynamicGetGroupResponse = new DynamicGetGroupResponse();

            using (SqlConnection c = new SqlConnection(ContextManager.configManager.getConnstionString()))
            {
                await c.OpenAsync();

                foreach (var p in CMDs)
                {
                    Stopwatch St = Stopwatch.StartNew();
                    try
                    {
                        var results = new List<List<IDictionary<string, object>>>();

                        int number = 0;
                        string message = null;
                        try
                        {
                            using (SqlDataReader Data = await _dynamicQuery.GetDataAsync(c, sessionKey, p.cmd, p.param == null ? new object[] { } : p.param.ToArray<object>()))
                            {
                                do
                                {
                                    var data = new List<IDictionary<string, object>>();

                                    if (Data != null && Data.HasRows)
                                    {
                                        while (await Data.ReadAsync())
                                        {
                                            var dict = new Dictionary<string, object>();

                                            for (int i = 0; i < Data.FieldCount; i++)
                                            {
                                                try
                                                {
                                                    dict.Add(Data.GetName(i), Data.IsDBNull(i) ? null : Data.GetValue(i));
                                                }
                                                catch (ArgumentException e)
                                                {
                                                    //Ignore
                                                }
                                            }

                                            data.Add(dict);
                                        }
                                    }
                                    results.Add(data);

                                } while (Data != null && Data.NextResult());
                            }

                            this.NotifyUser(p);
                        }
                        catch (SqlException sqle)
                        {
                            number = sqle.Number;
                            message = sqle.Message;

                            ContextManager.loggerManager.error(sqle, "Error in 'DynamicGetController' in 'Set' method.");
                        }
                        DynamicGetSingleResponse singleResponse = new DynamicGetSingleResponse()
                        {
                            cmd = p.cmd,
                            inner_results = results,
                            error_number = number,
                            message = message
                        };

                        dynamicGetGroupResponse.requests.Add(singleResponse);
                    }
                    finally
                    {
                        St.Stop();
                        ContextManager.loggerManager.debug("Cmd: " + p.cmd + " -> " + St.Elapsed.TotalMilliseconds.ToString("0").PadLeft(5) + " ms" + (St.Elapsed.TotalMilliseconds > 10000 ?" ALERT > 10sec" : ""));
                    }
                }
            }

            return dynamicGetGroupResponse;
        }

        public async Task<int> SetAsync(List<DynamicGetRequest> CMDs, string sessionKey)
        {
            if (string.IsNullOrWhiteSpace(sessionKey))
            {
                var e = new Exception("SessionKey is empty !!!");
                ContextManager.loggerManager.error(e);
                throw e;
            }
            int rowcount = 0;
            using (SqlConnection c = new SqlConnection(ContextManager.configManager.getConnstionString()))
            {
                await c.OpenAsync();
                var t = c.BeginTransaction();
                foreach (var p in CMDs)
                {
                    Stopwatch St = Stopwatch.StartNew();

                    try {
                        rowcount = await _dynamicQuery.UpdateDataAsync(c, t, sessionKey, p.cmd, p.useNullValues, p.param.ToArray<object>());

                        this.NotifyUser(p);
                    }
                    finally
                    {
                        St.Stop();
                        if (St.Elapsed.TotalMilliseconds > 10000) ContextManager.loggerManager.debug("Cmd: " + p.cmd + " -> " + St.Elapsed.TotalMilliseconds.ToString("0").PadLeft(5) + " ms");
                    }
                }
                t.Commit();
            }

            return rowcount;
        }

        private void NotifyUser(DynamicGetRequest param)
        {
            if (!param.extraParam.ContainsKey("usridtonotify")) return;
            int UsrId = 0;
            string Cmd = "sync";
            if (param.extraParam.ContainsKey("notifytext") && param.extraParam["notifytext"] != null) Cmd = HttpUtility.UrlDecode(param.extraParam["notifytext"] as string);

            ContextManager.loggerManager.info("useridtonotify=" + param.extraParam["usridtonotify"] + " Cmd=" + Cmd);

            if (!int.TryParse(param.extraParam["usridtonotify"] as string, out UsrId))
            {
                string usridtonotify = param.extraParam["usridtonotify"] as string;
                string[] ids = usridtonotify.Split(',');

                for (int i = 0; i < ids.Length; i++)
                {
                    int id = 0;

                    if (int.TryParse(ids[i], out id))
                    {
                        string PushQueueName = System.Configuration.ConfigurationManager.AppSettings["PushQueueName"];
                        if (!String.IsNullOrEmpty(PushQueueName) && MobilisisCommon.PushNotifications.Service.QueueName != PushQueueName)
                        {
                            MobilisisCommon.PushNotifications.Service.QueueName = PushQueueName;
                            ContextManager.loggerManager.info("PushNotifications.Service.QueueName set to: " + MobilisisCommon.PushNotifications.Service.QueueName);
                        }
                        MobilisisCommon.PushNotifications.Service.AlertUser(id, Cmd, "");
                        ContextManager.loggerManager.info("User " + ids[i] + " notified");
                    }
                }

                ContextManager.loggerManager.info("Multi usrtonotify parameter:'" + param.extraParam["usridtonotify"] + "'"); return;
            }
            try
            {
                string PushQueueName = System.Configuration.ConfigurationManager.AppSettings["PushQueueName"];
                if (!String.IsNullOrEmpty(PushQueueName) && MobilisisCommon.PushNotifications.Service.QueueName != PushQueueName)
                {
                    MobilisisCommon.PushNotifications.Service.QueueName = PushQueueName;
                    ContextManager.loggerManager.info("PushNotifications.Service.QueueName set to: " + MobilisisCommon.PushNotifications.Service.QueueName);
                }
                MobilisisCommon.PushNotifications.Service.AlertUser(UsrId, Cmd, "");
                ContextManager.loggerManager.info("User " + UsrId + " notified");
            }
            catch (Exception e) { ContextManager.loggerManager.info("User " + UsrId + " notification failed:" + e); }
        }
    }
}
