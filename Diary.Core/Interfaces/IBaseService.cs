using System;
using System.Collections.Generic;

namespace Diary.Core.Interfaces
{
	public interface IBaseService<TModel, TIdType, TCreateModel, TUpdateModel, TViewModel> 
		where TModel : class, IBaseEntity<TIdType>
		where TIdType : struct
		where TCreateModel : class, ICreateModel
		where TUpdateModel : class, IUpdateModel<TIdType>
		where TViewModel : class, IViewModel<TIdType>
	{
		IEnumerable<TViewModel> GetAll();

		TViewModel Get(TIdType id);

		IEnumerable<TViewModel> Find(Func<TModel, Boolean> predicate);

		TIdType Create(TCreateModel item);

		void Update(TUpdateModel item);

		void Delete(TIdType id);
	}
}
