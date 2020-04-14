using Diary.Core;
using System;

namespace Diary.DAL.Entities
{
	public class UploadedFile : BaseEntity
	{
		public UploadedFile()
		{
		}

		public UploadedFile(UploadedFile file)
		{
			this.Id = file.Id;
			this.Name = file.Name;
			this.Path = file.Path;
			this.Record = file.Record;
		}

		public string Name { get; set; }

		public string Path { get; set; }

		public bool IsImage { get; set; }

		public Guid RecordId { get; set; }

		public Record Record { get; set; }
	}
}
