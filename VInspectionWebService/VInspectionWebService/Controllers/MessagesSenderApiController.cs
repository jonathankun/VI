using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace VInspectionWebService.Controllers
{
    public class MessagesSenderApiController : ApiController
    {
        public bool SendEmail(string name, List<string> receivers, string subject, string message)
        {
            try
            {
                for (int i = 0; i < receivers.Count; i++)
                {
                    string receiverEmail = receivers[i];

                    if (ModelState.IsValid)
                    {
                        var senderEmail = new MailAddress("skpplantas@gmail.com", name);
                        var receiveremail = new MailAddress(receiverEmail, "Responsable");

                        const string password = "plant17esencia25messie3000";

                        var sub = subject;
                        var body = message;

                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };

                        using (var mess = new MailMessage(senderEmail, receiveremail)
                        {
                            Subject = subject,
                            Body = body
                        })
                        {
                            smtp.Send(mess);
                        }
                        Debug.Print("\n\nEmail sended\n\n");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.Print("\n\nThere are some problems in sending Email\n\n");
                return false;
            }

            
        }
    }
}
