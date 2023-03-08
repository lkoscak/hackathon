using FleetWebApi.BusinessLogic.Login.Model;
using System.Data;
using System.Data.SqlClient;
using WebApi.Core.Context;
using WebApi.Core.DB;
using WebApi.Core.Repository;

namespace FleetWebApi.BusinessLogic.Login.Repository
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        public LoginRepository(IContextManager contextManager) : base(contextManager)
        {
        }

        public async Task<string> login(LoginData loginData)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.GetSessionKey", Connection, Transaction))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@login", SqlDbType.NVarChar).Value = loginData.username;
                    sqlCommand.Parameters.Add("@password", SqlDbType.NVarChar).Value = loginData.password;
                    sqlCommand.Parameters.Add("@typeId", SqlDbType.Int).Value = 10;
                    if (loginData.applicationId != null) sqlCommand.Parameters.Add("@applicationId", SqlDbType.Int).Value = loginData.applicationId;

                    string sesKey = null;

                    using (SqlDataReader Data = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (Data.HasRows)
                        {
                            if (await Data.ReadAsync())
                            {
                                sesKey = Data["ses_key"].ToString();
                                ContextManager.loggerManager.info("[getSessionKey] Session key => " + sesKey);
                            }
                        }
                        else
                        {
                            ContextManager.loggerManager.info("[getSessionKey] DB response conatins no data!");
                        }
                    }
                    return sesKey;
                }
            }
            catch (Exception ex)
            {
                ContextManager.loggerManager.error(ex);
                return null;
            }
        }
    }
}