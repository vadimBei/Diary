using Diary.BLL.Models.Record;
using Diary.BLL.Models.SearchOfRecord;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diary.BLL.Services.SearchService
{
	public interface ISearchService
	{
		Task<IEnumerable<RecordModel>> RecordsByCurrentUser(SearchModel searchModel);
	}
}
