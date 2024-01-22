using System.Net;
using System.Net.Mail;

namespace bookingservice.service;

public class EmailManager
{
    private readonly ILogger _logger;
    private readonly SmtpClient _client;
    public EmailManager(ILogger logger)
    {
        _logger = logger;
        try
        {
            _client = new SmtpClient();
            _client.Port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")!);
            _client.Host = Environment.GetEnvironmentVariable("SMTP_HOST")!;
            _client.Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("SMTP_USER"),
                Environment.GetEnvironmentVariable("SMTP_PASSWORD"));
            _client.EnableSsl = false;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {Message}", ex.Message);
            throw;
        } 
    }

    public void SendEmail(string address, string subject, string text)
    {
        try
        {
            var email = new MailMessage();
            email.Subject = subject;
            email.Body = text;
            var emailAddressFrom = new MailAddress("noreply@bookingservice.com");
            var emailAddressTo = new MailAddress(address);
            email.From = emailAddressFrom;
            email.To.Add(emailAddressTo);
            _client.Send(email);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {Message}", ex.Message);
            throw;
        } 
    }
}