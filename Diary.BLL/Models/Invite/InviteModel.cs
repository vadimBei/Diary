using Diary.Core.Interfaces;
using System;

namespace Diary.BLL.Models.Invite
{
	public class InviteCreateModel : ICreateModel
	{
		public InviteCreateModel()
		{
			DateCreation = DateTime.Now;
		}

		public string EmailNewUser { get; set; }

		public DateTime DateCreation { get; set; }

		public DateTime ModifiedDate { get; set; }
	}

	public class InviteUpdateModel : InviteCreateModel, IUpdateModel<Guid>
	{
		public Guid Id { get; set; }
	}

	public class InviteModel : InviteUpdateModel, IViewModel<Guid>
	{

	}
}
