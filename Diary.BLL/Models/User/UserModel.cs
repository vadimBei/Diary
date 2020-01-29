using Diary.BLL.Models.Record;
using Diary.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Diary.BLL.Models.User
{
	public class UserCreateModel : ICreateModel
	{
		public string Email { get; set; }

		public string UserName { get; set; }

		public DateTime? DeletedDate { get; set; }

		public byte[] CryptoKey { get; set; }

		public DateTime DateCreation { get; set; }

		public DateTime ModifiedDate { get; set; }
	}

	public class UserUpdateModel : UserCreateModel, IUpdateModel<Guid>
	{
		public Guid Id { get; set; }
	}

	public class UserModel : UserUpdateModel, IViewModel<Guid>
	{
		public List<RecordModel> RecordModels { get; set; }
	}
}
