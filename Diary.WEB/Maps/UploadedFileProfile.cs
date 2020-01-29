using AutoMapper;
using Diary.BLL.Models.UploadedFile;
using Diary.WEB.ViewModels.UploadedFile;

namespace Diary.WEB.Maps
{
	public class UploadedFileProfile : Profile
	{
		public UploadedFileProfile()
		{
			CreateMap<UploadedFileViewModel, UploadedFileCreateModel>().ReverseMap();
		}
	}
}
