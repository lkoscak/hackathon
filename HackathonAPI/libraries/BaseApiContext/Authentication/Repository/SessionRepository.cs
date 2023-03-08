using WebApi.Core.Authentication.Repository.Model;
using WebApi.Core.Caching;
using WebApi.Core.Commons;
using WebApi.Core.Config;
using WebApi.Core.Context;
using WebApi.Core.DynamicSQL;
using WebApi.Core.Logging;
using WebApi.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApi.Core.Authentication.Repository
{
    public class SessionRepository : BaseRepository, ISessionRepository
    {
        private DynamicQuery _dynamicQuery;

        public SessionRepository(
            IContextManager contextManager
            ) : base(contextManager)
        {
            _dynamicQuery = new DynamicQuery(ContextManager.loggerManager);
        }

        public Session GetActiveByKey(string sessionKey)
        {
            try
            {
                using (SqlConnection c = new SqlConnection(ContextManager.configManager.getConnstionString()))
                {
                    c.Open();

                    using (SqlCommand cmd = new SqlCommand("CheckSessionIsActive", c))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sesKey", SqlDbType.NVarChar).Value = sessionKey;

                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                return Session.Create(r);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ContextManager.loggerManager.error(e);
            }
            return null;
        }


        public Session GetActiveByB2BKey(string sessionKey)
        {
            try
            {
                using (SqlConnection c = new SqlConnection(ContextManager.configManager.getConnstionString()))
                {
                    c.Open();

                    using (SqlCommand cmd = new SqlCommand("CheckSessionIsActiveB2B", c))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sesKey", SqlDbType.NVarChar).Value = sessionKey;

                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                return Session.Create(r);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ContextManager.loggerManager.error(e);
            }
            return null;
        }

        public Task<LoginDTO> Authenticate(string username, string password, string ipAddress, bool keepActiveSessions = false)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SqlConnection c = new SqlConnection(ContextManager.configManager.getConnstionString()))
                    {
                        c.Open();
                        MobilisisCommon40.Timezone.UserTimezone userTimezone = MobilisisCommon40.Timezone.UserTimezone.LoadUserTimezone(c, username, password);

                        using (SqlCommand cmd = new SqlCommand("L1_new", c))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@login", SqlDbType.VarChar).Value = username;
                            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
                            cmd.Parameters.Add("@ip", SqlDbType.VarChar).Value = ipAddress;
                            cmd.Parameters.Add("@browser", SqlDbType.VarChar).Value = ContextManager.userAgent;
                            cmd.Parameters.Add("@loginDatetimeOffset", SqlDbType.DateTimeOffset).Value = userTimezone != null ? userTimezone.GetUserDateTimeOffset() : DateTimeOffset.Now;
                            cmd.Parameters.Add("@keepActiveSessions", SqlDbType.Bit).Value = keepActiveSessions;

                            ContextManager.loggerManager.debug(Common.ToString(cmd));

                            using (SqlDataReader sqlData = cmd.ExecuteReader())
                            {
                                if (sqlData.Read())
                                {
                                    Guid sessionKey = Guid.Empty;

                                    if (sqlData[0] == DBNull.Value)
                                        throw new Exception("Received empty session key from DB");

                                    if (!Guid.TryParse(sqlData[0].ToString().Trim(), out sessionKey))
                                        throw new Exception("Session key not valid GUID");

                                    LoginDTO s = new LoginDTO();
                                    s.SessionKey = sessionKey.ToString();

                                    return s;
                                }
                                else
                                {
                                    ContextManager.loggerManager.error(null, "No data from L1_new SP");
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ContextManager.loggerManager.error(e);
                }

                return null;
            });
        }

        public Task<bool> KeepAlive(string sessionKey)
        {
            return Task.Run(() =>
            {
                using (SqlConnection c = new SqlConnection(ContextManager.configManager.getConnstionString()))
                {
                    c.Open();
                    using (SqlDataReader r = _dynamicQuery.GetData(c, ContextManager.sessionKey, 3892))
                    {
                        if (r.HasRows)
                        {
                            ContextManager.cacheManager.fromCacheToken(sessionKey);
                            //Common.ApiCache.KeepAlive(Common.SessionKey); //keep alive cache object
                            return true;
                        }
                    }
                }
                ContextManager.cacheManager.removeCacheToken(sessionKey);
                //Common.ApiCache.Remove(Common.SessionKey); //remove from cache
                return false;
            });
        }

        public bool Logout()
        {
            try
            {
                ContextManager.cacheManager.removeCacheToken(ContextManager.sessionKey);

                using (SqlConnection c = new SqlConnection(ContextManager.configManager.getConnstionString()))
                {
                    c.Open();
                    MobilisisCommon40.Timezone.UserTimezone userTimezone = MobilisisCommon40.Timezone.UserTimezone.LoadUserTimezone(c, ContextManager.sessionKey);

                    using (SqlCommand cmd = new SqlCommand("LOGOUT", c))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@key", SqlDbType.VarChar).Value = ContextManager.sessionKey;
                        cmd.Parameters.Add("@logoutDatetimeOffset", SqlDbType.DateTimeOffset).Value = userTimezone != null ? userTimezone.GetUserDateTimeOffset() : DateTimeOffset.Now;

                        ContextManager.loggerManager.debug(Common.ToString(cmd));

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                ContextManager.loggerManager.error(e);
            }

            return false;
        }
    }
}