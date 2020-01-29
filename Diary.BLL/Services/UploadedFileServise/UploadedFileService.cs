using AutoMapper;
using Diary.BLL.Models.UploadedFile;
using Diary.BLL.Services.UserService;
using Diary.Core;
using Diary.DAL.Common;
using Diary.DAL.Entities;
using System;

namespace Diary.BLL.Services.UploadedFileServise
{
	public class UploadedFileService : BaseService<UploadedFile, Guid, UploadedFileCreateModel, UploadedFileUpdateModel, UploadedFileModel>, IUploadedFileService
	{
		public UploadedFileService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
		{
		}
	}
}
