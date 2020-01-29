using System;

namespace Diary.BLL.Models
{
	public class BaseModel
	{
		public Guid Id { get; set; }

		public DateTime DateCreation { get; set; }

		public DateTime ModifiedDate { get; set; }
	}
}
