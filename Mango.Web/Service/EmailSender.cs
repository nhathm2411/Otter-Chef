using Mango.Web.Service.IService;
using System;
using System.Net;
using System.Net.Mail;

namespace Mango.Web.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message, string logoPath, string codeOTP)
        {
            var mail = "nhathmce170171@fpt.edu.vn";  // Địa chỉ email gửi
            var password = "gwhw tcbd cmgh pgpo";    // Mật khẩu hoặc App password của email

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            // Tạo MailMessage
            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                IsBodyHtml = true // Kích hoạt HTML
            };

            mailMessage.To.Add(email);

            // Tạo LinkedResource cho hình ảnh
            if (File.Exists(logoPath))
            {
                LinkedResource inlineLogo = new LinkedResource(logoPath)
                {
                    ContentId = "OtterChefLogo", // 'cid' sẽ được sử dụng trong HTML
                    ContentType = new System.Net.Mime.ContentType("image/png")
                };

                // Tạo AlternateView để xử lý HTML + Hình ảnh nhúng
                var htmlView = AlternateView.CreateAlternateViewFromString(message, null, "text/html");
                htmlView.LinkedResources.Add(inlineLogo);
                mailMessage.AlternateViews.Add(htmlView);
            }
            else
            {
                // Nếu hình ảnh không tồn tại, chỉ gửi nội dung mà không có hình ảnh
                mailMessage.Body = message;
            }

            // Gửi email
            return client.SendMailAsync(mailMessage);
        }

        public Task SendEmailAsync(string email, string subject, string message, string logoPath)
        {
            var mail = "nhathmce170171@fpt.edu.vn";  // Địa chỉ email gửi
            var password = "gwhw tcbd cmgh pgpo";    // Mật khẩu hoặc App password của email

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            // Tạo MailMessage
            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                IsBodyHtml = true // Kích hoạt HTML để cho phép hiển thị hình ảnh
            };

            mailMessage.To.Add(email);

            // Tạo LinkedResource cho hình ảnh nhúng
            if (File.Exists(logoPath))
            {
                LinkedResource inlineLogo = new LinkedResource(logoPath)
                {
                    ContentId = "OtterChefLogo", // 'cid' sẽ được sử dụng trong HTML
                    ContentType = new System.Net.Mime.ContentType("image/png")
                };

                // Tạo AlternateView cho HTML
                var htmlView = AlternateView.CreateAlternateViewFromString(message, null, "text/html");
                htmlView.LinkedResources.Add(inlineLogo);  // Thêm hình ảnh vào nội dung email
                mailMessage.AlternateViews.Add(htmlView);
            }
            else
            {
                // Nếu không có hình ảnh, chỉ gửi phần nội dung HTML
                mailMessage.Body = message;
            }

            // Gửi email
            return client.SendMailAsync(mailMessage);
        }

    }
}
