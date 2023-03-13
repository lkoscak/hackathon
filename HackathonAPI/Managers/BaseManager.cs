using BaseApiContext.ServiceResponse;
using HackathonAPI.Models;
using HackathonAPI.Models.Report;
using HackathonAPI.Mqtt;
using HackathonAPI.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Core.BusinessLogicManager;
using WebApi.Core.Context;
using WebApi.Core.Files.Model;

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

        public async Task<ServiceResponse<List<GroupModel>>> GetAllGroups()
        {
            ServiceResponse<List<GroupModel>> response = await _baseRepository.GetAllGroups();
            return response;
        }

        public async Task<ServiceResponse<List<Team>>> GetAllTeams()
        {
            ServiceResponse<List<Team>> response = await _baseRepository.GetAllTeams();
            return response;
        }

        public async Task<ServiceResponse<List<Report>>> GetAllReports()
        {
            ServiceResponse<List<Report>> response = await _baseRepository.GetAllReports();
            return response;
        }
        public async Task<ServiceResponse<Report>> GetReport(int id)
        {
            ServiceResponse<Report> response = await _baseRepository.GetReport(id);
            return response;
        }
        public async Task<ServiceResponse<Report>> CreateReport(ReportCreate report, List<MultipartFileData> images)
        {
            ServiceResponse<Report> response = null;
            try
            {
                DataTable formatedImages = FormatImagesForDB(images);
                response = await _baseRepository.CreateReport(report, formatedImages);
                if (response.IsSuccess)
                {
                    _contextManager.loggerManager.info("Report successfully created. Transaction commited.");
                    _contextManager.dbManager.Commit(this);
                    MqttManager mqttManager = new MqttManager(_contextManager);
                    await mqttManager.Publish("hackathon/report/new", Newtonsoft.Json.JsonConvert.SerializeObject(response.Data));
                    await mqttManager.DisconnectFromMqttServer();
                }
                else
                {
                    _contextManager.loggerManager.info("Report could not be created. Transaction rollback");
                    _contextManager.dbManager.Rollback(this);
                }
            }
            catch (Exception ex)
            {
                _contextManager.dbManager.Rollback(this);
                _contextManager.loggerManager.error(ex);
                response.IsSuccess = false;
            }
            return response;
        }
        public async Task<ServiceResponse<Report>> UpdateReport(int id, ReportUpdate report, List<MultipartFileData> images)
        {
            ServiceResponse<Report> response = null;
            try
            {
                DataTable formatedImages = FormatImagesForDB(images);
                response = await _baseRepository.UpdateReport(id, report, formatedImages);
                if (response.IsSuccess)
                {
                    _contextManager.loggerManager.info("Report successfully updated. Transaction commited.");
                    _contextManager.dbManager.Commit(this);
                    MqttManager mqttManager = new MqttManager(_contextManager);
                    await mqttManager.Publish("hackathon/report/update", Newtonsoft.Json.JsonConvert.SerializeObject(response.Data));
                    await mqttManager.DisconnectFromMqttServer();
                }
                else
                {
                    _contextManager.loggerManager.info("Report could not be updated. Transaction rollback");
                    _contextManager.dbManager.Rollback(this);
                }
            }
            catch (Exception ex)
            {
                _contextManager.dbManager.Rollback(this);
                _contextManager.loggerManager.error(ex);
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<ServiceResponse<ApiFileResult>> GetReportImage(int id)
        {
            ServiceResponse<ApiFileResult> response = await _baseRepository.GetReportImage(id);
            return response;
        }

        public async Task<ServiceResponse<bool>> ChangeReportStatus(int id, ReportStatusChange statusData)
        {
            ServiceResponse<bool> response = await _baseRepository.ChangeStatus(id, statusData.status);
            try
            {
                if (response.IsSuccess)
                {
                    _contextManager.loggerManager.info("Report status successfully changed. Transaction commited.");
                    _contextManager.dbManager.Commit(this);
                    MqttManager mqttManager = new MqttManager(_contextManager);
                    statusData.id = id;
                    await mqttManager.Publish("hackathon/report/status", Newtonsoft.Json.JsonConvert.SerializeObject(statusData));
                    await mqttManager.DisconnectFromMqttServer();
                }
                else
                {
                    _contextManager.loggerManager.info("Report status could not be changed. Transaction rollback");
                    _contextManager.dbManager.Rollback(this);
                }
            }
            catch (Exception ex)
            {
                _contextManager.dbManager.Rollback(this);
                _contextManager.loggerManager.error(ex);
                response.IsSuccess = false;
            }
            return response;
        }

        private DataTable FormatImagesForDB(List<MultipartFileData> images) {
            List<Image> formatedImages = new List<Image>();
            try
            {
                foreach (MultipartFileData image in images)
                {
                    byte[] buffer = null;
                    using (FileStream fs = File.OpenRead(image.LocalFileName))
                    {
                        buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, (int)fs.Length);
                    }
                    formatedImages.Add(new Image
                    {
                        name = Path.GetFileName(string.Concat(image.Headers.ContentDisposition.FileName.Split(Path.GetInvalidFileNameChars()))),
                        type = Path.GetExtension(string.Concat(image.Headers.ContentDisposition.FileName.Split(Path.GetInvalidFileNameChars()))),
                        content = buffer,
                    });
                }
            }
            catch (Exception){}

            DataTable DT = new DataTable("dbo.Report_image");
            DT.Columns.Add("ri_name", typeof(string));
            DT.Columns.Add("ri_type", typeof(string));
            DT.Columns.Add("ri_content", typeof(byte[]));

            foreach (Image image in formatedImages)
            {
                DataRow row = DT.NewRow();
                row["ri_name"] = image.name;
                row["ri_type"] = image.type;
                row["ri_content"] = image.content;
                DT.Rows.Add(row);
            }

            return DT;
        }
    }
}