using BaseApiContext.ServiceResponse;
using HackathonAPI.Models;
using HackathonAPI.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonAPI.Repositories.Base
{
    public interface IBaseRepository
    {
        Task<ServiceResponse<List<Status>>> GetAllStatuses();

        Task<ServiceResponse<List<GroupModel>>> GetAllGroups();

        Task<ServiceResponse<List<Team>>> GetAllTeams();
        Task<ServiceResponse<List<Report>>> GetAllReports();

        Task<ServiceResponse<Report>> CreateReport(ReportCreate report);
        Task<ServiceResponse<Report>> UpdateReport(int id, ReportUpdate report);
        Task<ServiceResponse<Report>> GetReport(int id);
        Task<ServiceResponse<bool>> ChangeStatus(int id, int status);
    }
}
