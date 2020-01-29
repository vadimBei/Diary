using System.ComponentModel.DataAnnotations;

namespace Diary.WEB.ViewModels.Account
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Не вказаний нікнейм")]
		[Display(Name = "Нікнейм")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Не введений пароль")]
		[Display(Name = "Пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Запам'ятати мене")]
		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }
	}
}
