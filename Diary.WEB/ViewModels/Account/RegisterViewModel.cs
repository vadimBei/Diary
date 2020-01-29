using System.ComponentModel.DataAnnotations;

namespace Diary.WEB.ViewModels.Account
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Не вказаний нікнейм")]
		[Display(Name = "Нікнейм")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Не вказаний email")]
		[Display(Name = "Email")]
		[EmailAddress(ErrorMessage = "Некоректна email адреса")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Не введений пароль")]
		[Display(Name = "Пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Повторно не введений пароль")]
		[Display(Name = "Підтвердіть пароль")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Паролі не співпадають")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Не введена капча")]
		[Display(Name = "Введіть код з картинки")]
		public string Captcha { get; set; }
	}
}
