using System.ComponentModel.DataAnnotations;

namespace Diary.WEB.ViewModels.Account
{
	public class ChangePasswordViewModel
	{
		public string Email { get; set; }

		[Required(ErrorMessage = "Не введений поточний пароль пароль")]
		[DataType(DataType.Password)]
		public string CurrentPassword { get; set; }

		[Required(ErrorMessage = "Не введений пароль")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		public string UserName { get; set; }
	}
}
