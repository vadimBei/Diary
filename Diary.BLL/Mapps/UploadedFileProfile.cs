using AutoMapper;
using Diary.BLL.Models.UploadedFile;
using Diary.DAL.Entities;

namespace Diary.BLL.Mapps
{
	public class UploadedFileProfile : Profile
	{
		public UploadedFileProfile()
		{
			CreateMap<UploadedFileCreateModel, UploadedFile>()
				.ForMember(dest => dest.Id, opt => opt.Ignore());

			CreateMap<UploadedFileUpdateModel, UploadedFile>()
				.IncludeBase<UploadedFileCreateModel, UploadedFile>();

			CreateMap<UploadedFile, UploadedFileModel>()
				.ReverseMap();
		}
	}
}
