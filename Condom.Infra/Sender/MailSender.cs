using Condom.Infra.Resources;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Sender
{
    public class MailSender
    {
        protected SendGridClient Client { get; set; }
        public MailSender()
        {
            Client = new SendGridClient(Settings.GridToken);
        }

        public async Task SendForgetPassword(string emailTo, string token, string name = "Usuário")
        {
            var to = new EmailAddress(emailTo, name);
            var from = new EmailAddress("RM92593@fiap.com.br", "Condom");
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, "d-4920d173373a4f0e82b313829ef65efe", new { url = $"https://condom.azurewebsites.net/account/reset?token={token}&email={emailTo}" });
            var response = await Client.SendEmailAsync(msg);
        }
        public async Task SendConfirmation(string emailTo, string token, string name = "Usuário")
        {
            var to = new EmailAddress(emailTo, name);
            var from = new EmailAddress("RM92593@fiap.com.br", "Condom");
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, "d-966fdc70beeb42b3b932d854ca7052c2", new { url = $"https://condom.azurewebsites.net/account/register/confirm?token={token}&email={emailTo}", first_name = name });
            var response = await Client.SendEmailAsync(msg);
        }
    }
}

