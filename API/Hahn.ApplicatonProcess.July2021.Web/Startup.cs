using FluentValidation.AspNetCore;
using Hahn.ApplicationProcess.July2021.Data.Repository;
using Hahn.ApplicationProcess.July2021.Data.Service.Implementations;
using Hahn.ApplicationProcess.July2021.Data.Service.Interfaces;
using Hahn.ApplicationProcess.July2021.Domain;
using Hahn.ApplicationProcess.July2021.Web.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Hahn.ApplicationProcess.July2021.Web
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

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            services.AddSignalR(e => {
                e.EnableDetailedErrors = true;
                e.MaximumReceiveMessageSize = 102400000;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("http://localhost:8080");
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hahn.ApplicatonProcess.July2021.Web", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddFluentValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddControllers().AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()).Filter().Select().OrderBy().Expand().SetMaxTop(null).Count());

            //This is could be AddDbContextPool, depends on the use case
            services.AddDbContext<Context>(opts => opts.UseInMemoryDatabase(EnvironmentVariables.DatabaseName));

            //Register Services
            services.AddTransient<IGetAssetsService, GetAssetsService>();
            services.AddTransient<IAddUserService, AddUserService>();
            services.AddTransient<IUpdateService, UpdateService>();
            services.AddTransient<IDeleteUserService, DeleteUserService>();
            services.AddTransient<IGetAssetsRequestService, GetAssetsMockedRequestService>(); //Mock: GetAssetsMockedRequestService

            //cache 
            services.AddMemoryCache();

            //UoW
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Repositories
            services.AddTransient<IUserRepository, UserRepository>();
        }

        private IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();

            //odataBuilder.EntitySet<User>("users");
            //odataBuilder.EntitySet<Asset>("assets");

            odataBuilder.EnableLowerCamelCase();

            return odataBuilder.GetEdmModel();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hahn.ApplicatonProcess.July2021.Web v1"));
                app.UseExceptionHandler("/api/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/api/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AssetsHub>("/hubs/assets");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
