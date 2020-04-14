using MimeKit;
using System.Threading.Tasks;

namespace Diary.BLL.Services.EmailSender
{
	public class EmailSenderService : IEmailSenderService
	{
		public async Task SendEmailAcync(string email, string subject, string message)
		{
			var emailMessage = new MimeMessage();

			emailMessage.From.Add(new MailboxAddress("Day book", "bey1705@gmail.com"));
			emailMessage.To.Add(new MailboxAddress("", email));
			emailMessage.Subject = subject;
			emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = message
			};

			using (var client = new MailKit.Net.Smtp.SmtpClient())
			{
				await client.ConnectAsync("smtp.gmail.com", 465, true);
				await client.AuthenticateAsync("bey1705@gmail.com", "mon@h46progr@mmer");
				await client.SendAsync(emailMessage);

				await client.DisconnectAsync(true);
			}
		}
	}
}
