using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.OJTBatchResponse
{
    public class ListActiveOjtbatchforTrainer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int? NumberTrainee { get; set; }

        public string Status { get; set; }

        public string UniversityCode { get; set; }

        public string UniversityName { get; set; }

    }
}
