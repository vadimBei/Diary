using Diary.BLL.Models.UploadedFile;
using Diary.Core.Interfaces;
using Diary.DAL.Entities;
using System;

namespace Diary.BLL.Services.UploadedFileServise
{
	public interface IUploadedFileService : IBaseService<UploadedFile, Guid, UploadedFileCreateModel, UploadedFileUpdateModel, UploadedFileModel>
	{
	}
}
