using System;

namespace Diary.WEB.ViewModels.Common
{
	public class PagingViewModel
	{
		public int PageNumber { get; private set; }
		public int TotalPages { get; private set; }

		public PagingViewModel(int count, int pageNumber, int pageSize)
		{
			PageNumber = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);
		}

		public bool HasPreviousPage
		{
			get
			{
				return (PageNumber > 1);
			}
		}

		public bool HasNextPage
		{
			get
			{
				return (PageNumber < TotalPages);
			}
		}
	}
}
