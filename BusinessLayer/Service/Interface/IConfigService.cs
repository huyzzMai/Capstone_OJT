using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.SkillRequest;
using BusinessLayer.Payload.ResponseModel.SkillResponse;
using BusinessLayer.Payload.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Payload.ResponseModel.ConfigResponse;
using BusinessLayer.Payload.RequestModel.ConfigRequest;

namespace BusinessLayer.Service.Interface
{
    public interface IConfigService
    {
        Task UpdateConfig(List<UpdateConfigRequest> list);

        Task<List<ConfigListResponse>> GetConfigList();
    }
}
