using Diary.BLL.Models.UploadedFile;
using Diary.BLL.Models.User;
using Diary.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Diary.BLL.Models.Record
{
	public class RecordCreateModel : ICreateModel
	{
		public string Name { get; set; }

		public byte[] Text { get; set; }

		public Guid UserId { get; set; }

		public byte[] IvKey { get; set; }

		public List<UploadedFileModel> UploadedFileModels { get; set; }

		public DateTime DateCreation { get; set; }

		public DateTime ModifiedDate { get; set; }

	}

	public class RecordUpdateModel : RecordCreateModel, IUpdateModel<Guid>
	{
		public Guid Id { get; set; }
	}

	public class RecordModel : RecordUpdateModel, IViewModel<Guid>
	{
		public UserModel UserModel { get; set; }
	}
}
