﻿using BusinessLayer.Models.RequestModel.CriteriaRequest;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ICriteriaService
    {
        Task<Criteria> CreateCriteria(CreateCriteriaRequest request);
    }
}