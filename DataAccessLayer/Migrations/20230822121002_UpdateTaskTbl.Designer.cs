﻿// <auto-generated />
using System;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(OJTDbContext))]
    [Migration("20230822121002_UpdateTaskTbl")]
    partial class UpdateTaskTbl
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<bool?>("IsCompulsory")
                        .HasColumnType("bit");

                    b.Property<int?>("PositionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("PositionId");

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

            modelBuilder.Entity("DataAccessLayer.Models.Formula", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Calculation")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Formula");
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

                    b.Property<int?>("Type")
                        .HasColumnType("int");

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

                    b.Property<int?>("TemplateId")
                        .HasColumnType("int");

                    b.Property<int?>("UniversityId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.HasIndex("UniversityId");

                    b.ToTable("OJTBatch");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Position", b =>
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

                    b.ToTable("Position");
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

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TaskAccomplished", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<string>("TrelloTaskId")
                        .HasColumnType("nvarchar(40)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TaskAccomplished");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Template", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("StartCell")
                        .HasColumnType("nvarchar(5)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("UniversityId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(MAX)");

                    b.HasKey("Id");

                    b.HasIndex("UniversityId");

                    b.ToTable("Template");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TemplateHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FormulaId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsCriteria")
                        .HasColumnType("bit");

                    b.Property<string>("MatchedAttribute")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TemplateId")
                        .HasColumnType("int");

                    b.Property<double?>("TotalPoint")
                        .HasColumnType("float");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FormulaId");

                    b.HasIndex("TemplateId");

                    b.ToTable("TemplateHeader");
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

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(500)");

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

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("OJTBatchId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("PositionId")
                        .HasColumnType("int");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("ResetPassordCode")
                        .HasColumnType("nvarchar(10)");

                    b.Property<int?>("Role")
                        .HasColumnType("int");

                    b.Property<string>("RollNumber")
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StudentCode")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TrelloId")
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserReferenceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OJTBatchId");

                    b.HasIndex("PositionId");

                    b.HasIndex("UserReferenceId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DataAccessLayer.Models.UserCriteria", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("TemplateHeaderId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Point")
                        .HasColumnType("float");

                    b.Property<int?>("TrainerIdMark")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "TemplateHeaderId");

                    b.HasIndex("TemplateHeaderId");

                    b.ToTable("UserCriteria");
                });

            modelBuilder.Entity("DataAccessLayer.Models.UserSkill", b =>
                {
                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("CurrentLevel")
                        .HasColumnType("int");

                    b.Property<int?>("InitLevel")
                        .HasColumnType("int");

                    b.HasKey("SkillId", "UserId");

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

                    b.HasOne("DataAccessLayer.Models.Position", "Position")
                        .WithMany("CoursePositions")
                        .HasForeignKey("PositionId");

                    b.Navigation("Course");

                    b.Navigation("Position");
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
                    b.HasOne("DataAccessLayer.Models.Template", "Template")
                        .WithMany("OJTBatches")
                        .HasForeignKey("TemplateId");

                    b.HasOne("DataAccessLayer.Models.University", "University")
                        .WithMany("OJTBatches")
                        .HasForeignKey("UniversityId");

                    b.Navigation("Template");

                    b.Navigation("University");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TaskAccomplished", b =>
                {
                    b.HasOne("DataAccessLayer.Models.User", "User")
                        .WithMany("TaskAccomplished")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Template", b =>
                {
                    b.HasOne("DataAccessLayer.Models.University", "University")
                        .WithMany("Templates")
                        .HasForeignKey("UniversityId");

                    b.Navigation("University");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TemplateHeader", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Formula", "Formula")
                        .WithMany()
                        .HasForeignKey("FormulaId");

                    b.HasOne("DataAccessLayer.Models.Template", "Template")
                        .WithMany("TemplateHeaders")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Formula");

                    b.Navigation("Template");
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

                    b.HasOne("DataAccessLayer.Models.Position", "Position")
                        .WithMany("Users")
                        .HasForeignKey("PositionId");

                    b.HasOne("DataAccessLayer.Models.User", "Trainer")
                        .WithMany("Trainees")
                        .HasForeignKey("UserReferenceId");

                    b.Navigation("OJTBatch");

                    b.Navigation("Position");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("DataAccessLayer.Models.UserCriteria", b =>
                {
                    b.HasOne("DataAccessLayer.Models.TemplateHeader", "TemplateHeader")
                        .WithMany("UserCriterias")
                        .HasForeignKey("TemplateHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Models.User", "User")
                        .WithMany("UserCriterias")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TemplateHeader");

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

            modelBuilder.Entity("DataAccessLayer.Models.OJTBatch", b =>
                {
                    b.Navigation("Trainees");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Position", b =>
                {
                    b.Navigation("CoursePositions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Skill", b =>
                {
                    b.Navigation("CourseSkills");

                    b.Navigation("UserSkills");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Template", b =>
                {
                    b.Navigation("OJTBatches");

                    b.Navigation("TemplateHeaders");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TemplateHeader", b =>
                {
                    b.Navigation("UserCriterias");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TrainingPlan", b =>
                {
                    b.Navigation("TrainingPlanDetails");

                    b.Navigation("UserTrainingPlans");
                });

            modelBuilder.Entity("DataAccessLayer.Models.University", b =>
                {
                    b.Navigation("OJTBatches");

                    b.Navigation("Templates");
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
