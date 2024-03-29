﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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

        public DbSet<Registration> Certificates { get; set; }

        public DbSet<University> Universities { get; set; }

        public DbSet<Template> Templates { get; set; }

        public DbSet<OJTBatch> OJTBatches { get; set; }

        public DbSet<TrainingPlan> TrainingPlans { get; set; }

        public DbSet<TrainingPlanDetail> TrainingPlanDetails { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<CoursePosition> CoursePositions { get; set; }

        public DbSet<UserCriteria> UserCriterias { get; set; }

        public DbSet<UserTrainingPlan> UserTrainingPlans { get; set; }

        public DbSet<UserSkill> UserSkills { get; set; }

        public DbSet<CourseSkill> CourseSkills { get; set; }

        public DbSet<TemplateHeader> TemplateHeaders { get; set; }

        public DbSet<Formula> Formulas { get; set; }

        public DbSet<TaskAccomplished> TaskAccomplisheds { get; set; }

        public DbSet<Config> Configs { get; set; }  

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCriteria>()
                .HasKey(c => new { c.UserId, c.TemplateHeaderId });

            modelBuilder.Entity<UserTrainingPlan>()
                .HasKey(c => new { c.UserId, c.TrainingPlanId });

            modelBuilder.Entity<Registration>()
                .HasKey(c => new { c.CourseId, c.UserId });

            modelBuilder.Entity<CourseSkill>()
                .HasKey(c => new { c.SkillId, c.CourseId });

            modelBuilder.Entity<UserSkill>()
                .HasKey(c => new { c.SkillId, c.UserId });

            modelBuilder.Entity<Config>().HasData(
            new Config { Id = 1, Name = "Total Working Days Per Month",  Value = 20},
            new Config { Id = 2, Name = "Work Hours Required", Value = 8}
            );

            modelBuilder.Entity<Template>()
           .HasOne(e => e.University)
           .WithMany(e => e.Templates)
           .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
