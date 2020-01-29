using System;

namespace Diary.WEB.ViewModels.User
{
	public class UserViewModel
	{
		public Guid Id { get; set; }

		public string Email { get; set; }

		public string UserName { get; set; }

		public DateTime? DeletedDate { get; set; }

		public DateTime DateCreation { get; set; }

		public DateTime ModifiedDate { get; set; }
		
		public bool EmptyCryptoKey { get; set; }

	}
}
