using Diary.BLL.Models;
using Diary.WEB.ViewModels.UploadedFile;
using Diary.WEB.ViewModels.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Diary.WEB.ViewModels.Record
{
	public class RecordViewModel : BaseModel
	{
		[Required(ErrorMessage = "Поле не може бути пустим")]
		[MaxLength(50, ErrorMessage = "Перевищена максимальна кількість символів. Максимальна кількість символів: 50.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Поле не може бути пустим")]
		[MaxLength(500, ErrorMessage = "Перевищена максимальна кількість символів. Максимальна кількість символів: 500.")]
		public string Text { get; set; }

		public Guid UserId { get; set; }

		public UserViewModel UserViewModel { get; set; }

		public bool IsImage { get; set; }

		public byte[] IvKey { get; set; }

		public List<UploadedFileViewModel> UploadedFileViewModels { get; set; }

		public List<IFormFile> UploadedFiles { get; set; }
	}
}
