using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POTracker.API.Notifications.Repository;

namespace POTracker.API.Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : ControllerBase
    {
        // GET: api/WhatsApp
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET: api/WhatsApp/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/WhatsApp
        [HttpPost]
        public void Post()
        {
            TwilioClientRepository twilioRepo = new TwilioClientRepository("your account id","token");
            try
            {
                twilioRepo.SendNotification();

            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT: api/WhatsApp/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
