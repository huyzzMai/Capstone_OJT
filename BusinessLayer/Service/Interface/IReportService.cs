using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IReportService
    {
       Task<byte[]> ExportReportExcelFileFromUniversity(string index,byte[] excelStream, int templateid);
    }
}
