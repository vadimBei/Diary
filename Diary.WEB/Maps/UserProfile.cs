using AutoMapper;
using Diary.BLL.Models.User;
using Diary.WEB.ViewModels.User;

namespace Diary.WEB.Maps
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<UserViewModel, UserCreateModel>().ReverseMap();
		}
	}
}
