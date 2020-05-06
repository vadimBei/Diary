using Diary.BLL.Services.AesCryptoProvider;
using Diary.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;


namespace Diary.BLL.Infrastructure
{
	public class DbInitializer
	{
		public DbInitializer()
		{
		}

		public async Task InitializeAsync(UserManager<User> userManager, RoleManager<AppRole> roleManager, IAesCryptoProviderService aesCryptoProviderService)
		{
			string adminEmail = "admin@gmail.com";
			string adminPassword = "Admin@49";
			string adminName = "Adminchuk";

			if (await roleManager.FindByNameAsync("admin") == null)
			{
				await roleManager.CreateAsync(new AppRole() { Name = "admin" });
			}
			if (await roleManager.FindByNameAsync("user") == null)
			{
				await roleManager.CreateAsync(new AppRole() { Name = "user" });
			}
			if (await userManager.FindByNameAsync(adminEmail) == null)
			{
				var key = aesCryptoProviderService.GenerateKey();

				var admin = new User()
				{
					Email = adminEmail,
					UserName = adminName,
					DateCreation = DateTime.Now,
					ModifiedDate = DateTime.Now,
					CryptoKey = key
				};

				IdentityResult result = await userManager.CreateAsync(admin, adminPassword);

				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(admin, "admin");
				}
			}
		}
	}
}
