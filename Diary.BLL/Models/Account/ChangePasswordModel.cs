namespace Diary.BLL.Models.Account
{
	public class ChangePasswordModel : BaseModel
	{
		public string Email { get; set; }

		public string NewPassword { get; set; }
		
		public string UserName { get; set; }
	}
}
