using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POTracker.BackgroundJobs
{
    interface IEmailService
    {
        public IEnumerable<string> GetAllMails();

        public IEnumerable<string> GetUnreadMails();
    }
}
