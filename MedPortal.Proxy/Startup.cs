using AutoMapper;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using MedPortal.Data.Repositories;
using MedPortal.Proxy.Mapping;
using MedPortal.Proxy.Middleware;
using MedPortal.Proxy.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;

namespace MedPortal.Proxy
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
                
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddScoped<IRestClient>(service => new RestClient(apiConfiguration.BaseAddress)
            {
                Authenticator = new HttpBasicAuthenticator(apiConfiguration.Login, apiConfiguration.Password)
            });

            services.AddDbContext<IDataContext, DataContext>(option => option.UseSqlServer(
                Configuration.GetConnectionString("MedPortal")
                ));
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IHighloadedRepository<HCity>, CityRepository>();
            services.AddTransient<IHighloadedRepository<HBranch>, BranchRepository>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseMiddleware<ExceptionMiddleware>();

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}