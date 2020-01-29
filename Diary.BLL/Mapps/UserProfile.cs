using AutoMapper;
using Diary.BLL.Models.User;
using Diary.DAL.Entities;

namespace Diary.BLL.Mapps
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<UserCreateModel, User>()
				.ForMember(dest => dest.Id, opt => opt.Ignore());

			CreateMap<UserUpdateModel, User>()
				.IncludeBase<UserCreateModel, User>();

			CreateMap<User, UserModel>()
				.ReverseMap();
		}
	}
}
