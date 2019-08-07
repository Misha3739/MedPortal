using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using MedPortal.Data.Repositories;
using MedPortal.Proxy.Mapping;
using MedPortal.Proxy.Middleware;
using MedPortal.Proxy.Utility;
using Rest;
using Microsoft.EntityFrameworkCore;

namespace MedPortal.WebApiClient
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
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            services.AddScoped(options => mappingConfig.CreateMapper());


            var apiConfiguration = Configuration.GetSection(nameof(ApiConfiguration)).Get<ApiConfiguration>();

            services.AddCors(options => {
                options.AddPolicy("AllowClientAppOrigin",
                    builder => builder
                    .WithOrigins("localhost:4200", "http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    //builder => builder.AllowAnyOrigin()
                    );
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IRestClient, RestClient>(/*service => new RestClient()
            {
                BaseAddress = apiConfiguration.BaseAddress
                Authenticator = new HttpBasicAuthenticator(apiConfiguration.Login, apiConfiguration.Password)
            }*/);

            services.AddDbContext<IDataContext, DataContext>(option => option.UseSqlServer(
                Configuration.GetConnectionString("MedPortal")
                ));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IRepository<Log>, LogRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IRepository<HDoctor>, DoctorRepository>();
            services.AddTransient<IRepository<HClinic>, ClinicRepository>();
            services.AddTransient<IClinicRepository, ClinicRepository>();
            services.AddTransient<IDoctorRepository, DoctorRepository>();
            services.AddTransient(typeof(IHighloadedRepository<>), typeof(HighloadedRepository<>));

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseCors("AllowClientAppOrigin");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            

            //app.UseSpa(spa =>
            //{
            //    // To learn more about options for serving an Angular SPA from ASP.NET Core,
            //    // see https://go.microsoft.com/fwlink/?linkid=864501

            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseAngularCliServer(npmScript: "start");
            //    }
            //});
        }
    }
}
