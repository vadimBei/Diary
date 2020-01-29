using Diary.Core.Interfaces;
using System;

namespace Diary.Core
{
	public class BaseEntity: IBaseEntity<Guid>
	{
		public Guid Id { get; set; }

		public DateTime DateCreation { get; set; }

		public DateTime ModifiedDate { get; set; }
	}
}
