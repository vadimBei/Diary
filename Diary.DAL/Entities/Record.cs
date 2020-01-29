using Diary.Core;
using System;
using System.Collections.Generic;

namespace Diary.DAL.Entities
{
	public class Record : BaseEntity
	{
		public string Name { get; set; }

		public byte[] Text { get; set; }

		public Guid UserId { get; set; }

		public User User { get; set; }

		public byte[] IvKey { get; set; }

		public List<UploadedFile> UploadedFiles { get; set; }
	}
}
