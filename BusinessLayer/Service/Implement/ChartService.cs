using BusinessLayer.Payload.ResponseModel.ChartResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace BusinessLayer.Service.Implement
{
    public class ChartService: IChartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BatchAndTraineeResponse> getBatchAndTrainee(int year)
        {
           try
            {
                var batch = await _unitOfWork.OJTBatchRepository
                            .Get(b => b.StartTime.Value.Year <= year 
                            && year <= b.EndTime.Value.Year, "Trainees");
                if (!batch.Any())
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND,"Not found any batches");
                }
                var response =new BatchAndTraineeResponse();
                for(int i=1;i <=12;i++)
                {
                    var batchesInMonth = batch.Where(b =>(b.StartTime.Value.Month <= i && b.EndTime.Value.Month >= i));

                    int totalTraineesInMonth = batchesInMonth.Sum(b => b.Trainees.Count(t=>t.Status==CommonEnums.USER_STATUS.ACTIVE));

                    if (batchesInMonth.Any())
                    {
                        response.NumberOfOjtBatches.Add(batchesInMonth.Count());
                    }
                    else
                    {
                        response.NumberOfOjtBatches.Add(0); 
                    }
                    response.NumberofTrainees.Add(totalTraineesInMonth);

                }
                return response;

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

        public async Task<List<ChartCircleResponse>> getTraineeByPosition()
        {
            try
            {
                var totalusers = await _unitOfWork.UserRepository.Get(c => c.Status == CommonEnums.USER_STATUS.ACTIVE
                                                                    && c.Role==CommonEnums.ROLE.TRAINEE 
                                                                    && c.Position !=null, "Position");
                var listpostion = await _unitOfWork.PositionRepository.Get();
                if (!totalusers.Any())
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Not found any users");
                }
                List<ChartCircleResponse> tmp = new List<ChartCircleResponse>();            
                var positionCounts = totalusers
                    .GroupBy(u => u.PositionId)
                    .Select(group => new
                    {
                        Position = listpostion.FirstOrDefault(c=>c.Id == group.Key).Name,
                        Count = group.Count()
                    })
                    .OrderByDescending(item => item.Count)
                    .Take(3);
                int totaltop = 0;
                foreach (var item in positionCounts)
                {
                    var response= new ChartCircleResponse();
                    response.label = item.Position.ToString();
                    response.value = item.Count;
                    totaltop= totaltop + item.Count;
                    tmp.Add(response);
                }
                var other = new ChartCircleResponse();
                other.label = "Other";
                other.value =totalusers.Count()- totaltop;
                tmp.Add(other);          
                return tmp;
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

        public async Task<List<ChartCircleResponse>> getTrainerWithMostTrainees(int number)
        {
            try
            {
                var totalusers = await _unitOfWork.UserRepository.Get(c=>c.Status==CommonEnums.USER_STATUS.ACTIVE);
                if (!totalusers.Any())
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Not found any users");
                }
                var traineruser = totalusers.Where(c=>c.Role==CommonEnums.ROLE.TRAINER);
                List<ChartCircleResponse> tmp = new List<ChartCircleResponse>();
                foreach (var item in traineruser)
                {
                    var response= new ChartCircleResponse();
                    response.label = item.FirstName + " " + item.LastName;
                    response.value = totalusers.Count(c=>c.UserReferenceId==item.Id);
                    tmp.Add(response);
                }
                var responses = tmp.OrderByDescending(c=>c.value).Take(number).ToList();
                return responses;
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
