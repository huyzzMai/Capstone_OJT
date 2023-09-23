using API.Hubs;
using BusinessLayer.Service;
using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Base;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            services.AddDbContext<OJTDbContext>(
               options => options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            // Use for user repo and service

            //services.AddScoped<IUserRepository, UserRepository>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITemplateHeaderService, TemplateHeaderService>();
            services.AddScoped<IOJTBatchService, OJTBatchService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<ITrainingPlanService, TrainingPlanService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IUserCriteriaService, UserCriteriaService>();
            services.AddScoped<IFormulaService, FormulaService>();
            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped<IChartService, ChartService>();
            services.AddSingleton<OperandService>();
            services.AddHttpClient();


            services.AddControllersWithViews()
                .AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters();

            services.AddControllers()
                .AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["Jwt:Audience"],
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://kns-ojt-management.onrender.com",
                        "https://trello.com",
                        "https://api.trello.com"
                        )
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });
            services.AddSignalR();
            services.AddMemoryCache();
            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using bearer scheme (\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                opt.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalHub>("/signalhub");
            });

        }
    }
}
