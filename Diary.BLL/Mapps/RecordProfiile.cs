using AutoMapper;
using Diary.BLL.Models.Record;
using Diary.DAL.Entities;

namespace Diary.BLL.Mapps
{
	public class RecordProfile : Profile
	{
		public RecordProfile()
		{
			CreateMap<RecordCreateModel, Record>()
				.ForMember(dest => dest.Id, opt => opt.Ignore());

			CreateMap<RecordUpdateModel, Record>()
				.IncludeBase<RecordCreateModel, Record>();

			CreateMap<Record, RecordModel>()
				.ReverseMap();
		}
	}
}
