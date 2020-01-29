using Microsoft.AspNetCore.Identity;
using System;

namespace Diary.DAL.Entities
{
	public partial class RoleClaim : IdentityRoleClaim<Guid>
	{

	}

	public partial class UserLogin : IdentityUserLogin<Guid>
	{

	}

	public partial class UserClaim : IdentityUserClaim<Guid>
	{

	}

	public partial class UserRole : IdentityUserRole<Guid>
	{

	}

	public partial class UserToken : IdentityUserToken<Guid>
	{

	}

	public partial class AppRole : IdentityRole<Guid>
	{

	}
}