using System;

namespace Diary.BLL.Models.SearchOfRecord
{
	public class SearchModel
	{
		public SearchModel()
		{

		}

		public SearchModel(DateTime startDate, DateTime endDate, string searchString)
		{
			StartDate = startDate;
			EndDate = endDate;
			SearchString = searchString;
		}

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public string SearchString { get; set; }
	}
}
