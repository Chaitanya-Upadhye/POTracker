using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.IO;


namespace POTracker.BackgroundJobs
{
    public class EmailClient:IEmailService
    {
        //this is going to be the recurring task
        private readonly string mailServer= "imap.mail.yahoo.com", login= "testaccount7537@yahoo.com", password= "pqhspzskjhspogor";
        private readonly int port=993;
        private readonly bool ssl=true;
        EmailModel emailModel = new EmailModel();

       

        public IEnumerable<string> GetUnreadMails()
        {
            var messages = new List<string>();

            using (var client = new ImapClient())
            {
                client.Connect(mailServer, port, ssl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(login, password);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var results = inbox.Search(SearchOptions.All, SearchQuery.Not(SearchQuery.Seen));
                foreach (var uniqueId in results.UniqueIds)
                {
                    var message = inbox.GetMessage(uniqueId);

                    messages.Add(message.HtmlBody);

                    //Mark message as read
                    //inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                }

                client.Disconnect(true);
            }

            return messages;
        }

        public async Task GetAllMails()
        {

            List<MimeMessage> messages = new List<MimeMessage>();
            IEnumerable<MailboxAddress> fromAddresses = new List<MailboxAddress>();
            //List<MimeMessage> finalEmails = new List<MimeMessage>();
            List<EmailModel> finalEmails = new List<EmailModel>();


            using (var client = new ImapClient())
            {
                client.Connect(mailServer, port, ssl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(login, password);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var results = inbox.Count();
                
                for(int i=0; i<results;i++)
                {
                    MimeMessage  message= inbox.GetMessage(i);
                    
                    messages.Add(message);
                    //Mark message as read
                    //inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                }
                foreach (var item in messages)
                {
                    foreach (var from in item.From.Mailboxes.Where(x=>x.Address== "uzzomanis@gmail.com"))
                    {
                        

                        foreach (var attachment in item.Attachments.Where(x=>x.ContentDisposition.FileName.Contains("PDF")))
                        {
                            EmailModel emailModel = new EmailModel();

                            using (var memory = new MemoryStream())
                            {
                                    if (attachment is MimePart)
                                        ((MimePart)attachment).Content.DecodeTo(memory);
                                    else
                                        ((MessagePart)attachment).Message.WriteTo(memory);

                                    emailModel.attachedPdf = memory.ToArray();
                                emailModel.from = from.Address;


                            }
                            finalEmails.Add(emailModel);

                        }




                    }
                }
                client.Disconnect(true);
            }


            using (var httpClient=new HttpClient())
            {
                
                StringContent content = new StringContent(JsonConvert.SerializeObject(finalEmails), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:44376/api/Email", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //log this response
                }

            }




        }
    }

  

}
