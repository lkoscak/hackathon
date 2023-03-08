using BaseApiContext.ServiceResponse;
using HackathonAPI.Models;
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
    }
}
