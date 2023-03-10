using BaseApiContext.ServiceResponse;
using HackathonAPI.Managers;
using HackathonAPI.Models;
using HackathonAPI.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Core.Context;
using WebApi.Core.Controller;

namespace HackathonAPI.Controllers
{
    [RoutePrefix("v1/hackathon")]
    public class BaseController : BaseApiController
    {
        public object ServiceReponse { get; private set; }

        public BaseController(IContextManager contextManager) : base(contextManager)
        {
            contextManager.loggerManager.addCustomRelativePath("Hackathon API");
        }

        [HttpGet]
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Ok($"Hackathon API {DateTime.Now}");
        }
        [HttpGet]
        [Route("status")]
        public async Task<IHttpActionResult> GetAllStatuses()
        {
            using (ContextManager)
            {
                try
                {
                    using (BaseManager bManager = new BaseManager(ContextManager))
                    {
                        ServiceResponse<List<Status>> response = await bManager.GetAllStatuses();
                        if (response.IsSuccess) { 
                            return Ok(response.Data);
                        }
                        else{
                            return InternalServerError();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ContextManager.loggerManager.error(ex, "Error in GetAllStatuses");
                    return InternalServerError();
                }
            }
        }

        [HttpGet]
        [Route("group")]
        public async Task<IHttpActionResult> GetAllGroups()
        {
            using (ContextManager)
            {
                try
                {
                    using (BaseManager bManager = new BaseManager(ContextManager))
                    {
                        ServiceResponse<List<GroupModel>> response = await bManager.GetAllGroups();
                        if (response.IsSuccess)
                        {
                            return Ok(response.Data);
                        }
                        else
                        {
                            return InternalServerError();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ContextManager.loggerManager.error(ex, "Error in GetAllGroups");
                    return InternalServerError();
                }
            }
        }

        [HttpGet]
        [Route("team")]
        public async Task<IHttpActionResult> GetAllTeams()  
        {
            using (ContextManager)
            {
                try
                {
                    using (BaseManager bManager = new BaseManager(ContextManager))
                    {
                        ServiceResponse<List<Team>> response = await bManager.GetAllTeams();
                        if (response.IsSuccess)
                        {
                            return Ok(response.Data);
                        }
                        else
                        {
                            return InternalServerError();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ContextManager.loggerManager.error(ex, "Error in GetAllTeams");
                    return InternalServerError();
                }
            }
        }

        [HttpGet]
        [Route("report")]
        public async Task<IHttpActionResult> GetAllReports()
        {
            using (ContextManager)
            {
                try
                {
                    using (BaseManager bManager = new BaseManager(ContextManager))
                    {
                        ServiceResponse<List<Report>> response = await bManager.GetAllReports();
                        if (response.IsSuccess)
                        {
                            return Ok(response.Data);
                        }
                        else
                        {
                            return InternalServerError();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ContextManager.loggerManager.error(ex, "Error in GetAllReports");
                    return InternalServerError();
                }
            }
        }

        [HttpPost]
        [Route("report")]
        public async Task<IHttpActionResult> CreateReport(ReportCreate report)
        {
            using (ContextManager)
            {
                try
                {
                    using (BaseManager bManager = new BaseManager(ContextManager))
                    {
                        ServiceResponse<Report> response = await bManager.CreateReport(report);
                        if (response.IsSuccess)
                        {
                            return Ok(response.Data.id);
                        }
                        else
                        {
                            return InternalServerError();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ContextManager.loggerManager.error(ex, "Error in CreateReport");
                    return InternalServerError();
                }
            }
        }
    }
}