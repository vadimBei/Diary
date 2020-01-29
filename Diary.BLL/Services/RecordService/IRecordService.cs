using Diary.BLL.Models.Record;
using Diary.Core.Interfaces;
using Diary.DAL.Entities;
using System;

namespace Diary.BLL.Services.RecordService
{
	public interface IRecordService: IBaseService<Record, Guid, RecordCreateModel, RecordUpdateModel, RecordModel>
	{
	}
}
