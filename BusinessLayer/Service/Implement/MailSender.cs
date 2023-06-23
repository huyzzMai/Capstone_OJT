using DataAccessLayer.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Net.Http;
//using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace BusinessLayer.Service.Implement
{
    public class MailSender
    {
        public static string Content(String username, string resetCode)
        {
            return 
                //"\tEmail Reset Password\n" +
                "\tDear " + username + " ,\n" +
                "\tThis is an email to confirm your reset password password. Please use code below to reset your password.\n" +
                "\t" + resetCode + "\n" +
                "\t\n" +
                "\tThank you for your attention. Have a nice day!\n" +
                "\n";
        }

        public void Send(string sendto, string username, string code)
        {
            try
            {

                IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", true, true)
                 .Build();

                //MailMessage mail = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient();

                //mail.From = new MailAddress(config["HostEmail:Email"]);
                //mail.To.Add(sendto);
                //mail.Subject = "KNS - Reset Password Code";
                //mail.IsBodyHtml = true;
                //mail.Body = Content(username, code);

                //mail.Priority = MailPriority.High;

                //SmtpServer.Host = "smtp.gmail.com";
                //SmtpServer.Port = 587;
                //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //SmtpServer.UseDefaultCredentials = false;
                //SmtpServer.Credentials = new System.Net.NetworkCredential(config["HostEmail:Email"], config["HostEmail:Password"]);
                //SmtpServer.EnableSsl = true;

                //SmtpServer.Send(mail);
                //return "OK";


                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("KNS OJT Support", config["HostEmail:Email"]));
                email.To.Add(new MailboxAddress("KNS OJT User", sendto));

                email.Subject = "KNS - Reset Password Code";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = Content(username, code)
                };
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate(config["HostEmail:Email"], config["HostEmail:Password"]);

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
