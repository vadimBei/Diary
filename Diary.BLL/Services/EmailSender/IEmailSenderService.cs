using System.Threading.Tasks;

namespace Diary.BLL.Services.EmailSender
{
	public interface IEmailSenderService
	{
		Task SendEmailAcync(string email, string subject, string message);
	}
}
