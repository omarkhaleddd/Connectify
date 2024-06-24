using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : APIBaseController
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _senderName;
        private readonly string _senderEmail;
        private readonly UserManager<AppUser> _manager;
        private readonly IConfiguration _configuration;


        public MailController(IConfiguration configuration, UserManager<AppUser> manager)
        {
            var smtpSettings = configuration.GetSection("SmtpSettings");
            _manager = manager;
            _configuration = configuration;
        }
        [HttpPost("SendMail/{recipientEmail}")]
        public async Task<ActionResult> SendEmail(string recipientEmail)
        {
            var fromEmail = _configuration["Mail:FromEmail"];
            var fromPassword = _configuration["Mail:FromPassword"];
            var smtpServer = _configuration["Mail:SmtpServer"];
            var port = Convert.ToInt32(_configuration["Mail:Port"]);
            using (var message = new MailMessage())
            {

                try
                {
                    message.From = new MailAddress(fromEmail);
                    message.To.Add(new MailAddress(recipientEmail));
                    message.Subject = "Login";
                    message.Body = "You just logged in to your account on connectify , if it is not you change the password ASAP!";
                    message.IsBodyHtml = true; // Set to true for HTML content

                    using (var smtpClient = new SmtpClient(smtpServer, port))
                    {
                        smtpClient.Credentials = new NetworkCredential(fromEmail, fromPassword);
                        smtpClient.EnableSsl = true; // Enable SSL if needed
                        await smtpClient.SendMailAsync(message);
                    }
                    return Ok("Email sent!");
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }
        }
    }
}
