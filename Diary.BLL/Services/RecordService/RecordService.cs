using AutoMapper;
using Diary.BLL.Models.Record;
using Diary.Core;
using Diary.DAL.Common;
using Diary.DAL.Entities;
using System;

namespace Diary.BLL.Services.RecordService
{
	public class RecordService : BaseService<Record, Guid, RecordCreateModel, RecordUpdateModel, RecordModel>, IRecordService
	{
		public RecordService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
		{
		}
	}
}
