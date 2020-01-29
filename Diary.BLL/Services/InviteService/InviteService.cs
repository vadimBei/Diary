using AutoMapper;
using Diary.BLL.Models.Invite;
using Diary.Core;
using Diary.DAL.Common;
using Diary.DAL.Entities;
using System;

namespace Diary.BLL.Services.InviteService
{
	public class InviteService : BaseService<Invite, Guid, InviteCreateModel, InviteUpdateModel, InviteModel>, IInviteService
	{
		public InviteService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
		{
		}
	}
}
