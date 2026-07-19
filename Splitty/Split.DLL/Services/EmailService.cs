using Microsoft.Extensions.Configuration;
using Resend;
using Split.BLL.Interfaces;
using System.Net.Mail;

namespace Split.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IResend _resend;

        // Inject the official Resend client directly
        public EmailService(IResend resend)
        {
            _resend = resend;
        }

        public async Task SendTripSummaryAsync(string toEmail, string tripName, decimal totalCost, List<string> participantEmails, string shareableLink)
        {
            var participantsHtml = participantEmails != null && participantEmails.Any()
                ? "<ul>" + string.Join("", participantEmails.Select(email => $"<li>{email}</li>")) + "</ul>"
                : "<p><em>No participants listed yet.</em></p>";

            var message = new EmailMessage();
            message.From = "onboarding@resend.dev";
            message.To.Add(toEmail);
            message.Subject = $"Trip Summary: {tripName}";

            message.HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px;'>
                    <h2>Split Application</h2>
                    <p>Here is the latest expense summary for the <strong>{tripName}</strong> trip.</p>
                    <p><strong>Total Trip Cost:</strong> ${totalCost.ToString("0.00")}</p>
                    <h4>Crew Members:</h4>
                    {participantsHtml}
                    <a href='{shareableLink}'>View Full Dashboard</a>
                </div>";

            // Fire the email using the injected client
            await _resend.EmailSendAsync(message);
        }
    }
}