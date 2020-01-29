using Diary.BLL.Models.Record;
using Diary.Core.Interfaces;
using System;

namespace Diary.BLL.Models.UploadedFile
{
	public class UploadedFileCreateModel : ICreateModel
	{
		public string Name { get; set; }

		public string Path { get; set; }

		public Guid RecordId { get; set; }

		public DateTime DateCreation { get; set; }

		public DateTime ModifiedDate { get; set; }
	}

	public class UploadedFileUpdateModel : UploadedFileCreateModel, IUpdateModel<Guid>
	{
		public Guid Id { get; set; }
	}

	public class UploadedFileModel : UploadedFileUpdateModel, IViewModel<Guid>
	{
		public RecordModel RecordModel { get; set; }
	}
}
