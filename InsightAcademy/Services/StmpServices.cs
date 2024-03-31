using System.Net.Mail;
using System.Net;
using static System.Net.WebRequestMethods;
using InsightAcademy.Entities;

namespace InsightAcademy.Services
{
	public interface IStmpServices
	{
		public string sendStmpEmail(string stmp,int userid);
	}
	public class StmpServices:IStmpServices
	{
		private readonly IConfiguration _configuration;
		string message = "";
        public StmpServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string sendStmpEmail(string email,int userid)
		{
			string fromAddress = _configuration["Stmp:SenderEmail"];
			var toAddress = new MailAddress(email, "To person");
		    string fromPassword = _configuration["Stmp:SenderPwd"];
			const string subject = "Reset Password";
			string body = $"Click the link below to recover your password: https://localhost:44327/Authentication/ResetPassword?userid={userid}";
			try
			{
				using (MailMessage mail = new MailMessage())
				{
					mail.From = new MailAddress(fromAddress);
					mail.To.Add(toAddress);
					mail.Subject = subject;
					mail.Body = body;
					mail.IsBodyHtml = true;

					using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
					{
						smtp.UseDefaultCredentials = false;
						smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
						smtp.EnableSsl = true;
						smtp.Send(mail);
					}
					message = "Email sent successfully!";
				}
			}
			catch (Exception ex)
			{				
				message = "Failed to send email: " + ex.Message;
			}
				

			return message;
		}




	}
}
