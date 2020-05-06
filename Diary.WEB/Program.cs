using Diary.BLL.Infrastructure;
using Diary.BLL.Models.User;
using Diary.BLL.Services.AesCryptoProvider;
using Diary.DAL.Common;
using Diary.DAL.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Diary.WEB
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			//BuildWebHost(args).Run();
			var host = BuildWebHost(args);

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var aesCryptoProviderService = services.GetRequiredService<IAesCryptoProviderService>();

					var userManager = services.GetRequiredService<UserManager<User>>();
					var rolesManager = services.GetRequiredService<RoleManager<AppRole>>();

					DbInitializer dbInitializer = new DbInitializer();
					await dbInitializer.InitializeAsync(userManager, rolesManager, aesCryptoProviderService);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
				}
			}

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			.UseStartup<Startup>()
			.ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Trace))
			.Build();
	}
}
