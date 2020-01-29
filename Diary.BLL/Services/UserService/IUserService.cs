using Diary.BLL.Models.User;
using Diary.Core.Interfaces;
using Diary.DAL.Entities;
using System;

namespace Diary.BLL.Services.UserService
{
	public interface IUserService : IBaseService<User, Guid, UserCreateModel, UserUpdateModel, UserModel>
	{
	}
}
