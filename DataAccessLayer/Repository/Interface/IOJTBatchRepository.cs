using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface IOJTBatchRepository : IGenericRepository<OJTBatch>
    {
        Task<List<OJTBatch>> GetlistActiveOjtbatchWithFormula(int formulaId);

        Task<List<OJTBatch>> GetlistActiveOjtbatchWithTemplate(int temId);
    }
}
