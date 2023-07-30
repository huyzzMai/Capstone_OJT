using DataAccessLayer.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLayer.Service.Implement
{
    public class MailSender
    {
        public static string Content(string username, string resetCode)
        {
            return
                "<html>\n" +
                "<body>\n" +
                "\t<h2>Email Reset Password</h2>\n" +
                "\t<p>Dear " + username + " ,</p>\n" +
                "\t<p>This is email for confirm your registration account. Please use code below to reset your password.</p><br>\n" +
                "\t<b>" + resetCode + "</b>\n" +
                "\t<br>\n" +
                "\t<p>Thanks for using our product. Have a nice day!</p>\n" +
                "</body>\n" +
                "</html>";
        }

        public static string CreateAccountContent(string username, string email, string password)
        {
            return
                "<html>\n" +
                "<body>\n" +
                "\t<h2>KNS OJT Account</h2>\n" +
                "\t<p>Dear " + username + " ,</p>\n" +
                "\t<p>This is an email to let you know that you have an account in the KNS OJT System. The following information to log into our system is your email and password :</p>\n" +
                "\t<b>" + email + "</b><br>\n" +
                "\t<b>" + password + "</b>\n" +
                "\t<br>\n" +
                "\t<p>Please do not share information in this email to anyone. You should reset the password as you please. Have a nice day!</p>\n" +
                "</body>\n" +
                "</html>";
        }

        public void Send(string sendto, string username, string code)
        {
            try
            {

                IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", true, true)
                 .Build();

                #region Send mail with System.Net protocol
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient();

                mail.From = new MailAddress(config["HostEmail:Email"]);
                mail.To.Add(sendto);
                mail.Subject = "KNS OJT - Reset Password Code";
                mail.IsBodyHtml = true;
                mail.Body = Content(username, code);

                mail.Priority = MailPriority.High;

                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(config["HostEmail:Email"], config["HostEmail:Password"]);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                #endregion

                #region Send mail with MailKit protocol

                //var email = new MimeMessage();

                //email.From.Add(new MailboxAddress("KNS OJT Support", config["HostEmail:Email"]));
                //email.To.Add(new MailboxAddress("KNS OJT User", sendto));

                //email.Subject = "KNS - Reset Password Code";
                //email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                //{
                //    Text = Content(username, code)
                //};
                //using (var smtp = new SmtpClient())
                //{
                //    smtp.Connect("smtp.gmail.com", 587, false);

                //    // Note: only needed if the SMTP server requires authentication
                //    smtp.Authenticate(config["HostEmail:Email"], config["HostEmail:Password"]);

                //    smtp.Send(email);
                //    smtp.Disconnect(true);
                //}
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void SendMailCreateAccount(string sendto, string username, string email, string password)
        {
            try
            {

                IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", true, true)
                 .Build();

                #region Send mail with System.Net protocol
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient();

                mail.From = new MailAddress(config["HostEmail:Email"]);
                mail.To.Add(sendto);
                mail.Subject = "KNS OJT - New Account";
                mail.IsBodyHtml = true;
                mail.Body = CreateAccountContent(username, email, password);

                mail.Priority = MailPriority.High;

                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(config["HostEmail:Email"], config["HostEmail:Password"]);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
