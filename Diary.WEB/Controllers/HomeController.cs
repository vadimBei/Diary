using AutoMapper;
using Diary.BLL.Models.Record;
using Diary.BLL.Models.SearchOfRecord;
using Diary.BLL.Services.AesCryptoProvider;
using Diary.BLL.Services.SearchService;
using Diary.BLL.Services.SortingService;
using Diary.DAL.Entities;
using Diary.WEB.ViewModels.Common;
using Diary.WEB.ViewModels.Record;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diary.WEB.Controllers
{
	[Authorize(Roles = "admin, user")]
	public class HomeController : Controller
	{
		private readonly IAesCryptoProviderService _aesCryptoProvider;
		private readonly ISortingService _sortingService;
		private readonly UserManager<User> _userManager;
		private readonly ISearchService _searchService;
		private readonly IMapper _mapper;

		public HomeController(
			IAesCryptoProviderService aesCryptoProvider,
			ISortingService sortingService,
			UserManager<User> userManager,
			ISearchService searchService,
			IMapper mapper)
		{
			_aesCryptoProvider = aesCryptoProvider;
			_sortingService = sortingService;
			_searchService = searchService;
			_userManager = userManager;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index(string searchString, DateTime startDate, DateTime endDate, int page = 1)
		{
			var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

			SearchModel search = new SearchModel(startDate, endDate, searchString);

			IEnumerable<RecordModel> recordByCurrentUser = await _searchService.RecordsByCurrentUserAcync(search);

			// sort by date
			recordByCurrentUser = _sortingService.SortingRecordsByDate(recordByCurrentUser);

			var decryptedRecords = new List<RecordViewModel>();

			foreach (var record in recordByCurrentUser)
			{
				string decriptedText = _aesCryptoProvider.DecryptValue(record.Text, currentUser.CryptoKey, record.IvKey);

				if (decriptedText.Length > 200)
				{
					decriptedText = decriptedText.Substring(0, 200);
				}

				var decryptedRecord = _mapper.Map<RecordViewModel>(new RecordViewModel
				{
					Id = record.Id,
					Name = record.Name,
					DateCreation = record.DateCreation,
					ModifiedDate = record.ModifiedDate,
					IvKey = record.IvKey,
					Text = decriptedText
				});

				decryptedRecords.Add(decryptedRecord);
			}

			//string search
			if (!string.IsNullOrEmpty(searchString))
			{
				decryptedRecords = decryptedRecords.Where(r => r.Text.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
			}

			//number records on page
			int pageSize = 5;
			var count = decryptedRecords.Count();

			var listToDisplay = decryptedRecords.Skip((page - 1) * pageSize).Take(pageSize).ToList();

			PagingViewModel pagingViewModel = new PagingViewModel(count, page, pageSize);

			IndexViewModel indexViewModel = new IndexViewModel
			{
				PagingViewModel = pagingViewModel,
				Records = listToDisplay,
				SearchModel = search
			};

			return View(indexViewModel);
		}
	}
}
