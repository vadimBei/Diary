using AutoMapper;
using Diary.BLL.Models.User;
using Diary.Core;
using Diary.DAL.Common;
using Diary.DAL.Entities;
using System;

namespace Diary.BLL.Services.UserService
{
	public class UserService : BaseService<User, Guid, UserCreateModel, UserUpdateModel, UserModel>, IUserService
	{
		public UserService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
		{
		}
	}
}
