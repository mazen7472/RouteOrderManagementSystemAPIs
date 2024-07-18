using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using OrderManagementSystem.Core.Entites;
using OrderManagementSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services
{
    public class EmailSenderService : IEmailSenderService
    {
   
        private readonly IConfiguration _configuration;


        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["Email:EmailUserName"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

       
            try
            {
                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(_configuration["Email:EmailHost"], 587, false);
                    await smtp.AuthenticateAsync(_configuration["Email:EmailUserName"], _configuration["Email:EmailPassword"]);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Exception occurred while sending email: {ex.Message}");
               
            }



        }




    }
}

