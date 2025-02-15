using System;
using System.Net;
using System.Net.Mail;
using EShop.Services.Abstract;
using EShop.Shared.Configurations.Email;
using EShop.Shared.Dtos.ResponseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace EShop.Services.Concrete;

public class EmailManager : IEmailService
{
    private readonly EmailConfig _emailConfig;

    public EmailManager(IOptions<EmailConfig> emailConfig)
    {
        _emailConfig = emailConfig.Value;
    }

    public async Task<ResponseDto<NoContent>> SendEmailAsync(string emailTo, string subject, string htmlBody)
    {
        try
        {
            if (string.IsNullOrEmpty(_emailConfig.SmtpServer))
            {
                return ResponseDto<NoContent>.Fail("SMTP sunucu adresi yapılandırılmamış!", StatusCodes.Status500InternalServerError);
            }
            if (string.IsNullOrEmpty(_emailConfig.SmtpUser))
            {
                return ResponseDto<NoContent>.Fail("SMTP kullanıcı adı bilgisi yapılandırılmamış!", StatusCodes.Status500InternalServerError);
            }
            if (string.IsNullOrEmpty(_emailConfig.SmtpPassword))
            {
                return ResponseDto<NoContent>.Fail("SMTP şifresi yapılandırılmamış!", StatusCodes.Status500InternalServerError);
            }
            if (string.IsNullOrEmpty(emailTo))
            {
                return ResponseDto<NoContent>.Fail("Alıcı Email adresi boş olamaz!", StatusCodes.Status500InternalServerError);
            }
            if (!IsValidEmail(emailTo))
            {
                return ResponseDto<NoContent>.Fail("Email adresi geçersizdir!", StatusCodes.Status400BadRequest);
            }
            using var smtpClient = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailConfig.SmtpUser, _emailConfig.SmtpPassword),
                EnableSsl = true,
                Timeout = 10000 //10 saniye
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.SmtpUser),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true, //içeriğinde sadece yazı olmayacağı için, yazılan mailin arka planında aslınd ahtml formatı olduğu için IsBodyHtml yaptık
                To = { new MailAddress(emailTo) } //Alıcı adresi. birden fazla alıcı eklenebildiği için scope içerisine aldık o şekilde tanımladık.
            };
            await smtpClient.SendMailAsync(mailMessage); //mailMessage içerisindeki bilgileri kullanarak maili gönderir.
            return ResponseDto<NoContent>.Success(StatusCodes.Status200OK);
        }
        catch (SmtpException smtpex) //SMTP ile ilgili (yukarıdaki SendMailAsync) hata olduğunda burası sadece o sırada çalışacak.
        {
            return ResponseDto<NoContent>.Fail(smtpex.Message, StatusCodes.Status502BadGateway);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    private bool IsValidEmail(string emailAddress)
    {
        try
        {
            var addr = new MailAddress(emailAddress);
            return addr.Address == emailAddress; //Eğer dışarıdan gelen email adresi ile oluşturulan(addr) adres aynı/eşit ise true döner.
        }
        catch (System.Exception)
        {
            return false;
        }
    }

}
