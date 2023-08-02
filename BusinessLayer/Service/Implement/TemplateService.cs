using BusinessLayer.Models.RequestModel.TemplateRequest;
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
    public class TemplateService : ITemplateService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TemplateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateTemplate(CreateTemplateRequest request)
        {
            try
            {
                var activeoJTBatch = await _unitOfWork.OJTBatchRepository.GetFirst(c => c.UniversityId == request.UniversityId && c.IsDeleted == false && c.EndTime > DateTime.UtcNow.AddHours(7), "Trainees");
                if(activeoJTBatch == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET,"University is not in active batch");
                }
                var listuser= activeoJTBatch.Trainees.ToList();
                var Temp = new Template()
                {
                    Name = request.Name,
                    Url = request.Url,
                    StartCell = request.StartCell,
                    UniversityId = request.UniversityId,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    UpdatedAt = DateTime.UtcNow.AddHours(7),
                    Status = CommonEnums.TEMPLATE_STATUS.ACTIVE
                };
                await _unitOfWork.TemplateRepository.Add(Temp);
                foreach (var i in request.TemplateHeaders)
                {

                    var newtemp = new TemplateHeader()
                    {
                        TemplateId=Temp.Id,
                        Name=i.Name,
                        IsCriteria=i.IsCriteria,
                        MatchedAttribute=i.MatchedAttribute,
                        Order=i.Order,
                        TotalPoint=i.TotalPoint,
                        Status=CommonEnums.TEMPLATEHEADER_STATUS.ACTIVE,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        UpdatedAt = DateTime.UtcNow.AddHours(7),

                    };
                    await _unitOfWork.TemplateHeaderRepository.Add(newtemp);
                    if(i.IsCriteria==true)
                    {
                        foreach (var j in listuser)
                        {
                            var usercriteria = new UserCriteria()
                            {
                                UserId = j.Id,
                                TemplateHeaderId=newtemp.Id,
                                UpdatedDate=DateTime.UtcNow.AddHours(7),
                                CreatedDate=DateTime.UtcNow.AddHours(7),
                            };
                            await _unitOfWork.UserCriteriaRepository.Add(usercriteria);
                        }
                    }                    
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
