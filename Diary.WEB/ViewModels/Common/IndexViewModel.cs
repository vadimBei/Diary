using Diary.BLL.Models;
using Diary.BLL.Models.SearchOfRecord;
using Diary.WEB.ViewModels.Record;
using System.Collections.Generic;

namespace Diary.WEB.ViewModels.Common
{
	public class IndexViewModel
	{
		public IEnumerable<RecordViewModel> Records { get; set; }

		public PagingViewModel PagingViewModel { get; set; }

		public SearchModel SearchModel { get; set; }
	}
}
