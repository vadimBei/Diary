using Diary.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Diary.DAL.Common
{
	public class ApplicationDbContext : IdentityDbContext<User, AppRole, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<RoleClaim>().HasKey(p => new { p.Id });
			builder.Entity<UserLogin>().HasKey(p => new { p.UserId });
			builder.Entity<UserClaim>().HasKey(p => new { p.Id });
			builder.Entity<UserRole>().HasKey(p => new { p.UserId, p.RoleId });
			builder.Entity<UserToken>().HasKey(p => new { p.UserId });
			builder.Entity<AppRole>().HasKey(p => new { p.Id });

			base.OnModelCreating(builder);
		}

		public DbSet<Invite> Invites { get; set; }
		public DbSet<UploadedFile> UploadedFiles { get; set; }
		public DbSet<Record> Records { get; set; }
	}
}
