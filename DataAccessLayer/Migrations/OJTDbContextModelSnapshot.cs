﻿// <auto-generated />
using System;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(OJTDbContext))]
    partial class OJTDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataAccessLayer.Models.Attendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("PresentDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("TotalTime")
                        .HasColumnType("time");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Attendance");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Certificate", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EnrollDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SubmitDate")
                        .HasColumnType("datetime2");

                    b.HasKey("CourseId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("Certificate");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PlatformName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("DataAccessLayer.Models.CoursePosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsCompulsory")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("CoursePosition");
                });

            modelBuilder.Entity("DataAccessLayer.Models.CourseSkill", b =>
                {
                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int?>("AfterwardLevel")
                        .HasColumnType("int");

                    b.Property<int?>("RecommendedLevel")
                        .HasColumnType("int");

                    b.HasKey("SkillId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("CourseSkill");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Criteria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("TotalPoint")
                        .HasColumnType("int");

                    b.Property<int>("UniversityId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UniversityId");

                    b.ToTable("Criteria");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("DataAccessLayer.Models.OJTBatch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UniversityId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UniversityId");

                    b.ToTable("OJTBatch");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TaskAccomplished", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTimeOffset?>("AccomplishDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<DateTimeOffset?>("DueDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTimeOffset?>("StartDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TaskAccomplished");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TrainingPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("TrainingPlan");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TrainingPlanDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsEvaluativeTask")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("TrainingPlanId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TrainingPlanId");

                    b.ToTable("TrainingPlanDetail");
                });

            modelBuilder.Entity("DataAccessLayer.Models.University", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImgURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("University");
                });

            modelBuilder.Entity("DataAccessLayer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("AvatarURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Birthday")
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("OJTBatchId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ResetPassordCode")
                        .HasColumnType("nvarchar(10)");

                    b.Property<int?>("Role")
                        .HasColumnType("int");

                    b.Property<string>("RollNumber")
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TrelloId")
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserReferenceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OJTBatchId");

                    b.HasIndex("UserReferenceId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DataAccessLayer.Models.UserCriteria", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CriteriaId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Point")
                        .HasColumnType("int");

                    b.Property<int?>("TrainerIdMark")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CriteriaId");

                    b.HasIndex("CriteriaId");

                    b.ToTable("UserCriteria");
                });

            modelBuilder.Entity("DataAccessLayer.Models.UserSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CurrentLevel")
                        .HasColumnType("int");

                    b.Property<int?>("InitLevel")
                        .HasColumnType("int");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SkillId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSkill");
                });

            modelBuilder.Entity("DataAccessLayer.Models.UserTrainingPlan", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("TrainingPlanId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsOwner")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "TrainingPlanId");

                    b.HasIndex("TrainingPlanId");

                    b.ToTable("UserTrainingPlan");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Attendance", b =>
                {
                    b.HasOne("DataAccessLayer.Models.User", "User")
                        .WithMany("Attendances")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Certificate", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Course", "Course")
                        .WithMany("Certificates")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Models.User", "User")
                        .WithMany("Certificates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Models.CoursePosition", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Course", "Course")
                        .WithMany("CoursePositions")
                        .HasForeignKey("CourseId");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("DataAccessLayer.Models.CourseSkill", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Course", "Course")
                        .WithMany("CourseSkills")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Models.Skill", "Skill")
                        .WithMany("CourseSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Criteria", b =>
                {
                    b.HasOne("DataAccessLayer.Models.University", "University")
                        .WithMany("Criterias")
                        .HasForeignKey("UniversityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("University");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Notification", b =>
                {
                    b.HasOne("DataAccessLayer.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Models.OJTBatch", b =>
                {
                    b.HasOne("DataAccessLayer.Models.University", "University")
                        .WithMany("OJTBatches")
                        .HasForeignKey("UniversityId");

                    b.Navigation("University");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TaskAccomplished", b =>
                {
                    b.HasOne("DataAccessLayer.Models.User", "User")
                        .WithMany("TaskAccomplished")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TrainingPlanDetail", b =>
                {
                    b.HasOne("DataAccessLayer.Models.TrainingPlan", "TrainingPlan")
                        .WithMany("TrainingPlanDetails")
                        .HasForeignKey("TrainingPlanId");

                    b.Navigation("TrainingPlan");
                });

            modelBuilder.Entity("DataAccessLayer.Models.User", b =>
                {
                    b.HasOne("DataAccessLayer.Models.OJTBatch", "OJTBatch")
                        .WithMany("Trainees")
                        .HasForeignKey("OJTBatchId");

                    b.HasOne("DataAccessLayer.Models.User", "Trainer")
                        .WithMany("Trainees")
                        .HasForeignKey("UserReferenceId");

                    b.Navigation("OJTBatch");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("DataAccessLayer.Models.UserCriteria", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Criteria", "Criteria")
                        .WithMany("UserCriterias")
                        .HasForeignKey("CriteriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Models.User", "User")
                        .WithMany("UserCriterias")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Criteria");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Models.UserSkill", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Skill", "Skill")
                        .WithMany("UserSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Models.User", "User")
                        .WithMany("UserSkills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skill");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Models.UserTrainingPlan", b =>
                {
                    b.HasOne("DataAccessLayer.Models.TrainingPlan", "TrainingPlan")
                        .WithMany("UserTrainingPlans")
                        .HasForeignKey("TrainingPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Models.User", "User")
                        .WithMany("UserTrainingPlans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingPlan");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Course", b =>
                {
                    b.Navigation("Certificates");

                    b.Navigation("CoursePositions");

                    b.Navigation("CourseSkills");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Criteria", b =>
                {
                    b.Navigation("UserCriterias");
                });

            modelBuilder.Entity("DataAccessLayer.Models.OJTBatch", b =>
                {
                    b.Navigation("Trainees");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Skill", b =>
                {
                    b.Navigation("CourseSkills");

                    b.Navigation("UserSkills");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TrainingPlan", b =>
                {
                    b.Navigation("TrainingPlanDetails");

                    b.Navigation("UserTrainingPlans");
                });

            modelBuilder.Entity("DataAccessLayer.Models.University", b =>
                {
                    b.Navigation("Criterias");

                    b.Navigation("OJTBatches");
                });

            modelBuilder.Entity("DataAccessLayer.Models.User", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("Certificates");

                    b.Navigation("Notifications");

                    b.Navigation("TaskAccomplished");

                    b.Navigation("Trainees");

                    b.Navigation("UserCriterias");

                    b.Navigation("UserSkills");

                    b.Navigation("UserTrainingPlans");
                });
#pragma warning restore 612, 618
        }
    }
}
