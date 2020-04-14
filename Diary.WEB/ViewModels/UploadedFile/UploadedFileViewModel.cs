using Diary.BLL.Models;
using Diary.WEB.ViewModels.Record;
using System;

namespace Diary.WEB.ViewModels.UploadedFile
{
	public class UploadedFileViewModel : BaseModel
	{
		public string Name { get; set; }

		public string Path { get; set; }

		public bool IsImage { get; set; }

		public Guid RecordId { get; set; }

		public RecordViewModel RecordViewModel { get; set; }
	}
}
