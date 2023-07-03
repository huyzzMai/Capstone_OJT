using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.ExcelRequest
{
    public class ReportExcelRequest
    {
        public int BatchId { get; set; }
        public List<PairDataExcel> PairDataExcel { get; set; }
    }
}
