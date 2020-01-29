using AutoMapper;
using Diary.BLL.Services.AesCryptoProvider;
using Diary.BLL.Services.CaptchaService;
using Diary.BLL.Services.EmailSender;
using Diary.BLL.Services.InviteService;
using Diary.BLL.Services.RecordService;
using Diary.BLL.Services.SearchService;
using Diary.BLL.Services.SortingService;
using Diary.BLL.Services.UploadedFileServise;
using Diary.BLL.Services.UserService;
using Diary.DAL.Common;
using Diary.DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Diary.WEB
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
			services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<User, AppRole>(opts =>
			{
				opts.Password.RequireUppercase = false;
			})
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddScoped<IEmailSenderService, EmailSenderService>();
			services.AddScoped<IAesCryptoProviderService, AesCryptoProviderService>();
			services.AddScoped<IRecordService, RecordService>();
			services.AddScoped<IInviteService, InviteService>();
			services.AddScoped<ICaptchaService, CaptchaService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IUploadedFileService, UploadedFileService>();
			services.AddScoped<ISearchService, SearchService>();
			services.AddScoped<ISortingService, SortingService>();

			services.Configure<FormOptions>(options =>
			{
				// Set the limit to 256 MB
				options.MultipartBodyLengthLimit = 268435456; 
			});

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(20);
				options.Cookie.HttpOnly = true;
			});

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			loggerFactory.AddDebug();
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseAuthentication();
			app.UseSession();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Account}/{action=Login}/{id?}");
				routes.MapRoute("Register", "Register/{*pathinfo}", new { Controller = "Account", Action = "Register" });
			});
		}
	}
}
