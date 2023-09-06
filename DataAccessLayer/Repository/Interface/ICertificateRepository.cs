﻿using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface ICertificateRepository : IGenericRepository<Register>
    {
        Task<Register> GetCertificateWithUserAndCourse(int userId, int couseId);
        Task<List<Register>> GetListCertificateOfTraineeWithUserAndCourse(int userId);
    }
}
