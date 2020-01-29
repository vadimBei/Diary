using System.Threading.Tasks;

namespace Diary.BLL.Services.EmailSender
{
	public interface IEmailSenderService
	{
		Task SendEmail(string email, string subject, string message);
	}
}
