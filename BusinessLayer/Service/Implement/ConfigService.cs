using BusinessLayer.Payload.RequestModel.ConfigRequest;
using BusinessLayer.Payload.ResponseModel.ConfigResponse;
using BusinessLayer.Payload.ResponseModel.SkillResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class ConfigService : IConfigService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ConfigService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ConfigListResponse>> GetConfigList()
        {
           try
            {
                var list = await _unitOfWork.ConfigRepository.Get();
                var listresponse = list.Select(c =>
                {
                    return new ConfigListResponse()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Value = c.Value
                    };
                }).ToList();

                return listresponse;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task UpdateConfig(List<UpdateConfigRequest> list)
        {
            try
            {            
                if (list.Any(c=>c.value<0))
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Value can not lower than 0");
                }
              foreach(var item in list)
                {
                    var tmp = await _unitOfWork.ConfigRepository.GetFirst(c=>c.Id==item.Id);
                    if (tmp == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Config not found");
                    }
                    tmp.Value = item.value;
                    await _unitOfWork.ConfigRepository.Update(tmp);
                }
               
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
