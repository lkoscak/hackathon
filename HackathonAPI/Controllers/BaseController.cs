using BaseApiContext.ServiceResponse;
using BaseApiContext.Validation.Attribute;
using HackathonAPI.Managers;
using HackathonAPI.Models;
using HackathonAPI.Models.Report;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Core.Context;
using WebApi.Core.Controller;
using WebApi.Core.Files.Model;

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
        public async Task<IHttpActionResult> CreateReport()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            using (ContextManager)
            {
                try
                {
                    ReportCreate report = null;
                    string root = System.Configuration.ConfigurationManager.AppSettings["DocumentImagesPath"];
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                        ContextManager.loggerManager.info("Creating directory => " + root);
                    }
                    var provider = new MultipartFormDataStreamProvider(root);
                    await Request.Content.ReadAsMultipartAsync(provider);
                    foreach (var key in provider.FormData.AllKeys)
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {
                            if (key.Equals("data"))
                            {
                                report = JsonConvert.DeserializeObject<ReportCreate>(val);
                            }
                        }
                    }

                    if (report == null)
                    {
                        ContextManager.loggerManager.info("Request data not provided");
                        return BadRequest("Request data not provided");
                    }
                    string validationError = report.getValidationErrors();
                    if (validationError != null)
                    {
                        ContextManager.loggerManager.info($"Request data not valid: {validationError}");
                        return BadRequest(validationError);
                    }
                    if (!report.AreCoordsValid())
                    {
                        ContextManager.loggerManager.info("Coordinates (lat, lng) are not valid");
                        return BadRequest("Coordinates (lat, lng) are not valid");
                    }

                    List<MultipartFileData> attachments = new List<MultipartFileData>();
                    attachments.AddRange(provider.FileData);

                    using (BaseManager bManager = new BaseManager(ContextManager))
                    {
                        ServiceResponse<List<GroupModel>> getGroupResponse = await bManager.GetAllGroups();
                        if (!getGroupResponse.IsSuccess || !getGroupResponse.Data.Any(group => group.id == report.group))
                        {
                            return BadRequest($"Provided group: {report.group} does not exist");
                        }
                        ServiceResponse<Report> response = await bManager.CreateReport(report, attachments);
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

        //[HttpPost]
        //[Route("report")]
        //[ModelStateValidationActionFilter]
        //public async Task<IHttpActionResult> CreateReport(ReportCreate report)
        //{
        //    if (report == null) {
        //        ContextManager.loggerManager.info("Request data not provided");
        //        return BadRequest("Request data not provided");
        //    }
        //    if (!report.AreCoordsValid())
        //    {
        //        ContextManager.loggerManager.info("Coordinates (lat, lng) are not valid");
        //        return BadRequest("Coordinates (lat, lng) are not valid");
        //    }
        //    using (ContextManager)
        //    {
        //        try
        //        {
        //            using (BaseManager bManager = new BaseManager(ContextManager))
        //            {
        //                ServiceResponse<List<GroupModel>> getGroupResponse = await bManager.GetAllGroups();
        //                if (!getGroupResponse.IsSuccess || !getGroupResponse.Data.Any(group => group.id == report.group))
        //                {
        //                    return BadRequest($"Provided group: {report.group} does not exist");
        //                }
        //                ServiceResponse<Report> response = await bManager.CreateReport(report);
        //                if (response.IsSuccess)
        //                {
        //                    return Ok(response.Data.id);
        //                }
        //                else
        //                {
        //                    return InternalServerError();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ContextManager.loggerManager.error(ex, "Error in CreateReport");
        //            return InternalServerError();
        //        }
        //    }
        //}

        //[HttpPut]
        //[Route("report/{id}")]
        //[ModelStateValidationActionFilter]
        //public async Task<IHttpActionResult> UpdateReport([FromBody] ReportUpdate report, [FromUri] int id = 0)
        //{
        //    if (report == null)
        //    {
        //        ContextManager.loggerManager.info("Request data not provided");
        //        return BadRequest("Request data not provided");
        //    }
        //    if (!report.AreCoordsValid())
        //    {
        //        ContextManager.loggerManager.info("Coordinates (lat, lng) are not valid");
        //        return BadRequest("Coordinates (lat, lng) are not valid");
        //    }
        //    using (ContextManager)
        //    {
        //        try
        //        {
        //            using (BaseManager bManager = new BaseManager(ContextManager))
        //            {
        //                ServiceResponse<Report> getReportResponse = await bManager.GetReport(id);
        //                if (!getReportResponse.IsSuccess)
        //                {
        //                    return BadRequest($"Report with given id: {id} does not exist");
        //                }
        //                ServiceResponse<List<GroupModel>> getGroupResponse = await bManager.GetAllGroups();
        //                if (!getGroupResponse.IsSuccess || !getGroupResponse.Data.Any(group => group.id == report.group))
        //                {
        //                    return BadRequest($"Provided group: {report.group} does not exist");
        //                }
        //                ServiceResponse<Report> response = await bManager.UpdateReport(id, report);
        //                if (response.IsSuccess)
        //                {
        //                    return Ok(response.Data.id);
        //                }
        //                else
        //                {
        //                    return InternalServerError();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ContextManager.loggerManager.error(ex, "Error in UpdateReport");
        //            return InternalServerError();
        //        }
        //    }
        //}
        [HttpPut]
        [Route("report/{id}")]
        public async Task<IHttpActionResult> UpdateReport([FromUri] int id = 0)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            using (ContextManager)
            {
                try
                {
                    ReportUpdate report = null;
                    string root = System.Configuration.ConfigurationManager.AppSettings["DocumentImagesPath"];
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                        ContextManager.loggerManager.info("Creating directory => " + root);
                    }
                    var provider = new MultipartFormDataStreamProvider(root);
                    await Request.Content.ReadAsMultipartAsync(provider);
                    foreach (var key in provider.FormData.AllKeys)
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {
                            if (key.Equals("data"))
                            {
                                report = JsonConvert.DeserializeObject<ReportUpdate>(val);
                            }
                        }
                    }

                    if (report == null)
                    {
                        ContextManager.loggerManager.info("Request data not provided");
                        return BadRequest("Request data not provided");
                    }
                    string validationError = report.getValidationErrors();
                    if (validationError != null)
                    {
                        ContextManager.loggerManager.info($"Request data not valid: {validationError}");
                        return BadRequest(validationError);
                    }
                    if (!report.AreCoordsValid())
                    {
                        ContextManager.loggerManager.info("Coordinates (lat, lng) are not valid");
                        return BadRequest("Coordinates (lat, lng) are not valid");
                    }

                    List<MultipartFileData> attachments = new List<MultipartFileData>();
                    attachments.AddRange(provider.FileData);

                    using (BaseManager bManager = new BaseManager(ContextManager))
                    {
                        ServiceResponse<Report> getReportResponse = await bManager.GetReport(id);
                        if (!getReportResponse.IsSuccess)
                        {
                            return BadRequest($"Report with given id: {id} does not exist");
                        }
                        ServiceResponse<List<GroupModel>> getGroupResponse = await bManager.GetAllGroups();
                        if (!getGroupResponse.IsSuccess || !getGroupResponse.Data.Any(group => group.id == report.group))
                        {
                            return BadRequest($"Provided group: {report.group} does not exist");
                        }
                        ServiceResponse<Report> response = await bManager.UpdateReport(id, report, attachments);
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

        [HttpPost]
        [Route("report/{id}/status")]
        [ModelStateValidationActionFilter]
        public async Task<IHttpActionResult> ChangeReportStatus([FromBody] ReportStatusChange statusData, [FromUri] int id = 0)
        {
            if (statusData == null)
            {
                ContextManager.loggerManager.info("Request data not provided");
                return BadRequest("Request data not provided");
            }
            using (ContextManager)
            {
                try
                {
                    using (BaseManager bManager = new BaseManager(ContextManager))
                    {
                        ServiceResponse<Report> getReportResponse = await bManager.GetReport(id);
                        if (!getReportResponse.IsSuccess)
                        {
                            return BadRequest($"Report with given id: {id} does not exist");
                        }
                        ServiceResponse<List<Status>> getStatusResponse = await bManager.GetAllStatuses();
                        if (!getStatusResponse.IsSuccess || !getStatusResponse.Data.Any(st => (st.id == statusData.status && st.id != 4)))
                        {
                            return BadRequest($"Provided status: {statusData.status} does not exist");
                        }
                        ServiceResponse<bool> response = await bManager.ChangeReportStatus(id, statusData);
                        if (response.IsSuccess)
                        {
                            return Ok();
                        }
                        else
                        {
                            return InternalServerError();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ContextManager.loggerManager.error(ex, "Error in ChangeReportStatus");
                    return InternalServerError();
                }
            }
        }

        [HttpDelete]
        [Route("report/{id}")]
        public async Task<IHttpActionResult> DeleteReport([FromUri] int id = 0)
        {
            using (ContextManager)
            {
                try
                {
                    using (BaseManager bManager = new BaseManager(ContextManager))
                    {
                        ServiceResponse<Report> getReportResponse = await bManager.GetReport(id);
                        if (!getReportResponse.IsSuccess)
                        {
                            return BadRequest($"Report with given id: {id} does not exist or is already deleted");
                        }
                        ServiceResponse<bool> response = await bManager.ChangeReportStatus(id, new ReportStatusChange { status = 4});
                        if (response.IsSuccess)
                        {
                            return Ok();
                        }
                        else
                        {
                            return InternalServerError();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ContextManager.loggerManager.error(ex, "Error in DeleteReport");
                    return InternalServerError();
                }
            }
        }

        [HttpGet]
        [Route("report/image/{id}")]
        public async Task<IHttpActionResult> GetReportImage([FromUri] int id = 0)
        {
            try
            {
                using (BaseManager bManager = new BaseManager(ContextManager))
                {
                    ServiceResponse<ApiFileResult> response = await bManager.GetReportImage(id);
                    if (response.IsSuccess)
                    {
                        return response.Data;
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                ContextManager.loggerManager.error(ex, "Error in GetReportImage");
                return InternalServerError();
            }
        }
    }
}