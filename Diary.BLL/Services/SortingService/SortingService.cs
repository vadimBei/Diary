using Diary.BLL.Models.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.BLL.Services.SortingService
{
	public class SortingService : ISortingService
	{
		public IEnumerable<RecordModel> SortingRecordsByDate(IEnumerable<RecordModel> recordModels)
		{
			recordModels = recordModels.OrderByDescending(d => d.ModifiedDate).ToList();

			return recordModels;
		}
	}
}
