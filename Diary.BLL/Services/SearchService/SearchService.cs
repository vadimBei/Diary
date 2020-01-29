using Diary.BLL.Models.Record;
using Diary.BLL.Models.SearchOfRecord;
using Diary.BLL.Services.RecordService;
using Diary.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diary.BLL.Services.SearchService
{
	public class SearchService : ISearchService
	{
		private readonly UserManager<User> _userManager;
		private readonly IRecordService _recordService;
		private readonly IHttpContextAccessor _accessor;

		public SearchService(
			IRecordService recordService,
			UserManager<User> userManager,
			IHttpContextAccessor accessor)
		{
			_recordService = recordService;
			_userManager = userManager;
			_accessor = accessor;
		}

		public async Task<IEnumerable<RecordModel>> RecordsByCurrentUser(SearchModel searchModel)
		{
			var minDate = DateTime.MinValue;

			var currentUserName = _accessor.HttpContext.User.Identity.Name;
			var currentUser = await _userManager.FindByNameAsync(currentUserName);

			var recordByCurrentUser = _recordService.GetAll().Where(r => r.UserId == currentUser.Id);

			if (searchModel.StartDate != minDate && searchModel.EndDate != minDate)
			{
				recordByCurrentUser = recordByCurrentUser.Where(r => r.DateCreation >= searchModel.StartDate && r.DateCreation <= searchModel.EndDate);
			}
			else if (searchModel.EndDate != DateTime.MinValue)
			{
				recordByCurrentUser = recordByCurrentUser.Where(r => r.DateCreation.Date <= searchModel.EndDate.Date);
			}
			else if (searchModel.StartDate != DateTime.MinValue)
			{
				recordByCurrentUser = recordByCurrentUser.Where(r => r.DateCreation >= searchModel.StartDate);
			}

			return recordByCurrentUser;
		}
	}
}
