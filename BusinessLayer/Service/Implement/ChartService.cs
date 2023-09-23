using BusinessLayer.Service.Interface;
using DataAccessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class ChartService: IChartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
