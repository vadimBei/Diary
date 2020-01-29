using AutoMapper;
using Diary.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diary.Core
{
	public class BaseService<TModel, TIdType, TCreateModel, TUpdateModel, TViewModel> :
		IBaseService<TModel, TIdType, TCreateModel, TUpdateModel, TViewModel>
		where TIdType : struct
		where TModel : class, IBaseEntity<TIdType>
		where TCreateModel : class, ICreateModel
		where TUpdateModel : class, IUpdateModel<TIdType>
		where TViewModel : class, IViewModel<TIdType>
	{
		private readonly DbContext _dbContext;
		private readonly IMapper _mapper;

		protected BaseService(
			DbContext dbContext,
			IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		protected DbSet<TModel> DbSet
		{
			get
			{
				return _dbContext.Set<TModel>();
			}
		}

		public TIdType Create(TCreateModel item)
		{
			var createModel = _mapper.Map<TModel>(item);
			var model = DbSet.Add(createModel);
			_dbContext.SaveChanges();

			return model.Entity.Id;
		}

		public void Delete(TIdType id)
		{
			var item = DbSet.FirstOrDefault(x => x.Id.Equals(id));

			if (item != null)
			{
				_dbContext.Remove(item);
				_dbContext.SaveChanges();
			}
		}

		public IEnumerable<TViewModel> Find(Func<TModel, bool> predicate)
		{
			var result = DbSet.Where(predicate);
			return _mapper.Map<IEnumerable<TViewModel>>(result);
		}

		public TViewModel Get(TIdType id)
		{
			var item = DbSet.FirstOrDefault(x => x.Id.Equals(id));

			if (item != null)
			{
				return _mapper.Map<TViewModel>(item);
			}

			return null;
		}

		public IEnumerable<TViewModel> GetAll()
		{
			return _mapper.Map<IEnumerable<TViewModel>>(DbSet);
		}

		public void Update(TUpdateModel item)
		{
			var existingItem = DbSet.FirstOrDefault(x => x.Id.Equals(item.Id));

			if (existingItem != null)
			{	
				_mapper.Map(item, existingItem);
				_dbContext.Update(existingItem);
				_dbContext.SaveChanges();
			}
		}
	}
}
