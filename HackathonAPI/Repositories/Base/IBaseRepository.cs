using BaseApiContext.ServiceResponse;
using HackathonAPI.Models;
using HackathonAPI.Models.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Core.Files.Model;

namespace HackathonAPI.Repositories.Base
{
    public interface IBaseRepository
    {
        Task<ServiceResponse<List<Status>>> GetAllStatuses();

        Task<ServiceResponse<List<GroupModel>>> GetAllGroups();

        Task<ServiceResponse<List<Team>>> GetAllTeams();
        Task<ServiceResponse<List<Report>>> GetAllReports();

        Task<ServiceResponse<Report>> CreateReport(ReportCreate report, DataTable images);
        Task<ServiceResponse<Report>> UpdateReport(int id, ReportUpdate report, DataTable images);
        Task<ServiceResponse<Report>> GetReport(int id);
        Task<ServiceResponse<bool>> ChangeStatus(int id, int status);
        Task<ServiceResponse<ApiFileResult>> GetReportImage(int id);
    }
}
