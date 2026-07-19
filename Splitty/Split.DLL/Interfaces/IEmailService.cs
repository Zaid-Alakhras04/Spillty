using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Split.BLL.Interfaces
{
    public interface IEmailService
    {
        Task SendTripSummaryAsync(string toEmail, string tripName, decimal totalCost, List<string> participantEmails , string shareableLink);
    }
}
