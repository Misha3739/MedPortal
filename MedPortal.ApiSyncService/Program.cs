using System;
using AutoMapper;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using MedPortal.Data.Repositories;
using MedPortal.Proxy.Mapping;
using MedPortal.Proxy.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using RestSharp.Authenticators;
using ServiceLocator;
using System.Configuration;
using MedPortal.ApiSyncService.Engine;

namespace MedPortal.ApiSyncService
{
	class Program
	{
		
		static void Main(string[] args)
		{
			IServiceCollection services = new ServiceCollection();
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			services.AddTransient<IMapper>(options => mappingConfig.CreateMapper());

			DIProvider.ServiceProvider = services.BuildServiceProvider();

			var apiConfiguration = (ApiConfiguration)ConfigurationManager.GetSection(nameof(ApiConfiguration));

			services.AddScoped<IRestClient>(service => new RestClient(apiConfiguration.BaseAddress)
			{
				Authenticator = new HttpBasicAuthenticator(apiConfiguration.Login, apiConfiguration.Password)
			});

			services.AddDbContext<IDataContext, DataContext>(option => option.UseSqlServer(
				ConfigurationManager.ConnectionStrings["MedPortal"].ConnectionString
			));

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddTransient<IRepository<Log>, LogRepository>();
			services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
			services.AddTransient(typeof(IHighloadedRepository<>), typeof(HighloadedRepository<>));

			var engine = new SyncEngine(
				DIProvider.ServiceProvider.GetService<IHighloadedRepository<HCity>>(),
				DIProvider.ServiceProvider.GetService<IHighloadedRepository<HDistrict>>(),
				DIProvider.ServiceProvider.GetService<IHighloadedRepository<HStation>>(),
				DIProvider.ServiceProvider.GetService<IHighloadedRepository<HSpeciality>>(),
				DIProvider.ServiceProvider.GetService<IRepository<HClinicStations>>(),
				DIProvider.ServiceProvider.GetService<IHighloadedRepository<HClinic>>(),
				DIProvider.ServiceProvider.GetService<IHighloadedRepository<HStreet>>()
				);

			engine.SyncAll().GetAwaiter().GetResult();

			Console.ReadLine();

		}
	}

	public static class DIProvider {
		public static IServiceProvider ServiceProvider;
	}
}
