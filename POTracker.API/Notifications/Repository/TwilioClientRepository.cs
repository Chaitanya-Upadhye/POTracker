using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Clients;

namespace POTracker.API.Notifications.Repository
{
    public class TwilioClientRepository
    {

        private readonly TwilioRestClient _restClient;
        public TwilioClientRepository(string accountSid, string authToken)
        {
            _restClient = new TwilioRestClient(accountSid, authToken);
        }


        public bool SendNotification()
        {
            try
            {
               
                var messageOptions = new CreateMessageOptions(
           new PhoneNumber("whatsapp:+91{your number}"));
                messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
                messageOptions.Body = "Greetings from whatsapp notfication service";
                
                
                var message = MessageResource.Create(messageOptions,_restClient);

                return true;
            }
            
            catch (Exception)
            {

                throw;
            }
        }


    }
}
