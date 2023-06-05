using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class OJTDbContext : DbContext
    {
        public OJTDbContext(DbContextOptions options) : base(options) { }

        #region DbSet
        public DbSet<Criteria> Criterias { get; set; }
        public DbSet<CriteriaDetail> CriteriaDetails { get; set; }
        #endregion
    }
}
