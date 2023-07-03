using DataAccessLayer.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ICriteriaRepository CriteriaRepository { get; }
        IOJTBatchRepository OJTBatchRepository { get; }
    }
}
