using BaseApiContext.ServiceResponse;
using HackathonAPI.Models;
using HackathonAPI.Models.Report;
using HackathonAPI.Mqtt;
using HackathonAPI.Repositories.Base;
using System;
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
        public async Task<ServiceResponse<Report>> CreateReport(ReportCreate report)
        {
            ServiceResponse<Report> response = await _baseRepository.CreateReport(report);
            try
            {
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
        public async Task<ServiceResponse<Report>> UpdateReport(int id, ReportUpdate report)
        {
            ServiceResponse<Report> response = await _baseRepository.UpdateReport(id, report);
            try
            {
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
    }
}