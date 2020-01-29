using AutoMapper;
using Diary.BLL.Models.Invite;
using Diary.WEB.ViewModels.Invite;

namespace Diary.WEB.Maps
{
	public class InviteProfile : Profile
	{
		public InviteProfile()
		{
			CreateMap<InviteViewModel, InviteCreateModel>().ReverseMap();

			//CreateMap<InviteCreateModel, InviteViewModel>().ReverseMap();
		}
	}
}
