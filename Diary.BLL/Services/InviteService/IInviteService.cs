using Diary.BLL.Models.Invite;
using Diary.Core.Interfaces;
using Diary.DAL.Entities;
using System;

namespace Diary.BLL.Services.InviteService
{
	public interface IInviteService : IBaseService<Invite, Guid, InviteCreateModel, InviteUpdateModel, InviteModel>
	{
	}
}
