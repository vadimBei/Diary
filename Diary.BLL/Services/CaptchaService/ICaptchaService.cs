using Diary.BLL.Models;
using Microsoft.AspNetCore.Http;

namespace Diary.BLL.Services.CaptchaService
{
	public interface ICaptchaService
	{
		string GenerateCaptchaCode();

		bool ValidateCaptchaCode(string userInputCaptcha, HttpContext context);

		CaptchaResult GenerateCaptchaImage(int width, int height, string captchaCode);


	}
}
