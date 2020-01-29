using System;

namespace Diary.Core.Interfaces
{
	public interface IBaseEntity<TId>
		where TId : struct
	{
		TId Id { get; }

		DateTime DateCreation { get; set; }

		DateTime ModifiedDate { get; set; }
	}
}
