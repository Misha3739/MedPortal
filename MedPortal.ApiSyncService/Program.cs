﻿using System;
using System.Collections.Generic;
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
using System.Globalization;
using System.Threading.Tasks;
using MedPortal.ApiSyncService.Engine;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;

namespace MedPortal.ApiSyncService
{
	class Program
	{
		
		static void Main(string[] args) {
			RegisterServices();
			

			var engine = DIProvider.ServiceProvider.GetService<SyncEngine>();

			try {
				engine.SyncAll().GetAwaiter().GetResult();
			} catch (Exception e) {
				Console.WriteLine(e);
			}
			

			DisposeServices();

			Console.ReadLine();

		}

		private static void RegisterServices()
		{
			IServiceCollection services = new ServiceCollection();
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			services.AddTransient<IMapper>(options => mappingConfig.CreateMapper());



			var apiConfiguration = new ApiConfiguration() {
				BaseAddress = ConfigurationManager.AppSettings["BaseAddress"],
				Login = ConfigurationManager.AppSettings["Login"],
				Password = ConfigurationManager.AppSettings["Password"]
			};


			services.AddTransient<IRestClient>(service => new RestClient(apiConfiguration.BaseAddress)
			{
				Authenticator = new HttpBasicAuthenticator(apiConfiguration.Login, apiConfiguration.Password)
			});

			services.AddDbContext<IDataContext, DataContext>(option => option.UseSqlServer(
				ConfigurationManager.ConnectionStrings["MedPortal"].ConnectionString
			), ServiceLifetime.Transient);

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddTransient<IRepository<Log>, LogRepository>();
			services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
			services.AddTransient(typeof(IHighloadedRepository<>), typeof(HighloadedRepository<>));

			services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

			services.AddTransient<SyncEngine>();

			services.AddLogging(configure => configure.AddConsole());

			services.Configure<RequestLocalizationOptions>(options =>
				new RequestLocalizationOptions {
					DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US")),
					SupportedCultures = new List<CultureInfo> {
						new CultureInfo("ru-RU")
					}
				});

			DIProvider.ServiceProvider = services.BuildServiceProvider();
		}

		private static void DisposeServices()
		{
			if (DIProvider.ServiceProvider == null)
			{
				return;
			}
			if (DIProvider.ServiceProvider is IDisposable)
			{
				((IDisposable)DIProvider.ServiceProvider).Dispose();
			}
		}
	}

	public static class DIProvider {
		public static IServiceProvider ServiceProvider;
	}
}