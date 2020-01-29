using AutoMapper;
using Diary.BLL.Models.Invite;
using Diary.DAL.Entities;

namespace Diary.BLL.Mapps
{
	public class InviteProfile : Profile
	{
		public InviteProfile()
		{
			CreateMap<InviteCreateModel, Invite>()
				.ForMember(dest => dest.Id, opt => opt.Ignore());

			CreateMap<InviteUpdateModel, Invite>()
				.IncludeBase<InviteCreateModel, Invite>();

			CreateMap<Invite, InviteModel>()
				.ReverseMap();
		}
	}
}
