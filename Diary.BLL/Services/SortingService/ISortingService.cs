using Diary.BLL.Models.Record;
using System.Collections.Generic;


namespace Diary.BLL.Services.SortingService
{
	public interface ISortingService
	{
		IEnumerable<RecordModel> SortingRecordsByDate(IEnumerable<RecordModel> recordModels);
	}
}
