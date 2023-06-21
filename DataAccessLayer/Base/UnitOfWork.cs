using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Implement;
using DataAccessLayer.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OJTDbContext _context;
        public UnitOfWork(OJTDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository => new UserRepository(_context, this);

        public ICriteriaRepository CriteriaRepository => new CriteriaRepository(_context, this);
    }
}
