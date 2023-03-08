using BaseApiContext.ServiceResponse;
using HackathonAPI.Managers;
using HackathonAPI.Models;
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
    }
}