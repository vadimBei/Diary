using Diary.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;


namespace Diary.DAL.Common
{
	public class DbInitializer
	{
		public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<AppRole> roleManager)
		{
			string adminEmail = "bey1705@gmail.com";
			string adminPassword = "Vados@17";
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
				var admin = new User {
					Email = adminEmail,
					UserName = adminName,
					DateCreation = DateTime.Now,
					ModifiedDate = DateTime.Now
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
