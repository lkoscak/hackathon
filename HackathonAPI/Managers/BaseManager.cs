﻿using BaseApiContext.ServiceResponse;
using HackathonAPI.Models;
using HackathonAPI.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Core.BusinessLogicManager;
using WebApi.Core.Context;

namespace HackathonAPI.Managers
{
    public class BaseManager : BLManager
    {
        private readonly IContextManager _contextManager;
        private readonly IBaseRepository _baseRepository;

        public BaseManager(IContextManager contextManager) : base(contextManager)
        {
            _contextManager = contextManager;
            _baseRepository = contextManager.resolveDI<IBaseRepository>();
        }

        public async Task<ServiceResponse<List<Status>>> GetAllStatuses()
        {
            ServiceResponse<List<Status>> response = await _baseRepository.GetAllStatuses();
            return response;
        }
    }
}