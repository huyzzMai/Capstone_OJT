using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class OJTDbContext : DbContext
    {
        public OJTDbContext(DbContextOptions options) : base(options) { }

        #region DbSet
        public DbSet<User> Users { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Certificate> Certificates { get; set; }

        public DbSet<Criteria> Criterias { get; set; }

        public DbSet<CriteriaDetail> CriteriaDetails { get; set; }

        public DbSet<University> Universities { get; set; }

        public DbSet<OJTBatch> OJTBatches { get; set; }

        public DbSet<TrainingPlan> TrainingPlans { get; set; }

        public DbSet<TrainingPlanDetail> TrainingPlanDetails { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<CoursePosition> CoursePositions { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", true, true)
                 .Build();
                String connectionString = config["ConnectionStrings:DBConnection"];
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
