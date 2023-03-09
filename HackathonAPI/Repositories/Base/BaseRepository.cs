using BaseApiContext.ServiceResponse;
using HackathonAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
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

        public async Task<ServiceResponse<List<GroupModel>>> GetAllGroups()
        {
            try
            {
                List<GroupModel> groups = new List<GroupModel>();

                using (SqlCommand sqlCommand = new SqlCommand("dbo.GetAllGroups", Connection, Transaction))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader data = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                GroupModel group = new GroupModel();
                                if (data["group_id"] != DBNull.Value) group.group_id = int.Parse(data["group_id"].ToString());
                                if (data["group_name"] != DBNull.Value) group.group_name = data["group_name"].ToString();
                                if (data["group_description"] != DBNull.Value) group.group_description = data["group_description"].ToString();
                                groups.Add(group);
                            }
                        }
                    }
                }

                return new ServiceResponse<List<GroupModel>>
                {
                    IsSuccess = true,
                    Message = null,
                    StatusCode = 200,
                    Data = groups
                };
            }
            catch (Exception)
            {
                return new ServiceResponse<List<GroupModel>>
                {
                    IsSuccess = false,
                    Message = null,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public async Task<ServiceResponse<List<Status>>> GetAllStatuses()
        {
            try
            {
                List<Status> statuses = new List<Status>();
                using (SqlCommand sqlCommand = new SqlCommand("dbo.GetAllStatuses", Connection, Transaction))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader data = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                Status status = new Status();
                                if (data["status_id"] != DBNull.Value) status.id = int.Parse(data["status_id"].ToString());
                                if (data["status_name"] != DBNull.Value) status.name = data["status_name"].ToString();
                                if (data["status_description"] != DBNull.Value) status.description = data["status_description"].ToString();
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

        public async Task<ServiceResponse<List<Team>>> GetAllTeams()
        {
            try
            {
                List<Team> teams = new List<Team>();
                using (SqlCommand sqlCommand = new SqlCommand("dbo.GetAllTeams", Connection, Transaction))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader data = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                Team team = new Team();
                                if (data["team_id"] != DBNull.Value) team.team_id = int.Parse(data["status_id"].ToString());
                                if (data["team_name"] != DBNull.Value) team.team_name = data["status_name"].ToString();
                                if (data["team_color"] != DBNull.Value) team.team_color = data["status_description"].ToString();
                                if (data["team_icon"] != DBNull.Value) team.team_icon = data["status_description"].ToString();
                                teams.Add(team);
                            }
                        }
                    }
                }
                return new ServiceResponse<List<Team>>
                {
                    IsSuccess = true,
                    Message = null,
                    StatusCode = 200,
                    Data = teams
                };
            }
            catch (Exception)
            {
                return new ServiceResponse<List<Team>>
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