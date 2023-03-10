﻿using BaseApiContext.ServiceResponse;
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
    }
}
