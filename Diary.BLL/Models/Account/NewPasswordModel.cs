namespace Diary.BLL.Models.Account
{
	public class NewPasswordModel
	{
		public string Email { get; set; }

		public string UserName { get; set; }

		public string NewPassword { get; set; }

		public string CurrentPassword { get; set; }
	}
}
