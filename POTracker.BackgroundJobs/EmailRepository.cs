using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POTracker.BackgroundJobs
{
    public class EmailRepository:IEmailService
    {
        //this is going to be the recurring task
        private readonly string mailServer= "imap.mail.yahoo.com", login= "testaccount7537@yahoo.com", password= "pqhspzskjhspogor";
        private readonly int port=993;
        private readonly bool ssl=true;

        //public EmailRepository(string mailServer, int port, bool ssl, string login, string password)
        //{
        //    this.mailServer = mailServer;
        //    this.port = port;
        //    this.ssl = ssl;
        //    this.login = login;
        //    this.password = password;
        //}

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

        public  IEnumerable<Object> GetAllMails()
        {
            List<MimeMessage> messages = new List<MimeMessage>();
            IEnumerable<MailboxAddress> fromAddresses = new List<MailboxAddress>();
            List<MimeMessage> finalEmails = new List<MimeMessage>();

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
                    fromAddresses= item.From.Mailboxes;
                    foreach (var from in fromAddresses)
                    {
                        if(from.Address == "uzzomanis@gmail.com") finalEmails.Add(item);
                        
                    }
                }



                client.Disconnect(true);
            }

            return finalEmails;
        }
    }

  

}
