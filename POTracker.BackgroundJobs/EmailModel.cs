using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;

namespace POTracker.BackgroundJobs
{
    public class EmailModel
    {
        public string from { get; set; }
        public byte[] attachedPdf { get; set; }




    }
}
