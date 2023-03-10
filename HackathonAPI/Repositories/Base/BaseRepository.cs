using BaseApiContext.ServiceResponse;
using HackathonAPI.Models;
using HackathonAPI.Models.Report;
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

        public async Task<ServiceResponse<Report>> CreateReport(ReportCreate report)
        {
            try
            {
                Report createdReport = null;
                using (SqlCommand sqlCommand = new SqlCommand("dbo.CreateReport", Connection, Transaction))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@title", SqlDbType.NVarChar).Value = report.title;
                    sqlCommand.Parameters.Add("@description", SqlDbType.NVarChar).Value = report.description;
                    sqlCommand.Parameters.Add("@creator", SqlDbType.NVarChar).Value = report.creator;
                    sqlCommand.Parameters.Add("@images", SqlDbType.NVarChar).Value = report.images != null ? string.Join(";;;", report.images) : "";
                    sqlCommand.Parameters.Add("@category", SqlDbType.NVarChar).Value = report.category;
                    sqlCommand.Parameters.Add("@group", SqlDbType.Int).Value = report.group;
                    sqlCommand.Parameters.Add("@lat", SqlDbType.Float).Value = report.lat;
                    sqlCommand.Parameters.Add("@lng", SqlDbType.Float).Value = report.lng;
                    sqlCommand.Parameters.Add("@key", SqlDbType.NVarChar).Value = "1";

                    using (SqlDataReader data = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                createdReport = new Report();
                                if (data["report_id"] != DBNull.Value) createdReport.id = int.Parse(data["report_id"].ToString());
                                if (data["report_title"] != DBNull.Value) createdReport.title = data["report_title"].ToString();
                                if (data["report_decription"] != DBNull.Value) createdReport.description = data["report_decription"].ToString();
                                if (data["report_created"] != DBNull.Value) DateTime.TryParse(data["report_created"].ToString(), out createdReport.created);
                                if (data["report_creator"] != DBNull.Value) createdReport.creator = data["report_creator"].ToString();
                                if (data["report_status"] != DBNull.Value) createdReport.status = int.Parse(data["report_status"].ToString());
                                if (data["report_group"] != DBNull.Value) createdReport.group = int.Parse(data["report_group"].ToString());
                                if (data["report_category"] != DBNull.Value) createdReport.category = data["report_category"].ToString();
                                if (data["report_team"] != DBNull.Value) createdReport.team = int.Parse(data["report_team"].ToString());
                                if (data["report_lat"] != DBNull.Value) createdReport.lat = float.Parse(data["report_lat"].ToString());
                                if (data["report_lng"] != DBNull.Value) createdReport.lng = float.Parse(data["report_lng"].ToString());
                                createdReport.images = new List<string>();
                                if (data["report_images"] != DBNull.Value)
                                {
                                    createdReport.images.AddRange(data["report_images"].ToString().Split(new string[] { ";;;" }, StringSplitOptions.None));
                                }
                            }
                        }
                    }
                }
                return new ServiceResponse<Report>
                {
                    IsSuccess = createdReport != null,
                    Message = null,
                    StatusCode = 200,
                    Data = createdReport
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Report>
                {
                    IsSuccess = false,
                    Message = null,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public async Task<ServiceResponse<bool>> ChangeStatus(int id, int status)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.ChangeStatus", Connection, Transaction))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Parameters.Add("@status", SqlDbType.Int).Value = status;
                    sqlCommand.Parameters.Add("@key", SqlDbType.NVarChar).Value = "1";
                    await sqlCommand.ExecuteNonQueryAsync();
                }
                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Message = null,
                    StatusCode = 200,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Message = null,
                    StatusCode = 500,
                    Data = false
                };
            }
        }

        public async Task<ServiceResponse<Report>> UpdateReport(int id, ReportUpdate report)
        {
            try
            {
                Report updatedReport = null;
                using (SqlCommand sqlCommand = new SqlCommand("dbo.UpdateReport", Connection, Transaction))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Parameters.Add("@title", SqlDbType.NVarChar).Value = report.title;
                    sqlCommand.Parameters.Add("@description", SqlDbType.NVarChar).Value = report.description;
                    sqlCommand.Parameters.Add("@creator", SqlDbType.NVarChar).Value = report.creator;
                    sqlCommand.Parameters.Add("@images", SqlDbType.NVarChar).Value = report.images != null ? string.Join(";;;", report.images) : "";
                    sqlCommand.Parameters.Add("@category", SqlDbType.NVarChar).Value = report.category;
                    sqlCommand.Parameters.Add("@group", SqlDbType.Int).Value = report.group;
                    sqlCommand.Parameters.Add("@lat", SqlDbType.Float).Value = report.lat;
                    sqlCommand.Parameters.Add("@lng", SqlDbType.Float).Value = report.lng;

                    using (SqlDataReader data = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                updatedReport = new Report();
                                if (data["report_id"] != DBNull.Value) updatedReport.id = int.Parse(data["report_id"].ToString());
                                if (data["report_title"] != DBNull.Value) updatedReport.title = data["report_title"].ToString();
                                if (data["report_decription"] != DBNull.Value) updatedReport.description = data["report_decription"].ToString();
                                if (data["report_created"] != DBNull.Value) DateTime.TryParse(data["report_created"].ToString(), out updatedReport.created);
                                if (data["report_creator"] != DBNull.Value) updatedReport.creator = data["report_creator"].ToString();
                                if (data["report_status"] != DBNull.Value) updatedReport.status = int.Parse(data["report_status"].ToString());
                                if (data["report_group"] != DBNull.Value) updatedReport.group = int.Parse(data["report_group"].ToString());
                                if (data["report_category"] != DBNull.Value) updatedReport.category = data["report_category"].ToString();
                                if (data["report_team"] != DBNull.Value) updatedReport.team = int.Parse(data["report_team"].ToString());
                                if (data["report_lat"] != DBNull.Value) updatedReport.lat = float.Parse(data["report_lat"].ToString());
                                if (data["report_lng"] != DBNull.Value) updatedReport.lng = float.Parse(data["report_lng"].ToString());
                                updatedReport.images = new List<string>();
                                if (data["report_images"] != DBNull.Value)
                                {
                                    updatedReport.images.AddRange(data["report_images"].ToString().Split(new string[] { ";;;" }, StringSplitOptions.None));
                                }
                            }
                        }
                    }
                }
                return new ServiceResponse<Report>
                {
                    IsSuccess = updatedReport != null,
                    Message = null,
                    StatusCode = 200,
                    Data = updatedReport
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Report>
                {
                    IsSuccess = false,
                    Message = null,
                    StatusCode = 500,
                    Data = null
                };
            }
        }

        public async Task<ServiceResponse<Report>> GetReport(int id)
        {
            try
            {
                Report report = null;
                using (SqlCommand sqlCommand = new SqlCommand("dbo.GetReportById", Connection, Transaction))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Parameters.Add("@key", SqlDbType.NVarChar).Value = "1";

                    using (SqlDataReader data = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                report = new Report();
                                if (data["report_id"] != DBNull.Value) report.id = int.Parse(data["report_id"].ToString());
                                if (data["report_title"] != DBNull.Value) report.title = data["report_title"].ToString();
                                if (data["report_decription"] != DBNull.Value) report.description = data["report_decription"].ToString();
                                if (data["report_created"] != DBNull.Value) DateTime.TryParse(data["report_created"].ToString(), out report.created);
                                if (data["report_creator"] != DBNull.Value) report.creator = data["report_creator"].ToString();
                                if (data["report_status"] != DBNull.Value) report.status = int.Parse(data["report_status"].ToString());
                                if (data["report_group"] != DBNull.Value) report.group = int.Parse(data["report_group"].ToString());
                                if (data["report_category"] != DBNull.Value) report.category = data["report_category"].ToString();
                                if (data["report_team"] != DBNull.Value) report.team = int.Parse(data["report_team"].ToString());
                                if (data["report_lat"] != DBNull.Value) report.lat = float.Parse(data["report_lat"].ToString());
                                if (data["report_lng"] != DBNull.Value) report.lng = float.Parse(data["report_lng"].ToString());
                                report.images = new List<string>();
                                if (data["report_images"] != DBNull.Value)
                                {
                                    report.images.AddRange(data["report_images"].ToString().Split(new string[] { ";;;" }, StringSplitOptions.None));
                                }
                            }
                        }
                    }
                }
                return new ServiceResponse<Report>
                {
                    IsSuccess = report != null,
                    Message = null,
                    StatusCode = 200,
                    Data = report
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Report>
                {
                    IsSuccess = false,
                    Message = null,
                    StatusCode = 500,
                    Data = null
                };
            }
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
                                if (data["group_id"] != DBNull.Value) group.id = int.Parse(data["group_id"].ToString());
                                if (data["group_name"] != DBNull.Value) group.name = data["group_name"].ToString();
                                if (data["group_description"] != DBNull.Value) group.description = data["group_description"].ToString();
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

        public async Task<ServiceResponse<List<Report>>> GetAllReports()
        {
            try
            {
                List<Report> reports = new List<Report>();
                using (SqlCommand sqlCommand = new SqlCommand("dbo.GetAllReports", Connection, Transaction))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader data = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                Report report = new Report();
                                if (data["report_id"] != DBNull.Value) report.id = int.Parse(data["report_id"].ToString());
                                if (data["report_title"] != DBNull.Value) report.title = data["report_title"].ToString();
                                if (data["report_decription"] != DBNull.Value) report.description = data["report_decription"].ToString();
                                if (data["report_created"] != DBNull.Value) DateTime.TryParse(data["report_created"].ToString(), out report.created);
                                if (data["report_creator"] != DBNull.Value) report.creator = data["report_creator"].ToString();
                                if (data["report_status"] != DBNull.Value) report.status = int.Parse(data["report_status"].ToString());
                                if (data["report_group"] != DBNull.Value) report.group = int.Parse(data["report_group"].ToString());
                                if (data["report_category"] != DBNull.Value) report.category = data["report_category"].ToString();
                                if (data["report_team"] != DBNull.Value) report.team = int.Parse(data["report_team"].ToString());
                                if (data["report_lat"] != DBNull.Value) report.lat = float.Parse(data["report_lat"].ToString());
                                if (data["report_lng"] != DBNull.Value) report.lng = float.Parse(data["report_lng"].ToString());
                                report.images = new List<string>();
                                if (data["report_images"] != DBNull.Value) {
                                    report.images.AddRange(data["report_images"].ToString().Split(new string[] { ";;;" }, StringSplitOptions.None));
                                }
                                reports.Add(report);
                            }
                        }
                    }
                }
                return new ServiceResponse<List<Report>>
                {
                    IsSuccess = true,
                    Message = null,
                    StatusCode = 200,
                    Data = reports
                };
            }
            catch (Exception)
            {
                return new ServiceResponse<List<Report>>
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
                                if (data["team_id"] != DBNull.Value) team.id = int.Parse(data["team_id"].ToString());
                                if (data["team_name"] != DBNull.Value) team.name = data["team_name"].ToString();
                                if (data["team_color"] != DBNull.Value) team.color = data["team_color"].ToString();
                                if (data["team_icon"] != DBNull.Value) team.icon = data["team_icon"].ToString();
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