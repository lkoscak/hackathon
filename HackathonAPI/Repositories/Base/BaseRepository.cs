using BaseApiContext.ServiceResponse;
using HackathonAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApi.Core.Config;
using WebApi.Core.Context;

namespace HackathonAPI.Repositories.Base
{
    public class BaseRepository : WebApi.Core.Repository.BaseRepository, IBaseRepository
    {
        private readonly IConfigManager _configManager;
        private readonly IContextManager _contextManager;
        public BaseRepository(IContextManager contextManager, IConfigManager configManager) : base(contextManager)
        {
            _configManager = configManager;
            _contextManager = contextManager;
        }

        public async Task<ServiceResponse<List<Status>>> GetAllStatuses()
        {
            try
            {
                List<Status> statuses = new List<Status>();
                using (SqlCommand sqlCommand = new SqlCommand("dbo.Test", Connection, Transaction))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@testValue", SqlDbType.Int).Value = 1111;

                    using (SqlDataReader data = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                Status status = new Status();
                                if (data["test_value"] != DBNull.Value) status.id = int.Parse(data["test_value"].ToString());
                                statuses.Add(status);
                            }
                        }
                    }
                }
                return new ServiceResponse<List<Status>>
                {
                    IsSuccess = true,
                    Message = null,
                    StatusCode = 200,
                    Data = statuses
                };
            }
            catch (Exception)
            {
                return new ServiceResponse<List<Status>>
                {
                    IsSuccess = false,
                    Message = null,
                    StatusCode = 500,
                    Data = null
                };
            }
        }
    }
}