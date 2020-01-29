using System;
using System.Collections.Generic;
using System.Text;

namespace Diary.Core.Interfaces
{
	public interface ICreateModel
	{
		DateTime DateCreation { get; set; }

		DateTime ModifiedDate { get; set; }
	}

	public interface IUpdateModel<TIdType> : ICreateModel
		where TIdType : struct
	{
		TIdType Id { get; }
	}

	public interface IViewModel<TIdType> : IUpdateModel<TIdType>
		where TIdType : struct
	{
	}
}
