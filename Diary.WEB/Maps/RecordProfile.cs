using AutoMapper;
using Diary.BLL.Models.Record;
using Diary.WEB.ViewModels.Record;

namespace Diary.WEB.Maps
{
	public class RecordProfile : Profile
	{
		public RecordProfile()
		{
			CreateMap<RecordViewModel, RecordCreateModel>().ReverseMap();

			//CreateMap<RecordViewModel, RecordUpdateModel>().ReverseMap();
		}
	}
}
